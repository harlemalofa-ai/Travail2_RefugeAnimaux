using RefugeAnimaux.coucheAccesBD;
using RefugeAnimaux.classesMetier;
using Npgsql;


namespace RefugeAnimaux.couchePresentation
{
    public class Presentation
    {
        private readonly AccesBD bd;

        public Presentation()
        {
            bd = new AccesBD();
        }

        public void Menu()
        {
            AfficherTableauDeBord();
            MenuPrincipal();
        }

        private void MenuPrincipal()
        {
            int choix;

            do
            {
                Titre("REFUGE ANIMAUX");

                Console.WriteLine("1. Gestion des animaux");
                Console.WriteLine("2. Gestion des contacts");
                Console.WriteLine("3. Gestion des adoptions");
                Console.WriteLine("4. Gestion des familles d'accueil");
                Console.WriteLine("5. Gestion des vaccins");
                Console.WriteLine("6. Gestion du refuge");
                Console.WriteLine("7. Gestion des couleurs et rôles");
                Console.WriteLine("8. Statistiques");
                Console.WriteLine("9. Vérifier la cohérence du refuge");
                Console.WriteLine("10. Diagnostic du refuge");
                Console.WriteLine("0. Quitter");
                Console.WriteLine();

                choix = LireInt("Votre choix : ");

                switch (choix)
                {
                    case 1:
                        MenuAnimaux();
                        break;

                    case 2:
                        MenuContacts();
                        break;

                    case 3:
                        MenuAdoptions();
                        break;

                    case 4:
                        MenuFamillesAccueil();
                        break;

                    case 5:
                        MenuVaccins();
                        break;

                    case 6:
                        MenuRefuge();
                        break;

                    case 7:
                        MenuCouleursRoles();
                        break;

                    case 8:
                        AfficherStatistiques();
                        break;

                    case 9:
                        VerifierCoherenceRefuge();
                        break;

                    case 10:
                        DiagnosticRefuge();
                        break;

                    case 0:
                        AfficherSucces("Fermeture de l'application.");
                        break;

                    default:
                        AfficherErreur("Choix invalide.");
                        Pause();
                        break;
                }

            } while (choix != 0);
        }

        private void MenuAnimaux()
        {
            int choix;

            NettoyerEcran();

            do
            {
                Titre("GESTION DES ANIMAUX");

                Console.WriteLine("1. Afficher tous les animaux");
                Console.WriteLine("2. Ajouter un animal");
                Console.WriteLine("3. Consulter un animal");
                Console.WriteLine("4. Supprimer un animal");
                Console.WriteLine("5. Ajouter une information sur un animal");
                Console.WriteLine("6. Supprimer une information sur un animal");
                Console.WriteLine("7. Rechercher un animal par nom");
                Console.WriteLine("0. Retour");
                Console.WriteLine();

                choix = LireInt("Votre choix : ");

                switch (choix)
                {
                    case 1:
                        AfficherAnimaux();
                        break;

                    case 2:
                        AjouterAnimal();
                        break;

                    case 3:
                        ConsulterAnimal();
                        break;

                    case 4:
                        SupprimerAnimal();
                        break;

                    case 5:
                        AjouterInformationAnimal();
                        break;

                    case 6:
                        SupprimerInformationAnimal();
                        break;

                    case 7:
                        RechercherAnimalParNom();
                        break;

                    case 0:
                        break;

                    default:
                        AfficherErreur("Choix invalide.");
                        Pause();
                        break;
                }

            } while (choix != 0);
        }

        private void MenuContacts()
        {
            int choix;

            do
            {
                Titre("GESTION DES CONTACTS");

                Console.WriteLine("1. Ajouter un contact");
                Console.WriteLine("2. Consulter un contact");
                Console.WriteLine("3. Modifier un contact");
                Console.WriteLine("4. Supprimer un contact");
                Console.WriteLine("5. Rechercher un contact par nom");
                Console.WriteLine("6. Afficher tous les contacts");
                Console.WriteLine("0. Retour");
                Console.WriteLine();

                choix = LireInt("Votre choix : ");

                switch (choix)
                {
                    case 1:
                        AjouterContact();
                        break;

                    case 2:
                        ConsulterContact();
                        break;

                    case 3:
                        ModifierContact();
                        break;

                    case 4:
                        SupprimerContact();
                        break;

                    case 5:
                        RechercherContactParNom();
                        break;

                    case 6:
                        AfficherContacts();
                        break;

                    case 0:
                        break;

                    default:
                        AfficherErreur("Choix invalide.");
                        Pause();
                        break;
                }

            } while (choix != 0);
        }

        private void MenuAdoptions()
        {
            int choix;

            do
            {
                Titre("GESTION DES ADOPTIONS");

                Console.WriteLine("1. Ajouter une adoption");
                Console.WriteLine("2. Modifier le statut d'une adoption");
                Console.WriteLine("3. Lister les adoptions");
                Console.WriteLine("4. Consulter une adoption");
                Console.WriteLine("0. Retour");
                Console.WriteLine();

                choix = LireInt("Votre choix : ");

                switch (choix)
                {
                    case 1:
                        AjouterAdoption();
                        break;

                    case 2:
                        ModifierStatutAdoption();
                        break;

                    case 3:
                        ListerAdoptions();
                        break;

                    case 4:
                        ConsulterAdoption();
                        break;

                    case 0:
                        break;

                    default:
                        AfficherErreur("Choix invalide.");
                        Pause();
                        break;
                }

            } while (choix != 0);
        }

        private void MenuFamillesAccueil()
        {
            int choix;

            do
            {
                Titre("GESTION DES FAMILLES D'ACCUEIL");

                Console.WriteLine("1. Ajouter une famille d'accueil à un animal");
                Console.WriteLine("2. Lister les familles d'accueil d'un animal");
                Console.WriteLine("3. Lister les animaux accueillis par une famille");
                Console.WriteLine("4. Consulter une famille d'accueil");
                Console.WriteLine("5. Clôturer une famille d'accueil");
                Console.WriteLine("0. Retour");
                Console.WriteLine();

                choix = LireInt("Votre choix : ");

                switch (choix)
                {
                    case 1:
                        AjouterFamilleAccueil();
                        break;

                    case 2:
                        ListerFamillesAccueilAnimal();
                        break;

                    case 3:
                        ListerAnimauxParFamilleAccueil();
                        break;

                    case 4:
                        ConsulterFamilleAccueil();
                        break;

                    case 5:
                        CloturerFamilleAccueil();
                        break;

                    case 0:
                        break;

                    default:
                        AfficherErreur("Choix invalide.");
                        Pause();
                        break;
                }

            } while (choix != 0);
        }

        private void MenuVaccins()
        {
            int choix;

            do
            {
                Titre("GESTION DES VACCINS");

                Console.WriteLine("1. Ajouter un type de vaccin");
                Console.WriteLine("2. Ajouter une vaccination à un animal");
                Console.WriteLine("3. Consulter un vaccin");
                Console.WriteLine("4. Supprimer un vaccin");
                Console.WriteLine("5. Afficher tous les vaccins");
                Console.WriteLine("0. Retour");
                Console.WriteLine();

                choix = LireInt("Votre choix : ");

                switch (choix)
                {
                    case 1:
                        AjouterVaccin();
                        break;

                    case 2:
                        AjouterVaccination();
                        break;

                    case 3:
                        ConsulterVaccin();
                        break;

                    case 4:
                        SupprimerVaccin();
                        break;

                    case 5:
                        AfficherVaccins();
                        break;

                    case 0:
                        break;

                    default:
                        AfficherErreur("Choix invalide.");
                        Pause();
                        break;
                }

            } while (choix != 0);
        }

        private void MenuRefuge()
        {
            int choix;

            do
            {
                Titre("GESTION DU REFUGE");

                Console.WriteLine("1. Lister les animaux présents au refuge");
                Console.WriteLine("2. Ajouter une entrée");
                Console.WriteLine("3. Ajouter une sortie");
                Console.WriteLine("0. Retour");
                Console.WriteLine();

                choix = LireInt("Votre choix : ");

                switch (choix)
                {
                    case 1:
                        ListerAnimauxPresents();
                        break;

                    case 2:
                        AjouterEntree();
                        break;

                    case 3:
                        AjouterSortie();
                        break;

                    case 0:
                        break;

                    default:
                        AfficherErreur("Choix invalide.");
                        Pause();
                        break;
                }

            } while (choix != 0);
        }

        private void MenuCouleursRoles()
        {
            int choix;

            do
            {
                Titre("GESTION DES COULEURS ET RÔLES");

                Console.WriteLine("1. Ajouter une couleur");
                Console.WriteLine("2. Ajouter une couleur à un animal");
                Console.WriteLine("3. Ajouter un rôle");
                Console.WriteLine("4. Attribuer un rôle à un contact");
                Console.WriteLine("5. Consulter une couleur");
                Console.WriteLine("6. Consulter un rôle");
                Console.WriteLine("7. Consulter une compatibilité");
                Console.WriteLine("8. Supprimer une couleur");
                Console.WriteLine("9. Supprimer un rôle");
                Console.WriteLine("10. Supprimer une compatibilité");
                Console.WriteLine("11. Afficher toutes les couleurs");
                Console.WriteLine("12. Afficher tous les rôles");
                Console.WriteLine("13. Afficher toutes les compatibilités");
                Console.WriteLine("0. Retour");
                Console.WriteLine();

                choix = LireInt("Votre choix : ");

                switch (choix)
                {
                    case 1:
                        AjouterCouleur();
                        break;

                    case 2:
                        AjouterCouleurAnimal();
                        break;

                    case 3:
                        AjouterRole();
                        break;

                    case 4:
                        AjouterRoleContact();
                        break;

                    case 5:
                        ConsulterCouleur();
                        break;

                    case 6:
                        ConsulterRole();
                        break;

                    case 7:
                        ConsulterCompatibilite();
                        break;

                    case 8:
                        SupprimerCouleur();
                        break;

                    case 9:
                        SupprimerRole();
                        break;

                    case 10:
                        SupprimerCompatibilite();
                        break;

                    case 11:
                        AfficherCouleurs();
                        break;

                    case 12:
                        AfficherRoles();
                        break;

                    case 13:
                        AfficherCompatibilites();
                        break;

                    case 0:
                        break;

                    default:
                        AfficherErreur("Choix invalide.");
                        Pause();
                        break;
                }

            } while (choix != 0);
        }
        private void Titre(string titre)
        {
            NettoyerEcran();
            Console.WriteLine("========================================");
            Console.WriteLine(titre);
            Console.WriteLine("========================================");
            Console.WriteLine();
        }

