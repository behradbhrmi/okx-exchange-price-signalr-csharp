using StackExchange.Redis;

namespace Redis;
class Redis
{
    private static IDatabase db;
    private static void Init()
    {
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
        db = redis.GetDatabase();
    }


    static string[] GetUserKeyValue()
    {
        string[] KeyValue = {"", ""};

        Console.WriteLine("key: ");
        KeyValue[0] = Console.ReadLine();

        Console.WriteLine("value: ");
        KeyValue[1] = Console.ReadLine();

        return KeyValue;
    }


    static void SetData(String[] KeyValue)
    {  
        if (KeyValue[0] != "" && KeyValue[1] != "")
        {
            db.StringSet(KeyValue[0], KeyValue[1]);
            Console.WriteLine("Saved");
        }
        else
        {
            Console.WriteLine("Invalid Data");
        }
    }

    static string? ReadData(string key)
    {
        return db.StringGet(key);
    }



    static void Main(string[] args)
    {
        Init();

        Console.WriteLine("--Inserting Data--");

        SetData(GetUserKeyValue());


        Console.WriteLine("--Retrieving Data--");

        Console.WriteLine("Enter Key: ");
        string? value = ReadData(Console.ReadLine());
        Console.WriteLine($"Vale: {value}");
    }
}