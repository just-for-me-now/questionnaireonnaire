using System.ComponentModel.DataAnnotations.Schema;

namespace Questionnaireonnaire.Models
{
    public class Question
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public bool disabled { get; set; } = false;
        

        public ICollection<Answer> Answers { get; set; } = new HashSet<Answer>();
    }
}
