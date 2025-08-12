using System.Security.Cryptography.X509Certificates;
using AutoAppdater.Command;
using AutoAppdater.Consoles;
using AutoAppdater.Property;

namespace AutoAppdater.Interfaces
{
    public class Interface
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
        public bool Observing { get { return observing; } }
        public bool PasswordObserving { get { return PasswordObserving;} }
        bool pswObserving = false;
        bool observing = false;
        public int[] ObservingRegion { get { return observingRegion.ToArray(); } }
        List<int> observingRegion = [];
        readonly Log.Log log = Common.Common.DefaultLogHost;
        readonly PropertyGroup config = new PropertyGroup();
        readonly Consoles.Console observerHost;
        readonly Consoles.Console candHost;
        readonly Consoles.Console passwordHost;
        readonly CancellationTokenSource observerCts = new CancellationTokenSource();
        readonly bool observerPausing = false;
        readonly object obl = new object();
        readonly object stateLocker = new object();
        ObserverValue obsVal = new ObserverValue();
        public Interface(Consoles.Console observerHost, Consoles.Console candHost, Consoles.Console passwordHost)
        {
            this.observerHost = observerHost;
            this.candHost = candHost;
            this.passwordHost = passwordHost;
        }
        void Call(string[] args)
        {
            CommandResponse? c = CommandSet.LocalCallCommandComponent(args);
            log.Error(DefaultValue.err_com_not_found);
        }
        public  void BeginObserve(int displayChannel)
        {
            lock (stateLocker)
            {
                if (observing) return;
                obsVal = new ObserverValue();
                observing = true;
                observingRegion = [displayChannel];
                Task t = Task.Run(() => Observer(), observerCts.Token);
            }
        }
        public  void StopObserve()
        {
            lock (stateLocker)
            {
                if (!observing) return;
                observing = false;
                observingRegion = [];
                observerCts.Cancel();
                observerHost.RemoveAll();
                obsVal = new ObserverValue();
            }
        }
        void ResumeObserver()
        {
            lock (stateLocker)
            {
                if (!observing || !observerPausing) return;
                observing = true;
                observerHost.ShowAll();
                Task t = Task.Run(() => Observer(), observerCts.Token);
            }
        }
        void PauseObserve()
        {
            lock (stateLocker)
            {
                if (!observing || observerPausing) return;
                observing = false;
                observerCts.Cancel();
                observerHost.HideAll();
            }
        }
        class ObserverValue
        {
            internal string currentSentence = "";
            internal int currentPosition = 0;
            internal List<string> history = [];
            internal int historyLen = -1;
            internal List<string> cand = [];
            internal string candKeep = "";
            internal int candLen = -1;
        }
        void Observer()
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
                //init
                if (observerHost.ColumnCount == 0) observerHost.Add("");
                while (observing)
                {
                    ConsoleKeyInfo info = System.Console.ReadKey(true);
                    if (info.Key == move_left)
                    {
                        if (obsVal.currentPosition > 0)
                        {
                            obsVal.currentPosition--;
                        }
                    }
                    else
                    if (info.Key == move_right)
                    {
                        if (obsVal.currentPosition < obsVal.currentSentence.Length)
                        {
                            obsVal.currentPosition++;
                        }
                    }
                    else
                    if (info.Key == hist_up)
                    {
                        if (obsVal.history.Count > 0)
                        {
                            if (obsVal.historyLen < 0)
                            {
                                obsVal.currentSentence = obsVal.history[obsVal.history.Count - 1];
                                obsVal.currentPosition = obsVal.currentSentence.Length;
                                obsVal.historyLen = obsVal.history.Count - 1;
                                obsVal.history.Add(obsVal.currentSentence);
                                if (obsVal.history.Count > hist_count)
                                {
                                    obsVal.history.RemoveAt(0);
                                }
                            }
                            else
                            {
                                if (obsVal.historyLen > 0)
                                {
                                    obsVal.currentSentence = obsVal.history[obsVal.historyLen - 1];
                                    obsVal.currentPosition = obsVal.currentSentence.Length;
                                    obsVal.historyLen--;
                                }
                            }
                            observerHost.ReplaceAt(0, obsVal.currentSentence);
                        }
                    }
                    else
                    if (info.Key == hist_down)
                    {
                        if (obsVal.history.Count > 0)
                        {
                            if (obsVal.historyLen + 1 < obsVal.history.Count)
                            {
                                obsVal.currentSentence = obsVal.history[obsVal.historyLen + 1];
                                obsVal.currentPosition = obsVal.currentSentence.Length;
                                obsVal.historyLen++;
                                observerHost.ReplaceAt(0, obsVal.currentSentence);
                            }
                        }
                    }
                    else
                    if (info.Key == cand_next)
                    {
                        if (cand_enable)
                        {
                            if (obsVal.cand.Count > 0)
                            {
                                obsVal.historyLen = -1;
                                obsVal.history.Add(obsVal.currentSentence);
                                if (obsVal.history.Count > hist_count)
                                {
                                    obsVal.history.RemoveAt(0);
                                }
                                if (obsVal.candLen < 0)
                                {
                                    obsVal.candLen = 0;
                                    obsVal.candKeep = obsVal.currentSentence;
                                    obsVal.currentSentence = obsVal.cand[obsVal.candLen];
                                    obsVal.currentPosition = obsVal.currentSentence.Length;
                                }
                                else if (obsVal.candLen + 1 < obsVal.cand.Count)
                                {
                                    obsVal.candLen++;
                                    obsVal.currentSentence = obsVal.cand[obsVal.candLen];
                                    obsVal.currentPosition = obsVal.currentSentence.Length;
                                }
                                else
                                {
                                    obsVal.candLen = -1;
                                    obsVal.currentSentence = obsVal.candKeep;
                                    obsVal.currentPosition = obsVal.currentSentence.Length;
                                    obsVal.candKeep = "";
                                }
                            }
                        }
                    }
                    else
                    if (info.Key == backspace)
                    {
                        if (obsVal.currentPosition != 0)
                        {
                            obsVal.historyLen = -1;
                            obsVal.candLen = -1;
                            obsVal.currentSentence = obsVal.currentSentence.Remove(obsVal.currentPosition - 1, 1);
                            obsVal.currentPosition--;
                            //set candidacies
                            if (obsVal.currentSentence.Replace(split, "").Length != 0)
                            {
                                Candidacies[] c = CommandSet.GetCandidacies(obsVal.currentSentence.Split(split));
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
                                    obsVal.cand = lis;
                                }
                            }
                        }
                    }
                    else
                    if (info.Key == space)
                    {
                        obsVal.historyLen = -1;
                        obsVal.candLen = -1;
                        obsVal.currentSentence = obsVal.currentSentence.Insert(obsVal.currentPosition, info.KeyChar.ToString());
                        obsVal.currentPosition++;
                        //set candidacies
                        if (obsVal.currentSentence.Replace(split, "").Length != 0)
                        {
                            Candidacies[] c = CommandSet.GetCandidacies(obsVal.currentSentence.Split(split));
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
                                obsVal.cand = lis;
                            }
                        }
                    }
                    else
                    if (info.Key == enter)
                    {
                        obsVal.historyLen = -1;
                        obsVal.candLen = -1;
                        obsVal.cand = [];
                        obsVal.candKeep = "";
                        obsVal.history.Add(obsVal.currentSentence);
                        if (obsVal.history.Count > hist_count)
                        {
                            obsVal.history.RemoveAt(0);
                        }
                        obsVal.currentSentence = "";
                        Task t = Task.Run(() => Call(obsVal.currentSentence.Split(split)));
                    }
                    else
                    {
                        obsVal.historyLen = -1;
                        obsVal.candLen = -1;
                        obsVal.currentSentence = obsVal.currentSentence.Insert(obsVal.currentPosition, info.KeyChar.ToString());
                        obsVal.currentPosition++;
                        //set candidacies
                        if (obsVal.currentSentence.Replace(split, "").Length != 0)
                        {
                            Candidacies[] c = CommandSet.GetCandidacies(obsVal.currentSentence.Split(split));
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
                                obsVal.cand = lis;
                            }
                        }
                    }
                }
            }
        }
        string? PasswordInterruptor()
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
        public  string? PasswordInputRequest()
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