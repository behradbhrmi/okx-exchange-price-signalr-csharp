using StackExchange.Redis;
using Newtonsoft.Json;
using Models;

namespace csTradePiceWebSocket.DB;
class DBManager
{
    private readonly IDatabase _cache;
    private DataBaseIO _db;

    public DBManager()
    {
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
        _cache = redis.GetDatabase();
        _db = new DataBaseIO();
    }

    public string FetchPrice(string currencyName, string startDate, string endDate)
    {

        string key = $"{currencyName}/{startDate}/{endDate}";

        var data = FetchRedis(key);

        if (data == null)
        {
            var dbData = DataSerializer(
                _db.FetchPrice(currencyName,
                Convert.ToDateTime(startDate),
                Convert.ToDateTime(endDate)));

            WriteRedis(key, dbData);
            return dbData;
        }
        return data;
    }

    public string FetchCurrencies()
    {
        return DataSerializer(_db.FetchCurrencies());
    }

    public string DataSerializer(Array InputList)
    {
        return JsonConvert.SerializeObject(InputList);
    }

    public void WriteRedis(string key, string value)
    {
        _cache.StringSet(key, value);
    }

    public string? FetchRedis(string key)
    {
        string? cachedData = _cache.StringGet(key);

        if (!string.IsNullOrEmpty(cachedData)) return cachedData;
        else return null;
    }
}

class DataBaseIO
{
    public Array FetchCurrencies()
    {
        using (var db = new DataBaseContext()) { return db.Prices.Select(c => c.Currency).Distinct().ToArray(); }
    }

    public Array FetchPrice(string currencyName, DateTime startDate, DateTime endDate)
    {
        using (var db = new DataBaseContext())
        {
            var pricesInRange = db.Prices
            .Where(p => p.Currency == currencyName && p.dateTime >= startDate && p.dateTime <= endDate)
            .ToArray();
            return pricesInRange;
        }
    }

    public void WriteDB(Quote currencyQoute)
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