namespace DotNetCoreSqlDb.Models
{
    public class PseudocodeGameViewModel
    {
        public string GameTitle { get; set; } = "";
        public string Subtitle { get; set; } = "";
        public List<PseudocodeQuestion> Questions { get; set; } = new();
    }
}