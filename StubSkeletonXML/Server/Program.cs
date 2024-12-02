namespace SSX.Main {
  using SSX.Server;
  public class Program {
    public static void Main(string[] args) {
      ClientManager manager = new(6010);
      manager.Run();
    }
  }
}