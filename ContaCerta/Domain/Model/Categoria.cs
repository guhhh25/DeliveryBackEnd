using System.ComponentModel.DataAnnotations;

namespace ContaCerta.Model
{
    public class Categoria : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Tamanho { get; set; } = string.Empty;

        [Required]
        public Guid EmpresaId { get; set; }
    }
} 