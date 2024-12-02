using System.Net.Sockets;
using System.Xml;

namespace SSX.Client {
  //STUB
  public class Client {
    private readonly string _host;
    private readonly int _port;
    private readonly XmlUtilities _xmlUtils = new();
    private readonly XmlTextWriter _writer;

    private readonly List<Actor> Actors = new List<Actor> {
      new Actor("American", "Leonardo DiCaprio", "Jack Dawson (Titanic)", 1997),
      new Actor("British", "Daniel Radcliffe", "Harry Potter (Harry Potter series)", 2001),
      new Actor("Australian", "Hugh Jackman", "Wolverine (X-Men)", 2000),
      new Actor("Indian", "Shah Rukh Khan", "Raj Malhotra (Dilwale Dulhania Le Jayenge)", 1995)
    };

    public Client(string host, int port) {
      this._host = host;
      this._port = port;
    }

    public void Run() {
      try {
        using TcpClient connection = new(_host, _port); // initialising client and connecting to host:port
        using StreamWriter writer = new(connection.GetStream()); // initialising writer to be able to write to server
        using StreamReader reader = new(connection.GetStream()); // initialising reader to be able to read from server
        Console.WriteLine("[CLIENT] Connected to " + _host + ":" + _port);

        // Continue running until false or break
        while (true) {
          SleepOutput(); // sleeping between console writes, to make output easier to read for the user
          Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - -");

          SleepOutput();
          Console.WriteLine("[CLIENT] Please choose an actor to send to the server or exit by writing EXIT." +
                            "\nYou can choose an actor by writing [first name] [surname]."
          );

          // looping through actors to list options
          for (int i = 0; i < Actors.Count - 1; i++) {
            Console.WriteLine($"\t[CLIENT] {i}. {Actors[i].name}");
          }

          string? inputLine = Console.ReadLine();
          if (string.IsNullOrWhiteSpace(inputLine)) { // null handling for user input
            Console.WriteLine("[CLIENT] Input cannot be empty. Please try again.");
            continue;
          }

          // user wishes to exit the connection
          if (inputLine.Equals("EXIT", StringComparison.OrdinalIgnoreCase)) {
            SendLine(writer, inputLine); // sending exit to server
            string? exitResponse = reader.ReadLine();
            Console.WriteLine("[SERVER] " + exitResponse); // reading and writing server response
            Console.WriteLine("[CLIENT] Exiting...");
            break; // break out of loop, closing the connection
          }

          string[] inputParts = inputLine.Split(' ');
          foreach (Actor actor in Actors) {
            string[] actorParts = actor.name.Split(' ');
            if (actorParts.Equals(inputParts)) {
              
            }
          }

          // send user input (not null here)
          SendLine(writer, inputLine); // sending user line to server

          string? response = reader.ReadLine(); // reading response from server
          if (response == null) { // null handling for response
            Console.WriteLine("[CLIENT] No response from server. Server might have closed the conneciton. Try reconnecting.");
            break;
          }

          // read server response (not null here)
          SleepOutput();
          Console.WriteLine("[SERVER] " + response);
        }
      } catch (IOException e) {
        Console.Error.WriteLine("[CLIENT] Connection failed: " + e.Message);
      } catch (Exception e) {
        Console.Error.WriteLine("[CLIENT] Unexpected error: " + e.Message);
      }
    }

    // method to send line to server, using StreamWriter to send the messages
    private void SendLine(StreamWriter writer, string line) {
      SleepOutput();
      Console.WriteLine("[CLIENT] sending line: " + line); // sleeping to make console output easier to read for the user
      writer.WriteLine(line); // writes the line to the writers buffer
      writer.Flush(); // clears the writers buffer and sends the line
    }

    //private void SendXml(Actor actor) {
    //  string xml = _xmlUtils.Serialize(actor);
    //}

    // method to sleep to make terminal output easier readable to the user
    private void SleepOutput() {
      Thread.Sleep(500);
    }
  }
}