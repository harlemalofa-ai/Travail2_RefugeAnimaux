using Npgsql;
using RefugeAnimaux.classesMetier;

namespace RefugeAnimaux.coucheAccesBD
{
    public class AccesBD
    {
        private readonly string chaineConnexion;
        private NpgsqlConnection? connexion;

        public AccesBD()
        {
            chaineConnexion = ConfigurationConnexion.ObtenirChaineConnexion();
        }

        public void OuvrirConnexion()
        {
            if (connexion == null)
            {
                connexion = new NpgsqlConnection(chaineConnexion);
            }

            if (connexion.State == System.Data.ConnectionState.Broken)
            {
                connexion.Dispose();
                connexion = new NpgsqlConnection(chaineConnexion);
            }

            if (connexion.State != System.Data.ConnectionState.Open)
            {
                connexion.Open();
            }
        }

        public void FermerConnexion()
        {
            if (connexion != null)
            {
                if (connexion.State != System.Data.ConnectionState.Closed)
                {
                    connexion.Close();
                }

                connexion.Dispose();
                connexion = null;
            }
        }

        private DateTime ConvertirEnDateTime(object valeur, string colonne)
        {
            if (valeur is DateOnly dateOnly)
            {
                return dateOnly.ToDateTime(TimeOnly.MinValue);
            }

            if (valeur is DateTime dateTime)
            {
                return dateTime.Date;
            }

            if (DateTime.TryParse(valeur.ToString(), out DateTime dateConvertie))
            {
                return dateConvertie.Date;
            }

            throw new InvalidCastException($"La colonne {colonne} ne contient pas une date valide.");
        }

        private DateTime LireDate(NpgsqlDataReader lecteur, string colonne)
        {
            return ConvertirEnDateTime(lecteur[colonne], colonne);
        }

        private DateTime? LireDateNullable(NpgsqlDataReader lecteur, string colonne)
        {
            if (lecteur[colonne] == DBNull.Value)
            {
                return null;
            }

            return ConvertirEnDateTime(lecteur[colonne], colonne);
        }

        private object ValeurOuDbNull(object? valeur)
        {
            return valeur == null ? DBNull.Value : valeur;
        }

        private Animal CreerAnimalDepuisLecteur(NpgsqlDataReader lecteur)
        {
            return new Animal(
                lecteur["identifiant"].ToString()!,
                lecteur["nom"].ToString()!,
                lecteur["type"].ToString()!,
                lecteur["sexe"].ToString()!,
                lecteur["particularites"] == DBNull.Value ? null : lecteur["particularites"].ToString(),
                LireDateNullable(lecteur, "date_deces"),
                lecteur["description"] == DBNull.Value ? null : lecteur["description"].ToString(),
                LireDateNullable(lecteur, "date_sterilisation"),
                (bool)lecteur["sterilise"],
                LireDate(lecteur, "date_naissance")
            );
        }

        public List<Animal> GetAnimaux()
        {
            List<Animal> animaux = new List<Animal>();

            string sql = "SELECT * FROM animal ORDER BY nom";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            animaux.Add(CreerAnimalDepuisLecteur(lecteur));
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return animaux;
        }

        public void AjouterAnimal(Animal animal)
        {
            string sql = @"
        INSERT INTO animal (
            identifiant,
            nom,
            type,
            sexe,
            particularites,
            date_deces,
            description,
            date_sterilisation,
            sterilise,
            date_naissance
        )
        VALUES (
            @identifiant,
            @nom,
            @type,
            @sexe,
            @particularites,
            @date_deces,
            @description,
            @date_sterilisation,
            @sterilise,
            @date_naissance
        )";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@identifiant", animal.Identifiant);
                    commande.Parameters.AddWithValue("@nom", animal.Nom);
                    commande.Parameters.AddWithValue("@type", animal.Type);
                    commande.Parameters.AddWithValue("@sexe", animal.Sexe);

                    commande.Parameters.AddWithValue(
                        "@particularites",
                        animal.Particularites == null ? DBNull.Value : animal.Particularites
                    );

                    commande.Parameters.AddWithValue(
                        "@date_deces",
                        animal.DateDeces == null ? DBNull.Value : animal.DateDeces
                    );

                    commande.Parameters.AddWithValue(
                        "@description",
                        animal.Description == null ? DBNull.Value : animal.Description
                    );

                    commande.Parameters.AddWithValue(
                        "@date_sterilisation",
                        animal.DateSterilisation == null ? DBNull.Value : animal.DateSterilisation
                    );

                    commande.Parameters.AddWithValue("@sterilise", animal.Sterilise);
                    commande.Parameters.AddWithValue("@date_naissance", animal.DateNaissance);

