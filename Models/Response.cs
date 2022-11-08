using System.ComponentModel.DataAnnotations.Schema;

namespace Questionnaireonnaire.Models
{
    public class Response
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserDataId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public Question Question { get; set; } = new Question();
        public Answer Answer { get; set; } = new Answer();
    }
}
