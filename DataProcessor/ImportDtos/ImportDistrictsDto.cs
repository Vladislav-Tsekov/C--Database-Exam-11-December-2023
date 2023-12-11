using Cadastre.Other;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("District")]
    public class ImportDistrictsDto
    {
        [Required]
        [MinLength(Validate.DistrictNameMinLength)]
        [MaxLength(Validate.DistrictNameMaxLength)]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Required]
        [XmlAttribute("Region")]
        public string Region { get; set; }

        [Required]
        [StringLength(Validate.PostalCodeFixedLength)]
        [RegularExpression(Validate.PostalCodeRegEx)]
        [XmlElement("PostalCode")]
        public string PostalCode { get; set; }

        [XmlArray("Properties")]
        public ImportDistrictsPropsDto[] Properties { get; set; }
    }
}