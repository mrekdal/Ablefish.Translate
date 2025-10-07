using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace PostTranslations.Models
{
    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(Root));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (Root)serializer.Deserialize(reader);
    // }

    [XmlRoot(ElementName = "import")]
    public class Import
    {

        [XmlAttribute(AttributeName = "namespace")]
        public string Namespace { get; set; }
    }

    [XmlRoot(ElementName = "element")]
    public class Element
    {

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "minOccurs")]
        public int MinOccurs { get; set; }

        [XmlElement(ElementName = "complexType")]
        public ComplexType ComplexType { get; set; }

        [XmlAttribute(AttributeName = "Ordinal")]
        public int Ordinal { get; set; }

        [XmlAttribute(AttributeName = "IsDataSet")]
        public bool IsDataSet { get; set; }
    }

    [XmlRoot(ElementName = "sequence")]
    public class Sequence
    {

        [XmlElement(ElementName = "element")]
        public List<Element> Element { get; set; }
    }

    [XmlRoot(ElementName = "attribute")]
    public class Attribute
    {

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "use")]
        public string Use { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "ref")]
        public string Ref { get; set; }

        [XmlAttribute(AttributeName = "Ordinal")]
        public int Ordinal { get; set; }
    }

    [XmlRoot(ElementName = "complexType")]
    public class ComplexType
    {

        [XmlElement(ElementName = "sequence")]
        public Sequence Sequence { get; set; }

        [XmlElement(ElementName = "attribute")]
        public List<Attribute> Attribute { get; set; }

        [XmlElement(ElementName = "choice")]
        public Choice Choice { get; set; }
    }

    [XmlRoot(ElementName = "choice")]
    public class Choice
    {

        [XmlElement(ElementName = "element")]
        public List<Element> Element { get; set; }

        [XmlAttribute(AttributeName = "maxOccurs")]
        public string MaxOccurs { get; set; }
    }

    [XmlRoot(ElementName = "schema")]
    public class Schema
    {

        [XmlElement(ElementName = "import")]
        public Import Import { get; set; }

        [XmlElement(ElementName = "element")]
        public Element Element { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlAttribute(AttributeName = "xsd")]
        public string Xsd { get; set; }

        [XmlAttribute(AttributeName = "msdata")]
        public string Msdata { get; set; }
    }

    [XmlRoot(ElementName = "resheader")]
    public class Resheader
    {

        [XmlElement(ElementName = "value")]
        public string Value { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "data")]
    public class Data
    {

        [XmlElement(ElementName = "value")]
        public string Value { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "space")]
        public string Space { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "root")]
    public class ResourceFile
    {

        [XmlElement(ElementName = "schema")]
        public Schema? Schema { get; set; }

        [XmlElement(ElementName = "resheader")]
        public List<Resheader>? Resheader { get; set; }

        [XmlElement(ElementName = "data")]
        public List<Data> Data { get; set; }
    }

    [XmlRoot(ElementName = "root")]
    public class ResourceFileSimple
    {

        [XmlElement(ElementName = "data")]
        public List<Data> Data { get; set; }
    }

}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
