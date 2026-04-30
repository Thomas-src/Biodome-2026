namespace BioDomes.Domains.Core
{
    public enum BiodomeEtat
    {
        Critique,
        Instable,
        Optimal
    }

    public static class BiodomeEtatCompanion
    {
        public static BiodomeEtat DeterminerEtat(Biodome biodome)
        {
            // Etat de base (Uc 5)
            BiodomeEtat etat = DeterminerEtatTemperature(biodome.Temperature);

            // Etat du réseau trophique (Uc 7)
            if (!NutrientChainBalanced(biodome))
            {
                etat = DegraderEtat(etat);
            }

            return etat;
        }

        private static BiodomeEtat DeterminerEtatTemperature(double temperature)
        {
            if (temperature < -20 || temperature > 40)
                return BiodomeEtat.Critique;

            if (temperature < 0 || temperature > 30)
                return BiodomeEtat.Instable;

            return BiodomeEtat.Optimal;
        }

        private static BiodomeEtat DegraderEtat(BiodomeEtat etat)
        {
            return etat switch
            {
                BiodomeEtat.Optimal => BiodomeEtat.Instable,
                BiodomeEtat.Instable => BiodomeEtat.Critique,
                _ => BiodomeEtat.Critique
            };
        }

        private static bool NutrientChainBalanced(Biodome biodome)
        {
            IList<Species> allSpecies = biodome.Species.ToList();

            // Clé = le nom de la proie, valeur = le poids total demandé par tous les prédateurs
            IDictionary<string, double> demandByPredatorMap = new Dictionary<string, double>();

            foreach (Species predator in allSpecies)
            {
                if (predator.DietType == DietType.Photosynthèse)
                    continue;

                IList<SpeciesClassification> eatableClasses = NutrientChain.CanEat(predator.DietType);

                IList<Species> availablePredators = allSpecies
                    .Where(p =>
                        eatableClasses.Contains(p.SpeciesClassification) &&
                        p.Nom != predator.Nom)
                    .ToList();

                // Si aucune proie n'est disponible / trouvé
                if (!availablePredators.Any())
                {
                    return false;
                }

                double predatorWeight = predator.Poids * predator.Quantity;

                // Pour répartir la demande du prédateur équitablement sur ses proies
                double demandPerPredator = predatorWeight / availablePredators.Count;

                foreach (Species prey in availablePredators)
                {
                    if (!demandByPredatorMap.ContainsKey(prey.Nom))
                        demandByPredatorMap[prey.Nom] = 0;

                    demandByPredatorMap[prey.Nom] += demandPerPredator;
                }
            }

            // Vérifie que chaque espece à suffisament de proies
            foreach (var (preyName, totalDemand) in demandByPredatorMap)
            {
                Species pactual = allSpecies.First(s => s.Nom == preyName);
                double availableWeight = pactual.Poids * pactual.Quantity;

                if (availableWeight < totalDemand)
                {
                    return false;
                }
            }

            return true;
        }


        // Le calcul de pression et d'humidité n'intervient pas dans les UC5 et 7
        // On l'utilise uniquement pour vérifié la cohérence et qu'une humidité soit impossible vis a vis sa température
        private static double PressionMax(double t)
        {
            return Math.Pow(6.112 * Math.E, (17.67 * t) / (t + 243.5));
        }

        private static double HumiditeMax(double t)
        {
            return (PressionMax(t) * 216.7) / 273.15;
        }
    }
}