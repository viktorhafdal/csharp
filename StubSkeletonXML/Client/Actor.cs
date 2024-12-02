namespace SSX.Client {
  
  [Serializable]
  public class Actor {
    public string nationality { get; set; }
    public string name { get; set; }
    public string role { get; set; }
    public int year { get; set; }

    public Actor(string nationality, string name, string role, int year) {
      this.nationality = nationality;
      this.name = name;
      this.role = role;
      this.year = year;
    }

    public override string ToString() {
      string info = $"Nationality: {nationality}, Name: {name}, Role: {role}, Year: {year}";
      return info;
    }
  }
}