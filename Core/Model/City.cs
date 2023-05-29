namespace Model
{
    public readonly record struct City(string CityName, string CountryName);
    public readonly record struct CityWithPopulation(string CityName, string CountryName, int Population);
    public readonly record struct CityWithPopulationArea(string CityName, string CountryName, int Population, double Area);
    public readonly record struct CityWithPopulationAreaMayor(string CityName, string CountryName, int Population, double Area, string MayorName, int ElectedYear);
    public readonly record struct CityWithPopulationAreaMayorCountry(string CityName, int CityPopulation, double CityArea, string MayorName, int ElectedYear, string CountryName, string DialingCode, string PrimeMinister, string Currency, double CountryPopulation, string OfficialLanguage);
}
