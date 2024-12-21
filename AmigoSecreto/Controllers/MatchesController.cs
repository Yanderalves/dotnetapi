using AmigoSecreto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AmigoSecreto.Controllers
{
    [Route("api/matches")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly AmigoSecretoContext _context;

        public MatchesController(AmigoSecretoContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetMatches()
        {
            try
            {
                List<Matches> matches =  await _context.Matches.ToListAsync();

                if (!matches.IsNullOrEmpty())
                    return Ok(new ApiResponse { success = true, Data = matches });
                return NotFound(new ApiResponse { success = true, Message = "Nenhum sorteio foi encontrado" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { success = false, message = "Ocorreu um erro interno."});

            }
        }
    }
}

