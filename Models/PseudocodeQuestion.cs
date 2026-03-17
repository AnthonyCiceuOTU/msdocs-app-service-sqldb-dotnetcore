namespace DotNetCoreSqlDb.Models
{
    public class PseudocodeQuestion
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Type { get; set; } = ""; // mcq, order, bugfix
        public string Difficulty { get; set; } = "";
        public string Scenario { get; set; } = "";
        public string CodeBlock { get; set; } = "";
        public List<string> Options { get; set; } = new();
        public int CorrectOptionIndex { get; set; } = -1;
        public List<string> CorrectOrder { get; set; } = new();
        public string CorrectTextAnswer { get; set; } = "";
        public string Explanation { get; set; } = "";
    }
}