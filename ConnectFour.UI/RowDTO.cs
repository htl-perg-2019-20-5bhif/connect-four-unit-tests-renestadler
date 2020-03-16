using System.Text.Json.Serialization;

public class RowDTO
{
    [JsonPropertyName("row")]
    public int Row { get; set; }
}