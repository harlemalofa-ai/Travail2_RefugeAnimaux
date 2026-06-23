# Refuge Animaux — Application Console

Projet réalisé par **Harlem Alofa** dans le cadre du projet de développement SGDB.

Ce dépôt contient la **partie Console** du projet Refuge Animaux.  
La partie WPF se trouve dans un dépôt séparé : `ProjetSGDB_RefugeAnimauxWPF`.

---

## Objectif

L’application permet de gérer les données principales d’un refuge pour animaux depuis une interface Console :

```text
- animaux
- contacts
- entrées et sorties du refuge
- adoptions
- familles d’accueil
- vaccins et vaccinations
- couleurs
- rôles
- compatibilités
- statistiques
- diagnostic de cohérence
```

---

## Technologies utilisées

```text
- C#
- .NET 8
- PostgreSQL
- Npgsql
- SQL
- Visual Studio
- Git / GitHub
```

---

## Architecture

L’application est organisée en couches :

```text
RefugeAnimaux/
├── Program.cs
├── classesMetier/
├── coucheAccesBD/
└── couchePresentation/
```

### Couche métier

Le dossier `classesMetier` contient les classes principales du domaine :

```text
Animal
Contact
Adoption
FamilleAccueil
Vaccin
Vaccination
Couleur
Role
Compatibilite
AniEntree
AniSortie
```

Il contient aussi les classes de validation :

```text
ValidationDates
ValidationExistence
ValidationMetier
```

### Couche accès aux données

Le dossier `coucheAccesBD` contient l’accès à PostgreSQL avec Npgsql.

La classe `AccesBD.cs` contient les opérations SQL.  
La classe `ConfigurationConnexion.cs` centralise la chaîne de connexion.

### Couche présentation

Le dossier `couchePresentation` contient la classe `Presentation.cs`, qui gère les menus, les saisies, les affichages et les messages d’erreur.

---

## Configuration PostgreSQL

La base utilisée s’appelle :

```text
refuge_animaux
```

Par défaut, l’application utilise :

```text
Host=localhost
Database=refuge_animaux
Username=postgres
Password=Jesus2001
```

La connexion peut être personnalisée avec une variable d’environnement :

```text
REFUGE_ANIMAUX_DB
```

Exemple :

```text
Host=localhost;Database=refuge_animaux;Username=postgres;Password=mon_mot_de_passe
```

Ou avec les variables séparées :

```text
REFUGE_DB_HOST
REFUGE_DB_NAME
REFUGE_DB_USER
REFUGE_DB_PASSWORD
```

---

## Fonctionnalités principales

### Animaux

```text
- ajouter un animal avec son entrée au refuge dans une transaction
- consulter les animaux
- rechercher un animal par identifiant ou par nom
- afficher une fiche animal détaillée
- supprimer un animal si les règles le permettent
```

### Contacts

```text
- ajouter un contact
- modifier un contact
- consulter les contacts
- consulter les données liées à un contact
- supprimer un contact si les règles le permettent
```

### Refuge

```text
- gérer les entrées
- gérer les sorties
- afficher les animaux présents au refuge
```

### Adoptions et familles d’accueil

```text
- ajouter une adoption
- modifier le statut d’une adoption
- placer un animal en famille d’accueil
- clôturer une famille d’accueil
```

### Vaccins, couleurs, rôles et compatibilités

```text
- ajouter un vaccin
- vacciner un animal
- ajouter une couleur
- associer une couleur à un animal
- ajouter un rôle
- associer un rôle à un contact
- gérer les compatibilités
```

### Diagnostic

```text
- statistiques
- cohérence du refuge
- diagnostic des anomalies possibles
```

---

## Améliorations apportées

Cette version corrige plusieurs points techniques :

```text
- connexion PostgreSQL centralisée dans ConfigurationConnexion
- connexion recréée proprement par opération
- ajout animal + entrée dans une transaction
- état animal aligné avec la WPF avec >=
- lecture des dates rendue plus robuste
- AjouterVaccin sécurisé avec try/finally
- GetAnimauxPresentsAuRefuge plus cohérent avec l’état réel
- contrôle des tables/colonnes autorisées dans Existe()
- README spécifique Console
- .gitignore propre
```

---

## Lancement

Depuis Visual Studio :

```text
F5
```

Depuis le terminal :

```bash
dotnet run --project RefugeAnimaux
```

---

## Limites restantes

Le projet est fonctionnel et plus propre, mais certaines améliorations resteraient possibles :

```text
- séparer AccesBD.cs en plusieurs repositories
- diviser Presentation.cs en plusieurs classes
- ajouter des tests unitaires
- ajouter davantage de comportements dans les classes métier
```

Ces limites sont documentées, mais elles ne bloquent pas le fonctionnement du projet.
