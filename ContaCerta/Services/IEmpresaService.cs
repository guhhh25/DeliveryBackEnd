using ContaCerta.Model;

namespace ContaCerta.Services
{
    public interface IEmpresaService
    {
        Task<IEnumerable<Empresa>> GetAllAsync();
        Task<Empresa?> GetByIdAsync(Guid id);
        Task<Empresa?> GetByUserIdAsync(Guid userId);
        Task<Empresa> CreateAsync(Empresa empresa);
        Task<Empresa> UpdateAsync(Empresa empresa);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
} 