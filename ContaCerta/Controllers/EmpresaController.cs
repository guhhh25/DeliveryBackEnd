using ContaCerta.Model;
using ContaCerta.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ContaCerta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaService _empresaService;

        public EmpresaController(IEmpresaService empresaService)
        {
            _empresaService = empresaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var empresas = await _empresaService.GetAllAsync();
                return Ok(empresas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erro interno do servidor", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var empresa = await _empresaService.GetByIdAsync(id);
                if (empresa == null)
                    return NotFound(new { Message = "Empresa não encontrada" });

                return Ok(empresa);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erro interno do servidor", Error = ex.Message });
            }
        }

        [HttpGet("my-empresa")]
        public async Task<IActionResult> GetMyEmpresa()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                    return BadRequest(new { Message = "ID do usuário inválido" });

                var empresa = await _empresaService.GetByUserIdAsync(userId);
                if (empresa == null)
                    return NotFound(new { Message = "Empresa não encontrada para este usuário" });

                return Ok(empresa);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erro interno do servidor", Error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmpresaRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                    return BadRequest(new { Message = "ID do usuário inválido" });

                // Verificar se o usuário já tem uma empresa
                var existingEmpresa = await _empresaService.GetByUserIdAsync(userId);
                if (existingEmpresa != null)
                    return BadRequest(new { Message = "Usuário já possui uma empresa cadastrada" });

                var empresa = new Empresa
                {
                    Nome = request.Nome,
                    NumeroRegistro = request.NumeroRegistro,
                    Email = request.Email,
                    Telefone = request.Telefone,
                    Cep = request.Cep,
                    Endereco = request.Endereco,
                    Logo = request.Logo,
                    UserId = userId
                };

                var createdEmpresa = await _empresaService.CreateAsync(empresa);

                return CreatedAtAction(nameof(GetById), new { id = createdEmpresa.Id }, createdEmpresa);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erro interno do servidor", Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmpresaRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                    return BadRequest(new { Message = "ID do usuário inválido" });

                var existingEmpresa = await _empresaService.GetByIdAsync(id);
                if (existingEmpresa == null)
                    return NotFound(new { Message = "Empresa não encontrada" });

                // Verificar se a empresa pertence ao usuário
                if (existingEmpresa.UserId != userId)
                    return Forbid();

                var empresa = new Empresa
                {
                    Id = id,
                    Nome = request.Nome,
                    NumeroRegistro = request.NumeroRegistro,
                    Email = request.Email,
                    Telefone = request.Telefone,
                    Cep = request.Cep,
                    Endereco = request.Endereco,
                    Logo = request.Logo,
                    UserId = userId
                };

                var updatedEmpresa = await _empresaService.UpdateAsync(empresa);

                return Ok(updatedEmpresa);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erro interno do servidor", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                    return BadRequest(new { Message = "ID do usuário inválido" });

                var existingEmpresa = await _empresaService.GetByIdAsync(id);
                if (existingEmpresa == null)
                    return NotFound(new { Message = "Empresa não encontrada" });

                // Verificar se a empresa pertence ao usuário
                if (existingEmpresa.UserId != userId)
                    return Forbid();

                var deleted = await _empresaService.DeleteAsync(id);
                if (!deleted)
                    return NotFound(new { Message = "Empresa não encontrada" });

                return Ok(new { Message = "Empresa deletada com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erro interno do servidor", Error = ex.Message });
            }
        }
    }

    public class CreateEmpresaRequest
    {
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

        public string? Logo { get; set; }
    }

    public class UpdateEmpresaRequest
    {
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

        public string? Logo { get; set; }
    }
} 