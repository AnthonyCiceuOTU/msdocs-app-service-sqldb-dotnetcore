using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetCoreSqlDb.Models
{
    public class SyntaxGameQuestions
    {
        [Key]
        public Guid ID { get; set; } // Primary Key for the contact record

        public int QuestionNumber { get; set; }
        public string Pseudocode { get; set; } = "";
        public string Java { get; set; } = "";
        public string CSharp { get; set; } = "";
        public string Python { get; set; } = "";


    }
}