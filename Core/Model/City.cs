namespace Model
{
    public record struct City(string CityName, string CountryName);
    public record struct CityWithPopulation(string CityName, string CountryName, int Population);
    public record struct CityWithPopulationArea(string CityName, string CountryName, int Population, double Area);
    public record struct CityWithPopulationAreaMayor(string CityName, string CountryName, int Population, double Area, string MayorName, int Age);
}
