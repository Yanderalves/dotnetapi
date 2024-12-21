namespace AmigoSecreto.Models;

public class ApiResponse
{
    public bool success { get; set; }
    public object? Data { get; set; } = null;
    public string? Message { get; set; } = null;
}