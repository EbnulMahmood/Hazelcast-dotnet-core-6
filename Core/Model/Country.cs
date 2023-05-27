namespace Model
{
    public readonly record struct Country(string CountryName, string DialingCode, string PrimeMinister, string Currency, double Population, string OfficialLanguage);
}
