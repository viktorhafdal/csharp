using System.Xml;
using System.Xml.Linq;

namespace XML_Parser {

    public class Program {
        public static void Main(string[] args) {
            //List to store all actors from original .xml file
            List<Actor> actors = [];

            //Declaring which file to read from
            string path = @"C:\Users\Viktor\Documents\GitHub\Side-Projects\csharp\XML_Parser\XML_Parser\actors.xml";
            XmlReader reader = XmlReader.Create(path);
           
            //Reading the file and storing the actors in the list
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
            reader.Close();
            
            //Creating actors element and adding all actors to it
            XElement xActors = new XElement("actors");
            foreach (Actor actor in actors) {
                Console.WriteLine(actor.ToString());

                XElement xActor = new XElement("actor", new XAttribute("nationality", $"{actor.nationality}"),
                                                    new XElement("name", $"{actor.name}"),
                                                    new XElement("role", $"{actor.role}"),
                                                    new XElement("year", actor.year)
                                               );
                xActors.Add(xActor);
            }

            //Path for new file
            string fileName = @"C:\Users\Viktor\Documents\GitHub\Side-Projects\csharp\XML_Parser\XML_Parser\newActors.xml";

            //XDocument doc = new();
            //doc.Add(xActors);
            //doc.Save(fileName);

            ////Writing the new file
            try {
                using (FileStream fs = new(fileName, FileMode.Create))
                using (StreamWriter sw = new(fs))
                using (XmlTextWriter xmlWriter = new(sw)) {
                    xmlWriter.Formatting = Formatting.Indented; //Formatting the file with indentation, so it's not printed to one line.
                    xmlWriter.Indentation = 2;

                    xmlWriter.WriteStartDocument(); //Including start xml declaration
                    xActors.WriteTo(xmlWriter); //Writing actors element to file
                }
            } catch {}
        }
    }
}