                    commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public void AjouterAnimalAvecEntree(Animal animal, AniEntree entree)
        {
            string sqlAnimal = @"
        INSERT INTO animal (
            identifiant,
            nom,
            type,
            sexe,
            particularites,
            date_deces,
            description,
            date_sterilisation,
            sterilise,
            date_naissance
        )
        VALUES (
            @identifiant,
            @nom,
            @type,
            @sexe,
            @particularites,
            @date_deces,
            @description,
            @date_sterilisation,
            @sterilise,
            @date_naissance
        )";

            string sqlEntree = @"
        INSERT INTO ani_entree
        (
            ani_identifiant,
            date_entree,
            raison,
            entree_contact
        )
        VALUES
        (
            @animal,
            @date,
            @raison,
            @contact
        )";

            try
            {
                OuvrirConnexion();

                using (NpgsqlTransaction transaction = connexion!.BeginTransaction())
                {
                    try
                    {
                        using (NpgsqlCommand commandeAnimal = new NpgsqlCommand(sqlAnimal, connexion, transaction))
                        {
                            commandeAnimal.Parameters.AddWithValue("@identifiant", animal.Identifiant);
                            commandeAnimal.Parameters.AddWithValue("@nom", animal.Nom);
                            commandeAnimal.Parameters.AddWithValue("@type", animal.Type);
                            commandeAnimal.Parameters.AddWithValue("@sexe", animal.Sexe);
                            commandeAnimal.Parameters.AddWithValue("@particularites", ValeurOuDbNull(animal.Particularites));
                            commandeAnimal.Parameters.AddWithValue("@date_deces", ValeurOuDbNull(animal.DateDeces));
                            commandeAnimal.Parameters.AddWithValue("@description", ValeurOuDbNull(animal.Description));
                            commandeAnimal.Parameters.AddWithValue("@date_sterilisation", ValeurOuDbNull(animal.DateSterilisation));
                            commandeAnimal.Parameters.AddWithValue("@sterilise", animal.Sterilise);
                            commandeAnimal.Parameters.AddWithValue("@date_naissance", animal.DateNaissance);

                            commandeAnimal.ExecuteNonQuery();
                        }

                        using (NpgsqlCommand commandeEntree = new NpgsqlCommand(sqlEntree, connexion, transaction))
                        {
                            commandeEntree.Parameters.AddWithValue("@animal", entree.AniIdentifiant);
                            commandeEntree.Parameters.AddWithValue("@date", entree.DateEntree);
                            commandeEntree.Parameters.AddWithValue("@raison", entree.Raison);
                            commandeEntree.Parameters.AddWithValue("@contact", entree.EntreeContact);

                            commandeEntree.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public Animal? GetAnimalParIdentifiant(string identifiant)
        {
            Animal? animal = null;

            string sql = "SELECT * FROM animal WHERE identifiant = @identifiant";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@identifiant", identifiant);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        if (lecteur.Read())
                        {
                            animal = CreerAnimalDepuisLecteur(lecteur);
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return animal;
        }

        public bool SupprimerAnimal(string identifiant)
        {
            string sql = "DELETE FROM animal WHERE identifiant = @identifiant";

            int nbLignes;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@identifiant", identifiant);
                    nbLignes = commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nbLignes > 0;
        }

        public void AjouterContact(Contact contact)
        {
            string sql = @"
        INSERT INTO contact (
            contact_identifiant,
            nom,
            prenom,
            rue,
            cp,
            localite,
            registre_national,
            gsm,
            telephone,
            email
        )
        VALUES (
            @id,
            @nom,
            @prenom,
            @rue,
            @cp,
            @localite,
            @registre,
            @gsm,
            @telephone,
            @email
        )";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@id", contact.ContactIdentifiant);
                    commande.Parameters.AddWithValue("@nom", contact.Nom);
                    commande.Parameters.AddWithValue("@prenom", contact.Prenom);
                    commande.Parameters.AddWithValue("@rue", contact.Rue);
                    commande.Parameters.AddWithValue("@cp", contact.Cp);
                    commande.Parameters.AddWithValue("@localite", contact.Localite);
                    commande.Parameters.AddWithValue("@registre", contact.RegistreNational);
                    commande.Parameters.AddWithValue("@gsm", contact.Gsm == null ? DBNull.Value : contact.Gsm);
                    commande.Parameters.AddWithValue("@telephone", contact.Telephone == null ? DBNull.Value : contact.Telephone);
                    commande.Parameters.AddWithValue("@email", contact.Email == null ? DBNull.Value : contact.Email);

                    commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public Contact? GetContactParIdentifiant(int id)
        {
            Contact? contact = null;

            string sql = "SELECT * FROM contact WHERE contact_identifiant = @id";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@id", id);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        if (lecteur.Read())
                        {
                            contact = new Contact(
                                (int)lecteur["contact_identifiant"],
                                lecteur["nom"].ToString()!,
                                lecteur["prenom"].ToString()!,
                                lecteur["rue"].ToString()!,
                                lecteur["cp"].ToString()!,
                                lecteur["localite"].ToString()!,
                                lecteur["registre_national"].ToString()!,
                                lecteur["gsm"] == DBNull.Value ? null : lecteur["gsm"].ToString(),
                                lecteur["telephone"] == DBNull.Value ? null : lecteur["telephone"].ToString(),
                                lecteur["email"] == DBNull.Value ? null : lecteur["email"].ToString()
                            );
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return contact;
        }

        public bool SupprimerContact(int id)
        {
            string sql = "DELETE FROM contact WHERE contact_identifiant = @id";

            int nbLignes;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@id", id);
                    nbLignes = commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nbLignes > 0;
        }

        public bool ModifierContact(Contact contact)
        {
            string sql = @"
        UPDATE contact
        SET nom = @nom,
            prenom = @prenom,
            rue = @rue,
            cp = @cp,
            localite = @localite,
            registre_national = @registre,
            gsm = @gsm,
            telephone = @telephone,
            email = @email
        WHERE contact_identifiant = @id";

            int nbLignes;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@id", contact.ContactIdentifiant);
                    commande.Parameters.AddWithValue("@nom", contact.Nom);
                    commande.Parameters.AddWithValue("@prenom", contact.Prenom);
                    commande.Parameters.AddWithValue("@rue", contact.Rue);
                    commande.Parameters.AddWithValue("@cp", contact.Cp);
                    commande.Parameters.AddWithValue("@localite", contact.Localite);
                    commande.Parameters.AddWithValue("@registre", contact.RegistreNational);
                    commande.Parameters.AddWithValue("@gsm", contact.Gsm == null ? DBNull.Value : contact.Gsm);
                    commande.Parameters.AddWithValue("@telephone", contact.Telephone == null ? DBNull.Value : contact.Telephone);
                    commande.Parameters.AddWithValue("@email", contact.Email == null ? DBNull.Value : contact.Email);

                    nbLignes = commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nbLignes > 0;
        }

        public void AjouterVaccin(Vaccin vaccin)
        {
            string sql =
                @"INSERT INTO vaccin
        (
            identifiant,
            nom
        )
        VALUES
        (
            @id,
            @nom
        )";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande =
                    new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue(
                        "@id",
                        vaccin.Identifiant
                    );

                    commande.Parameters.AddWithValue(
                        "@nom",
                        vaccin.Nom
                    );

                    commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public void AjouterAdoption(Adoption adoption)
        {
            string sql = @"
        INSERT INTO adoption
        (
            ani_identifiant,
            adop_contact,
            date_demande,
            statut
        )
        VALUES
        (
            @animal,
            @contact,
            @date,
            @statut
        )";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", adoption.AniIdentifiant);
                    commande.Parameters.AddWithValue("@contact", adoption.AdopContact);
                    commande.Parameters.AddWithValue("@date", adoption.DateDemande);
                    commande.Parameters.AddWithValue("@statut", adoption.Statut);

                    commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public bool ModifierStatutAdoption(
    string animal,
    int contact,
    DateTime dateDemande,
    string statut)
        {
            string sql = @"
        UPDATE adoption
        SET statut = @statut
        WHERE ani_identifiant = @animal
        AND adop_contact = @contact
        AND date_demande = @date";

            int nbLignes;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@statut", statut);
                    commande.Parameters.AddWithValue("@animal", animal);
                    commande.Parameters.AddWithValue("@contact", contact);
                    commande.Parameters.AddWithValue("@date", dateDemande);

                    nbLignes = commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nbLignes > 0;
        }

        public List<Adoption> GetAdoptions()
        {
            List<Adoption> adoptions = new List<Adoption>();

            string sql = "SELECT * FROM adoption ORDER BY date_demande DESC";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            Adoption adoption = new Adoption(
                                lecteur["ani_identifiant"].ToString()!,
                                (int)lecteur["adop_contact"],
                                LireDate(lecteur, "date_demande"),
                                lecteur["statut"].ToString()!
                            );

                            adoptions.Add(adoption);
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return adoptions;
        }

        public void AjouterFamilleAccueil(FamilleAccueil famille)
        {
            string sql = @"
        INSERT INTO famille_accueil
        (
            fa_ani_identifiant,
            fa_contact,
            date_debut,
            date_fin
        )
        VALUES
        (
            @animal,
            @contact,
            @debut,
            @fin
        )";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", famille.FaAniIdentifiant);
                    commande.Parameters.AddWithValue("@contact", famille.FaContact);
                    commande.Parameters.AddWithValue("@debut", famille.DateDebut);
                    commande.Parameters.AddWithValue("@fin", famille.DateFin == null ? DBNull.Value : famille.DateFin);

                    commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public List<FamilleAccueil> GetFamillesAccueilParAnimal(string identifiantAnimal)
        {
            List<FamilleAccueil> familles = new List<FamilleAccueil>();

            string sql = @"
        SELECT *
        FROM famille_accueil
        WHERE fa_ani_identifiant = @animal
        ORDER BY date_debut";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", identifiantAnimal);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            FamilleAccueil famille = new FamilleAccueil(
                                lecteur["fa_ani_identifiant"].ToString()!,
                                (int)lecteur["fa_contact"],
                                LireDate(lecteur, "date_debut"),
                                LireDateNullable(lecteur, "date_fin")
                            );

                            familles.Add(famille);
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return familles;
        }

        public void AjouterCouleur(Couleur couleur)
        {
            string sql = @"
        INSERT INTO couleur(col_identifiant, nom_couleur)
        VALUES(@id, @nom)";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@id", couleur.ColIdentifiant);
                    commande.Parameters.AddWithValue("@nom", couleur.NomCouleur);

                    commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public bool ModifierDescriptionAnimal(string identifiant, string description)
        {
            string sql = @"
        UPDATE animal
        SET description = @description
        WHERE identifiant = @identifiant";

            int nbLignes;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@description", description);
                    commande.Parameters.AddWithValue("@identifiant", identifiant);

                    nbLignes = commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nbLignes > 0;
        }

        public bool ModifierParticularitesAnimal(string identifiant, string particularites)
        {
            string sql = @"
        UPDATE animal
        SET particularites = @particularites
        WHERE identifiant = @identifiant";

            int nbLignes;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@particularites", particularites);
                    commande.Parameters.AddWithValue("@identifiant", identifiant);

                    nbLignes = commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nbLignes > 0;
        }

        public void AjouterCompatibiliteAnimal(
    string identifiantAnimal,
    int compatibiliteId,
    string valeur,
    string? description)
        {
            string sql = @"
        INSERT INTO ani_compatibilite
        (
            ani_identifiant,
            comp_identifiant,
            valeur,
            description
        )
        VALUES
        (
            @animal,
            @compatibilite,
            @valeur,
            @description
        )";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", identifiantAnimal);
                    commande.Parameters.AddWithValue("@compatibilite", compatibiliteId);
                    commande.Parameters.AddWithValue("@valeur", valeur);
                    commande.Parameters.AddWithValue("@description", description == null ? DBNull.Value : description);

                    commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public bool SupprimerDescriptionAnimal(string identifiant)
        {
            string sql = @"
        UPDATE animal
        SET description = NULL
        WHERE identifiant = @identifiant";

            int nbLignes;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@identifiant", identifiant);
                    nbLignes = commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nbLignes > 0;
        }

        public bool SupprimerParticularitesAnimal(string identifiant)
        {
            string sql = @"
        UPDATE animal
        SET particularites = NULL
        WHERE identifiant = @identifiant";

            int nbLignes;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@identifiant", identifiant);
                    nbLignes = commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nbLignes > 0;
        }

        public bool SupprimerCompatibiliteAnimal(string identifiantAnimal, int compatibiliteId)
        {
            string sql = @"
        DELETE FROM ani_compatibilite
        WHERE ani_identifiant = @animal
        AND comp_identifiant = @compatibilite";

            int nbLignes;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", identifiantAnimal);
                    commande.Parameters.AddWithValue("@compatibilite", compatibiliteId);

                    nbLignes = commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nbLignes > 0;
        }

        public List<Animal> GetAnimauxPresentsAuRefuge()
        {
            List<Animal> animaux = new List<Animal>();

            string sql = @"
        SELECT a.*
        FROM animal a
        WHERE a.date_deces IS NULL
        AND EXISTS (
            SELECT 1
            FROM ani_entree e
            WHERE e.ani_identifiant = a.identifiant
        )
        AND NOT EXISTS (
            SELECT 1
            FROM adoption ad
            WHERE ad.ani_identifiant = a.identifiant
            AND ad.statut = 'acceptee'
        )
        AND NOT EXISTS (
            SELECT 1
            FROM famille_accueil fa
            WHERE fa.fa_ani_identifiant = a.identifiant
            AND fa.date_fin IS NULL
        )
        AND (
            (
                SELECT MAX(e.date_entree)
                FROM ani_entree e
                WHERE e.ani_identifiant = a.identifiant
            )
            >= COALESCE(
                (
                    SELECT MAX(s.date_sortie)
                    FROM ani_sortie s
                    WHERE s.ani_identifiant = a.identifiant
                ),
                DATE '0001-01-01'
            )
        )
        ORDER BY a.nom";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            animaux.Add(CreerAnimalDepuisLecteur(lecteur));
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return animaux;
        }

        public List<FamilleAccueil> GetAnimauxParFamilleAccueil(int contact)
        {
            List<FamilleAccueil> familles = new List<FamilleAccueil>();

            string sql = @"
        SELECT *
        FROM famille_accueil
        WHERE fa_contact = @contact
        ORDER BY date_debut";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@contact", contact);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            FamilleAccueil famille = new FamilleAccueil(
                                lecteur["fa_ani_identifiant"].ToString()!,
                                (int)lecteur["fa_contact"],
                                LireDate(lecteur, "date_debut"),
                                LireDateNullable(lecteur, "date_fin")
                            );

                            familles.Add(famille);
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return familles;
        }

        public void AjouterCouleurAnimal(string animalId, int couleurId)
        {
            string sql = @"
        INSERT INTO animal_couleur(col_identifiant, ani_identifiant)
        VALUES(@couleur, @animal)";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@couleur", couleurId);
                    commande.Parameters.AddWithValue("@animal", animalId);

                    commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public void AjouterRole(Role role)
        {
            string sql = @"
        INSERT INTO role(rol_identifiant, rol_nom)
        VALUES(@id, @nom)";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@id", role.RolIdentifiant);
                    commande.Parameters.AddWithValue("@nom", role.RolNom);

                    commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public void AjouterRoleContact(int contactId, int roleId)
        {
            string sql = @"
        INSERT INTO personne_role(pers_identifiant, rol_identifiant)
        VALUES(@contact, @role)";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@contact", contactId);
                    commande.Parameters.AddWithValue("@role", roleId);

                    commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public void AjouterEntree(AniEntree entree)
        {
            string sql = @"
        INSERT INTO ani_entree
        (
            ani_identifiant,
            date_entree,
            raison,
            entree_contact
        )
        VALUES
        (
            @animal,
            @date,
            @raison,
            @contact
        )";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", entree.AniIdentifiant);
                    commande.Parameters.AddWithValue("@date", entree.DateEntree);
                    commande.Parameters.AddWithValue("@raison", entree.Raison);
                    commande.Parameters.AddWithValue("@contact", entree.EntreeContact);

                    commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public void AjouterSortie(AniSortie sortie)
        {
            string sql = @"
        INSERT INTO ani_sortie
        (
            ani_identifiant,
            date_sortie,
            raison,
            sortie_contact
        )
        VALUES
        (
            @animal,
            @date,
            @raison,
            @contact
        )";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", sortie.AniIdentifiant);
                    commande.Parameters.AddWithValue("@date", sortie.DateSortie);
                    commande.Parameters.AddWithValue("@raison", sortie.Raison);
                    commande.Parameters.AddWithValue("@contact", sortie.SortieContact);

                    commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public void AjouterVaccination(Vaccination vaccination)
        {
            string sql = @"
        INSERT INTO vaccination
        (
            vac_animal,
            id_vaccin,
            vaccination_date
        )
        VALUES
        (
            @animal,
            @vaccin,
            @date
        )";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", vaccination.VacAnimal);
                    commande.Parameters.AddWithValue("@vaccin", vaccination.IdVaccin);
                    commande.Parameters.AddWithValue("@date", vaccination.VaccinationDate);

                    commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public List<string> GetCouleursAnimal(string identifiantAnimal)
        {
            List<string> couleurs = new List<string>();

            string sql = @"
        SELECT c.nom_couleur
        FROM animal_couleur ac
        JOIN couleur c
            ON ac.col_identifiant = c.col_identifiant
        WHERE ac.ani_identifiant = @animal
        ORDER BY c.nom_couleur";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", identifiantAnimal);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            couleurs.Add(lecteur["nom_couleur"].ToString()!);
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return couleurs;
        }

        public List<string> GetVaccinationsAnimal(string identifiantAnimal)
        {
            List<string> vaccinations = new List<string>();

            string sql = @"
        SELECT v.nom, va.vaccination_date
        FROM vaccination va
        JOIN vaccin v
            ON va.id_vaccin = v.identifiant
        WHERE va.vac_animal = @animal
        ORDER BY va.vaccination_date";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", identifiantAnimal);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            DateTime dateVaccination = LireDate(lecteur, "vaccination_date");

                            string ligne =
                                lecteur["nom"].ToString()!
                                + " - "
                                + dateVaccination.ToString("yyyy-MM-dd");

                            vaccinations.Add(ligne);
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return vaccinations;
        }

        public List<string> GetCompatibilitesAnimal(string identifiantAnimal)
        {
            List<string> compatibilites = new List<string>();

            string sql = @"
        SELECT c.type, ac.valeur, ac.description
        FROM ani_compatibilite ac
        JOIN compatibilite c
            ON ac.comp_identifiant = c.identifiant
        WHERE ac.ani_identifiant = @animal
        ORDER BY c.type";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", identifiantAnimal);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            string type = lecteur["type"].ToString()!;
                            string valeur = lecteur["valeur"].ToString()!;

                            string? description =
                                lecteur["description"] == DBNull.Value
                                ? null
                                : lecteur["description"].ToString();

                            string ligne = type + " : " + valeur;

                            if (!string.IsNullOrWhiteSpace(description))
                            {
                                ligne += " (" + description + ")";
                            }

                            compatibilites.Add(ligne);
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return compatibilites;
        }

        public List<string> GetEntreesAnimal(string identifiantAnimal)
        {
            List<string> entrees = new List<string>();

            string sql = @"
        SELECT date_entree, raison, entree_contact
        FROM ani_entree
        WHERE ani_identifiant = @animal
        ORDER BY date_entree";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", identifiantAnimal);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            DateTime dateEntree = LireDate(lecteur, "date_entree");

                            string ligne =
                                dateEntree.ToString("yyyy-MM-dd")
                                + " - "
                                + lecteur["raison"].ToString()
                                + " | Contact : "
                                + lecteur["entree_contact"].ToString();

                            entrees.Add(ligne);
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return entrees;
        }

        public List<string> GetSortiesAnimal(string identifiantAnimal)
        {
            List<string> sorties = new List<string>();

            string sql = @"
        SELECT date_sortie, raison, sortie_contact
        FROM ani_sortie
        WHERE ani_identifiant = @animal
        ORDER BY date_sortie";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", identifiantAnimal);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            DateTime dateSortie = LireDate(lecteur, "date_sortie");

                            string ligne =
                                dateSortie.ToString("yyyy-MM-dd")
                                + " - "
                                + lecteur["raison"].ToString()
                                + " | Contact : "
                                + lecteur["sortie_contact"].ToString();

                            sorties.Add(ligne);
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return sorties;
        }

        public List<string> GetAdoptionsAnimal(string identifiantAnimal)
        {
            List<string> adoptions = new List<string>();

            string sql = @"
        SELECT adop_contact, date_demande, statut
        FROM adoption
        WHERE ani_identifiant = @animal
        ORDER BY date_demande";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", identifiantAnimal);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            DateTime dateDemande = LireDate(lecteur, "date_demande");

                            string ligne =
                                dateDemande.ToString("yyyy-MM-dd")
                                + " - "
                                + lecteur["statut"].ToString()
                                + " | Contact : "
                                + lecteur["adop_contact"].ToString();

                            adoptions.Add(ligne);
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return adoptions;
        }

