namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedDistrict = "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen = "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            StringBuilder output = new();

            XmlSerializer xmlSerializer = new(typeof(ImportDistrictsDto[]), new XmlRootAttribute("Districts"));

            StringReader reader = new(xmlDocument);

            ImportDistrictsDto[] districtsDtos = (ImportDistrictsDto[])xmlSerializer.Deserialize(reader);

            var validDistricts = new HashSet<District>();

            foreach (var currentDistrict in districtsDtos)
            {
                if (!IsValid(currentDistrict))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                if (validDistricts.Where(d => d.Name == currentDistrict.Name).Any())
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Region region;
                var tryParseRegion = Enum.TryParse<Region>(currentDistrict.Region.ToString(), out region);

                District district = new() 
                {
                    Name = currentDistrict.Name,
                    Region = region,
                    PostalCode = currentDistrict.PostalCode
                };

                foreach (var currentProp in currentDistrict.Properties)
                {
                    if (!IsValid(currentProp))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (validDistricts.Any(d => d.Properties.Any(p => p.PropertyIdentifier == currentProp.PropertyIdentifier))
                     || validDistricts.Any(d => d.Properties.Any(p => p.Address == currentProp.Address)))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (district.Properties.Any(p => p.PropertyIdentifier == currentProp.PropertyIdentifier))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (district.Properties.Any(p => p.Address == currentProp.Address))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    //TODO - Нужно е да проверите дали има друго Property в текущия District със същия адрес.
                    //Ако има, второто пропърти със същия адрес не се добавя към базата и се добавя ErrorMessage.
                    //Освен това, проверете дали има дублиращ се адрес и в базата.

                    DateTime propAcquisitionDate;
                    bool isAcquisitionDateValid = DateTime
                        .TryParseExact(currentProp.DateOfAcquisition, "dd/MM/yyyy", CultureInfo
                        .InvariantCulture, DateTimeStyles.None, out propAcquisitionDate);

                    Property property = new()
                    {
                        PropertyIdentifier = currentProp.PropertyIdentifier,
                        Area = currentProp.Area,
                        Details = currentProp.Details,
                        Address = currentProp.Address,
                        DateOfAcquisition = propAcquisitionDate,
                    };

                    district.Properties.Add(property);
                }

                validDistricts.Add(district);
                output.AppendLine(string.Format(SuccessfullyImportedDistrict, district.Name, district.Properties.Count));
            }

            dbContext.Districts.AddRange(validDistricts);
            dbContext.SaveChanges();

            return output.ToString().TrimEnd();
        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            StringBuilder output = new();

            ImportCitizensDto[] citizensDtos = JsonConvert.DeserializeObject<ImportCitizensDto[]>(jsonDocument);

            var validCitizens = new HashSet<Citizen>();

            foreach (var currentCitizen in citizensDtos)
            {
                if (!IsValid(currentCitizen))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                if (currentCitizen.MaritalStatus != "Unmarried"
                 && currentCitizen.MaritalStatus != "Married"
                 && currentCitizen.MaritalStatus != "Divorced"
                 && currentCitizen.MaritalStatus != "Widowed")
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime citizenBirthDate;
                bool isBirthDateValid = DateTime
                    .TryParseExact(currentCitizen.BirthDate, "dd-MM-yyyy", CultureInfo
                    .InvariantCulture, DateTimeStyles.None, out citizenBirthDate);

                MaritalStatus status;
                var tryParseStatus = Enum.TryParse<MaritalStatus>(currentCitizen.MaritalStatus.ToString(), out status);

                Citizen citizen = new()
                { 
                    FirstName = currentCitizen.FirstName,
                    LastName = currentCitizen.LastName,
                    BirthDate = citizenBirthDate,
                    MaritalStatus = status                    
                };

                foreach (var propId in currentCitizen.Properties)
                {
                    Property prop = dbContext.Properties.Find(propId);

                    if (prop == null) 
                    {
                        continue;
                    }

                    citizen.PropertiesCitizens.Add(new PropertyCitizen()
                    {
                        Property = prop      
                    });
                }

                validCitizens.Add(citizen);
                output.AppendLine(string.Format(SuccessfullyImportedCitizen, citizen.FirstName, citizen.LastName, citizen.PropertiesCitizens.Count));
            }

            dbContext.Citizens.AddRange(validCitizens);
            dbContext.SaveChanges();

            return output.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
