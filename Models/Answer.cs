using System.ComponentModel.DataAnnotations.Schema;

namespace Questionnaireonnaire.Models
{
    public class Answer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Text { get; set; } = "";
        
        public int QuestionId { get; set; }
    }
}
