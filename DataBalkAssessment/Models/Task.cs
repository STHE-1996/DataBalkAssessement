using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;

namespace DataBalkAssessment.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]  
        [MaxLength(200)] 
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]  
        public int AssigneeId { get; set; }

        [Required]  
        public DateTime DueDate { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        internal static async ValueTask<EntityEntry<Task>> CompletedTask()
        {
            throw new NotImplementedException();
        }
    }
}