        private void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Appuyez sur une touche...");
            Console.ReadKey(true);

            NettoyerEcran();
        }

        private void NettoyerEcran()
        {
            try
            {
                // Nettoyage classique
                Console.Clear();

                // Nettoyage plus fort pour Windows Terminal :
                // 2J = efface l'écran
                // 3J = efface l'historique du terminal
                // H  = remet le curseur en haut à gauche
                Console.Write("\u001b[2J\u001b[3J\u001b[H");
                Console.Out.Flush();

                Console.SetCursorPosition(0, 0);
            }
            catch
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine(new string('-', 80));
                    Console.WriteLine();
                }
                catch
                {
                    // On ne bloque jamais l'application pour un problème d'affichage.
                }
            }
        }

        private void AfficherSucces(string message)
        {
            Console.WriteLine();
            Console.WriteLine("[OK] " + message);
        }

        private void AfficherErreur(string message)
        {
            Console.WriteLine();
            Console.WriteLine("[ERREUR] " + message);
        }

        private void AfficherErreur(Exception ex)
        {
            Console.WriteLine();

            if (ex is PostgresException pgEx)
            {
                switch (pgEx.SqlState)
                {
                    case "23505":
                        Console.WriteLine("[ERREUR] Cette donnée existe déjà.");
                        break;

                    case "23503":
                        Console.WriteLine("[ERREUR] Impossible : une donnée liée est manquante ou encore utilisée ailleurs.");
                        break;

                    case "23514":
                        Console.WriteLine("[ERREUR] Une valeur ne respecte pas les règles autorisées.");
                        break;

                    case "23502":
                        Console.WriteLine("[ERREUR] Une valeur obligatoire est manquante.");
                        break;

                    default:
                        Console.WriteLine("[ERREUR PostgreSQL] " + pgEx.MessageText);
                        break;
                }
            }
            else
            {
                Console.WriteLine("[ERREUR] " + ex.Message);
            }
        }

        private int LireInt(string message)
        {
            int valeur;

            while (true)
            {
                Console.Write(message);
                string? saisie = Console.ReadLine();

                if (int.TryParse(saisie, out valeur))
                {
                    return valeur;
                }

                AfficherErreur("Veuillez entrer un nombre valide.");
            }
        }

        private DateTime LireDate(string message)
        {
            DateTime date;

            while (true)
            {
                Console.Write(message);
                string? saisie = Console.ReadLine();

                if (DateTime.TryParse(saisie, out date))
                {
                    return date;
                }

                AfficherErreur("Date invalide. Format conseillé : yyyy-mm-dd.");
            }
        }

        private string LireTexteObligatoire(string message)
        {
            string? texte;

            do
            {
                Console.Write(message);
                texte = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(texte))
                {
                    AfficherErreur("Cette valeur est obligatoire.");
                }

            } while (string.IsNullOrWhiteSpace(texte));

            return texte.Trim();
        }

        private string? LireTexteOptionnel(string message)
        {
            Console.Write(message);
            string? texte = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(texte))
            {
                return null;
            }

            return texte.Trim();
        }

        private bool LireOuiNon(string message)
        {
            while (true)
            {
                Console.Write(message + " (o/n) : ");
                string? saisie = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(saisie))
                {
                    AfficherErreur("Veuillez répondre par o ou n.");
                    continue;
                }

                saisie = saisie.Trim().ToLower();

                if (saisie == "o" || saisie == "oui")
                {
                    return true;
                }

                if (saisie == "n" || saisie == "non")
                {
                    return false;
                }

                AfficherErreur("Veuillez répondre par o ou n.");
            }
        }

        private string LireChoixTexte(string message, List<string> valeursAutorisees)
        {
            while (true)
            {
                Console.Write(message);
                string valeur = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (valeursAutorisees.Contains(valeur))
                {
                    return valeur;
                }

                AfficherErreur("Valeur invalide.");
                Console.WriteLine("Valeurs autorisées : " + string.Join(", ", valeursAutorisees));
            }
        }

        private void AfficherAnimaux()
        {
            try
            {
                Titre("LISTE DES ANIMAUX");

                List<Animal> animaux = bd.GetAnimaux();

                if (animaux.Count == 0)
                {
                    Console.WriteLine("Aucun animal enregistré.");
                }
                else
                {
                    foreach (Animal animal in animaux)
                    {
                        Console.WriteLine($"{animal.Identifiant} - {animal.Nom} - {animal.Type} - {animal.Sexe}");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AjouterAnimal()
        {
            try
            {
                Titre("AJOUT D'UN ANIMAL");

                string identifiant = LireTexteObligatoire("Identifiant animal : ");
                string nom = LireTexteObligatoire("Nom : ");

                string type = LireChoixTexte(
                    "Type (chat/chien) : ",
                    new List<string> { "chat", "chien" }
                );

                string sexe = LireChoixTexte(
                    "Sexe (M/F) : ",
                    new List<string> { "m", "f" }
                ).ToUpper();

                string? particularites = LireTexteOptionnel("Particularités : ");
                string? description = LireTexteOptionnel("Description : ");

                bool sterilise = LireOuiNon("L'animal est-il stérilisé ?");

                DateTime? dateSterilisation = null;

                if (sterilise)
                {
                    bool dateConnue = LireOuiNon("La date de stérilisation est-elle connue ?");

                    if (dateConnue)
                    {
                        dateSterilisation = LireDate("Date de stérilisation (yyyy-mm-dd) : ");
                    }
                }

                DateTime dateNaissance = LireDate("Date de naissance (yyyy-mm-dd) : ");
                if (ValidationDates.DateFuture(dateNaissance))
                {
                    AfficherErreur("La date de naissance ne peut pas être dans le futur.");
                    Pause();
                    return;
                }

                Animal animal = new Animal(
                    identifiant,
                    nom,
                    type,
                    sexe,
                    particularites,
                    null,
                    description,
                    dateSterilisation,
                    sterilise,
                    dateNaissance
                );

                Console.WriteLine();
                Console.WriteLine("Création de l'entrée au refuge");
                Console.WriteLine();

                Console.WriteLine("Raisons possibles :");
                Console.WriteLine("- abandon");
                Console.WriteLine("- errant");
                Console.WriteLine("- deces_proprietaire");
                Console.WriteLine("- saisie");
                Console.WriteLine("- retour_adoption");
                Console.WriteLine();

                string raison = LireChoixTexte(
                    "Raison d'entrée : ",
                    new List<string>
                    {
                        "abandon",
                        "errant",
                        "deces_proprietaire",
                        "saisie",
                        "retour_adoption"
                    }
                );

                int contact = LireInt("Identifiant du contact responsable de l'entrée : ");

                DateTime dateEntree;

                bool entreeAujourdhui = LireOuiNon("L'entrée a-t-elle lieu aujourd'hui ?");

                if (entreeAujourdhui)
                {
                    dateEntree = DateTime.Today;
                }
                else
                {
                    dateEntree = LireDate("Date d'entrée (yyyy-mm-dd) : ");
                }

                AniEntree entree = new AniEntree(
                    identifiant,
                    dateEntree,
                    raison,
                    contact
                );

                bd.AjouterAnimalAvecEntree(animal, entree);

                Console.WriteLine();
                AfficherSucces("Animal ajouté avec son entrée au refuge dans une transaction.");
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
                Console.WriteLine();
                Console.WriteLine("L'ajout de l'animal et de son entrée est transactionnel :");
                Console.WriteLine("si l'entrée échoue, l'animal n'est pas enregistré seul.");
            }

            Pause();
        }

        private void ConsulterAnimal()
        {
            try
            {
                Titre("CONSULTER UN ANIMAL");

                string identifiant = LireTexteObligatoire("Identifiant animal : ");

                Animal? animal = bd.GetAnimalParIdentifiant(identifiant);

                if (animal == null)
                {
                    Titre("CONSULTER UN ANIMAL");
                    AfficherErreur("Aucun animal trouvé avec cet identifiant.");
                    Pause();
                    return;
                }

                Titre("FICHE ANIMAL - INFORMATIONS PRINCIPALES");
                AfficherResumeAnimal(animal);
                Console.WriteLine("----------------------------------------");
                Console.WriteLine();

                string etatActuel = bd.GetEtatActuelAnimal(animal.Identifiant);

                Console.WriteLine($"État actuel       : {FormaterEtatAnimal(etatActuel)}");
                Console.WriteLine();

                Console.WriteLine($"Identifiant       : {animal.Identifiant}");
                Console.WriteLine($"Nom               : {animal.Nom}");
                Console.WriteLine($"Type              : {animal.Type}");
                Console.WriteLine($"Sexe              : {animal.Sexe}");
                Console.WriteLine($"Date naissance    : {animal.DateNaissance:yyyy-MM-dd}");
                Console.WriteLine($"Stérilisé         : {(animal.Sterilise ? "Oui" : "Non")}");

                if (animal.DateSterilisation != null)
                {
                    Console.WriteLine($"Date stérilisation: {animal.DateSterilisation:yyyy-MM-dd}");
                }

                if (animal.DateDeces != null)
                {
                    Console.WriteLine($"Date décès        : {animal.DateDeces:yyyy-MM-dd}");
                }

                Console.WriteLine();
                Console.WriteLine("DESCRIPTION");
                Console.WriteLine(string.IsNullOrWhiteSpace(animal.Description) ? "- Aucune description." : "- " + animal.Description);

                Console.WriteLine();
                Console.WriteLine("PARTICULARITÉS");
                Console.WriteLine(string.IsNullOrWhiteSpace(animal.Particularites) ? "- Aucune particularité." : "- " + animal.Particularites);

                Pause();

                Titre("FICHE ANIMAL - INFORMATIONS COMPLÉMENTAIRES");

                Console.WriteLine($"Animal : {animal.Identifiant} - {animal.Nom}");

                AfficherListe("Couleurs", bd.GetCouleursAnimal(animal.Identifiant));
                AfficherListe("Compatibilités", bd.GetCompatibilitesAnimal(animal.Identifiant));
                AfficherListe("Vaccinations", bd.GetVaccinationsAnimal(animal.Identifiant));
                AfficherListe("Entrées", bd.GetEntreesAnimal(animal.Identifiant));
                AfficherListe("Sorties", bd.GetSortiesAnimal(animal.Identifiant));
                AfficherListe("Adoptions", bd.GetAdoptionsAnimal(animal.Identifiant));
                AfficherListeFamillesAccueil(animal.Identifiant);
                AfficherListe("Historique chronologique", bd.GetHistoriqueAnimal(animal.Identifiant));
            }
            catch (Exception ex)
            {
                Titre("ERREUR");
                AfficherErreur(ex);
            }

            Pause();
        }

        private static void AfficherListe(string titre, List<string> elements)
        {
            Console.WriteLine();
            Console.WriteLine(titre.ToUpper());

            if (elements.Count == 0)
            {
                Console.WriteLine("- Aucune information.");
                return;
            }

            foreach (string element in elements)
            {
                Console.WriteLine("- " + element);
            }
        }

        private bool AnimalEstBloque(string identifiantAnimal)
        {
            string etat = bd.GetEtatActuelAnimal(identifiantAnimal);

            if (etat == "decede")
            {
                AfficherErreur("Action impossible : cet animal est décédé.");
                return true;
            }

            if (etat == "adopte")
            {
                AfficherErreur("Action impossible : cet animal est déjà adopté.");
                return true;
            }

            if (etat == "retour_proprietaire")
            {
                AfficherErreur("Action impossible : cet animal est retourné chez son propriétaire.");
                return true;
            }

            return false;
        }

        private bool AnimalPeutAllerEnFamilleAccueil(string identifiantAnimal)
        {
            string etat = bd.GetEtatActuelAnimal(identifiantAnimal);

            if (etat != "au_refuge")
            {
                AfficherErreur("Action impossible : l'animal doit être au refuge pour partir en famille d'accueil.");
                Console.WriteLine("État actuel : " + FormaterEtatAnimal(etat));
                return false;
            }

            return true;
        }

        private bool AnimalPeutAvoirAdoption(string identifiantAnimal)
        {
            string etat = bd.GetEtatActuelAnimal(identifiantAnimal);

            if (etat == "decede")
            {
                AfficherErreur("Action impossible : un animal décédé ne peut pas être adopté.");
                return false;
            }

            if (etat == "adopte")
            {
                AfficherErreur("Action impossible : cet animal est déjà adopté.");
                return false;
            }

            if (etat == "retour_proprietaire")
            {
                AfficherErreur("Action impossible : cet animal est retourné chez son propriétaire.");
                return false;
            }

            return true;
        }

        private bool AnimalPeutAvoirSortie(string identifiantAnimal)
        {
            string etat = bd.GetEtatActuelAnimal(identifiantAnimal);

            if (etat == "decede")
            {
                AfficherErreur("Action impossible : cet animal est déjà décédé.");
                return false;
            }

            if (etat == "adopte")
            {
                AfficherErreur("Action impossible : cet animal est déjà adopté.");
                return false;
            }

            if (etat == "retour_proprietaire")
            {
                AfficherErreur("Action impossible : cet animal est déjà retourné chez son propriétaire.");
                return false;
            }

            return true;
        }

        private bool AnimalPeutEtreVaccine(string identifiantAnimal)
        {
            string etat = bd.GetEtatActuelAnimal(identifiantAnimal);

            if (etat == "decede")
            {
                AfficherErreur("Action impossible : un animal décédé ne peut pas recevoir de vaccin.");
                return false;
            }

            return true;
        }

        private static string FormaterEtatAnimal(string etat)
        {
            return etat switch
            {
                "au_refuge" => "AU REFUGE",
                "adopte" => "ADOPTÉ",
                "famille_accueil" => "EN FAMILLE D'ACCUEIL",
                "retour_proprietaire" => "RETOURNÉ AU PROPRIÉTAIRE",
                "decede" => "DÉCÉDÉ",
                "sans_entree" => "SANS ENTRÉE ENREGISTRÉE",
                _ => "INCONNU"
            };
        }
        private void AfficherListeFamillesAccueil(string identifiantAnimal)
        {
            Console.WriteLine();
            Console.WriteLine("FAMILLES D'ACCUEIL");

            List<FamilleAccueil> familles = bd.GetFamillesAccueilParAnimal(identifiantAnimal);

            if (familles.Count == 0)
            {
                Console.WriteLine("- Aucune information.");
                return;
            }

            foreach (FamilleAccueil famille in familles)
            {
                string dateFin = famille.DateFin == null
                    ? "en cours"
                    : famille.DateFin.Value.ToString("yyyy-MM-dd");

                Console.WriteLine(
                    "- Contact : "
                    + famille.FaContact
                    + " | Début : "
                    + famille.DateDebut.ToString("yyyy-MM-dd")
                    + " | Fin : "
                    + dateFin
                );
            }
        }

        private void SupprimerAnimal()
        {
            try
            {
                Titre("SUPPRIMER UN ANIMAL");

                string identifiant = LireTexteObligatoire("Identifiant animal : ");

                if (!ValidationExistence.AnimalExiste(bd, identifiant))
                {
                    AfficherErreur("Animal introuvable.");
                    Pause();
                    return;
                }

                List<string> blocages = bd.GetBlocagesSuppressionAnimal(identifiant);

                if (blocages.Count > 0)
                {
                    AfficherErreur("Impossible de supprimer cet animal car il est encore lié à des données.");

                    Console.WriteLine();
                    Console.WriteLine("Données liées :");

                    foreach (string blocage in blocages)
                    {
                        Console.WriteLine("- " + blocage);
                    }

                    Pause();
                    return;
                }

                bool confirmation = LireOuiNon("Voulez-vous vraiment supprimer cet animal ?");

                if (!confirmation)
                {
                    AfficherErreur("Suppression annulée.");
                    Pause();
                    return;
                }

                bool supprime = bd.SupprimerAnimal(identifiant);

                if (supprime)
                {
                    AfficherSucces("Animal supprimé.");
                }
                else
                {
                    AfficherErreur("Animal introuvable.");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AjouterInformationAnimal()
        {
            try
            {
                Titre("AJOUTER UNE INFORMATION SUR UN ANIMAL");

                string identifiant = LireTexteObligatoire("Identifiant animal : ");

                Console.WriteLine("1. Modifier la description");
                Console.WriteLine("2. Modifier les particularités");
                Console.WriteLine("3. Ajouter une compatibilité");
                Console.WriteLine();

                int choix = LireInt("Votre choix : ");

                switch (choix)
                {
                    case 1:
                        ModifierDescriptionAnimal(identifiant);
                        break;

                    case 2:
                        ModifierParticularitesAnimal(identifiant);
                        break;

                    case 3:
                        AjouterCompatibiliteAnimal(identifiant);
                        break;

                    default:
                        AfficherErreur("Choix invalide.");
                        break;
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void ModifierDescriptionAnimal(string identifiant)
        {
            string description = LireTexteObligatoire("Nouvelle description : ");

            bool modifie = bd.ModifierDescriptionAnimal(identifiant, description);

            if (modifie)
            {
                AfficherSucces("Description modifiée.");
            }
            else
            {
                AfficherErreur("Animal introuvable.");
            }
        }

        private void ModifierParticularitesAnimal(string identifiant)
        {
            string particularites = LireTexteObligatoire("Nouvelles particularités : ");

            bool modifie = bd.ModifierParticularitesAnimal(identifiant, particularites);

            if (modifie)
            {
                AfficherSucces("Particularités modifiées.");
            }
            else
            {
                AfficherErreur("Animal introuvable.");
            }
        }

        private void AjouterCompatibiliteAnimal(string identifiant)
        {
            if (!ValidationExistence.AnimalExiste(bd, identifiant))
            {
                AfficherErreur("Animal introuvable.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Compatibilités disponibles :");

            List<string> compatibilites = bd.GetCompatibilitesDisponibles();

            if (compatibilites.Count == 0)
            {
                AfficherErreur("Aucune compatibilité disponible dans la base de données.");
                return;
            }

            foreach (string compatibilite in compatibilites)
            {
                Console.WriteLine("- " + compatibilite);
            }

            Console.WriteLine();

            int compId = LireInt("Identifiant compatibilité : ");

            if (!ValidationExistence.CompatibiliteExiste(bd, compId))
            {
                AfficherErreur("Compatibilité introuvable.");
                return;
            }

            if (bd.AnimalPossedeCompatibilite(identifiant, compId))
            {
                AfficherErreur("Cette compatibilité est déjà enregistrée pour cet animal.");
                return;
            }

            string valeur = LireChoixTexte(
                "Valeur (oui/non/non_teste) : ",
                new List<string> { "oui", "non", "non_teste" }
            );

            string? description = LireTexteOptionnel("Description éventuelle : ");

            bd.AjouterCompatibiliteAnimal(
                identifiant,
                compId,
                valeur,
                description
            );

            AfficherSucces("Compatibilité ajoutée.");
        }

        private void SupprimerInformationAnimal()
        {
            try
            {
                Titre("SUPPRIMER UNE INFORMATION SUR UN ANIMAL");

                string identifiant = LireTexteObligatoire("Identifiant animal : ");

                Console.WriteLine("1. Supprimer la description");
                Console.WriteLine("2. Supprimer les particularités");
                Console.WriteLine("3. Supprimer une compatibilité");
                Console.WriteLine();

                int choix = LireInt("Votre choix : ");

                switch (choix)
                {
                    case 1:
                        if (bd.SupprimerDescriptionAnimal(identifiant))
                        {
                            AfficherSucces("Description supprimée.");
                        }
                        else
                        {
                            AfficherErreur("Animal introuvable.");
                        }
                        break;

                    case 2:
                        if (bd.SupprimerParticularitesAnimal(identifiant))
                        {
                            AfficherSucces("Particularités supprimées.");
                        }
                        else
                        {
                            AfficherErreur("Animal introuvable.");
                        }
                        break;

                    case 3:
                        int compId = LireInt("Identifiant compatibilité : ");

                        if (bd.SupprimerCompatibiliteAnimal(identifiant, compId))
                        {
                            AfficherSucces("Compatibilité supprimée.");
                        }
                        else
                        {
                            AfficherErreur("Compatibilité introuvable.");
                        }
                        break;

                    default:
                        AfficherErreur("Choix invalide.");
                        break;
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AjouterContact()
        {
            try
            {
                Titre("AJOUT D'UN CONTACT");

                int id = LireInt("Identifiant contact : ");

                string nom = LireTexteObligatoire("Nom : ");
                string prenom = LireTexteObligatoire("Prénom : ");
                string rue = LireTexteObligatoire("Rue : ");
                string cp = LireTexteObligatoire("Code postal : ");
                string localite = LireTexteObligatoire("Localité : ");
                string registre = LireTexteObligatoire("Registre national : ");

                string? gsm = LireTexteOptionnel("GSM : ");
                string? telephone = LireTexteOptionnel("Téléphone : ");
                string? email = LireTexteOptionnel("Email : ");

                if (gsm == null && telephone == null && email == null)
                {
                    AfficherErreur("Un contact doit avoir au moins un moyen de contact : GSM, téléphone ou email.");
                    Pause();
                    return;
                }

                Contact contact = new Contact(
                    id,
                    nom,
                    prenom,
                    rue,
                    cp,
                    localite,
                    registre,
                    gsm,
                    telephone,
                    email
                );

                bd.AjouterContact(contact);

                AfficherSucces("Contact ajouté avec succès.");
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void ConsulterContact()
        {
            try
            {
                Titre("CONSULTER UN CONTACT");

                int id = LireInt("Identifiant contact : ");

                if (!ValidationExistence.ContactExiste(bd, id))
                {
                    AfficherErreur("Contact introuvable.");
                    Pause();
                    return;
                }

                Contact? contact = bd.GetContactParIdentifiant(id);

                if (contact == null)
                {
                    AfficherErreur("Contact introuvable.");
                    Pause();
                    return;
                }

                Titre("FICHE CONTACT - INFORMATIONS PRINCIPALES");

                AfficherResumeContact(contact);
                Console.WriteLine("----------------------------------------");
                Console.WriteLine();

                Console.WriteLine("INFORMATIONS GÉNÉRALES");
                Console.WriteLine($"Identifiant       : {contact.ContactIdentifiant}");
                Console.WriteLine($"Nom               : {contact.Nom}");
                Console.WriteLine($"Prénom            : {contact.Prenom}");
                Console.WriteLine($"Registre national : {contact.RegistreNational}");

                Console.WriteLine();
                Console.WriteLine("ADRESSE");
                Console.WriteLine($"{contact.Rue}, {contact.Cp} {contact.Localite}");

                Console.WriteLine();
                Console.WriteLine("MOYENS DE CONTACT");
                Console.WriteLine($"GSM       : {(string.IsNullOrWhiteSpace(contact.Gsm) ? "Non renseigné" : contact.Gsm)}");
                Console.WriteLine($"Téléphone : {(string.IsNullOrWhiteSpace(contact.Telephone) ? "Non renseigné" : contact.Telephone)}");
                Console.WriteLine($"Email     : {(string.IsNullOrWhiteSpace(contact.Email) ? "Non renseigné" : contact.Email)}");

                Pause();

                Titre("FICHE CONTACT - INFORMATIONS LIÉES");

                Console.WriteLine($"Contact : {contact.ContactIdentifiant} - {contact.Nom} {contact.Prenom}");

                AfficherListe("Rôles", bd.GetRolesContact(id));
                AfficherListe("Adoptions liées", bd.GetAdoptionsContact(id));
                AfficherListe("Familles d'accueil liées", bd.GetFamillesAccueilContact(id));
            }
            catch (Exception ex)
            {
                Titre("ERREUR");
                AfficherErreur(ex);
            }

            Pause();
        }

        private void ModifierContact()
        {
            try
            {
                Titre("MODIFIER UN CONTACT");

                int id = LireInt("Identifiant contact : ");

                Contact? ancienContact = bd.GetContactParIdentifiant(id);

                if (ancienContact == null)
                {
                    AfficherErreur("Contact introuvable.");
                    Pause();
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("Laissez vide pour conserver l'ancienne valeur.");
                Console.WriteLine();

                Console.WriteLine($"Ancien nom : {ancienContact.Nom}");
                string? nomSaisi = LireTexteOptionnel("Nouveau nom : ");
                string nom = nomSaisi ?? ancienContact.Nom;

                Console.WriteLine($"Ancien prénom : {ancienContact.Prenom}");
                string? prenomSaisi = LireTexteOptionnel("Nouveau prénom : ");
                string prenom = prenomSaisi ?? ancienContact.Prenom;

                Console.WriteLine($"Ancienne rue : {ancienContact.Rue}");
                string? rueSaisie = LireTexteOptionnel("Nouvelle rue : ");
                string rue = rueSaisie ?? ancienContact.Rue;

                Console.WriteLine($"Ancien code postal : {ancienContact.Cp}");
                string? cpSaisi = LireTexteOptionnel("Nouveau code postal : ");
                string cp = cpSaisi ?? ancienContact.Cp;

                Console.WriteLine($"Ancienne localité : {ancienContact.Localite}");
                string? localiteSaisie = LireTexteOptionnel("Nouvelle localité : ");
                string localite = localiteSaisie ?? ancienContact.Localite;

                Console.WriteLine($"Ancien registre national : {ancienContact.RegistreNational}");
                string? registreSaisi = LireTexteOptionnel("Nouveau registre national : ");
                string registre = registreSaisi ?? ancienContact.RegistreNational;

                Console.WriteLine($"Ancien GSM : {ancienContact.Gsm}");
                string? gsm = LireTexteOptionnel("Nouveau GSM : ") ?? ancienContact.Gsm;

                Console.WriteLine($"Ancien téléphone : {ancienContact.Telephone}");
                string? telephone = LireTexteOptionnel("Nouveau téléphone : ") ?? ancienContact.Telephone;

                Console.WriteLine($"Ancien email : {ancienContact.Email}");
                string? email = LireTexteOptionnel("Nouvel email : ") ?? ancienContact.Email;

                if (gsm == null && telephone == null && email == null)
                {
                    AfficherErreur("Un contact doit garder au moins un moyen de contact.");
                    Pause();
                    return;
                }

                Contact nouveauContact = new Contact(
                    id,
                    nom,
                    prenom,
                    rue,
                    cp,
                    localite,
                    registre,
                    gsm,
                    telephone,
                    email
                );

                bool modifie = bd.ModifierContact(nouveauContact);

                if (modifie)
                {
                    AfficherSucces("Contact modifié.");
                }
                else
                {
                    AfficherErreur("Modification impossible.");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void SupprimerContact()
        {
            try
            {
                Titre("SUPPRIMER UN CONTACT");

                int id = LireInt("Identifiant contact : ");

                if (!ValidationExistence.ContactExiste(bd, id))
                {
                    AfficherErreur("Contact introuvable.");
                    Pause();
                    return;
                }

                List<string> blocages = bd.GetBlocagesSuppressionContact(id);

                if (blocages.Count > 0)
                {
                    AfficherErreur("Impossible de supprimer ce contact car il est encore lié à des données.");

                    Console.WriteLine();
                    Console.WriteLine("Données liées :");

                    foreach (string blocage in blocages)
                    {
                        Console.WriteLine("- " + blocage);
                    }

                    Pause();
                    return;
                }

                bool confirmation = LireOuiNon("Voulez-vous vraiment supprimer ce contact ?");

                if (!confirmation)
                {
                    AfficherErreur("Suppression annulée.");
                    Pause();
                    return;
                }

                bool supprime = bd.SupprimerContact(id);

                if (supprime)
                {
                    AfficherSucces("Contact supprimé.");
                }
                else
                {
                    AfficherErreur("Contact introuvable.");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }
        private void AjouterVaccin()
        {
            try
            {
                Titre("AJOUT D'UN TYPE DE VACCIN");

                int id = LireInt("Identifiant vaccin : ");
                string nom = LireTexteObligatoire("Nom du vaccin : ");

                Vaccin vaccin = new Vaccin(id, nom);

                bd.AjouterVaccin(vaccin);

                AfficherSucces("Type de vaccin ajouté.");
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AjouterVaccination()
        {
            try
            {
                Titre("AJOUT D'UNE VACCINATION");

                string animal = LireTexteObligatoire("Identifiant animal : ");

                if (!ValidationExistence.AnimalExiste(bd, animal))
                {
                    AfficherErreur("Animal introuvable.");
                    Pause();
                    return;
                }

                string etat = bd.GetEtatActuelAnimal(animal);

                if (!ValidationMetier.PeutAjouterVaccination(etat))
                {
                    AfficherErreur(ValidationMetier.MessageActionImpossible(etat, "vaccination"));
                    Pause();
                    return;
                }

                int vaccin = LireInt("Identifiant vaccin : ");

                if (!ValidationExistence.VaccinExiste(bd, vaccin))
                {
                    AfficherErreur("Vaccin introuvable.");
                    Pause();
                    return;
                }

                DateTime dateVaccination;

                bool dateAujourdhui = LireOuiNon("La vaccination a-t-elle été faite aujourd'hui ?");

                if (dateAujourdhui)
                {
                    dateVaccination = DateTime.Today;
                }
                else
                {
                    dateVaccination = LireDate("Date de vaccination (yyyy-mm-dd) : ");
                }

                if (bd.VaccinationExiste(animal, vaccin, dateVaccination))
                {
                    AfficherErreur("Cette vaccination existe déjà pour cet animal à cette date.");
                    Pause();
                    return;
                }
                if (ValidationDates.DateFuture(dateVaccination))
                {
                    AfficherErreur("La date de vaccination ne peut pas être dans le futur.");
                    Pause();
                    return;
                }

                Vaccination vaccination = new Vaccination(
                    animal,
                    vaccin,
                    dateVaccination
                );

                bd.AjouterVaccination(vaccination);

                AfficherSucces("Vaccination enregistrée.");
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AjouterAdoption()
        {
            try
            {
                Titre("AJOUT D'UNE ADOPTION");

                string animal = LireTexteObligatoire("Identifiant animal : ");
                if (!ValidationExistence.AnimalExiste(bd, animal))
                {
                    AfficherErreur("Animal introuvable.");
                    Pause();
                    return;
                }
                string etat = bd.GetEtatActuelAnimal(animal);

                if (!ValidationMetier.PeutAjouterAdoption(etat))
                {
                    AfficherErreur(ValidationMetier.MessageActionImpossible(etat, "adoption"));
                    Pause();
                    return;
                }
                if (bd.AnimalAAdoptionAcceptee(animal))
                {
                    AfficherErreur("Cet animal possède déjà une adoption acceptée.");
                    Pause();
                    return;
                }
                int contact = LireInt("Identifiant contact adoptant/candidat : ");
                if (!ValidationExistence.ContactExiste(bd, contact))
                {
                    AfficherErreur("Contact introuvable.");
                    Pause();
                    return;
                }

                Adoption adoption = new Adoption(
                    animal,
                    contact,
                    DateTime.Today,
                    "demande"
                );

                bd.AjouterAdoption(adoption);

                AfficherSucces("Demande d'adoption créée.");
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void ModifierStatutAdoption()
        {
            try
            {
                Titre("MODIFIER LE STATUT D'UNE ADOPTION");

                string animal = LireTexteObligatoire("Identifiant animal : ");
                if (!ValidationExistence.AnimalExiste(bd, animal))
                {
                    AfficherErreur("Animal introuvable.");
                    Pause();
                    return;
                }
                string etat = bd.GetEtatActuelAnimal(animal);

                if (!ValidationMetier.PeutAjouterAdoption(etat) && etat != "au_refuge")
                {
                    AfficherErreur(ValidationMetier.MessageActionImpossible(etat, "modification adoption"));
                    Pause();
                    return;
                }
                int contact = LireInt("Identifiant contact : ");
                if (!ValidationExistence.ContactExiste(bd, contact))
                {
                    AfficherErreur("Contact introuvable.");
                    Pause();
                    return;
                }
                DateTime date = LireDate("Date de la demande (yyyy-mm-dd) : ");

                Console.WriteLine();
                Console.WriteLine("Statuts possibles :");
                Console.WriteLine("- demande");
                Console.WriteLine("- acceptee");
                Console.WriteLine("- rejet_comportement");
                Console.WriteLine("- rejet_environnement");
                Console.WriteLine();

                string statut = LireChoixTexte(
                    "Nouveau statut : ",
                    new List<string>
                    {
                        "demande",
                        "acceptee",
                        "rejet_comportement",
                        "rejet_environnement"
                    }
                );
                if (statut == "acceptee" && bd.AnimalAAdoptionAcceptee(animal))
                {
                    AfficherErreur("Cet animal possède déjà une adoption acceptée.");
                    Pause();
                    return;
                }

                bool modifie = bd.ModifierStatutAdoption(
                    animal,
                    contact,
                    date,
                    statut
                );

                if (!modifie)
                {
                    AfficherErreur("Adoption introuvable.");
                    Pause();
                    return;
                }

                AfficherSucces("Statut d'adoption modifié.");

                if (statut == "acceptee")
                {
                    Console.WriteLine();
                    Console.WriteLine("Création automatique de la sortie du refuge.");

                    DateTime dateSortie;

                    bool sortieAujourdhui = LireOuiNon("La sortie a-t-elle lieu aujourd'hui ?");

                    if (sortieAujourdhui)
                    {
                        dateSortie = DateTime.Today;
                    }
                    else
                    {
                        dateSortie = LireDate("Date de sortie (yyyy-mm-dd) : ");
                    }

                    if (bd.ExisteSortieAnimalDate(animal, dateSortie))
                    {
                        AfficherErreur("Une sortie existe déjà pour cet animal à cette date.");
                        Console.WriteLine("Le statut est modifié, mais aucune nouvelle sortie n'a été créée.");
                    }
                    else
                    {
                        AniSortie sortie = new AniSortie(
                            animal,
                            dateSortie,
                            "adoption",
                            contact
                        );

                        bd.AjouterSortie(sortie);

                        AfficherSucces("Sortie pour adoption enregistrée automatiquement.");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
                Console.WriteLine();
                Console.WriteLine("Remarque : si la sortie existe déjà pour cette date, PostgreSQL peut refuser le doublon.");
            }

            Pause();
        }

        private void ListerAdoptions()
        {
            try
            {
                Titre("LISTE DES ADOPTIONS");

                List<Adoption> adoptions = bd.GetAdoptions();

                if (adoptions.Count == 0)
                {
                    Console.WriteLine("Aucune adoption enregistrée.");
                }
                else
                {
                    foreach (Adoption adoption in adoptions)
                    {
                        Console.WriteLine(
                            $"Animal : {adoption.AniIdentifiant} | Contact : {adoption.AdopContact} | Date : {adoption.DateDemande:yyyy-MM-dd} | Statut : {adoption.Statut}"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AjouterFamilleAccueil()
        {
            try
            {
                Titre("AJOUT D'UNE FAMILLE D'ACCUEIL");

                string animal = LireTexteObligatoire("Identifiant animal : ");
                if (!ValidationExistence.AnimalExiste(bd, animal))
                {
                    AfficherErreur("Animal introuvable.");
                    Pause();
                    return;
                }
                string etat = bd.GetEtatActuelAnimal(animal);

                if (!ValidationMetier.PeutAjouterFamilleAccueil(etat))
                {
                    AfficherErreur(ValidationMetier.MessageActionImpossible(etat, "famille d'accueil"));
                    Pause();
                    return;
                }
                int contact = LireInt("Identifiant contact famille d'accueil : ");
                if (!ValidationExistence.ContactExiste(bd, contact))
                {
                    AfficherErreur("Contact introuvable.");
                    Pause();
                    return;
                }
                if (bd.AnimalAFamilleAccueilActive(animal))
                {
                    AfficherErreur("Cet animal a déjà une famille d'accueil active.");
                    Pause();
                    return;
                }

                DateTime dateDebut;

                bool aujourdHui = LireOuiNon("La famille d'accueil commence-t-elle aujourd'hui ?");

                if (aujourdHui)
                {
                    dateDebut = DateTime.Today;
                }
                else
                {
                    dateDebut = LireDate("Date de début (yyyy-mm-dd) : ");
                }

                if (bd.ExisteSortieAnimalDate(animal, dateDebut))
                {
                    AfficherErreur("Une sortie existe déjà pour cet animal à cette date.");
                    Console.WriteLine("La famille d'accueil n'a pas été ajoutée pour éviter une incohérence.");
                    Pause();
                    return;
                }
                DateTime? derniereEntree = bd.GetDerniereDateEntreeAnimal(animal);

                if (derniereEntree != null && dateDebut.Date < derniereEntree.Value.Date)
                {
                    AfficherErreur("La date de début de famille d'accueil ne peut pas être avant la dernière date d'entrée.");
                    Pause();
                    return;
                }

                FamilleAccueil famille = new FamilleAccueil(
                    animal,
                    contact,
                    dateDebut,
                    null
                );

                bd.AjouterFamilleAccueil(famille);

                AfficherSucces("Famille d'accueil ajoutée.");

                Console.WriteLine();
                Console.WriteLine("Création automatique de la sortie temporaire du refuge.");

                AniSortie sortie = new AniSortie(
                    animal,
                    dateDebut,
                    "famille_accueil",
                    contact
                );

                bd.AjouterSortie(sortie);

                AfficherSucces("Sortie temporaire vers famille d'accueil enregistrée automatiquement.");
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
                Console.WriteLine();
                Console.WriteLine("Remarque : si une sortie existe déjà pour cet animal à cette date, PostgreSQL peut refuser le doublon.");
            }

            Pause();
        }

        private void ListerFamillesAccueilAnimal()
        {
            try
            {
                Titre("FAMILLES D'ACCUEIL D'UN ANIMAL");

                string identifiant = LireTexteObligatoire("Identifiant animal : ");

                List<FamilleAccueil> familles = bd.GetFamillesAccueilParAnimal(identifiant);

                if (familles.Count == 0)
                {
                    Console.WriteLine("Aucune famille d'accueil trouvée pour cet animal.");
                }
                else
                {
                    foreach (FamilleAccueil famille in familles)
                    {
                        string dateFin = famille.DateFin == null
                            ? "en cours"
                            : famille.DateFin.Value.ToString("yyyy-MM-dd");

                        Console.WriteLine(
                            $"Contact : {famille.FaContact} | Début : {famille.DateDebut:yyyy-MM-dd} | Fin : {dateFin}"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void ListerAnimauxParFamilleAccueil()
        {
            try
            {
                Titre("ANIMAUX ACCUEILLIS PAR UNE FAMILLE");

                int contact = LireInt("Identifiant contact famille : ");

                List<FamilleAccueil> familles = bd.GetAnimauxParFamilleAccueil(contact);

                if (familles.Count == 0)
                {
                    Console.WriteLine("Aucun animal trouvé pour cette famille d'accueil.");
                }
                else
                {
                    foreach (FamilleAccueil famille in familles)
                    {
                        string dateFin = famille.DateFin == null
                            ? "en cours"
                            : famille.DateFin.Value.ToString("yyyy-MM-dd");

                        Console.WriteLine(
                            $"Animal : {famille.FaAniIdentifiant} | Début : {famille.DateDebut:yyyy-MM-dd} | Fin : {dateFin}"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void ListerAnimauxPresents()
        {
            try
            {
                Titre("ANIMAUX PRÉSENTS AU REFUGE");

                List<Animal> animaux = bd.GetAnimauxPresentsAuRefuge();

                if (animaux.Count == 0)
                {
                    Console.WriteLine("Aucun animal actuellement présent au refuge.");
                }
                else
                {
                    foreach (Animal animal in animaux)
                    {
                        Console.WriteLine(
                            $"{animal.Identifiant} - {animal.Nom} - {animal.Type} - {animal.Sexe}"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AjouterEntree()
        {
            try
            {
                Titre("AJOUT D'UNE ENTRÉE");

                string animal = LireTexteObligatoire("Identifiant animal : ");
                if (!ValidationExistence.AnimalExiste(bd, animal))
                {
                    AfficherErreur("Animal introuvable.");
                    Pause();
                    return;
                }
                string etat = bd.GetEtatActuelAnimal(animal);

                if (etat != "famille_accueil")
                {
                    AfficherErreur("Une entrée manuelle est autorisée uniquement pour le retour d'une famille d'accueil.");
                    Console.WriteLine("État actuel : " + FormaterEtatAnimal(etat));
                    Pause();
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("Raisons possibles :");
                Console.WriteLine("- abandon");
                Console.WriteLine("- errant");
                Console.WriteLine("- deces_proprietaire");
                Console.WriteLine("- saisie");
                Console.WriteLine("- retour_adoption");
                Console.WriteLine();

                string raison = LireChoixTexte(
                    "Raison : ",
                    new List<string>
                    {
                        "abandon",
                        "errant",
                        "deces_proprietaire",
                        "saisie",
                        "retour_adoption"
                    }
                );

                int contact = LireInt("Identifiant contact : ");
                if (!ValidationExistence.ContactExiste(bd, contact))
                {
                    AfficherErreur("Contact introuvable.");
                    Pause();
                    return;
                }

                DateTime dateEntree;

                bool aujourdHui = LireOuiNon("L'entrée a-t-elle lieu aujourd'hui ?");

                if (aujourdHui)
                {
                    dateEntree = DateTime.Today;
                }
                else
                {
                    dateEntree = LireDate("Date d'entrée (yyyy-mm-dd) : ");
                }
                if (ValidationDates.DateFuture(dateEntree))
                {
                    AfficherErreur("La date d'entrée ne peut pas être dans le futur.");
                    Pause();
                    return;
                }
                AniEntree entree = new AniEntree(
                    animal,
                    dateEntree,
                    raison,
                    contact
                );

                bd.AjouterEntree(entree);

                AfficherSucces("Entrée enregistrée.");
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AjouterSortie()
        {
            try
            {
                Titre("AJOUT D'UNE SORTIE");

                string animal = LireTexteObligatoire("Identifiant animal : ");
                if (!ValidationExistence.AnimalExiste(bd, animal))
                {
                    AfficherErreur("Animal introuvable.");
                    Pause();
                    return;
                }
                string etat = bd.GetEtatActuelAnimal(animal);

                if (!ValidationMetier.PeutAjouterSortie(etat))
                {
                    AfficherErreur(ValidationMetier.MessageActionImpossible(etat, "sortie"));
                    Pause();
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("Raisons possibles :");
                Console.WriteLine("- adoption");
                Console.WriteLine("- retour_proprietaire");
                Console.WriteLine("- deces_animal");
                Console.WriteLine();

                string raison = LireChoixTexte(
                    "Raison : ",
                    new List<string>
                    {
                        "adoption",
                        "retour_proprietaire",
                        "deces_animal"
                    }
                );

                int contact = LireInt("Identifiant contact : ");
                if (!ValidationExistence.ContactExiste(bd, contact))
                {
                    AfficherErreur("Contact introuvable.");
                    Pause();
                    return;
                }

                DateTime dateSortie;

                bool aujourdHui = LireOuiNon("La sortie a-t-elle lieu aujourd'hui ?");

                if (aujourdHui)
                {
                    dateSortie = DateTime.Today;
                }
                else
                {
                    dateSortie = LireDate("Date de sortie (yyyy-mm-dd) : ");
                }
                if (ValidationDates.DateFuture(dateSortie))
                {
                    AfficherErreur("La date de sortie ne peut pas être dans le futur.");
                    Pause();
                    return;
                }
                DateTime? derniereEntree = bd.GetDerniereDateEntreeAnimal(animal);

                if (derniereEntree != null && dateSortie.Date < derniereEntree.Value.Date)
                {
                    AfficherErreur("La date de sortie ne peut pas être avant la dernière date d'entrée.");
                    Pause();
                    return;
                }

                AniSortie sortie = new AniSortie(
                    animal,
                    dateSortie,
                    raison,
                    contact
                );

                bd.AjouterSortie(sortie);

                AfficherSucces("Sortie enregistrée.");

                if (raison == "deces_animal")
                {
                    bool dateModifiee = bd.ModifierDateDecesAnimal(animal, dateSortie);

                    if (dateModifiee)
                    {
                        AfficherSucces("Date de décès de l'animal mise à jour automatiquement.");
                    }
                    else
                    {
                        AfficherErreur("La sortie est enregistrée, mais la date de décès n'a pas pu être mise à jour.");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AjouterCouleur()
        {
            try
            {
                Titre("AJOUTER UNE COULEUR");

                int id = LireInt("Identifiant couleur : ");
                string nom = LireTexteObligatoire("Nom couleur : ");

                bd.AjouterCouleur(new Couleur(id, nom));

                AfficherSucces("Couleur ajoutée.");
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AjouterCouleurAnimal()
        {
            try
            {
                Titre("AJOUTER UNE COULEUR À UN ANIMAL");

                string animalId = LireTexteObligatoire("Identifiant animal : ");
                if (!ValidationExistence.AnimalExiste(bd, animalId))
                {
                    AfficherErreur("Animal introuvable.");
                    Pause();
                    return;
                }
                int couleurId = LireInt("Identifiant couleur : ");
                if (!ValidationExistence.CouleurExiste(bd, couleurId))
                {
                    AfficherErreur("Couleur introuvable.");
                    Pause();
                    return;
                }

                bd.AjouterCouleurAnimal(animalId, couleurId);
                if (bd.AnimalPossedeCouleur(animalId, couleurId))
                {
                    AfficherErreur("Cette couleur est déjà associée à cet animal.");
                    Pause();
                    return;
                }

                AfficherSucces("Couleur associée à l'animal.");
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AjouterRole()
        {
            try
            {
                Titre("AJOUTER UN RÔLE");

                int id = LireInt("Identifiant rôle : ");

                Console.WriteLine();
                Console.WriteLine("Rôles conseillés :");
                Console.WriteLine("- benevole");
                Console.WriteLine("- adoptant");
                Console.WriteLine("- candidat");
                Console.WriteLine("- famille_accueil");
                Console.WriteLine();

                string nom = LireTexteObligatoire("Nom rôle : ");

                bd.AjouterRole(new Role(id, nom));

                AfficherSucces("Rôle ajouté.");
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AjouterRoleContact()
        {
            try
            {
                Titre("ATTRIBUER UN RÔLE À UN CONTACT");

                int contactId = LireInt("Identifiant contact : ");
                if (!ValidationExistence.ContactExiste(bd, contactId))
                {
                    AfficherErreur("Contact introuvable.");
                    Pause();
                    return;
                }
                int roleId = LireInt("Identifiant rôle : ");
                if (!ValidationExistence.RoleExiste(bd, roleId))
                {
                    AfficherErreur("Rôle introuvable.");
                    Pause();
                    return;
                }

                bd.AjouterRoleContact(contactId, roleId);
                if (bd.ContactPossedeRole(contactId, roleId))
                {
                    AfficherErreur("Ce rôle est déjà attribué à ce contact.");
                    Pause();
                    return;
                }

                AfficherSucces("Rôle attribué au contact.");
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void ConsulterVaccin()
        {
            try
            {
                Titre("CONSULTER UN VACCIN");

                int id = LireInt("Identifiant vaccin : ");

                if (!ValidationExistence.VaccinExiste(bd, id))
                {
                    AfficherErreur("Vaccin introuvable.");
                    Pause();
                    return;
                }

                Vaccin? vaccin = bd.GetVaccinParIdentifiant(id);

                if (vaccin == null)
                {
                    AfficherErreur("Vaccin introuvable.");
                    Pause();
                    return;
                }

                Titre("FICHE VACCIN");

                Console.WriteLine("INFORMATIONS GÉNÉRALES");
                Console.WriteLine($"Identifiant : {vaccin.Identifiant}");
                Console.WriteLine($"Nom         : {vaccin.Nom}");

                AfficherListe("Animaux vaccinés", bd.GetAnimauxVaccinesParVaccin(id));
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void ConsulterAdoption()
        {
            try
            {
                Titre("CONSULTER UNE ADOPTION");

                string animal = LireTexteObligatoire("Identifiant animal : ");

                if (!ValidationExistence.AnimalExiste(bd, animal))
                {
                    AfficherErreur("Animal introuvable.");
                    Pause();
                    return;
                }

                int contact = LireInt("Identifiant contact : ");

                if (!ValidationExistence.ContactExiste(bd, contact))
                {
                    AfficherErreur("Contact introuvable.");
                    Pause();
                    return;
                }

                DateTime date = LireDate("Date de la demande (yyyy-mm-dd) : ");

                Adoption? adoption = bd.GetAdoption(animal, contact, date);

                if (adoption == null)
                {
                    AfficherErreur("Adoption introuvable.");
                    Pause();
                    return;
                }

                Titre("FICHE ADOPTION");

                Console.WriteLine("INFORMATIONS GÉNÉRALES");
                Console.WriteLine($"Animal        : {adoption.AniIdentifiant}");
                Console.WriteLine($"Contact       : {adoption.AdopContact}");
                Console.WriteLine($"Date demande  : {adoption.DateDemande:yyyy-MM-dd}");
                Console.WriteLine($"Statut        : {adoption.Statut}");

                Console.WriteLine();
                Console.WriteLine("CONTEXTE ANIMAL");
                Console.WriteLine("État actuel   : " + FormaterEtatAnimal(bd.GetEtatActuelAnimal(adoption.AniIdentifiant)));
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void ConsulterFamilleAccueil()
        {
            try
            {
                Titre("CONSULTER UNE FAMILLE D'ACCUEIL");

                string animal = LireTexteObligatoire("Identifiant animal : ");

                if (!ValidationExistence.AnimalExiste(bd, animal))
                {
                    AfficherErreur("Animal introuvable.");
                    Pause();
                    return;
                }

                int contact = LireInt("Identifiant contact famille : ");

                if (!ValidationExistence.ContactExiste(bd, contact))
                {
                    AfficherErreur("Contact introuvable.");
                    Pause();
                    return;
                }

                DateTime dateDebut = LireDate("Date de début (yyyy-mm-dd) : ");

                FamilleAccueil? famille = bd.GetFamilleAccueil(animal, contact, dateDebut);

                if (famille == null)
                {
                    AfficherErreur("Famille d'accueil introuvable.");
                    Pause();
                    return;
                }

                Titre("FICHE FAMILLE D'ACCUEIL");

                Console.WriteLine("INFORMATIONS GÉNÉRALES");
                Console.WriteLine($"Animal       : {famille.FaAniIdentifiant}");
                Console.WriteLine($"Contact      : {famille.FaContact}");
                Console.WriteLine($"Date début   : {famille.DateDebut:yyyy-MM-dd}");
                Console.WriteLine($"Date fin     : {(famille.DateFin == null ? "En cours" : famille.DateFin.Value.ToString("yyyy-MM-dd"))}");

                Console.WriteLine();
                Console.WriteLine("CONTEXTE ANIMAL");
                Console.WriteLine("État actuel  : " + FormaterEtatAnimal(bd.GetEtatActuelAnimal(famille.FaAniIdentifiant)));
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void ConsulterCouleur()
        {
            try
            {
                Titre("CONSULTER UNE COULEUR");

                int id = LireInt("Identifiant couleur : ");

                if (!ValidationExistence.CouleurExiste(bd, id))
                {
                    AfficherErreur("Couleur introuvable.");
                    Pause();
                    return;
                }

                Couleur? couleur = bd.GetCouleurParIdentifiant(id);

                if (couleur == null)
                {
                    AfficherErreur("Couleur introuvable.");
                    Pause();
                    return;
                }

                Titre("FICHE COULEUR");

                Console.WriteLine("INFORMATIONS GÉNÉRALES");
                Console.WriteLine($"Identifiant : {couleur.ColIdentifiant}");
                Console.WriteLine($"Nom         : {couleur.NomCouleur}");

                AfficherListe("Animaux avec cette couleur", bd.GetAnimauxParCouleur(id));
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void ConsulterRole()
        {
            try
            {
                Titre("CONSULTER UN RÔLE");

                int id = LireInt("Identifiant rôle : ");

                if (!ValidationExistence.RoleExiste(bd, id))
                {
                    AfficherErreur("Rôle introuvable.");
                    Pause();
                    return;
                }

                Role? role = bd.GetRoleParIdentifiant(id);

                if (role == null)
                {
                    AfficherErreur("Rôle introuvable.");
                    Pause();
                    return;
                }

                Titre("FICHE RÔLE");

                Console.WriteLine("INFORMATIONS GÉNÉRALES");
                Console.WriteLine($"Identifiant : {role.RolIdentifiant}");
                Console.WriteLine($"Nom         : {role.RolNom}");

                AfficherListe("Contacts avec ce rôle", bd.GetContactsParRole(id));
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void ConsulterCompatibilite()
        {
            try
            {
                Titre("CONSULTER UNE COMPATIBILITÉ");

                int id = LireInt("Identifiant compatibilité : ");

                if (!ValidationExistence.CompatibiliteExiste(bd, id))
                {
                    AfficherErreur("Compatibilité introuvable.");
                    Pause();
                    return;
                }

                Compatibilite? compatibilite = bd.GetCompatibiliteParIdentifiant(id);

                if (compatibilite == null)
                {
                    AfficherErreur("Compatibilité introuvable.");
                    Pause();
                    return;
                }

                Titre("FICHE COMPATIBILITÉ");

                Console.WriteLine("INFORMATIONS GÉNÉRALES");
                Console.WriteLine($"Identifiant : {compatibilite.Identifiant}");
                Console.WriteLine($"Type        : {compatibilite.Type}");

                AfficherListe("Animaux avec cette compatibilité", bd.GetAnimauxParCompatibilite(id));
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AfficherStatistiques()
        {
            try
            {
                Titre("STATISTIQUES DU REFUGE");

                Console.WriteLine("ANIMAUX");
                Console.WriteLine($"Total animaux                 : {bd.Compter("animal")}");
                Console.WriteLine($"Animaux au refuge             : {bd.CompterAnimauxParEtat("au_refuge")}");
                Console.WriteLine($"Animaux adoptés               : {bd.CompterAnimauxParEtat("adopte")}");
                Console.WriteLine($"Animaux en famille d'accueil  : {bd.CompterAnimauxParEtat("famille_accueil")}");
                Console.WriteLine($"Animaux décédés               : {bd.CompterAnimauxParEtat("decede")}");
                Console.WriteLine($"Animaux retour propriétaire   : {bd.CompterAnimauxParEtat("retour_proprietaire")}");

                Console.WriteLine();
                Console.WriteLine("DONNÉES");
                Console.WriteLine($"Contacts                      : {bd.Compter("contact")}");
                Console.WriteLine($"Adoptions                     : {bd.Compter("adoption")}");
                Console.WriteLine($"Familles d'accueil            : {bd.Compter("famille_accueil")}");
                Console.WriteLine($"Vaccinations                  : {bd.Compter("vaccination")}");
                Console.WriteLine($"Vaccins                       : {bd.Compter("vaccin")}");
                Console.WriteLine($"Couleurs                      : {bd.Compter("couleur")}");
                Console.WriteLine($"Compatibilités                : {bd.Compter("compatibilite")}");
                Console.WriteLine($"Rôles                         : {bd.Compter("role")}");
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void VerifierCoherenceRefuge()
        {
            try
            {
                Titre("VÉRIFICATION DE COHÉRENCE");

                List<string> problemes = bd.VerifierCoherenceRefuge();

                if (problemes.Count == 0)
                {
                    AfficherSucces("Aucun problème de cohérence détecté.");
                }
                else
                {
                    AfficherErreur("Des problèmes de cohérence ont été détectés.");

                    Console.WriteLine();

                    foreach (string probleme in problemes)
                    {
                        Console.WriteLine("- " + probleme);
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void RechercherAnimalParNom()
        {
            try
            {
                Titre("RECHERCHER UN ANIMAL");

                string nom = LireTexteObligatoire("Nom ou partie du nom : ");

                List<Animal> animaux = bd.RechercherAnimauxParNom(nom);

                if (animaux.Count == 0)
                {
                    Console.WriteLine("Aucun animal trouvé.");
                }
                else
                {
                    foreach (Animal animal in animaux)
                    {
                        Console.WriteLine($"{animal.Identifiant} - {animal.Nom} - {animal.Type} - {animal.Sexe}");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void RechercherContactParNom()
        {
            try
            {
                Titre("RECHERCHER UN CONTACT");

                string recherche = LireTexteObligatoire("Nom ou prénom : ");

                List<Contact> contacts = bd.RechercherContactsParNom(recherche);

                if (contacts.Count == 0)
                {
                    Console.WriteLine("Aucun contact trouvé.");
                }
                else
                {
                    foreach (Contact contact in contacts)
                    {
                        Console.WriteLine(
                            $"{contact.ContactIdentifiant} - {contact.Nom} {contact.Prenom} - {contact.Gsm} - {contact.Email}"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void CloturerFamilleAccueil()
        {
            try
            {
                Titre("CLÔTURER UNE FAMILLE D'ACCUEIL");

                string animal = LireTexteObligatoire("Identifiant animal : ");

                if (!ValidationExistence.AnimalExiste(bd, animal))
                {
                    AfficherErreur("Animal introuvable.");
                    Pause();
                    return;
                }

                string etat = bd.GetEtatActuelAnimal(animal);

                if (etat != "famille_accueil")
                {
                    AfficherErreur("Cet animal n'est pas actuellement en famille d'accueil.");
                    Console.WriteLine("État actuel : " + FormaterEtatAnimal(etat));
                    Pause();
                    return;
                }

                DateTime dateFin;

                bool aujourdHui = LireOuiNon("La famille d'accueil se termine-t-elle aujourd'hui ?");

                if (aujourdHui)
                {
                    dateFin = DateTime.Today;
                }
                else
                {
                    dateFin = LireDate("Date de fin (yyyy-mm-dd) : ");
                }

                if (ValidationDates.DateFuture(dateFin))
                {
                    AfficherErreur("La date de fin ne peut pas être dans le futur.");
                    Pause();
                    return;
                }

                bool cloture = bd.CloturerFamilleAccueil(animal, dateFin);

                if (!cloture)
                {
                    AfficherErreur("Aucune famille d'accueil active trouvée pour cet animal.");
                    Pause();
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("Création automatique du retour au refuge.");

                int contact = LireInt("Identifiant du contact responsable du retour : ");

                if (!ValidationExistence.ContactExiste(bd, contact))
                {
                    AfficherErreur("Contact introuvable.");
                    Pause();
                    return;
                }

                AniEntree entree = new AniEntree(
                    animal,
                    dateFin,
                    "retour_adoption",
                    contact
                );

                bd.AjouterEntree(entree);

                AfficherSucces("Famille d'accueil clôturée et retour au refuge enregistré.");
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void SupprimerVaccin()
        {
            try
            {
                Titre("SUPPRIMER UN VACCIN");

                int id = LireInt("Identifiant vaccin : ");

                if (!ValidationExistence.VaccinExiste(bd, id))
                {
                    AfficherErreur("Vaccin introuvable.");
                    Pause();
                    return;
                }

                int nbVaccinations = bd.CompterVaccinationsParVaccin(id);

                if (nbVaccinations > 0)
                {
                    AfficherErreur("Impossible de supprimer ce vaccin car il est utilisé.");

                    Console.WriteLine();
                    Console.WriteLine("Données liées :");
                    Console.WriteLine("- " + nbVaccinations + " vaccination(s)");

                    Pause();
                    return;
                }

                bool confirmation = LireOuiNon("Voulez-vous vraiment supprimer ce vaccin ?");

                if (!confirmation)
                {
                    AfficherErreur("Suppression annulée.");
                    Pause();
                    return;
                }

                bool supprime = bd.SupprimerVaccin(id);

                if (supprime)
                {
                    AfficherSucces("Vaccin supprimé.");
                }
                else
                {
                    AfficherErreur("Vaccin introuvable.");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void SupprimerCouleur()
        {
            try
            {
                Titre("SUPPRIMER UNE COULEUR");

                int id = LireInt("Identifiant couleur : ");

                if (!ValidationExistence.CouleurExiste(bd, id))
                {
                    AfficherErreur("Couleur introuvable.");
                    Pause();
                    return;
                }

                int nbAnimaux = bd.CompterAnimauxParCouleur(id);

                if (nbAnimaux > 0)
                {
                    AfficherErreur("Impossible de supprimer cette couleur car elle est utilisée.");

                    Console.WriteLine();
                    Console.WriteLine("Données liées :");
                    Console.WriteLine("- " + nbAnimaux + " animal(aux)");

                    Pause();
                    return;
                }

                bool confirmation = LireOuiNon("Voulez-vous vraiment supprimer cette couleur ?");

                if (!confirmation)
                {
                    AfficherErreur("Suppression annulée.");
                    Pause();
                    return;
                }

                bool supprime = bd.SupprimerCouleur(id);

                if (supprime)
                {
                    AfficherSucces("Couleur supprimée.");
                }
                else
                {
                    AfficherErreur("Couleur introuvable.");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void SupprimerRole()
        {
            try
            {
                Titre("SUPPRIMER UN RÔLE");

                int id = LireInt("Identifiant rôle : ");

                if (!ValidationExistence.RoleExiste(bd, id))
                {
                    AfficherErreur("Rôle introuvable.");
                    Pause();
                    return;
                }

                int nbContacts = bd.CompterContactsParRole(id);

                if (nbContacts > 0)
                {
                    AfficherErreur("Impossible de supprimer ce rôle car il est utilisé.");

                    Console.WriteLine();
                    Console.WriteLine("Données liées :");
                    Console.WriteLine("- " + nbContacts + " contact(s)");

                    Pause();
                    return;
                }

                bool confirmation = LireOuiNon("Voulez-vous vraiment supprimer ce rôle ?");

                if (!confirmation)
                {
                    AfficherErreur("Suppression annulée.");
                    Pause();
                    return;
                }

                bool supprime = bd.SupprimerRole(id);

                if (supprime)
                {
                    AfficherSucces("Rôle supprimé.");
                }
                else
                {
                    AfficherErreur("Rôle introuvable.");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void SupprimerCompatibilite()
        {
            try
            {
                Titre("SUPPRIMER UNE COMPATIBILITÉ");

                int id = LireInt("Identifiant compatibilité : ");

                if (!ValidationExistence.CompatibiliteExiste(bd, id))
                {
                    AfficherErreur("Compatibilité introuvable.");
                    Pause();
                    return;
                }

                int nbAnimaux = bd.CompterAnimauxParCompatibilite(id);

                if (nbAnimaux > 0)
                {
                    AfficherErreur("Impossible de supprimer cette compatibilité car elle est utilisée.");

                    Console.WriteLine();
                    Console.WriteLine("Données liées :");
                    Console.WriteLine("- " + nbAnimaux + " animal(aux)");

                    Pause();
                    return;
                }

                bool confirmation = LireOuiNon("Voulez-vous vraiment supprimer cette compatibilité ?");

                if (!confirmation)
                {
                    AfficherErreur("Suppression annulée.");
                    Pause();
                    return;
                }

                bool supprime = bd.SupprimerCompatibilite(id);

                if (supprime)
                {
                    AfficherSucces("Compatibilité supprimée.");
                }
                else
                {
                    AfficherErreur("Compatibilité introuvable.");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AfficherContacts()
        {
            try
            {
                Titre("LISTE DES CONTACTS");

                List<Contact> contacts = bd.GetContacts();

                if (contacts.Count == 0)
                {
                    Console.WriteLine("Aucun contact enregistré.");
                }
                else
                {
                    foreach (Contact contact in contacts)
                    {
                        Console.WriteLine($"{contact.ContactIdentifiant} - {contact.Nom} {contact.Prenom} - {contact.Gsm} - {contact.Email}");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AfficherVaccins()
        {
            try
            {
                Titre("LISTE DES VACCINS");

                List<Vaccin> vaccins = bd.GetVaccins();

                if (vaccins.Count == 0)
                {
                    Console.WriteLine("Aucun vaccin enregistré.");
                }
                else
                {
                    foreach (Vaccin vaccin in vaccins)
                    {
                        Console.WriteLine($"{vaccin.Identifiant} - {vaccin.Nom}");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AfficherCouleurs()
        {
            try
            {
                Titre("LISTE DES COULEURS");

                foreach (Couleur couleur in bd.GetCouleurs())
                {
                    Console.WriteLine($"{couleur.ColIdentifiant} - {couleur.NomCouleur}");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AfficherRoles()
        {
            try
            {
                Titre("LISTE DES RÔLES");

                foreach (Role role in bd.GetRoles())
                {
                    Console.WriteLine($"{role.RolIdentifiant} - {role.RolNom}");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AfficherCompatibilites()
        {
            try
            {
                Titre("LISTE DES COMPATIBILITÉS");

                foreach (Compatibilite compatibilite in bd.GetCompatibilites())
                {
                    Console.WriteLine($"{compatibilite.Identifiant} - {compatibilite.Type}");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void DiagnosticRefuge()
        {
            try
            {
                Titre("DIAGNOSTIC DU REFUGE");

                int score = 100;

                List<string> problemes = bd.VerifierCoherenceRefuge();

                Console.WriteLine("COHÉRENCE");
                Console.WriteLine();

                if (problemes.Count == 0)
                {
                    Console.WriteLine("[OK] Aucun problème de cohérence détecté.");
                }
                else
                {
                    score -= problemes.Count * 10;

                    Console.WriteLine("[ERREUR] Problèmes détectés :");

                    foreach (string probleme in problemes)
                    {
                        Console.WriteLine("- " + probleme);
                    }
                }

                if (score < 0)
                {
                    score = 0;
                }

                Console.WriteLine();
                Console.WriteLine("STATISTIQUES RAPIDES");
                Console.WriteLine();

                Console.WriteLine("Animaux au refuge            : " + bd.CompterAnimauxParEtat("au_refuge"));
                Console.WriteLine("Animaux adoptés              : " + bd.CompterAnimauxParEtat("adopte"));
                Console.WriteLine("Animaux en famille d'accueil : " + bd.CompterAnimauxParEtat("famille_accueil"));
                Console.WriteLine("Animaux décédés              : " + bd.CompterAnimauxParEtat("decede"));
                Console.WriteLine("Contacts                     : " + bd.Compter("contact"));
                Console.WriteLine("Vaccinations                 : " + bd.Compter("vaccination"));
                Console.WriteLine("Adoptions                    : " + bd.Compter("adoption"));

                Console.WriteLine();
                Console.WriteLine("SCORE QUALITÉ");
                Console.WriteLine();

                Console.WriteLine(score + " / 100");

                if (score == 100)
                {
                    AfficherSucces("Base de données cohérente.");
                }
                else if (score >= 70)
                {
                    Console.WriteLine("[INFO] Quelques problèmes sont à corriger.");
                }
                else
                {
                    AfficherErreur("La base contient plusieurs incohérences importantes.");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AfficherTableauDeBord()
        {
            try
            {
                Titre("TABLEAU DE BORD DU REFUGE");

                Console.WriteLine("ANIMAUX");
                Console.WriteLine($"Au refuge             : {bd.CompterAnimauxParEtat("au_refuge")}");
                Console.WriteLine($"En famille d'accueil  : {bd.CompterAnimauxParEtat("famille_accueil")}");
                Console.WriteLine($"Adoptés               : {bd.CompterAnimauxParEtat("adopte")}");
                Console.WriteLine($"Décédés               : {bd.CompterAnimauxParEtat("decede")}");

                Console.WriteLine();
                Console.WriteLine("DONNÉES PRINCIPALES");
                Console.WriteLine($"Contacts              : {bd.Compter("contact")}");
                Console.WriteLine($"Vaccinations          : {bd.Compter("vaccination")}");
                Console.WriteLine($"Adoptions             : {bd.Compter("adoption")}");
                Console.WriteLine($"Familles d'accueil    : {bd.Compter("famille_accueil")}");

                Console.WriteLine();
                Console.WriteLine("ÉTAT DU SYSTÈME");

                List<string> problemes = bd.VerifierCoherenceRefuge();

                if (problemes.Count == 0)
                {
                    AfficherSucces("Aucune incohérence détectée.");
                }
                else
                {
                    AfficherErreur(problemes.Count + " incohérence(s) détectée(s).");
                    Console.WriteLine("Utilisez le menu Diagnostic du refuge pour voir les détails.");
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }

            Pause();
        }

        private void AfficherResumeAnimal(Animal animal)
        {
            string etat = bd.GetEtatActuelAnimal(animal.Identifiant);

            Console.WriteLine("RÉSUMÉ INTELLIGENT");
            Console.WriteLine("- État actuel : " + FormaterEtatAnimal(etat));
            Console.WriteLine("- Vaccination(s) : " + bd.CompterVaccinationsAnimal(animal.Identifiant));
            Console.WriteLine("- Adoption(s) : " + bd.CompterAdoptionsAnimal(animal.Identifiant));
            Console.WriteLine("- Famille(s) d'accueil : " + bd.CompterFamillesAccueilAnimal(animal.Identifiant));
            Console.WriteLine("- Compatibilité(s) : " + bd.CompterCompatibilitesAnimal(animal.Identifiant));
            Console.WriteLine("- Couleur(s) : " + bd.CompterCouleursAnimal(animal.Identifiant));

            Console.WriteLine();

            if (etat == "au_refuge")
            {
                Console.WriteLine("- Résultat : animal actuellement disponible au refuge.");
            }
            else if (etat == "famille_accueil")
            {
                Console.WriteLine("- Résultat : animal actuellement accueilli par une famille.");
            }
            else if (etat == "adopte")
            {
                Console.WriteLine("- Résultat : animal adopté.");
            }
            else if (etat == "decede")
            {
                Console.WriteLine("- Résultat : animal décédé.");
            }
            else if (etat == "retour_proprietaire")
            {
                Console.WriteLine("- Résultat : animal retourné chez son propriétaire.");
            }
        }

        private void AfficherResumeContact(Contact contact)
        {
            Console.WriteLine("RÉSUMÉ INTELLIGENT");
            Console.WriteLine("- Rôle(s) : " + bd.CompterRolesContact(contact.ContactIdentifiant));
            Console.WriteLine("- Adoption(s) liée(s) : " + bd.CompterAdoptionsContact(contact.ContactIdentifiant));
            Console.WriteLine("- Famille(s) d'accueil liée(s) : " + bd.CompterFamillesContact(contact.ContactIdentifiant));

            Console.WriteLine();

            if (!string.IsNullOrWhiteSpace(contact.Gsm) || !string.IsNullOrWhiteSpace(contact.Telephone) || !string.IsNullOrWhiteSpace(contact.Email))
            {
                Console.WriteLine("- Résultat : contact joignable.");
            }
            else
            {
                Console.WriteLine("- Résultat : aucun moyen de contact renseigné.");
            }
        }
    }
}