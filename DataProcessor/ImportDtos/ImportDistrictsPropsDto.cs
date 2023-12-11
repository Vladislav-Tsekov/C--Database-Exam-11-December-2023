using Cadastre.Other;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("Property")]
    public class ImportDistrictsPropsDto
    {
        [Required]
        [MinLength(Validate.PropIdentifierMinLength)]
        [MaxLength(Validate.PropIdentifierMaxLength)]
        [XmlElement("PropertyIdentifier")]
        public string PropertyIdentifier { get; set; }

        [Required]
        [Range(Validate.PropAreaMinValueRange, Validate.PropAreaMaxValueRange)]
        [XmlElement("Area")]
        public int Area { get; set; }

        [MinLength(Validate.PropDetailsMinLength)]
        [MaxLength(Validate.PropDetailsMaxLength)]
        [XmlElement("Details")]
        public string Details { get; set; }

        [Required]
        [MinLength(Validate.PropAddressMinLength)]
        [MaxLength(Validate.PropAddressMaxLength)]
        [XmlElement("Address")]
        public string Address { get; set; }

        [XmlElement("DateOfAcquisition")]
        public string DateOfAcquisition { get; set; }
    }
}