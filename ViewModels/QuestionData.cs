using Questionnaireonnaire.Models;

namespace Questionnaireonnaire.ViewModels
{
    public class QuestionData
    {
        public Question Question { get; set; } = new Question();
        public int UserId { get; set; }

    }
}