        public bool ExisteSortieAnimalDate(string identifiantAnimal, DateTime dateSortie)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM ani_sortie
        WHERE ani_identifiant = @animal
        AND date_sortie = @date";

            int nombre;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", identifiantAnimal);
                    commande.Parameters.AddWithValue("@date", dateSortie);

                    nombre = Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nombre > 0;
        }

        public bool ModifierDateDecesAnimal(string identifiant, DateTime dateDeces)
        {
            string sql = @"
        UPDATE animal
        SET date_deces = @dateDeces
        WHERE identifiant = @identifiant";

            int nbLignes;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@dateDeces", dateDeces);
                    commande.Parameters.AddWithValue("@identifiant", identifiant);

                    nbLignes = commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nbLignes > 0;
        }

        public string GetEtatActuelAnimal(string identifiantAnimal)
        {
            string sql = @"
        SELECT
            a.date_deces,

            (
                SELECT MAX(e.date_entree)
                FROM ani_entree e
                WHERE e.ani_identifiant = a.identifiant
            ) AS derniere_entree,

            (
                SELECT MAX(s.date_sortie)
                FROM ani_sortie s
                WHERE s.ani_identifiant = a.identifiant
            ) AS derniere_sortie,

            (
                SELECT s.raison
                FROM ani_sortie s
                WHERE s.ani_identifiant = a.identifiant
                ORDER BY s.date_sortie DESC
                LIMIT 1
            ) AS derniere_raison_sortie

        FROM animal a
        WHERE a.identifiant = @animal";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", identifiantAnimal);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        if (!lecteur.Read())
                        {
                            return "inconnu";
                        }

