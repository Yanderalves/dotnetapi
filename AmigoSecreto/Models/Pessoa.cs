using System.ComponentModel.DataAnnotations;

namespace AmigoSecreto.Models;

public class Pessoa
{
    [Required(ErrorMessage = "Inserir o nome é obrigatório")]
    public string Nome { get; set; }
    
    [Required(ErrorMessage = "Inserir o email é obrigatório")]
    [EmailAddress(ErrorMessage = "Insira um email no formato válido")] 
    public string Email { get; set; }
    
    public long Id { get; set; }
}