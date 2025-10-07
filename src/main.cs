

namespace AutoAppdater
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Task t = Task.Delay(-1);
            t.Wait();
        }
    }
}