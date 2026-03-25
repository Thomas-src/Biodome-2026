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
        public static BiodomeEtat DeterminerEtat(double temperature, double humiditeAbsolue)
        {
            double hMax = HumiditeMax(temperature);

            if (humiditeAbsolue > hMax || temperature < -20 || temperature > 40)
            {
                return BiodomeEtat.Critique;
            }

            if (temperature < 0 || temperature > 30)
            {
                return BiodomeEtat.Instable;
            }

            return BiodomeEtat.Optimal;
        }

        private static double PressionMax(double t)
        {
            return Math.Pow(6.112 * Math.E, (17.67 * t)/(t+243.5));
        }
        
        private static double HumiditeMax(double t)
        {
            return (PressionMax(t) * 216.7)/273.15;
        }
    }
    
}