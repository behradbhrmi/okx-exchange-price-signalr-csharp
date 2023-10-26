using System.ComponentModel.DataAnnotations;

public class PriceModel
{
    [Key]
    public int Id { get; set; }
    public string Currency { get; set; } = null!;
    public DateTime dateTime { get; set; }
    public Decimal Price { get; set; }
}