using System.Xml.Serialization;

public class UserModel
{
    [XmlElement(ElementName="FirstName")] 
    public  string? FirstName { get; set; }
    
    [XmlElement(ElementName="LastName")] 
    public string? LastName { get; set; }
    
    [XmlElement(ElementName="Valid")] 
    public bool? Valid { get; set; }
    
    [XmlElement(ElementName="Address")] 
    public string? Address { get; set; }
    
    [XmlElement(ElementName="Age")] 
    public int? Age { get; set; }
    
    [XmlElement(ElementName="Education")] 
    public List<string>? Education { get; set; }
}