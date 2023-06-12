using System.ComponentModel.DataAnnotations;

namespace BelshezaProject.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; }
        public int DisplayOder { get; set; }
    }
}
