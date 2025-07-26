using AutoAppdater.Console;
using AutoAppdater.Property;

namespace AutoAppdater.Interface
{
    public static class Interface
    {
        public static bool Observing { get{ return observing; } }
        static bool observing = false;
        static Log.Log log = Common.Common.DefaultLogHost;
        static PropertyGroup config;
        static Console.Console channel;
        static CancellationTokenSource observerCts;
        static object obl = new object();
        public static void BeginObserve()
        {
            if (observing) return;
            observing = true;
            CancellationToken token = observerCts.Token;
            Task t = Task.Run(() => Observer(), token);
        }
        public static void StopObserve()
        {
            if (!observing) return;
            observing = false;
            observerCts.Cancel();
            //ato syori
        }
        static void Observer()
        {
            lock (obl)
            {
                while (observing)
                {

                }
            }
        }
    }
    public class CommandResponse
    {
        
    }
}