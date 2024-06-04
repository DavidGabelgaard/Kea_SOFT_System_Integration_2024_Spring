using System.Xml.Serialization;
using YamlDotNet.Serialization;

namespace System_Integration_ASP.NET.User;
[XmlRoot(ElementName="Users")]
public class UserListModel
{
    [XmlElement(ElementName="UserModel")] 
    [YamlMember(Alias = "Users")]
    public required List<UserModel> Users { get; set; }
}