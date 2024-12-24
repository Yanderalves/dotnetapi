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

        [HttpPost]
        [EndpointSummary("Realizar sorteio do Amigo Secreto")]
        public async Task<IActionResult> GerarSorteio()
        {
            Random random = new Random();
            var lista_pessoas = (await _context.Pessoas.ToListAsync());
            Dictionary<Pessoa, bool> sorteados_receivers = new Dictionary<Pessoa, bool>();
            Dictionary<Pessoa, bool> sorteados_senders = new Dictionary<Pessoa, bool>();
            int choice = -1, count = 0;

            foreach (var pessoa in lista_pessoas)
            {
                while (count < lista_pessoas.Count * 2)
                {
                    choice = random.Next(0, lista_pessoas.Count - 1);
                    var receiver = lista_pessoas.Find(item => item.Id == lista_pessoas[choice].Id);
                    
                    if (!sorteados_receivers.ContainsKey(receiver) && !sorteados_senders.ContainsKey(pessoa) && receiver.Id != pessoa.Id)
                    {
                        Matches match = new Matches()
                        {
                            SenderId = pessoa.Id,
                            ReceiverId = receiver.Id
                        };
                        count++;
                        sorteados_senders.Add(pessoa, true);
                        sorteados_receivers.Add(receiver, true);
                        _context.Matches.AddAsync(match);
                        _context.SaveChangesAsync();
;                    }
                }
            }
            
            
            return Ok();
        }
    }
}