                        if (lecteur["date_deces"] != DBNull.Value)
                        {
                            return "decede";
                        }

                        DateTime? derniereEntree = LireDateNullable(lecteur, "derniere_entree");

                        DateTime? derniereSortie = LireDateNullable(lecteur, "derniere_sortie");

                        string? raisonSortie = lecteur["derniere_raison_sortie"] == DBNull.Value
                            ? null
                            : lecteur["derniere_raison_sortie"].ToString();

                        if (derniereEntree == null)
                        {
                            return "sans_entree";
                        }

                        if (derniereSortie == null || derniereEntree >= derniereSortie)
                        {
                            return "au_refuge";
                        }

                        return raisonSortie switch
                        {
                            "adoption" => "adopte",
                            "famille_accueil" => "famille_accueil",
                            "retour_proprietaire" => "retour_proprietaire",
                            "deces_animal" => "decede",
                            _ => "sorti"
                        };
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        private bool Existe(string table, string colonne, object valeur)
        {
            Dictionary<string, HashSet<string>> colonnesAutorisees = new Dictionary<string, HashSet<string>>
            {
                ["animal"] = new HashSet<string> { "identifiant" },
                ["contact"] = new HashSet<string> { "contact_identifiant" },
                ["vaccin"] = new HashSet<string> { "identifiant" },
                ["couleur"] = new HashSet<string> { "col_identifiant" },
                ["role"] = new HashSet<string> { "rol_identifiant" },
                ["compatibilite"] = new HashSet<string> { "identifiant" }
            };

            if (!colonnesAutorisees.ContainsKey(table) || !colonnesAutorisees[table].Contains(colonne))
            {
                throw new ArgumentException("Table ou colonne non autorisée pour une vérification d'existence.");
            }

            string sql = $"SELECT COUNT(*) FROM {table} WHERE {colonne} = @valeur";

            int nombre;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@valeur", valeur);
                    nombre = Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nombre > 0;
        }

