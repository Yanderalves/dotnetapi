namespace AmigoSecreto.Models;

public class Matches
{
    public Guid Id { get; set; }
    public long SenderId { get; set; }
    public Pessoa Sender { get; set; }
    public long ReceiverId { get; set; }
    public Pessoa Receiver { get; set; }
}