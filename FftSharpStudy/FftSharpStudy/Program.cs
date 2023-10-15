Console.WriteLine(Math.Pow(12200, 2));

double[] frequencies = {20, 1000, 20000};
foreach (double f in frequencies)
{
    Console.WriteLine($"A({f} Hz) = {CalculateAWeighting(f)} dB");
}

static double CalculateAWeighting(double frequency)
{
    double f = frequency;
    double numerator = 1.2589 * Math.Pow(f, 4);
    double denominator = ((Math.Pow(f, 2) + Math.Pow(20.6, 2)) * Math.Sqrt((Math.Pow(f, 2) + Math.Pow(107.7, 2))*(Math.Pow(f, 2) + Math.Pow(737.9, 2))) * (Math.Pow(f, 2) + Math.Pow(12194, 2)));
    double aWeighting = 20 * Math.Log10(numerator / denominator) + 2.00;
    aWeighting = 20 * Math.Log10((Math.Pow(12200, 2) * Math.Pow(f, 4)) / ((Math.Pow(f, 2) + Math.Pow(20.6, 2)) *
                                                                          (Math.Pow(f, 2) + Math.Pow(107.7, 2)) *
                                                                          (Math.Pow(f, 2) + Math.Pow(737.9, 2)) *
                                                                          (Math.Pow((Math.Pow(f, 2) + Math.Pow(12200, 2)), 2))));
    return aWeighting;
}