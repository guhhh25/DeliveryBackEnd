using System.ComponentModel.DataAnnotations;

namespace ContaCerta.Model
{
    public class Empresa : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public int NumeroRegistro { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int Telefone { get; set; }

        [Required]
        [StringLength(10)]
        public string Cep { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Endereco { get; set; } = string.Empty;

        [Required]
        public Guid UserId { get; set; }


        public virtual User User { get; set; } = null!;

        [StringLength(5000000)] 
        public string? Logo { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
} 