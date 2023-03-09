namespace FlightSimulator
{
    internal class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Starting simulator");
            Random random = new Random();

            var client = new HttpClient();

            int i = 1;
            while (true)
            {
                int num = random.Next(0, 2);
                if (num == 1)
                {
                    await client.GetAsync($"http://localhost:5242/land/A{i}");
                }
                else
                {
                    await client.GetAsync($"http://localhost:5242/takeoff/B{i}");
                }
                i++;
                await Task.Delay(2000);
            }
        }
    }
}