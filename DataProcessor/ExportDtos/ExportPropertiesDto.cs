using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ExportDtos
{
    [XmlType("Property")]
    public class ExportPropertiesDto
    {
        //[XmlElement("PostalCode")]
        //public ExportPropDistrictDto District { get; set; }

        //[XmlElement("PropertyIdentifier")]
        //public string PropertyIdentifier { get; set; }

        //[XmlElement("Area")]
        //public int Area { get; set; }

        //[XmlElement("DateOfAcquisition")]
        //public string DateOfAcquisition { get; set; }

        [XmlAttribute("postal-code")]
        public string PostalCode { get; set; }

        [XmlElement("PropertyIdentifier")]
        public string PropertyIdentifier { get; set; }

        [XmlElement("Area")]
        public int Area { get; set; }

        [XmlElement("DateOfAcquisition")]
        public string DateOfAcquisition { get; set; }
    }
}
