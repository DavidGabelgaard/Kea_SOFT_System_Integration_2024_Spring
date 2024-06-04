using System.Collections;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


namespace System_Integration_ASP.NET.User;

[Route("api/[controller]/api/[controller]")]
public class UserController : Controller
{
    [Route("Json")]
    [HttpGet]
    public Task<object> GetJsonUser()
    {
        const string path = "../../DataFiles/User.json";

        if (!System.IO.File.Exists(path))
        {
            return Task.FromResult<object>(BadRequest("Could not find file"));
        }

        var json = System.IO.File.ReadAllText(path);

        var users = JsonConvert.DeserializeObject<UserListModel>(json);

        return Task.FromResult<object>(Ok(users));
    }


    [Route("Csv")]
    [HttpGet]
    public async Task<object> GetCsvUser()
    {
        const string path = "../../DataFiles/User.csv";

        if (!System.IO.File.Exists(path))
        {
            return BadRequest("Could not find file");
        }

        var users = await TranslateCsvToUserList(path);

        return Ok(users);
    }

    [Route("Xml")]
    [HttpGet]
    public async Task<object> GetXmlUser()
    {
        const string path = "../../DataFiles/User.xml";

        if (!System.IO.File.Exists(path))
        {
            return BadRequest("Could not find file");
        }

        var users = await TranslateXmlToUserList(path);

        return Ok(users);
    }


    [Route("Yaml")]
    [HttpGet]
    public async Task<object> GetYamlUser()
    {
        const string path = "../../DataFiles/User.Yaml";

        if (!System.IO.File.Exists(path))
        {
            return BadRequest("Could not find file");
        }

        var users = TranslateYamlToUserList(path);

        return Ok(users);
    }

    [Route("Txt")]
    [HttpGet]
    public async Task<object> GetTxtUser()
    {
        const string path = "../../DataFiles/User.Txt";

        if (!System.IO.File.Exists(path))
        {
            return BadRequest("Could not find file");
        }

        var users = TranslateTxtToUserList(path);

        if (users == null)
        {
            BadRequest("Could not get users from the file");
        }

        return Ok(users);
    }

    private Task<List<UserModel>> TranslateCsvToUserList(string filePath)
    {
        var users = new List<UserModel>();

        using (var parser = new TextFieldParser(filePath))
        {
            parser.HasFieldsEnclosedInQuotes = true;
            parser.SetDelimiters(",");

            // Skip the header line
            parser.ReadLine();

            while (!parser.EndOfData)
            {
                var fields = parser.ReadFields();

                if (fields != null && fields.Length >= 6)
                {
                    var user = new UserModel
                    {
                        FirstName = fields[0],
                        LastName = fields[1],
                        Valid = bool.Parse(fields[2]),
                        Address = fields[3],
                        Age = int.Parse(fields[4]),
                        Education = new List<string>
                        {
                            fields[5],
                            fields[6]
                        }
                    };

                    users.Add(user);
                }
            }
        }

        return Task.FromResult(users);
    }

    private Task<UserListModel> TranslateXmlToUserList(string filePath)
    {
        var serializer = new XmlSerializer(typeof(UserListModel));

        using var streamReader = new StreamReader(filePath);

        return Task.FromResult((UserListModel)serializer.Deserialize(streamReader));
    }

    private UserListModel TranslateYamlToUserList(string filePath)
    {
        using var streamReader = new StreamReader(filePath);

        var deserializer = new DeserializerBuilder().Build();

        return deserializer.Deserialize<UserListModel>(streamReader);
    }

    private UserListModel? TranslateTxtToUserList(string filePath)
    {
        var users = new UserListModel()
        {
            Users = []
        };

        var lines = System.IO.File.ReadAllLines(filePath);

        var currentUser = new UserModel();
        
        foreach (var line in lines)
        {

            if (currentUser is { FirstName: not null, LastName: not null, Age: not null, Valid: not null } && line == string.Empty)
            {
                users.Users.Add(currentUser);
                currentUser = new UserModel();
            }
            
            var parts = line.Split(':');
            if (parts.Length == 2)
            {
                var key = parts[0].Trim();
                var value = parts[1].Trim();
                
                switch (key)
                {
                    case "FirstName":
                        currentUser.FirstName = value;
                        break;
                    case "LastName":
                        currentUser.LastName = value;
                        break;
                    case "Valid":
                        if (bool.TryParse(value, out var parsedValid))
                            currentUser.Valid = parsedValid;
                        break;
                    case "Address":
                        currentUser.Address = value;
                        break;
                    case "Age":
                        if (int.TryParse(value, out var parsedAge))
                            currentUser.Age = parsedAge;
                        break;
                    case "Education":
                        currentUser.Education ??= [];
                        currentUser.Education.Add(value);
                        break;
                }
            }
        }

        return users;
    }
}