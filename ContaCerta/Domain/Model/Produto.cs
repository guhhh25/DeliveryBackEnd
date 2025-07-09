using System.ComponentModel.DataAnnotations;

namespace ContaCerta.Model
{
    public class Produto : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public int Preco { get; set; }

        [StringLength(5000000)] // Limite de ~5MB em base64
        public string? Foto { get; set; }

        [Required]
        public DateTime TempoPreparo { get; set; }

        [Required]
        [Range(0, 5)]
        public int Avaliacao { get; set; }

        [Required]
        [Range(0, 100)]
        public int Desconto { get; set; }

        [Required]
        public Guid EmpresaId { get; set; }

        [Required]
        public Guid CategoriaId { get; set; }

        // Navegações
        public virtual Empresa Empresa { get; set; } = null!;
        public virtual Categoria Categoria { get; set; } = null!;
    }
} 