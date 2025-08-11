using AutoAppdater.Command;
using AutoAppdater.Consoles;
using AutoAppdater.Property;

namespace AutoAppdater.Interfaces
{
    public static class Interface
    {
        struct ConfigValue
        {
            public const bool Candidacies_Enable = true;
            public const int Candidacies_GetCount = 10;
            public const int History_RegistCount = 10;
            public const bool Password_Hide = true;
            public const int Password_Timeout = 300000;//5min.
        }
        struct DefaultValue
        {
            public const string err_com_not_found = "Unknown command input.Type 'help' to list keywords.";
        }
        public static bool Observing { get { return observing; } }
        public static bool PasswordObserving { get { return PasswordObserving;} }
        static bool pswObserving = false;
        static bool observing = false;
        public static int[] ObservingRegion { get { return observingRegion.ToArray(); } }
        static List<int> observingRegion = [];
        static Log.Log log = Common.Common.DefaultLogHost;
        static PropertyGroup config = new PropertyGroup();
        static Consoles.Console observerHost = new Consoles.Console();
        static Consoles.Console passwordHost = new Consoles.Console();
        static CancellationTokenSource observerCts = new CancellationTokenSource();
        static bool observerPausing = false;
        static object obl = new object();
        static object stateLocker = new object();
        static void Call(string[] args)
        {
            CommandResponse? c = CommandSet.CallCommandComponent(args);
            log.Error(DefaultValue.err_com_not_found);
        }
        public static void BeginObserve(int displayChannel)
        {
            lock (stateLocker)
            {
                if (observing) return;
                observing = true;
                observingRegion = [displayChannel];
                Task t = Task.Run(() => Observer(), observerCts.Token);
            }
        }
        public static void StopObserve()
        {
            lock (stateLocker)
            {
                if (!observing) return;
                observing = false;
                observingRegion = [];
                observerCts.Cancel();
                observerHost.RemoveAll();
            }
        }
        static void ResumeObserver()
        {
            lock (stateLocker)
            {
                if (!observing || !observerPausing) return;
                observing = true;
                Task t = Task.Run(() => Observer(), observerCts.Token);
            }
        }
        static void PauseObserve()
        {
            lock (stateLocker)
            {
                if (!observing || observerPausing) return;
                observing = false;
                observerCts.Cancel();
                observerHost.HideAll();
                //ato shori
            }
        }
        static void Observer()
        {
            lock (obl)
            {
                //config
                Property.Property? p;
                const ConsoleKey move_left = ConsoleKey.LeftArrow;
                const ConsoleKey move_right = ConsoleKey.RightArrow;
                const ConsoleKey hist_up = ConsoleKey.UpArrow;
                const ConsoleKey hist_down = ConsoleKey.DownArrow;
                const ConsoleKey cand_next = ConsoleKey.Tab;
                const ConsoleKey backspace = ConsoleKey.Backspace;
                const ConsoleKey space = ConsoleKey.Spacebar;
                const ConsoleKey enter = ConsoleKey.Enter;
                bool cand_enable = ConfigValue.Candidacies_Enable;
                p = config.GetPropertyByName(nameof(ConfigValue.Candidacies_Enable));
                if (p != null && p.Value.BoolValue != null) cand_enable = (bool)p.Value.BoolValue;
                int cand_count = ConfigValue.Candidacies_GetCount;
                p = config.GetPropertyByName(nameof(ConfigValue.Candidacies_GetCount));
                if (p != null && p.Value.IntValue != null) cand_count = (int)p.Value.IntValue;
                int hist_count = ConfigValue.History_RegistCount;
                p = config.GetPropertyByName(nameof(ConfigValue.History_RegistCount));
                if (p != null && p.Value.IntValue != null) hist_count = (int)p.Value.IntValue;
                string split = " ";
                //loopValue
                string currentSentence = "";
                int currentPosition = 0;
                List<string> history = [];
                int historyLen = -1;
                List<string> cand = [];
                string candKeep = "";
                int candLen = -1;
                while (observing)
                {
                    ConsoleKeyInfo info = System.Console.ReadKey(true);
                    if (info.Key == move_left)
                    {
                        if (currentPosition > 0)
                        {
                            currentPosition--;
                        }
                    }
                    else
                    if (info.Key == move_right)
                    {
                        if (currentPosition < currentSentence.Length)
                        {
                            currentPosition++;
                        }
                    }
                    else
                    if (info.Key == hist_up)
                    {
                        if (history.Count > 0)
                        {
                            if (historyLen < 0)
                            {
                                currentSentence = history[history.Count - 1];
                                currentPosition = currentSentence.Length;
                                historyLen = history.Count - 1;
                                history.Add(currentSentence);
                                if (history.Count > hist_count)
                                {
                                    history.RemoveAt(0);
                                }
                            }
                            else
                            {
                                if (historyLen > 0)
                                {
                                    currentSentence = history[historyLen - 1];
                                    currentPosition = currentSentence.Length;
                                    historyLen--;
                                }
                            }
                        }
                    }
                    else
                    if (info.Key == hist_down)
                    {
                        if (history.Count > 0)
                        {
                            if (historyLen + 1 < history.Count)
                            {
                                currentSentence = history[historyLen + 1];
                                currentPosition = currentSentence.Length;
                                historyLen++;
                            }
                        }
                    }
                    else
                    if (info.Key == cand_next)
                    {
                        if (cand_enable)
                        {
                            if (cand.Count > 0)
                            {
                                historyLen = -1;
                                history.Add(currentSentence);
                                if (history.Count > hist_count)
                                {
                                    history.RemoveAt(0);
                                }
                                if (candLen < 0)
                                {
                                    candLen = 0;
                                    candKeep = currentSentence;
                                    currentSentence = cand[candLen];
                                    currentPosition = currentSentence.Length;
                                }
                                else if (candLen + 1 < cand.Count)
                                {
                                    candLen++;
                                    currentSentence = cand[candLen];
                                    currentPosition = currentSentence.Length;
                                }
                                else
                                {
                                    candLen = -1;
                                    currentSentence = candKeep;
                                    currentPosition = currentSentence.Length;
                                    candKeep = "";
                                }
                            }
                        }
                    }
                    else
                    if (info.Key == backspace)
                    {
                        if (currentPosition != 0)
                        {
                            historyLen = -1;
                            candLen = -1;
                            currentSentence = currentSentence.Remove(currentPosition - 1, 1);
                            currentPosition--;
                            //set candidacies
                            if (currentSentence.Replace(split, "").Length != 0)
                            {
                                Candidacies[] c = CommandSet.GetCandidacies(currentSentence.Split(split));
                                if (c.Length > 0)
                                {
                                    List<string> lis = [string.Join(split, c[0].mostMatchArgs)];
                                    if (cand_count > lis.Count)
                                    {
                                        bool flag = true;
                                        bool end = false;
                                        foreach (string[] s in c[0].mostMatchFullArgs)
                                        {
                                            if (flag)
                                            {
                                                if (Array.Equals(lis[0], s))
                                                {
                                                    flag = false;
                                                }
                                            }
                                            else
                                            {
                                                lis.Add(string.Join(split, s));
                                                if (cand_count <= lis.Count)
                                                {
                                                    end = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (!end)
                                        {
                                            for (int i = 1; i < c.Length; i++)
                                            {
                                                foreach (string[] s in c[i].mostMatchFullArgs)
                                                {
                                                    lis.Add(string.Join(split, s));
                                                    if (cand_count <= lis.Count)
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    cand = lis;
                                }
                            }
                        }
                    }
                    else
                    if (info.Key == space)
                    {
                        historyLen = -1;
                        candLen = -1;
                        currentSentence = currentSentence.Insert(currentPosition, info.KeyChar.ToString());
                        currentPosition++;
                        //set candidacies
                        if (currentSentence.Replace(split, "").Length != 0)
                        {
                            Candidacies[] c = CommandSet.GetCandidacies(currentSentence.Split(split));
                            if (c.Length > 0)
                            {
                                List<string> lis = [string.Join(split, c[0].mostMatchArgs)];
                                if (cand_count > lis.Count)
                                {
                                    bool flag = true;
                                    bool end = false;
                                    foreach (string[] s in c[0].mostMatchFullArgs)
                                    {
                                        if (flag)
                                        {
                                            if (Array.Equals(lis[0], s))
                                            {
                                                flag = false;
                                            }
                                        }
                                        else
                                        {
                                            lis.Add(string.Join(split, s));
                                            if (cand_count <= lis.Count)
                                            {
                                                end = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (!end)
                                    {
                                        for (int i = 1; i < c.Length; i++)
                                        {
                                            foreach (string[] s in c[i].mostMatchFullArgs)
                                            {
                                                lis.Add(string.Join(split, s));
                                                if (cand_count <= lis.Count)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                cand = lis;
                            }
                        }
                    }
                    else
                    if (info.Key == enter)
                    {
                        historyLen = -1;
                        candLen = -1;
                        cand = [];
                        candKeep = "";
                        history.Add(currentSentence);
                        if (history.Count > hist_count)
                        {
                            history.RemoveAt(0);
                        }
                        currentSentence = "";
                        Task t = Task.Run(() => Call(currentSentence.Split(split)));
                    }
                    else
                    {
                        historyLen = -1;
                        candLen = -1;
                        currentSentence = currentSentence.Insert(currentPosition, info.KeyChar.ToString());
                        currentPosition++;
                        //set candidacies
                        if (currentSentence.Replace(split, "").Length != 0)
                        {
                            Candidacies[] c = CommandSet.GetCandidacies(currentSentence.Split(split));
                            if (c.Length > 0)
                            {
                                List<string> lis = [string.Join(split, c[0].mostMatchArgs)];
                                if (cand_count > lis.Count)
                                {
                                    bool flag = true;
                                    bool end = false;
                                    foreach (string[] s in c[0].mostMatchFullArgs)
                                    {
                                        if (flag)
                                        {
                                            if (Array.Equals(lis[0], s))
                                            {
                                                flag = false;
                                            }
                                        }
                                        else
                                        {
                                            lis.Add(string.Join(split, s));
                                            if (cand_count <= lis.Count)
                                            {
                                                end = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (!end)
                                    {
                                        for (int i = 1; i < c.Length; i++)
                                        {
                                            foreach (string[] s in c[i].mostMatchFullArgs)
                                            {
                                                lis.Add(string.Join(split, s));
                                                if (cand_count <= lis.Count)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                cand = lis;
                            }
                        }
                    }
                }
            }
        }
        static string? PasswordInterruptor()
        {
            Property.Property? p;
            const ConsoleKey backspace = ConsoleKey.Backspace;
            const ConsoleKey space = ConsoleKey.Spacebar;
            const ConsoleKey enter = ConsoleKey.Enter;
            bool psw_hide = ConfigValue.Password_Hide;
            p = config.GetPropertyByName(nameof(ConfigValue.Password_Hide));
            if (p != null && p.Value.BoolValue != null) psw_hide = (bool)p.Value.BoolValue;
            int psw_tout = ConfigValue.Password_Timeout;
            p = config.GetPropertyByName(nameof(ConfigValue.Password_Timeout));
            if (p != null && p.Value.IntValue != null) psw_tout = (int)p.Value.IntValue;
            string split = " ";
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationTokenSource ctsd = new CancellationTokenSource();
            bool entered = false;
            //loopValue
            string currentSentence = "";
            int currentPosition = 0;
            Task t = Task.Run(() =>
            {
                while (true)
                {
                    ConsoleKeyInfo info = System.Console.ReadKey(true);
                    if (info.Key == backspace)
                    {
                        if (currentPosition > 0)
                        {
                            currentSentence = currentSentence.Remove(currentPosition - 1, 1);
                            currentPosition--;
                        }
                    }
                    else if (info.Key == space)
                    {
                        currentSentence.Insert(currentPosition, split);
                        currentPosition++;
                    }
                    else if (info.Key == enter)
                    {
                        entered = true;
                        ctsd.Cancel();
                        break;
                    }
                    else
                    {
                        currentSentence.Insert(currentPosition, info.KeyChar.ToString());
                        currentPosition++;
                    }
                }
            }, cts.Token);
            Task q =Task.Delay(psw_tout,ctsd.Token);
            q.Wait();
            if (!entered)
            {
                cts.Cancel();
                return null;
            }
            else
            {
                return currentSentence;
            }
        }
        public static string? PasswordInputRequest()
        {
            lock (stateLocker)
            {
                if (pswObserving) return null;
                PauseObserve();
                string? input = PasswordInterruptor();
                ResumeObserver();
                return input;
            }
        }
    }
    public class CommandResponse
    {
        
    }
}