namespace BioDomes.Domains.Utils;

public class HumidityCalculator
{
    public static double MaxHumidityOf(double temp)
    {
        return (216.7 * 6.112 * Math.Exp((17.67 * temp) / (temp + 243.5)))
               / (temp + 273.15);
    }
}