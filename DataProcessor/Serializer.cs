using Cadastre.Data;
using Cadastre.DataProcessor.ExportDtos;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;


namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {
            DateTime acquisitionDate;
            bool isAcquisitionDateValid = DateTime
                .TryParseExact("01/01/2000", "dd/MM/yyyy", 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out acquisitionDate);

            var propertiesWithOwners = dbContext.Properties
                .Where(p => p.DateOfAcquisition >= acquisitionDate)
                .OrderByDescending(p => p.DateOfAcquisition)
                .ThenBy(p => p.PropertyIdentifier)
                .Select(p => new
                {
                    p.PropertyIdentifier,
                    p.Area,
                    p.Address,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Owners = p.PropertiesCitizens
                        .OrderBy(c => c.Citizen.LastName)
                        .Select(c => new
                        {
                            c.Citizen.LastName,
                            MaritalStatus = c.Citizen.MaritalStatus.ToString()
                        })
                })
                .ToArray();

            return JsonConvert.SerializeObject(propertiesWithOwners, Newtonsoft.Json.Formatting.Indented);
        }

        public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
        {
            StringBuilder output = new();

            XmlSerializer xmlSerializer = new(typeof(ExportPropertiesDto[]), new XmlRootAttribute("Properties"));

            XmlSerializerNamespaces namespaces = new();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new(output);

            var propertiesWithDistricts = dbContext.Properties
                .Where(pa => pa.Area >= 100)
                .OrderByDescending(pa => pa.Area)
                .ThenBy(pa => pa.DateOfAcquisition)
                .Select(pa => new ExportPropertiesDto
                {
                    PropertyIdentifier = pa.PropertyIdentifier,
                    Area = pa.Area,
                    DateOfAcquisition = pa.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    PostalCode = pa.District.PostalCode
                })
                .ToArray();

            var exportDto = new ExportPropDistrictDto { Properties = propertiesWithDistricts };

            xmlSerializer.Serialize(writer, propertiesWithDistricts, namespaces);
            return output.ToString();
        }
    }
}
