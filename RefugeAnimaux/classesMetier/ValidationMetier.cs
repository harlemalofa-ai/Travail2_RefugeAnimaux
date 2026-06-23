namespace RefugeAnimaux.classesMetier
{
    public static class ValidationMetier
    {
        public const string EtatAuRefuge = "au_refuge";
        public const string EtatFamilleAccueil = "famille_accueil";
        public const string EtatAdopte = "adopte";
        public const string EtatDecede = "decede";
        public const string EtatRetourProprietaire = "retour_proprietaire";
        public const string EtatSansEntree = "sans_entree";

        public static bool EstDisponibleAuRefuge(string etat)
        {
            return etat == EtatAuRefuge;
        }

        public static bool PeutAjouterAdoption(string etat)
        {
            return EstDisponibleAuRefuge(etat);
        }

        public static bool PeutAjouterFamilleAccueil(string etat)
        {
            return EstDisponibleAuRefuge(etat);
        }

        public static bool PeutAjouterSortie(string etat)
        {
            return EstDisponibleAuRefuge(etat);
        }

        public static bool PeutAjouterEntree(string etat)
        {
            return etat == EtatFamilleAccueil;
        }

        public static bool PeutAjouterVaccination(string etat)
        {
            return etat != EtatDecede
                && etat != EtatRetourProprietaire;
        }

        public static void VerifierActionAnimal(string etat, string action, Func<string, bool> regle)
        {
            if (!regle(etat))
            {
                throw new InvalidOperationException(MessageActionImpossible(etat, action));
            }
        }

        public static string MessageActionImpossible(string etat, string action)
        {
            string etatLisible = etat switch
            {
                EtatAuRefuge => "au refuge",
                EtatFamilleAccueil => "en famille d'accueil",
                EtatAdopte => "adopté",
                EtatDecede => "décédé",
                EtatRetourProprietaire => "retourné chez son propriétaire",
                EtatSansEntree => "sans entrée enregistrée",
                _ => "dans un état inconnu"
            };

            return $"Action impossible : l'animal est actuellement {etatLisible}. Action demandée : {action}.";
        }
    }
}
