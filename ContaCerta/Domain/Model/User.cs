using Microsoft.AspNetCore.Identity;

namespace ContaCerta.Model
{
    public class User : IdentityUser<Guid>
    {
        // Configurado para usar Guid como chave primária
    }
}
