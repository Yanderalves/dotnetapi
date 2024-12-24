using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmigoSecreto.Models;

namespace AmigoSecreto.Controllers
{
    [Route("api/pessoa")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly AmigoSecretoContext _context;

        public PessoaController(AmigoSecretoContext context)
        {
            _context = context;
        }

        [HttpGet]
        [EndpointSummary("Buscar todas as pessoas")]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas()
        {
            try
            {
                var data = await _context.Pessoas.ToListAsync();
                return Ok(new ApiResponse { success = true, Data = data });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse{success = false, Message = "Ocorreu um erro ao buscar dados. Tente novamente mais tarde."});
            }
           
        }

        [HttpGet("{id}")]
        [EndpointSummary("Buscar pessoa pelo ID")]
        
        public async Task<IActionResult> GetPessoa(long id)
        {
            try
            {
                var pessoa =  await _context.Pessoas.FindAsync(id);

                if (pessoa is null)
                    return NotFound(new ApiResponse{success = false, Message = "Pessoa não encontrada"});
                return Ok(new ApiResponse{success = true, Data = pessoa});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
        
        [HttpPost]
        [EndpointSummary("Cadastre uma nova pessoa.")]
        public async Task<IActionResult> PostPessoa(Pessoa pessoa)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var person = await _context.Pessoas.FirstOrDefaultAsync(item => item.Email == pessoa.Email);
                
                if(person is not null)
                    return Conflict(new ApiResponse {success = false, Message = "Já existe um usuário cadastrado com este email"});

                await _context.Pessoas.AddAsync(pessoa);
                await _context.SaveChangesAsync();
                
                return Created();
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse {success = false, Message = "Ocorreu erro ao tentar salvar o item"});
            }
            
        }

        [HttpDelete]
        [EndpointSummary("Deletar pessoa")]
        public async Task<IActionResult> DeletePessoa(string email)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                var pessoa = await _context.Pessoas.FirstOrDefaultAsync(item => item.Email == email);

                if (pessoa is null)
                    return NotFound(new ApiResponse
                    {
                        success = false,
                        Message = "Item não encontrado."
                    });
                
                _context.Pessoas.Remove(pessoa);
                _context.SaveChangesAsync();
                return NoContent();

            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse {success = false, Message = "Ocorreu erro interno."});
            }
            
        }
    }
}
