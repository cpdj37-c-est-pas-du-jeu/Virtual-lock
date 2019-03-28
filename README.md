![CPDJ Virtual lock logo](https://github.com/cpdj37-c-est-pas-du-jeu/Virtual-lock/blob/master/CPDJ_VirtualLock/ressources/images/logo/Virtual_lock_logo_128_x_128.png)

# C'est (pas) du jeu : Virtual-lock
> Serrure numerique avec compte a rebour

#### Disclaimer / Mise en garde

> **Warning** : This project and related release binaries are distributed **AS-IS**.<br>
> *C'est (pas) du jeu* association **IS NOT responsible** of any issue, crash, bug or harm done to your computer

> **Attention** : Ce project et ses executables sont fournis **EN L'ETAT**.<br>
> L'association *C'est (pas) du jeu* **n'est pas responsable** de quelquonques problemes, plantage, bug, ou dommage cause a votre materiel informatique

## Description

Elements :
- Compte a rebour
- Si le compte a rebour arrive a son echeance, echec.
- Si bonne saisie du mot-de-passe, arret du compte a rebour et affichage d'une image/video

Fonctionnalites additionnels :
- Configuration via formulaire *(sauvegarde entre chaque utilisation)*

Idees d'ajouts :
- Des evenements ponctuels
- Des alertes sonores pour rappeler aux joueurs l'echeance (renforce le concepte de pression)
  > Pour garantir la progression des joueurs : des indices (audio, videos, etc) se debloquent a certains intervales

[Sounds](https://github.com/cpdj37-c-est-pas-du-jeu/Virtual-lock/tree/master/CPDJ_VirtualLock/ressources/sounds/) are from the royalty free music [zapsplat](https://www.zapsplat.com)

## Installation

1 - Telecharger la derniere [**release**](https://github.com/cpdj37-c-est-pas-du-jeu/Virtual-lock/releases)<br>
2 - Lancer l'installation en double-cliquant sur l'installeur<br>

![Debut d'installation](https://github.com/cpdj37-c-est-pas-du-jeu/Virtual-lock/blob/master/misc/app_screenshots/latest/installation.PNG)

3 - Suivez les etapes d'installation<br>
4 - Optionellement, choisissez un dossier d'installation **et/ou** verifiez la taille requise<br>

![Dossier d'installation et taille requise](https://github.com/cpdj37-c-est-pas-du-jeu/Virtual-lock/blob/master/misc/app_screenshots/latest/installation_target.PNG)

5 - L'installation est un succes<br>

## Utilisation : Configuration

1 - Rendez-vous dans le dossier d'installation et cliquez sur l'executable **CPDJ_VirtualLock.exe**<br>

![Dossier d'installation](https://github.com/cpdj37-c-est-pas-du-jeu/Virtual-lock/blob/master/misc/app_screenshots/latest/installation_result.PNG)

2 - Une fenetre de configuration apparait, avec des valeurs fournient par defaut. Vous pouvez changer certaines valeurs, telle que :
- La duree du jeu
- Le mot de passe requis pour gagner le jeu
- Le nombre d'essaie avant blocage, la duree des blocages, et si les blocages sont definitifs
- Les images qui apparaissent en cas de defaites & victoires
- La musique d'ambiance
- Les sons en cas de defaite & victoire
- Le son en cas de saisie invalide du mot de passe
- Un son qui sera joue de facon recurrente, et les intervales entre chaque recurrence.

![Fenetre de configuration](https://github.com/cpdj37-c-est-pas-du-jeu/Virtual-lock/blob/master/misc/app_screenshots/latest/window_configuration.PNG)

3 - Validez votre formulaire en cliquant sur le bouton "Valider" en bas de celui-ci<br>

## Utilisation : Le jeu *(avec valeurs par defaut)*

> Dans cet exemple, nous utiliserons les valeurs de configuration fournient par defaut

1 - Une fois la configuration termine, le jeu se lance et la fenetre principale apparait.<br>

![Fenetre de demarrage](https://github.com/cpdj37-c-est-pas-du-jeu/Virtual-lock/blob/master/misc/app_screenshots/latest/window_main_unstarted.PNG)

2 - Cliquez sur "**Start**" pour demarrer la partie

Le chrono demarre, et les elements du jeu apparaissent

Composition de la fenetre :
- Un compteur central, qui indique le temps restant avant la defaite
- Une barre de progression qui indique le ratio de completion du temps ecoule par rapport a la duree totale
- Un champ de saisie du mot-de-passe (avec auto-focus) dans la partie inferieure.
- Un compteur de saisie restante avant blocage, dans la partie inferieure droite.

![Fenetre de jeu](https://github.com/cpdj37-c-est-pas-du-jeu/Virtual-lock/blob/master/misc/app_screenshots/latest/window_in_game.PNG)

2 - Saisie du mot de passe
- En cas de saisie invalide, le compteur de saisie invalide decremente. Si il atteint 0, la saisie est bloquee pour un certain temps. Une barre de progression apparait alors pour indiquer la duree du blocage.

- En cas de saisie correct, la partie est termine et l'ecran de victoire apparait

3 - Victoire & defaite

- En cas de defaite *(le temps total est ecoule)*, l'image de defaite apparait :

![Image de defaite par defaut](https://github.com/cpdj37-c-est-pas-du-jeu/Virtual-lock/blob/master/misc/app_screenshots/latest/window_success.PNG)

- En cas de victoire *(saisie du bon mot de passe)*, l'image de victoire apparait :

![Image de victoire par defaut](https://github.com/cpdj37-c-est-pas-du-jeu/Virtual-lock/blob/master/misc/app_screenshots/latest/window_success.PNG)
