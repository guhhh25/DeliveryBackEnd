using System.ComponentModel.DataAnnotations;

namespace ContaCerta.Model
{
    public abstract class BaseEntity
    {
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool Deleted { get; set; } = false;
    }
} 