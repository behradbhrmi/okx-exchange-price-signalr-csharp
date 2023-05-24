using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Models;

public class PriceModel
{
    [Key]
    public int Id { get; set; }
    public string Currency { get; set; } = null!;
    public DateTime dateTime { get; set; }
    public Decimal Price { get; set; }
}

public class DataBaseContext : DbContext
{
    public DbSet<PriceModel> Prices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(@"Data Source=D:\Workspace\PlayGround\csTradePiceWebSocket\prices.db");

}
