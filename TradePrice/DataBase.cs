using Models;
//using StackExchange.Redis;

namespace DBManager;

class DBManager
{
	public void WriteDB()
	{
	}

	public void WriteRedis()
	{
	}

	public List<PriceModel> ReadDB()
	{
		return new List<PriceModel>();
	}

	public string ReadRedis()
	{
		return "";
	}
}

class DataBaseIO
{
	public Array FetchCurrencies()
	{
		using (var db = new DataBaseContext())
		{
			var currencyNames = db.Prices.Select(c => c.Currency).Distinct().ToArray();
			return currencyNames;
		}
	}
	public List<PriceModel> FetchPrice(string currencyName, DateTime startDate, DateTime endDate)
	{
		using (var db = new DataBaseContext())
		{
			var pricesInRange = db.Prices
			.Where(p => p.Currency == currencyName && p.dateTime >= startDate && p.dateTime <= endDate)
			.ToList();
			return pricesInRange;
		}
	}
	public  void WriteDB(Quote currencyQoute)
	{
		PriceModel priceModel = new PriceModel();
		priceModel.Currency = currencyQoute.symbol;
		priceModel.Price = Convert.ToDecimal(currencyQoute.mid);
		priceModel.dateTime = DateTimeOffset.FromUnixTimeMilliseconds(currencyQoute.ts).DateTime;

		using (var db = new DataBaseContext())
		{
			db.Add(priceModel);
			db.SaveChanges();
		}
	}
}
//class RedisIO
//{
//	private static IDatabase db;
//	private static void Initialize()
//	{
//		ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
//		db = redis.GetDatabase(); 
//	}
//	private void SetData(string key,string value)	{db.StringSet(key, value);}

//	private string? GetData(string key)	{return db.StringGet(key);}
//}
