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
                
                await _context.Pessoas.AddAsync(pessoa);
                await _context.SaveChangesAsync();
                
                return Created();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu erro ao tentar salvar o item");
            }
            
        }
    }
}
