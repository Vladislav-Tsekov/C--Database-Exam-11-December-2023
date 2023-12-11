using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ExportDtos
{
    public class ExportPropDistrictDto
    {
        //[XmlAttribute("postal-code")]
        //public string PostalCode { get; set; }

        [XmlArray("Property")]
        public ExportPropertiesDto[] Properties { get; set; }
    }
}
