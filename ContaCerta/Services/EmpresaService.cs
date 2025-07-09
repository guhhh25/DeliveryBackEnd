using ContaCerta.Data;
using ContaCerta.Model;
using Microsoft.EntityFrameworkCore;

namespace ContaCerta.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly AppDbContext _context;

        public EmpresaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Empresa>> GetAllAsync()
        {
            return await _context.Empresas
                .Where(e => !e.Deleted)
                .Include(e => e.User)
                .ToListAsync();
        }

        public async Task<Empresa?> GetByIdAsync(Guid id)
        {
            return await _context.Empresas
                .Where(e => e.Id == id && !e.Deleted)
                .Include(e => e.User)
                .FirstOrDefaultAsync();
        }

        public async Task<Empresa?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Empresas
                .Where(e => e.UserId == userId && !e.Deleted)
                .Include(e => e.User)
                .FirstOrDefaultAsync();
        }

        public async Task<Empresa> CreateAsync(Empresa empresa)
        {
            empresa.CreatedAt = DateTime.UtcNow;
            empresa.UpdatedAt = DateTime.UtcNow;
            empresa.Deleted = false;

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

            return empresa;
        }

        public async Task<Empresa> UpdateAsync(Empresa empresa)
        {
            var existingEmpresa = await _context.Empresas.FindAsync(empresa.Id);
            if (existingEmpresa == null)
                throw new ArgumentException("Empresa não encontrada");

            existingEmpresa.Nome = empresa.Nome;
            existingEmpresa.NumeroRegistro = empresa.NumeroRegistro;
            existingEmpresa.Email = empresa.Email;
            existingEmpresa.Telefone = empresa.Telefone;
            existingEmpresa.Cep = empresa.Cep;
            existingEmpresa.Endereco = empresa.Endereco;
            existingEmpresa.Logo = empresa.Logo;
            existingEmpresa.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return existingEmpresa;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
                return false;

            empresa.Deleted = true;
            empresa.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Empresas
                .AnyAsync(e => e.Id == id && !e.Deleted);
        }
    }
} 