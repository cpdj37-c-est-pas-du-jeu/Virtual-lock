using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GCL
{
    public class DebugConsoleWindow : Window
    {
        public TextBox _txtBox = null;

        public DebugConsoleWindow()
            : base()
        {
            BrushConverter convert = new BrushConverter();
            _txtBox = new TextBox();
            _txtBox.Name = "textBox_debugConsole";
            _txtBox.Background = convert.ConvertFrom("#FF171717") as Brush;
            _txtBox.Foreground = Brushes.LightBlue;
            this.AddChild(_txtBox);
        }
    }

    public class Logger
    {
        public static Interface instance = null;

        public interface Interface
        {
            void Write(string msg);
            void Write(string[] msg);
        }
        public class NoLogger : GCL.Logger.Interface
        {
            public void Write(string msg) { }
            public void Write(string[] msg) { }
        }
        public class FileLogger : GCL.Logger.Interface
        {
            private string _filePath = null;

            public FileLogger(string filePath)
            {
                _filePath = filePath;
                File.AppendAllText(_filePath, string.Format(
                    "\n===========================\n" +
                    "======= {0:HH:mm:ss tt} =======\n" +
                    "===========================\n\n"
                    , DateTime.Now));
            }

            public void Write(string msg)
            {
                File.AppendAllText(_filePath, string.Format("[{0}] : {1}\n", System.DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), msg));
            }
            public void Write(string[] msg)
            {
                File.AppendAllText(_filePath, string.Format("[{0}] :\n", System.DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss")));
                File.AppendAllLines(_filePath, msg);
            }
        }
        // todo : deal with window.close if application.onLastWindowClosed
        public class TextBoxLogger : GCL.Logger.Interface
        {
            public TextBoxLogger(TextBox textBox)
            {
                logTextBox = textBox;
            }

            public TextBox logTextBox = null;

            //void Interface.Write(string msg)
            public void Write(string msg)
            {
                if (logTextBox == null)
                    throw new System.NullReferenceException("Logger.logTextBox is null");
                if (msg.Length == 0)
                    return;

                logTextBox.Text += string.Format("[{0}] : {1}\n", System.DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), msg);
                logTextBox.Focus();
                logTextBox.CaretIndex = logTextBox.Text.Length;
                logTextBox.ScrollToEnd();
            }
            public void Write(string[] msg)
            {
                foreach (var line in msg)
                {
                    Write(line);
                }
            }
        }
    }
}