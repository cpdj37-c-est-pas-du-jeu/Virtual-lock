using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GCL
{
    class TypeToTypeMap
    {
        private Dictionary<Type, object> values = new Dictionary<Type, object>();

        //public TypeToTypeMap(params dynamic[] args)
        public TypeToTypeMap() { }
        public TypeToTypeMap(params KeyValuePair<Type, object>[] args)
        {
            foreach (var arg in args)
            {
                values.Add(arg.Key, arg.Value);
            }
        }

        public void Add<T>(object value)
        {
            values[typeof(T)] = value;
        }

        public object Get<T>()
        {
            if (!values.ContainsKey(typeof(T)))
                throw new KeyNotFoundException("GCL.TypeToTypeMap.Get<T>");

            return values[typeof(T)];
        }
    }

    class TypeToFrameworkElement
    {
        public class Attributes
        {
            // Options
            public class UINotVisible : Attribute{ }
            public class UINotEditable : Attribute{ }
            // Types
            public class UIChrono : Attribute { }
            public class UITimeSpan : Attribute { }
            public class UIImageData : Attribute{ }
            public class UIFileData : Attribute{ }
            public class UIRichText : Attribute{ }
        }

        static public Window GetForm(object object_value)
        {
            var stackPanel = new StackPanel { Orientation = Orientation.Vertical };

            Dictionary<string, FieldInfo> fields = object_value.GetType().GetFields().ToDictionary
            (
                item => item.Name,
                item => item
            );

            //FieldInfo[] fields = object_value.GetType().GetFields();
            //PropertyInfo[] fields = typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields.Values)
            {
                //if (field.IsPublic && !field.IsStatic)
                {
                    Grid grid = new Grid { HorizontalAlignment = HorizontalAlignment.Stretch };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60, GridUnitType.Star) });

                    var label_field_name = new Label { Content = field.Name };
                    label_field_name.SetValue(Grid.ColumnProperty, 0);
                    grid.Children.Add(label_field_name);

                    var ui_element = GetFrameworkElement(field, field.GetValue(object_value));
                    ui_element.SetValue(Grid.ColumnProperty, 1);
                    ui_element.Name = field.Name;
                    grid.Children.Add(ui_element);

                    stackPanel.Children.Add(grid);
                }
            }

            Window window = new Window { Height = fields.Count * 40, Width = 600 };
            var validation_button = new Button { Content = "Validate" };
            validation_button.Click += (sender, e) =>
            {
                window.Close();
            };
            stackPanel.Children.Add(validation_button);
            window.Content = stackPanel;

            return window;
        }

        //static public FrameworkElement GetFrameworkElement<T>(T element_value)
        //{
        //    return GetFrameworkElement(typeof(T), element_value);
        //}
        static public FrameworkElement GetFrameworkElement(FieldInfo field, object element_value)
        {
            return GetFrameworkElement(field.FieldType, field.Name, element_value);
        }
        static public FrameworkElement GetFrameworkElement(Type type, string field_name, object element_value)
        {
            FrameworkElement value;

            #region special_cases
            if (Attribute.IsDefined(type, typeof(Attributes.UITimeSpan)))
            {
                value = new TextBox { Text = element_value?.ToString() ?? "" };
                var binding = new Binding(field_name)
                {
                    StringFormat = "dd-MM-yyyy"
                };
                value.SetBinding(TextBox.TextProperty, binding);
            }
            else if (Attribute.IsDefined(type, typeof(Attributes.UIChrono)))
            {
                value = new TextBox { Text = element_value?.ToString() ?? "" };
                var binding = new Binding(field_name)
                {
                    StringFormat = "hh:mm:ss" // %c
                };
                value.SetBinding(TextBox.TextProperty, binding);
            }
            else if (Attribute.IsDefined(type, typeof(Attributes.UIImageData)))
            {
                var img = new Image();
                img.Source = new BitmapImage(new Uri(element_value?.ToString() ?? "", UriKind.Absolute));
                value = img;
            }
            else if (Attribute.IsDefined(type, typeof(Attributes.UIFileData)))
            {
                var path_textBox = new TextBox { Text = element_value?.ToString() ?? "" };
                path_textBox.MouseDoubleClick += (sender, e) =>
                {
                    var fileDialog = new System.Windows.Forms.OpenFileDialog();
                    var result = fileDialog.ShowDialog();
                    switch (result)
                    {
                        case System.Windows.Forms.DialogResult.OK:
                            var file = fileDialog.FileName;
                            path_textBox.Text = file;
                            break;
                        case System.Windows.Forms.DialogResult.Cancel:
                        default:
                            path_textBox.Text = null;
                            break;
                    }
                };
                value = path_textBox;
            }
            else if (Attribute.IsDefined(type, typeof(Attributes.UIRichText)))
            {
                value = new RichTextBox { };
                throw new NotImplementedException();
            }
            else if (type.IsEnum)
            {
                var comboBox = new ComboBox();
                comboBox.ItemsSource = type.GetEnumValues().Cast<string>();
                comboBox.SelectedValue = element_value;
                value = comboBox;
            }
            #endregion
            else
            {   // default
                value = generators.ContainsKey(type) ? generators[type](element_value, field_name) : new TextBox { Text = element_value?.ToString() ?? "" };
            }

            #region additional qualifiers
            if (Attribute.IsDefined(type, typeof(Attributes.UINotEditable)))
                value.IsEnabled = false;
            if (Attribute.IsDefined(type, typeof(Attributes.UINotVisible)))
                value.Visibility = Visibility.Hidden;
            #endregion

            //value.DataContext = element_value;

            return value;
        }

        static private Dictionary<Type, Func<object, string, FrameworkElement>> generators = initialize_generators();
        static private Dictionary<Type, Func<object, string, FrameworkElement>> initialize_generators()
        {
            Dictionary<Type, Func<object, string, FrameworkElement>> value = new Dictionary<Type, Func<object, string, FrameworkElement>>();

            TextCompositionEventHandler numerical_previewTextInput = (sender, e) =>
            {
                Regex validation_regex = new Regex("[^0-9.-]+");
                e.Handled = validation_regex.IsMatch(e.Text);
            };

            value = new Dictionary<Type, Func<object, string, FrameworkElement>>
            {
                [typeof(bool)]   = (field_value, field_name) =>
                {
                    var check_box = new CheckBox { IsChecked = (bool?)field_value };
                    var binding = new Binding(field_name);
                    check_box.SetBinding(CheckBox.IsCheckedProperty, binding);
                    return check_box;
                },
                [typeof(string)] = (field_value, field_name) =>
                {
                    var text_box = new TextBox();
                    var binding = new Binding(field_name);
                    binding.Source = field_value;
                    text_box.SetBinding(TextBox.TextProperty, binding);
                    return text_box;
                },
                [typeof(String)] = (field_value, field_name) =>
                {
                    var text_box = new TextBox();
                    text_box.DataContext = field_value;
                    var binding = new Binding(field_name);
                    binding.Source = field_value;
                    text_box.SetBinding(TextBox.TextProperty, binding);
                    return text_box;
                },
                #region numerical values
                [typeof(int)] = (field_value, field_name) =>
                {
                    var ui_element = new TextBox { Text = field_value?.ToString() ?? "" };
                    ui_element.PreviewTextInput += numerical_previewTextInput;
                    return ui_element;
                },
                [typeof(Int16)] = (field_value, field_name) =>
                {
                    var ui_element = new TextBox { Text = field_value?.ToString() ?? "" };
                    ui_element.PreviewTextInput += numerical_previewTextInput;
                    return ui_element;
                },
                [typeof(Int32)] = (field_value, field_name) =>
                {
                    var ui_element = new TextBox { Text = field_value?.ToString() ?? "" };
                    ui_element.PreviewTextInput += numerical_previewTextInput;
                    return ui_element;
                },
                [typeof(Int64)] = (field_value, field_name) =>
                {
                    var ui_element = new TextBox { Text = field_value?.ToString() ?? "" };
                    ui_element.PreviewTextInput += numerical_previewTextInput;
                    return ui_element;
                },
                [typeof(uint)] = (field_value, field_name) =>
                {
                    var ui_element = new TextBox { Text = field_value?.ToString() ?? "" };
                    ui_element.PreviewTextInput += numerical_previewTextInput;
                    return ui_element;
                },
                [typeof(UInt16)] = (field_value, field_name) =>
                {
                    var ui_element = new TextBox { Text = field_value?.ToString() ?? "" };
                    ui_element.PreviewTextInput += numerical_previewTextInput;
                    return ui_element;
                },
                [typeof(UInt32)] = (field_value, field_name) =>
                {
                    var ui_element = new TextBox { Text = field_value?.ToString() ?? "" };
                    ui_element.PreviewTextInput += numerical_previewTextInput;
                    return ui_element;
                },
                [typeof(UInt64)] = (field_value, field_name) =>
                {
                    var ui_element = new TextBox { Text = field_value?.ToString() ?? "" };
                    ui_element.PreviewTextInput += numerical_previewTextInput;
                    return ui_element;
                }
                #endregion
            };

            return value;
        }
    }
}
