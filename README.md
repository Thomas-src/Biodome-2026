# Bio Domes

Projet d'application web pour l'UE19. Le projet prend la forme d'une solution .NET découpée en cinq projets :

- un projet d'application web Razor Page
- un projet de bibliothèque modélisant le domaine.
- un projet d'infrastructures, ce dernier reprenant notamment les éléments propres à EntityFrameworkCore.
- un projet de test permettant de tester le projet de bibliothèque modélisant le domaine.
- un projet de test permettant de tester le projet d'infrastructures.

La suite du document sera à compléter par vos soins.

## Membres de l'équipe

**MAES Tibo** (Q240075) (Responsable du déploiement)

**DERDEYN Thomas** (240206)

Lien git : https://ue19.cg.helmo.be/q240075

## Construction de la solution

**Etape 1** : Cloner le projet sur le git : https://git.helmo.be/students/info/q240075/biodomes 

**Etape 2** : L'ouvrir dans une IDE tel que Rider

**Etape 3** : Vérifier la connexion internet à l'école ou depuis le VPN

**Etape 4** : Démarrer le projet en https ou http

## Fonctionnalités implémentées

**US-1**  _(Page d'acceuil)_ : Totalement achevée

**US-2**  _(Authentification)_ : Totalement achevée

**US-3**  _(Création de compte)_ : Totalement achevée

**US-4**  _(Gestion du compte)_ : Totalement achevée

**US-5**  _(Création d’un biome)_ : Totalement achevée

**US-6**  _(Modification d’un biome)_ : Totalement achevée

**US-7**  _(Ajouter/Retirer des espèces dans un biome )_ : Totalement achevée 

**US-8**  _(Ajout d’une espèce)_ : Totalement achevée

**US-9**  _(Ajout d’un équipement)_ : Totalement achevée

**US-10** _(Consultation des biomes ayant le meilleur score)_ : Totalement achevée

**US-11** _(Consultation des biomes en équilibre)_ : Totalement achevée

**US-12** _(Rendre disponible une espèce )_ : Totalement achevée

**US-13** _(Rendre disponible un équipement)_ : Totalement achevée

**US-14** _(BONUS : Aide à la création d’un réseau trophique)_ : non-faite

**US-15** _(Statistiques et Graphiques)_ : partiellement achevée --> Il manque le diagramme par espèce

**US-16** _(Modifier ou supprimer une espèce ou un équipement)_ : Totalement achevée

## Données de connexion

**Compte 1 :**

Chercheur ayant crée un biome, une espèce et un équipement : 

_Nom_ : `Darwin`

_MDP_ : `Dwin1859$$` 


**Compte 2 :**

Compte vierge : 

_Nom_ : `JohnDoe`

_MDP_ : `DoeJ1234!`


**Compte Admin :**

_Nom_ : `Admin`

_MDP_ : `Test1234$`

## Éléments techniques notables

Bibliothèque JS graphiques : Nous avons utiliser la bibliothèque chart.js

Protection DDOS : Nous avons mis en place du proctection Ddos qui peut être assez contraignante. Si vous faites vos tests assez rapidement, vous risquez de vous faire bloquer assez rapidement, afin de vous facilité la vie, les valeurs a modifier se trouvent ligne 94 et 95 du program.cs.  
