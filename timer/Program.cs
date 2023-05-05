namespace Timer
{
    internal class Program
    {


        private TimeSpan Duration;


        private DateTime getTime() { return DateTime.Now; }


        async void Timer(CancellationToken token)
        {
            DateTime now = DateTime.Now;


            while (!token.IsCancellationRequested)
            {
                Duration = getTime() - now;

                string outPut = $"{Duration.Hours}:{Duration.Minutes}:{Duration.Seconds}";

                Console.WriteLine(outPut);

                await Task.Delay(1000);

            }
        }


        static void Main(string[] args)
        {
            Console.WriteLine("|=========( Timer )=========|");
            Console.WriteLine("Press any key to Start.");
            Console.ReadLine();

            Program program = new Program();

            var cancelToken = new CancellationTokenSource();


            Console.WriteLine("The timer stated, Press any key to Exit.");


            program.Timer(cancelToken.Token);


            Console.ReadLine();
            cancelToken.Cancel();

            Console.WriteLine($"Duration :  {program.Duration.TotalSeconds} Seconds");
            Console.WriteLine("|=======( Finished )=======|");

        }
    }
}