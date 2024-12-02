using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace SSX.Client {
  public class XmlUtilities {
    private readonly List<Actor> actors = new();

    public string ToXml(Actor actor, XmlTextWriter writer) {
      //Creating actors element and adding all actors to it
      XElement xActors = new XElement("actors");
      
      foreach (Actor a in actors) {
        Console.WriteLine(a.ToString());

        XElement xActor = new XElement("actor", new XAttribute("nationality", $"{a.nationality}"),
                                            new XElement("name", $"{a.name}"),
                                            new XElement("role", $"{a.role}"),
                                            new XElement("year", a.year)
                                       );
        xActors.Add(xActor);
      }

      string fileName = "newActor-" + DateTime.Now.ToString() + ".xml";

      //Writing the new file
      try {
        using (FileStream fs = new(fileName, FileMode.Create))
        using (StreamWriter sw = new(fs))
        using (XmlTextWriter xmlWriter = new(sw)) {
          xmlWriter.Formatting = Formatting.Indented; //Formatting the file with indentation, so it's not printed to one line.
          xmlWriter.Indentation = 4;

          xmlWriter.WriteStartDocument(); //Including start xml declaration
          xActors.WriteTo(xmlWriter); //Writing actors element to file

          return 
        }
      } catch { }
    }

    public void FromXml(string filepath, XmlReader reader) {
      List<Actor> actors = new();
      reader = XmlReader.Create(filepath);

      using (reader) {
        while (reader.Read()) {
          if (reader.IsStartElement("actor")) {
            string nationality = reader.GetAttribute("nationality") ?? "Unknown";

            reader.ReadToDescendant("name");
            string name = reader.ReadElementContentAsString();

            reader.ReadToNextSibling("role");
            string role = reader.ReadElementContentAsString();

            reader.ReadToNextSibling("year");
            int year = reader.ReadElementContentAsInt();

            //Actor actor = new(nationality, name, role, year);
            actors.Add(new(nationality, name, role, year));
          }
        }
      }
    }

    public static string Serialize(object obj) {
      StringWriter writer = new();

      XmlWriterSettings settings = new();
      settings.Indent = true;
      settings.IndentChars = "  ";

      XmlWriter writer = XmlWriter.Create(writer, settings);
      XmlSerializer serializer = new(obj.GetType());

      serializer.Serialize(writer, obj);
      writer.Flush();

      return writer.ToString();
    }

    public static object Deserialize(string xml, Type type) {
      StringReader stream = new(xml);

      XmlReader reader = XmlReader.Create(stream);

      XmlSerializer serializer = new(type);

      return serializer.Deserialize(reader);
    }
  }
}