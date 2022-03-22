using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BACK.Models
{
    public class CardModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string List { get; set; }
    }
}
