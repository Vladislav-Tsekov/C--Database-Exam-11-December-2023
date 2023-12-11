namespace Cadastre.Other
{
    public class Validate
    {
        //District
        public const int DistrictNameMinLength = 2;
        public const int DistrictNameMaxLength = 80;
        public const int PostalCodeFixedLength = 8;
        public const string PostalCodeRegEx = @"^[A-Z]{2}-\d{5}$";
        public const int DistrictRegionMinRange = 0;
        public const int DistrictRegionMaxRange = 3;

        //Property
        public const int PropIdentifierMinLength = 16;
        public const int PropIdentifierMaxLength = 20;
        public const int PropAreaMinValueRange = 0;
        public const int PropAreaMaxValueRange = int.MaxValue;
        public const int PropDetailsMinLength = 5;
        public const int PropDetailsMaxLength = 500;
        public const int PropAddressMinLength = 5;
        public const int PropAddressMaxLength = 200;

        //Citizen
        public const int CitizenFirstNameMinLength = 2;
        public const int CitizenFirstNameMaxLength = 30;
        public const int CitizenLastNameMinLength = 2;
        public const int CitizenLastNameMaxLength = 30;
        public const int MaritalStatusRangeMin = 0;
        public const int MaritalStatusRangeMax = 3;
    }
}