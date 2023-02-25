
using System.Xml.Schema;
using System.Xml;
using System.Xml.Linq;

internal class Config
{
  internal static ConfigStruct config = new();

  internal static bool LoadConfig()
  {
    Program.logger.Info("Loading config");

    string xml_path = "./config.xml";
    string xsd_path = "./config.xsd";

    if (File.Exists(xml_path) == false)
    {
      Console.WriteLine("Could not find XML configuration file.");
      return false;
    }
    if (File.Exists(xsd_path) == false)
    {
      Console.WriteLine("Could not find XSD configuration file.");
      return false;
    }

    string xml_content = File.ReadAllText(xml_path);
    string xsd_content = File.ReadAllText(xsd_path);

    bool validation_check = true;

    //XMLスキーマオブジェクトの生成
    XmlSchema schema = new();
    using (StringReader stringReader = new(xsd_content))
    {
      schema = XmlSchema.Read(stringReader, null)!;
    }
    // スキーマの追加
    XmlSchemaSet schemaSet = new();
    schemaSet.Add(schema);

    // XML文書の検証を有効化
    XmlReaderSettings settings = new()
    {
      ValidationType = ValidationType.Schema,
      Schemas = schemaSet
    };
    settings.ValidationEventHandler += (object? sender, ValidationEventArgs e) => {
      if (e.Severity == XmlSeverityType.Warning)
      {
        Console.WriteLine($"Validation Warning ({e.Message})");
      }
      if (e.Severity == XmlSeverityType.Error)
      {
        Console.WriteLine($"Validation Error ({e.Message})");
        validation_check = false;
      }
    };

    // XMLデータの読み込み
    using (StringReader stringReader = new(xml_content))
    using (XmlReader xmlReader = XmlReader.Create(stringReader, settings))
    {
      while (xmlReader.Read()) { }
    }

    if (validation_check == false)
    {
      Console.WriteLine("Validation failed");
      return false;
    }

    // XMLのパース
    var configNode = XElement.Parse(xml_content);

    var githubNode = configNode.Element("GitHub")!;
    GithubStruct githubStruct = new()
    {
      username = githubNode.Element("username")!.Value,
      token = githubNode.Element("token")!.Value,
    };
    config.github = githubStruct;

    var gitlabNode = configNode.Element("GitLab")!;
    GitlabStruct gitlabStruct = new()
    {
      username = gitlabNode.Element("username")!.Value,
      token = gitlabNode.Element("token")!.Value,
  };
    config.gitlab = gitlabStruct;

    config.output_path = configNode.Element("output_path")!.Value;

    Program.logger.Info("Config loaded");

    return true;
  }
}

internal struct ConfigStruct
{
  internal GithubStruct github;
  internal GitlabStruct gitlab;
  internal string output_path;
}

internal struct GithubStruct
{
  internal string username;
  internal string token;
}

internal struct GitlabStruct
{
  internal string username;
  internal string token;
}
