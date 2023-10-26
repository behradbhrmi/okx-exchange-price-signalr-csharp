using Microsoft.EntityFrameworkCore;

namespace Models;
public class DataBaseContext : DbContext
{
    public DbSet<PriceModel> Prices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(@"Data Source=D:\Workspace\#PlayGround\TradePrice\prices.db");
}
