using System.ComponentModel.DataAnnotations.Schema;

namespace Questionnaireonnaire.Models
{
    public class UserData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public int? NextQuestion { get; set; }
        public int? LastQuestion { get; set; }

        public ICollection<Response> Responses { get; set; } = new HashSet<Response>();
    }
}