        public bool AnimalExiste(string identifiant)
        {
            return Existe("animal", "identifiant", identifiant);
        }

        public bool ContactExiste(int identifiant)
        {
            return Existe("contact", "contact_identifiant", identifiant);
        }

        public bool VaccinExiste(int identifiant)
        {
            return Existe("vaccin", "identifiant", identifiant);
        }

        public bool CouleurExiste(int identifiant)
        {
            return Existe("couleur", "col_identifiant", identifiant);
        }

        public bool RoleExiste(int identifiant)
        {
            return Existe("role", "rol_identifiant", identifiant);
        }

        public bool CompatibiliteExiste(int identifiant)
        {
            return Existe("compatibilite", "identifiant", identifiant);
        }

        public bool AnimalPossedeCouleur(string animalId, int couleurId)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM animal_couleur
        WHERE ani_identifiant = @animal
        AND col_identifiant = @couleur";

            int nombre;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", animalId);
                    commande.Parameters.AddWithValue("@couleur", couleurId);

                    nombre = Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nombre > 0;
        }

        public bool ContactPossedeRole(int contactId, int roleId)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM personne_role
        WHERE pers_identifiant = @contact
        AND rol_identifiant = @role";

            int nombre;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@contact", contactId);
                    commande.Parameters.AddWithValue("@role", roleId);

                    nombre = Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nombre > 0;
        }

        public bool AnimalPossedeCompatibilite(string animalId, int compatibiliteId)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM ani_compatibilite
        WHERE ani_identifiant = @animal
        AND comp_identifiant = @compatibilite";

            int nombre;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", animalId);
                    commande.Parameters.AddWithValue("@compatibilite", compatibiliteId);

                    nombre = Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nombre > 0;
        }

        public bool VaccinationExiste(string animalId, int vaccinId, DateTime dateVaccination)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM vaccination
        WHERE vac_animal = @animal
        AND id_vaccin = @vaccin
        AND vaccination_date = @date";

            int nombre;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", animalId);
                    commande.Parameters.AddWithValue("@vaccin", vaccinId);
                    commande.Parameters.AddWithValue("@date", dateVaccination);

                    nombre = Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nombre > 0;
        }

        public List<string> GetCompatibilitesDisponibles()
        {
            List<string> compatibilites = new List<string>();

            string sql = @"
        SELECT identifiant, type
        FROM compatibilite
        ORDER BY identifiant";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            string ligne =
                                lecteur["identifiant"].ToString()
                                + " - "
                                + lecteur["type"].ToString();

                            compatibilites.Add(ligne);
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return compatibilites;
        }

        public List<string> GetRolesContact(int contactId)
        {
            List<string> roles = new List<string>();

            string sql = @"
        SELECT r.rol_nom
        FROM personne_role pr
        JOIN role r ON pr.rol_identifiant = r.rol_identifiant
        WHERE pr.pers_identifiant = @contact
        ORDER BY r.rol_nom";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@contact", contactId);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            roles.Add(lecteur["rol_nom"].ToString()!);
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return roles;
        }

        public List<string> GetAdoptionsContact(int contactId)
        {
            List<string> adoptions = new List<string>();

            string sql = @"
        SELECT ani_identifiant, date_demande, statut
        FROM adoption
        WHERE adop_contact = @contact
        ORDER BY date_demande DESC";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@contact", contactId);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            DateTime dateDemande = LireDate(lecteur, "date_demande");

                            adoptions.Add(
                                lecteur["ani_identifiant"].ToString()
                                + " - "
                                + dateDemande.ToString("yyyy-MM-dd")
                                + " - "
                                + lecteur["statut"].ToString()
                            );
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return adoptions;
        }

        public List<string> GetFamillesAccueilContact(int contactId)
        {
            List<string> familles = new List<string>();

            string sql = @"
        SELECT fa_ani_identifiant, date_debut, date_fin
        FROM famille_accueil
        WHERE fa_contact = @contact
        ORDER BY date_debut DESC";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@contact", contactId);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            DateTime dateDebut = LireDate(lecteur, "date_debut");

                            string dateFin = lecteur["date_fin"] == DBNull.Value
                                ? "en cours"
                                : LireDate(lecteur, "date_fin").ToString("yyyy-MM-dd");

                            familles.Add(
                                lecteur["fa_ani_identifiant"].ToString()
                                + " - début : "
                                + dateDebut.ToString("yyyy-MM-dd")
                                + " - fin : "
                                + dateFin
                            );
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return familles;
        }

        public List<string> GetAnimauxVaccinesParVaccin(int vaccinId)
        {
            List<string> animaux = new List<string>();

            string sql = @"
        SELECT a.identifiant, a.nom, v.vaccination_date
        FROM vaccination v
        JOIN animal a ON v.vac_animal = a.identifiant
        WHERE v.id_vaccin = @vaccin
        ORDER BY v.vaccination_date DESC";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@vaccin", vaccinId);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            DateTime dateVaccination = LireDate(lecteur, "vaccination_date");

                            animaux.Add(
                                lecteur["identifiant"].ToString()
                                + " - "
                                + lecteur["nom"].ToString()
                                + " - "
                                + dateVaccination.ToString("yyyy-MM-dd")
                            );
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return animaux;
        }

        public Vaccin? GetVaccinParIdentifiant(int id)
        {
            Vaccin? vaccin = null;

            string sql = @"
        SELECT identifiant, nom
        FROM vaccin
        WHERE identifiant = @id";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@id", id);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        if (lecteur.Read())
                        {
                            vaccin = new Vaccin(
                                (int)lecteur["identifiant"],
                                lecteur["nom"].ToString()!
                            );
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return vaccin;
        }

        public Adoption? GetAdoption(string animalId, int contactId, DateTime dateDemande)
        {
            Adoption? adoption = null;

            string sql = @"
        SELECT ani_identifiant, adop_contact, date_demande, statut
        FROM adoption
        WHERE ani_identifiant = @animal
        AND adop_contact = @contact
        AND date_demande = @date";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", animalId);
                    commande.Parameters.AddWithValue("@contact", contactId);
                    commande.Parameters.AddWithValue("@date", dateDemande);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        if (lecteur.Read())
                        {
                            adoption = new Adoption(
                                lecteur["ani_identifiant"].ToString()!,
                                (int)lecteur["adop_contact"],
                                LireDate(lecteur, "date_demande"),
                                lecteur["statut"].ToString()!
                            );
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return adoption;
        }

        public FamilleAccueil? GetFamilleAccueil(string animalId, int contactId, DateTime dateDebut)
        {
            FamilleAccueil? famille = null;

            string sql = @"
        SELECT fa_ani_identifiant, fa_contact, date_debut, date_fin
        FROM famille_accueil
        WHERE fa_ani_identifiant = @animal
        AND fa_contact = @contact
        AND date_debut = @dateDebut";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", animalId);
                    commande.Parameters.AddWithValue("@contact", contactId);
                    commande.Parameters.AddWithValue("@dateDebut", dateDebut);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        if (lecteur.Read())
                        {
                            famille = new FamilleAccueil(
                                lecteur["fa_ani_identifiant"].ToString()!,
                                (int)lecteur["fa_contact"],
                                LireDate(lecteur, "date_debut"),
                                LireDateNullable(lecteur, "date_fin")
                            );
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return famille;
        }

        public Couleur? GetCouleurParIdentifiant(int id)
        {
            Couleur? couleur = null;

            string sql = @"
        SELECT col_identifiant, nom_couleur
        FROM couleur
        WHERE col_identifiant = @id";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@id", id);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        if (lecteur.Read())
                        {
                            couleur = new Couleur(
                                (int)lecteur["col_identifiant"],
                                lecteur["nom_couleur"].ToString()!
                            );
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return couleur;
        }

        public List<string> GetAnimauxParCouleur(int couleurId)
        {
            List<string> animaux = new List<string>();

            string sql = @"
        SELECT a.identifiant, a.nom, a.type
        FROM animal_couleur ac
        JOIN animal a ON ac.ani_identifiant = a.identifiant
        WHERE ac.col_identifiant = @couleur
        ORDER BY a.nom";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@couleur", couleurId);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            animaux.Add(
                                lecteur["identifiant"].ToString()
                                + " - "
                                + lecteur["nom"].ToString()
                                + " - "
                                + lecteur["type"].ToString()
                            );
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return animaux;
        }

        public Role? GetRoleParIdentifiant(int id)
        {
            Role? role = null;

            string sql = @"
        SELECT rol_identifiant, rol_nom
        FROM role
        WHERE rol_identifiant = @id";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@id", id);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        if (lecteur.Read())
                        {
                            role = new Role(
                                (int)lecteur["rol_identifiant"],
                                lecteur["rol_nom"].ToString()!
                            );
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return role;
        }

        public List<string> GetContactsParRole(int roleId)
        {
            List<string> contacts = new List<string>();

            string sql = @"
        SELECT c.contact_identifiant, c.nom, c.prenom
        FROM personne_role pr
        JOIN contact c ON pr.pers_identifiant = c.contact_identifiant
        WHERE pr.rol_identifiant = @role
        ORDER BY c.nom, c.prenom";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@role", roleId);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            contacts.Add(
                                lecteur["contact_identifiant"].ToString()
                                + " - "
                                + lecteur["nom"].ToString()
                                + " "
                                + lecteur["prenom"].ToString()
                            );
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return contacts;
        }

        public Compatibilite? GetCompatibiliteParIdentifiant(int id)
        {
            Compatibilite? compatibilite = null;

            string sql = @"
        SELECT identifiant, type
        FROM compatibilite
        WHERE identifiant = @id";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@id", id);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        if (lecteur.Read())
                        {
                            compatibilite = new Compatibilite(
                                (int)lecteur["identifiant"],
                                lecteur["type"].ToString()!
                            );
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return compatibilite;
        }

        public List<string> GetAnimauxParCompatibilite(int compatibiliteId)
        {
            List<string> animaux = new List<string>();

            string sql = @"
        SELECT a.identifiant, a.nom, ac.valeur, ac.description
        FROM ani_compatibilite ac
        JOIN animal a ON ac.ani_identifiant = a.identifiant
        WHERE ac.comp_identifiant = @compatibilite
        ORDER BY a.nom";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@compatibilite", compatibiliteId);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            string ligne =
                                lecteur["identifiant"].ToString()
                                + " - "
                                + lecteur["nom"].ToString()
                                + " - "
                                + lecteur["valeur"].ToString();

                            if (lecteur["description"] != DBNull.Value)
                            {
                                ligne += " (" + lecteur["description"].ToString() + ")";
                            }

                            animaux.Add(ligne);
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return animaux;
        }

        public List<string> GetBlocagesSuppressionAnimal(string animalId)
        {
            List<string> blocages = new List<string>();

            string[] tables =
            {
        "ani_entree",
        "ani_sortie",
        "adoption",
        "famille_accueil",
        "vaccination",
        "animal_couleur",
        "ani_compatibilite"
    };

            string[] colonnes =
            {
        "ani_identifiant",
        "ani_identifiant",
        "ani_identifiant",
        "fa_ani_identifiant",
        "vac_animal",
        "ani_identifiant",
        "ani_identifiant"
    };

            string[] noms =
            {
        "entrées",
        "sorties",
        "adoptions",
        "familles d'accueil",
        "vaccinations",
        "couleurs",
        "compatibilités"
    };

            try
            {
                OuvrirConnexion();

                for (int i = 0; i < tables.Length; i++)
                {
                    string sql = $"SELECT COUNT(*) FROM {tables[i]} WHERE {colonnes[i]} = @animal";

                    using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                    {
                        commande.Parameters.AddWithValue("@animal", animalId);

                        int nombre = Convert.ToInt32(commande.ExecuteScalar());

                        if (nombre > 0)
                        {
                            blocages.Add(nombre + " " + noms[i]);
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return blocages;
        }

        public List<string> GetBlocagesSuppressionContact(int contactId)
        {
            List<string> blocages = new List<string>();

            string[] tables =
            {
        "ani_entree",
        "ani_sortie",
        "adoption",
        "famille_accueil",
        "personne_role"
    };

            string[] colonnes =
            {
        "entree_contact",
        "sortie_contact",
        "adop_contact",
        "fa_contact",
        "pers_identifiant"
    };

            string[] noms =
            {
        "entrées",
        "sorties",
        "adoptions",
        "familles d'accueil",
        "rôles"
    };

            try
            {
                OuvrirConnexion();

                for (int i = 0; i < tables.Length; i++)
                {
                    string sql = $"SELECT COUNT(*) FROM {tables[i]} WHERE {colonnes[i]} = @contact";

                    using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                    {
                        commande.Parameters.AddWithValue("@contact", contactId);

                        int nombre = Convert.ToInt32(commande.ExecuteScalar());

                        if (nombre > 0)
                        {
                            blocages.Add(nombre + " " + noms[i]);
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return blocages;
        }

        public int Compter(string table)
        {
            string sql = $"SELECT COUNT(*) FROM {table}";

            int nombre;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    nombre = Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nombre;
        }

        public int CompterAnimauxParEtat(string etatRecherche)
        {
            int compteur = 0;

            List<Animal> animaux = GetAnimaux();

            foreach (Animal animal in animaux)
            {
                string etat = GetEtatActuelAnimal(animal.Identifiant);

                if (etat == etatRecherche)
                {
                    compteur++;
                }
            }

            return compteur;
        }

        public List<string> VerifierCoherenceRefuge()
        {
            List<string> problemes = new List<string>();

            try
            {
                OuvrirConnexion();

                // 1. Plusieurs familles d'accueil actives pour le même animal
                string sqlFamillesActives = @"
            SELECT fa_ani_identifiant, COUNT(*)
            FROM famille_accueil
            WHERE date_fin IS NULL
            GROUP BY fa_ani_identifiant
            HAVING COUNT(*) > 1";

                using (NpgsqlCommand commande = new NpgsqlCommand(sqlFamillesActives, connexion))
                {
                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            problemes.Add(
                                "Animal " + lecteur["fa_ani_identifiant"] +
                                " possède plusieurs familles d'accueil actives."
                            );
                        }
                    }
                }

                // 2. Plusieurs adoptions acceptées pour le même animal
                string sqlAdoptionsAcceptees = @"
            SELECT ani_identifiant, COUNT(*)
            FROM adoption
            WHERE statut = 'acceptee'
            GROUP BY ani_identifiant
            HAVING COUNT(*) > 1";

                using (NpgsqlCommand commande = new NpgsqlCommand(sqlAdoptionsAcceptees, connexion))
                {
                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            problemes.Add(
                                "Animal " + lecteur["ani_identifiant"] +
                                " possède plusieurs adoptions acceptées."
                            );
                        }
                    }
                }

                // 3. Sortie sans entrée
                string sqlSortieSansEntree = @"
            SELECT s.ani_identifiant
            FROM ani_sortie s
            WHERE NOT EXISTS (
                SELECT 1
                FROM ani_entree e
                WHERE e.ani_identifiant = s.ani_identifiant
            )";

                using (NpgsqlCommand commande = new NpgsqlCommand(sqlSortieSansEntree, connexion))
                {
                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            problemes.Add(
                                "Animal " + lecteur["ani_identifiant"] +
                                " possède une sortie sans entrée."
                            );
                        }
                    }
                }

                // 4. Date décès avant date naissance
                string sqlDecesAvantNaissance = @"
            SELECT identifiant
            FROM animal
            WHERE date_deces IS NOT NULL
            AND date_deces < date_naissance";

                using (NpgsqlCommand commande = new NpgsqlCommand(sqlDecesAvantNaissance, connexion))
                {
                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            problemes.Add(
                                "Animal " + lecteur["identifiant"] +
                                " possède une date de décès avant sa date de naissance."
                            );
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return problemes;
        }

        public List<Animal> RechercherAnimauxParNom(string nomRecherche)
        {
            List<Animal> animaux = new List<Animal>();

            string sql = @"
        SELECT *
        FROM animal
        WHERE LOWER(nom) LIKE LOWER(@nom)
        ORDER BY nom";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@nom", "%" + nomRecherche + "%");

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            animaux.Add(CreerAnimalDepuisLecteur(lecteur));
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return animaux;
        }

        public List<Contact> RechercherContactsParNom(string recherche)
        {
            List<Contact> contacts = new List<Contact>();

            string sql = @"
        SELECT *
        FROM contact
        WHERE LOWER(nom) LIKE LOWER(@recherche)
        OR LOWER(prenom) LIKE LOWER(@recherche)
        ORDER BY nom, prenom";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@recherche", "%" + recherche + "%");

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            Contact contact = new Contact(
                                (int)lecteur["contact_identifiant"],
                                lecteur["nom"].ToString()!,
                                lecteur["prenom"].ToString()!,
                                lecteur["rue"].ToString()!,
                                lecteur["cp"].ToString()!,
                                lecteur["localite"].ToString()!,
                                lecteur["registre_national"].ToString()!,
                                lecteur["gsm"] == DBNull.Value ? null : lecteur["gsm"].ToString(),
                                lecteur["telephone"] == DBNull.Value ? null : lecteur["telephone"].ToString(),
                                lecteur["email"] == DBNull.Value ? null : lecteur["email"].ToString()
                            );

                            contacts.Add(contact);
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return contacts;
        }

        public DateTime? GetDerniereDateEntreeAnimal(string animalId)
        {
            DateTime? date = null;

            string sql = @"
        SELECT MAX(date_entree) AS derniere_entree
        FROM ani_entree
        WHERE ani_identifiant = @animal";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", animalId);

                    object? resultat = commande.ExecuteScalar();

                    if (resultat != null && resultat != DBNull.Value)
                    {
                        date = ((DateOnly)resultat).ToDateTime(TimeOnly.MinValue);
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return date;
        }
        public bool AnimalAFamilleAccueilActive(string animalId)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM famille_accueil
        WHERE fa_ani_identifiant = @animal
        AND date_fin IS NULL";

            int nombre;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", animalId);
                    nombre = Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nombre > 0;
        }

        public bool AnimalAAdoptionAcceptee(string animalId)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM adoption
        WHERE ani_identifiant = @animal
        AND statut = 'acceptee'";

            int nombre;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", animalId);
                    nombre = Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nombre > 0;
        }

        public bool CloturerFamilleAccueil(string animalId, DateTime dateFin)
        {
            string sql = @"
        UPDATE famille_accueil
        SET date_fin = @dateFin
        WHERE fa_ani_identifiant = @animal
        AND date_fin IS NULL";

            int nbLignes;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", animalId);
                    commande.Parameters.AddWithValue("@dateFin", dateFin);

                    nbLignes = commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nbLignes > 0;
        }

        public int CompterVaccinationsParVaccin(int vaccinId)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM vaccination
        WHERE id_vaccin = @vaccin";

            int nombre;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@vaccin", vaccinId);
                    nombre = Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nombre;
        }

        public bool SupprimerVaccin(int id)
        {
            string sql = "DELETE FROM vaccin WHERE identifiant = @id";

            int nbLignes;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@id", id);
                    nbLignes = commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nbLignes > 0;
        }

        public int CompterAnimauxParCouleur(int couleurId)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM animal_couleur
        WHERE col_identifiant = @couleur";

            int nombre;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@couleur", couleurId);
                    nombre = Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nombre;
        }

        public bool SupprimerCouleur(int id)
        {
            string sql = "DELETE FROM couleur WHERE col_identifiant = @id";

            int nbLignes;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@id", id);
                    nbLignes = commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nbLignes > 0;
        }

        public int CompterContactsParRole(int roleId)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM personne_role
        WHERE rol_identifiant = @role";

            int nombre;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@role", roleId);
                    nombre = Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nombre;
        }

        public bool SupprimerRole(int id)
        {
            string sql = "DELETE FROM role WHERE rol_identifiant = @id";

            int nbLignes;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@id", id);
                    nbLignes = commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nbLignes > 0;
        }

        public int CompterAnimauxParCompatibilite(int compatibiliteId)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM ani_compatibilite
        WHERE comp_identifiant = @compatibilite";

            int nombre;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@compatibilite", compatibiliteId);
                    nombre = Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nombre;
        }

        public bool SupprimerCompatibilite(int id)
        {
            string sql = "DELETE FROM compatibilite WHERE identifiant = @id";

            int nbLignes;

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@id", id);
                    nbLignes = commande.ExecuteNonQuery();
                }
            }
            finally
            {
                FermerConnexion();
            }

            return nbLignes > 0;
        }

        public List<Contact> GetContacts()
        {
            List<Contact> contacts = new List<Contact>();

            string sql = "SELECT * FROM contact ORDER BY nom, prenom";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                {
                    while (lecteur.Read())
                    {
                        contacts.Add(new Contact(
                            (int)lecteur["contact_identifiant"],
                            lecteur["nom"].ToString()!,
                            lecteur["prenom"].ToString()!,
                            lecteur["rue"].ToString()!,
                            lecteur["cp"].ToString()!,
                            lecteur["localite"].ToString()!,
                            lecteur["registre_national"].ToString()!,
                            lecteur["gsm"] == DBNull.Value ? null : lecteur["gsm"].ToString(),
                            lecteur["telephone"] == DBNull.Value ? null : lecteur["telephone"].ToString(),
                            lecteur["email"] == DBNull.Value ? null : lecteur["email"].ToString()
                        ));
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return contacts;
        }

        public List<Vaccin> GetVaccins()
        {
            List<Vaccin> vaccins = new List<Vaccin>();

            string sql = "SELECT identifiant, nom FROM vaccin ORDER BY nom";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                {
                    while (lecteur.Read())
                    {
                        vaccins.Add(new Vaccin(
                            (int)lecteur["identifiant"],
                            lecteur["nom"].ToString()!
                        ));
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return vaccins;
        }

        public List<Couleur> GetCouleurs()
        {
            List<Couleur> couleurs = new List<Couleur>();

            string sql = "SELECT col_identifiant, nom_couleur FROM couleur ORDER BY nom_couleur";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                {
                    while (lecteur.Read())
                    {
                        couleurs.Add(new Couleur(
                            (int)lecteur["col_identifiant"],
                            lecteur["nom_couleur"].ToString()!
                        ));
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return couleurs;
        }

        public List<Role> GetRoles()
        {
            List<Role> roles = new List<Role>();

            string sql = "SELECT rol_identifiant, rol_nom FROM role ORDER BY rol_nom";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                {
                    while (lecteur.Read())
                    {
                        roles.Add(new Role(
                            (int)lecteur["rol_identifiant"],
                            lecteur["rol_nom"].ToString()!
                        ));
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return roles;
        }

        public List<Compatibilite> GetCompatibilites()
        {
            List<Compatibilite> compatibilites = new List<Compatibilite>();

            string sql = "SELECT identifiant, type FROM compatibilite ORDER BY type";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                {
                    while (lecteur.Read())
                    {
                        compatibilites.Add(new Compatibilite(
                            (int)lecteur["identifiant"],
                            lecteur["type"].ToString()!
                        ));
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return compatibilites;
        }

        public List<string> GetHistoriqueAnimal(string animalId)
        {
            List<string> historique = new List<string>();

            string sql = @"
        SELECT date_evenement, type_evenement, description
        FROM (
            SELECT 
                date_entree AS date_evenement,
                'Entrée refuge' AS type_evenement,
                raison || ' | Contact : ' || entree_contact AS description
            FROM ani_entree
            WHERE ani_identifiant = @animal

            UNION ALL

            SELECT 
                date_sortie AS date_evenement,
                'Sortie refuge' AS type_evenement,
                raison || ' | Contact : ' || sortie_contact AS description
            FROM ani_sortie
            WHERE ani_identifiant = @animal

            UNION ALL

            SELECT 
                vaccination_date AS date_evenement,
                'Vaccination' AS type_evenement,
                'Vaccin : ' || v.nom AS description
            FROM vaccination va
            JOIN vaccin v ON va.id_vaccin = v.identifiant
            WHERE va.vac_animal = @animal

            UNION ALL

            SELECT 
                date_demande AS date_evenement,
                'Adoption' AS type_evenement,
                statut || ' | Contact : ' || adop_contact AS description
            FROM adoption
            WHERE ani_identifiant = @animal

            UNION ALL

            SELECT 
                date_debut AS date_evenement,
                'Famille accueil' AS type_evenement,
                'Début | Contact : ' || fa_contact AS description
            FROM famille_accueil
            WHERE fa_ani_identifiant = @animal

            UNION ALL

            SELECT 
                date_fin AS date_evenement,
                'Famille accueil' AS type_evenement,
                'Fin | Contact : ' || fa_contact AS description
            FROM famille_accueil
            WHERE fa_ani_identifiant = @animal
            AND date_fin IS NOT NULL
        ) h
        ORDER BY date_evenement, type_evenement";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", animalId);

                    using (NpgsqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            DateTime dateEvenement =
                                ((DateOnly)lecteur["date_evenement"])
                                .ToDateTime(TimeOnly.MinValue);

                            historique.Add(
                                dateEvenement.ToString("yyyy-MM-dd")
                                + " - "
                                + lecteur["type_evenement"].ToString()
                                + " - "
                                + lecteur["description"].ToString()
                            );
                        }
                    }
                }
            }
            finally
            {
                FermerConnexion();
            }

            return historique;
        }

        public int CompterVaccinationsAnimal(string animalId)
        {
            string sql = "SELECT COUNT(*) FROM vaccination WHERE vac_animal = @animal";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", animalId);
                    return Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public int CompterAdoptionsAnimal(string animalId)
        {
            string sql = "SELECT COUNT(*) FROM adoption WHERE ani_identifiant = @animal";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", animalId);
                    return Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public int CompterFamillesAccueilAnimal(string animalId)
        {
            string sql = "SELECT COUNT(*) FROM famille_accueil WHERE fa_ani_identifiant = @animal";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", animalId);
                    return Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public int CompterCompatibilitesAnimal(string animalId)
        {
            string sql = "SELECT COUNT(*) FROM ani_compatibilite WHERE ani_identifiant = @animal";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", animalId);
                    return Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public int CompterCouleursAnimal(string animalId)
        {
            string sql = "SELECT COUNT(*) FROM animal_couleur WHERE ani_identifiant = @animal";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@animal", animalId);
                    return Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public int CompterRolesContact(int contactId)
        {
            string sql = "SELECT COUNT(*) FROM personne_role WHERE pers_identifiant = @contact";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@contact", contactId);
                    return Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public int CompterAdoptionsContact(int contactId)
        {
            string sql = "SELECT COUNT(*) FROM adoption WHERE adop_contact = @contact";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@contact", contactId);
                    return Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }
        }

        public int CompterFamillesContact(int contactId)
        {
            string sql = "SELECT COUNT(*) FROM famille_accueil WHERE fa_contact = @contact";

            try
            {
                OuvrirConnexion();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue("@contact", contactId);
                    return Convert.ToInt32(commande.ExecuteScalar());
                }
            }
            finally
            {
                FermerConnexion();
            }
        }
    }
}
