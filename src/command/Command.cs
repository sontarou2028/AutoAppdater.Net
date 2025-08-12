using AutoAppdater.Interfaces;
using AutoAppdater.Common;
using AutoAppdater.MainSenderHost;

namespace AutoAppdater.Command
{
    //CommandManageSystem Ver1.0 Release 2025/7/19
    public enum CommandType
    {
        //Initial,
        Word,
        IntValue,
        BoolValue,
        StrValue,
        Option,
    }
    public class EventHandler
    {
        public delegate CommandResponse? baseDelegate0();
        public event baseDelegate0? CallStepEventHandler0;
        public delegate CommandResponse? baseDelegate1(string?[] value);
        public event baseDelegate1? CallStepEventHandler1;
        public delegate CommandResponse? baseDelegate2(IReadOnlyCommandComponent components);
        public event baseDelegate2? CallStepEventHandler2;
        public delegate CommandResponse? baseDelegate3(string[] args);
        public event baseDelegate2? CallStepEventHandler3;
        public (baseDelegate0? del0, baseDelegate1? del1, baseDelegate2? del2, baseDelegate2? del3) getEventHandler()
        {
            return (CallStepEventHandler0, CallStepEventHandler1, CallStepEventHandler2, CallStepEventHandler3);
        }
        public void SwapEventHandler((baseDelegate0? del0, baseDelegate1? del1, baseDelegate2? del2, baseDelegate2? del3) callStepEventHandler)
        {
            CallStepEventHandler0 = callStepEventHandler.del0;
            CallStepEventHandler1 = callStepEventHandler.del1;
            CallStepEventHandler2 = callStepEventHandler.del2;
            CallStepEventHandler3 = callStepEventHandler.del3;
        }
        internal bool ExistHandles()
        {
            if (CallStepEventHandler0 != null || CallStepEventHandler1 != null ||
            CallStepEventHandler2 != null || CallStepEventHandler3 != null)
                return true;
            else
                return false;
        }
        internal CommandResponse? Call(IReadOnlyCommandComponent component)
        {
            CommandResponse? res = null;
            if (CallStepEventHandler0 != null)
            {
                IEnumerable<CommandResponse?> tas = CallStepEventHandler0.GetInvocationList().OfType<baseDelegate0>().Select((x) => { return x.Invoke(); });
                foreach (CommandResponse? t in tas)
                {
                    CommandResponse? resp = t;
                    if (resp != null)
                    {
                        res = resp;
                    }
                }
                #region repair
                //await AutoSystem.TriggerCall(new TriggerHandle() { Type = ScriptTrigger.TriggerType.command_excute });
                #endregion
            }
            if (CallStepEventHandler1 != null)
            {
                List<string?> value = new List<string?>();
                IReadOnlyCommandComponent target = component;
                while (true)
                {
                    if (target.Type == CommandType.IntValue || target.Type == CommandType.BoolValue ||
                    target.Type == CommandType.StrValue || target.Type == CommandType.Option)
                    {
                        if (target.Value != null)
                        {
                            value.Add((string)target.Value);
                        }
                        else
                        {
                            value.Add(null);
                        }
                    }
                    else if (target.Type == CommandType.Word)
                    {
                        value.Add(null);
                    }
                    if (target.Components.Length == 0) break;
                    else target = target.Components[0];
                }
                IEnumerable<CommandResponse?> tas = CallStepEventHandler1.GetInvocationList().OfType<baseDelegate1>().Select((x) => { return x.Invoke(value.ToArray()); });
                foreach (CommandResponse? t in tas)
                {
                    CommandResponse? resp = t;
                    if (resp != null)
                    {
                        res = resp;
                    }
                }
                #region repair
                //await AutoSystem.TriggerCall(new TriggerHandle() { Type = ScriptTrigger.TriggerType.command_excute });
                #endregion
            }
            if (CallStepEventHandler2 != null)
            {
                IEnumerable<CommandResponse?> tas = CallStepEventHandler2.GetInvocationList().OfType<baseDelegate2>().Select((x) => { return x.Invoke(component); });
                foreach (CommandResponse? t in tas)
                {
                    CommandResponse? resp = t;
                    if (resp != null)
                    {
                        res = resp;
                    }
                }
                #region repair
                //await AutoSystem.TriggerCall(new TriggerHandle() { Type = ScriptTrigger.TriggerType.command_excute });
                #endregion
            }
            if (CallStepEventHandler3 != null)
            {
                List<string> args = new List<string>();
                IReadOnlyCommandComponent target = component;
                while (true)
                {
                    if (target.Type == CommandType.IntValue || target.Type == CommandType.BoolValue ||
                    target.Type == CommandType.StrValue || target.Type == CommandType.Option)
                    {
                        if (target.Value != null)
                        {
                            args.Add((string)target.Value);
                        }
                        else
                        {
                            args.Add("");
                        }
                    }
                    else if (target.Type == CommandType.Word)
                    {
                        args.Add(target.Name);
                    }
                    if (target.Components.Length == 0) break;
                    else target = target.Components[0];
                }
                IEnumerable<CommandResponse?> tas = CallStepEventHandler3.GetInvocationList().OfType<baseDelegate3>().Select((x) => { return x.Invoke(args.ToArray()); });
                foreach (CommandResponse? t in tas)
                {
                    CommandResponse? resp = t;
                    if (resp != null)
                    {
                        res = resp;
                    }
                }
                #region repair
                //await AutoSystem.TriggerCall(new TriggerHandle() { Type = ScriptTrigger.TriggerType.command_excute });
                #endregion
            }
            return res;
        }
    }
    /// <summary>
    /// Use this to setup command component.
    /// </summary>
    public class ICommandComponent
    {
        public string Name { get { return name; } }
        public string? Expression { get { return expression; } }
        public string[]? Candidacies { get { return candidacies; } }
        public CommandType Type { get { return type; } }
        public ICommandComponent[] Components { get { return components; } }
        public EventHandler Handler { get { return handler; } }

        string name;
        string? expression;
        string[]? candidacies;
        CommandType type;
        ICommandComponent[] components;
        EventHandler handler = new EventHandler();
        public ICommandComponent(string name, CommandType type)
        {
            this.name = name;
            expression = null;
            this.type = type;
            components = [];
        }
        public ICommandComponent(string name, CommandType type, ICommandComponent[] components)
        {
            this.name = name;
            expression = null;
            this.type = type;
            this.components = components;
        }
        public ICommandComponent(string name, string expression, CommandType type)
        {
            this.name = name;
            this.expression = expression;
            this.type = type;
            components = [];
        }
        public ICommandComponent(string name, string expression, CommandType type, ICommandComponent[] components)
        {
            this.name = name;
            this.expression = expression;
            this.type = type;
            this.components = components;
        }
        public ICommandComponent(string name, string[]? candidacies, CommandType type)
        {
            this.name = name;
            expression = null;
            this.candidacies = candidacies;
            this.type = type;
            components = [];
        }
        public ICommandComponent(string name, string[]? candidacies, CommandType type, ICommandComponent[] components)
        {
            this.name = name;
            expression = null;
            this.candidacies = candidacies;
            this.type = type;
            this.components = components;
        }
        public ICommandComponent(string name, string expression, string[]? candidacies, CommandType type)
        {
            this.name = name;
            this.expression = expression;
            this.candidacies = candidacies;
            this.type = type;
            components = [];
        }
        public ICommandComponent(string name, string expression, string[]? candidacies, CommandType type, ICommandComponent[] components)
        {
            this.name = name;
            this.expression = expression;
            this.candidacies = candidacies;
            this.type = type;
            this.components = components;
        }
    }
    /// <summary>
    /// Use this to internal computing of command components.
    /// </summary>
    internal class CommandComponent
    {
        public string Name { get { return name; } }
        public string? Expresssion { get { return expression; } }
        public string[]? Candidacies { get { return candidacies; } }
        public int Id { get { return id; } }
        public CommandType Type { get { return type; } }
        public string? Value { get { return value; } set { this.value = value; } }
        public CommandComponent[] Components { get { return components; } set { components = value; } }
        public EventHandler Handler { get { return handler; } }

        string name;
        string? expression;
        string[]? candidacies;
        int id;
        CommandType type;
        string? value;
        CommandComponent[] components;
        EventHandler handler = new EventHandler();
        public CommandComponent(string name, int id, CommandType type)
        {
            this.name = name;
            this.id = id;
            this.type = type;
            components = [];
        }
        public CommandComponent(string name, int id, CommandType type, CommandComponent[] components)
        {
            this.name = name;
            this.id = id;
            this.type = type;
            this.components = components;
        }
        public CommandComponent(string name, string? expression, int id, CommandType type)
        {
            this.name = name;
            this.expression = expression;
            this.id = id;
            this.type = type;
            components = [];
        }
        public CommandComponent(string name, string? expression, int id, CommandType type, CommandComponent[] components)
        {
            this.name = name;
            this.expression = expression;
            this.id = id;
            this.type = type;
            this.components = components;
        }
        public CommandComponent(string name, string[]? candidacies, int id, CommandType type)
        {
            this.name = name;
            this.candidacies = candidacies;
            this.id = id;
            this.type = type;
            components = [];
        }
        public CommandComponent(string name, string[]? candidacies, int id, CommandType type, CommandComponent[] components)
        {
            this.name = name;
            this.candidacies = candidacies;
            this.id = id;
            this.type = type;
            this.components = components;
        }
        public CommandComponent(string name, string? expression, string[]? candidacies, int id, CommandType type)
        {
            this.name = name;
            this.expression = expression;
            this.candidacies = candidacies;
            this.id = id;
            this.type = type;
            components = [];
        }
        public CommandComponent(string name, string? expression, string[]? candidacies, int id, CommandType type, CommandComponent[] components)
        {
            this.name = name;
            this.expression = expression;
            this.candidacies = candidacies;
            this.id = id;
            this.type = type;
            this.components = components;
        }
        /*
        public void SetValue(object? value)
        {
            this.value = value;
        }
        */
    }
    /// <summary>
    /// Use this to only returning.
    /// </summary>
    public class IReadOnlyCommandComponent
    {
        public string Name { get { return name; } }
        public CommandType Type { get { return type; } }
        public object? Value { get { return value; } }
        public IReadOnlyCommandComponent[] Components { get { return components; } }
        string name;
        CommandType type;
        object? value;
        IReadOnlyCommandComponent[] components;
        public IReadOnlyCommandComponent(string name, CommandType type, object? value)
        {
            this.name = name;
            this.type = type;
            this.value = value;
            this.components = [];
        }
        public IReadOnlyCommandComponent(string name, CommandType type, object? value, IReadOnlyCommandComponent[] components)
        {
            this.name = name;
            this.type = type;
            this.value = value;
            this.components = components;
        }
    }
    public enum CallOption
    {
        MatchStep,
        MatchTop,
        PerfectStep,
        Perfect,
    }

    public class Candidacies
    {
        public string mostMatch { get; }
        public string[] mostMatchArgs { get; }
        public string[][] mostMatchFullArgs { get; }
        internal Candidacies(string mostMatch, string[] mostMatchArgs, string[][] mostMatchFullArgs)
        {
            this.mostMatch = mostMatch;
            this.mostMatchArgs = mostMatchArgs;
            this.mostMatchFullArgs = mostMatchFullArgs;
        }
        public Candidacies ToCandidacies()
        {
            return new Candidacies(mostMatch, mostMatchArgs, mostMatchFullArgs);
        }
    }
    public enum DisplayOption
    {
        Ladder,
        Column,
    }
    public static class CommandSet
    {
        //static CommandComponent[] Components { get{ return components.ToArray(); } }
        static Log.Log log = Common.Common.DefaultLogHost;
        static List<CommandComponent> components = new List<CommandComponent>();
        static int idStatus = 0;
        static int getNewId { get { int ret = idStatus; idStatus++; return ret; } }
        public static void RegistCommandComponent(ICommandComponent component)
        {
            if (component.Components.Length == 0)
            {
                components.Add(new CommandComponent(component.Name, component.Candidacies, getNewId, component.Type));
                return;
            }
            int level = 0;
            List<int> taskc = [0];
            ICommandComponent icom = component;
            List<ICommandComponent> itarget = [icom];
            CommandComponent com = new CommandComponent(component.Name, component.Candidacies, getNewId, component.Type);
            List<CommandComponent> target = [com];
            while (true)
            {
                if (itarget[level].Components.Length > taskc[level])
                {
                    List<CommandComponent> lis = target[level].Components.ToList();
                    ICommandComponent c = itarget[level].Components[taskc[level]];
                    CommandComponent cc = new CommandComponent(c.Name, c.Expression, c.Candidacies, getNewId, c.Type);
                    cc.Handler.SwapEventHandler(c.Handler.getEventHandler());
                    lis.Add(cc);
                    target[level].Components = lis.ToArray();
                    taskc.Add(0);
                    itarget.Add(itarget[level].Components[taskc[level]]);
                    target.Add(target[level].Components[taskc[level]]);
                    level++;
                }
                else
                {
                    if (level == 0) break;
                    else
                    {
                        taskc.RemoveAt(level);
                        target.RemoveAt(level);
                        itarget.RemoveAt(level);
                        level--;
                        taskc[level]++;
                    }
                }
            }
            components.Add(com);
        }
        public static void RegistCommandComponent(ICommandComponent[] components)
        {
            foreach (ICommandComponent component in components)
            {
                RegistCommandComponent(component);
            }
        }

        public static Candidacies[] GetCandidacies(string[] args)
        {
            string[] True = { "true", "True" };
            string[] False = { "false", "False" };
            if (components.Count != 0 && args.Length != 0)
            {
                List<Candidacies> candidacies = [];
                for (int c = 0; c < components.Count; c++)
                {
                    List<CommandComponent> history = [components[c]];
                    List<int> refe = [0];
                    for (int i = 0; i < args.Length; i++)
                    {
                        string mostMatch;
                        List<string> mostMatchArgs = [];
                        List<string[]> mostMatchFullArgs = [];
                        if (history[i].Type == CommandType.Word)
                        {
                            if (args[i].Length <= history[i].Name.Length &&
                                    history[i].Name.Substring(0, args[i].Length).ToLower() == args[i].ToLower())
                            {
                                if (i == args.Length - 1)
                                {
                                    //last lap
                                    mostMatch = history[i].Name;
                                    foreach (CommandComponent commandComponent in history)
                                    {
                                        mostMatchArgs.Add(commandComponent.Name);
                                    }
                                    if (history[i].Components.Length != 0)
                                    {
                                        List<CommandComponent> subHistory = history.ToList();
                                        for (int d = 0; d < history[i].Components.Length; d++)
                                        {
                                            mostMatch = history[i].Name;
                                            int historyLen = history.Count;
                                            List<string> branch = mostMatchArgs.ToList();
                                            branch.Add(history[i].Components[d].Name);
                                            history.Add(history[i].Components[d]);//
                                            List<string[]> branches = [];
                                            List<int> reference = [0];
                                            bool foward = false;
                                            for (int j = historyLen; j > historyLen - 1;)
                                            {
                                                int refPosition = j - historyLen;
                                                if (history[j].Components.Length > reference[refPosition])
                                                {
                                                    branch.Add(history[j].Components[reference[refPosition]].Name);
                                                    history.Add(history[j].Components[reference[refPosition]]);
                                                    j++;
                                                    reference.Add(0);
                                                    foward = true;
                                                }
                                                else
                                                {
                                                    if (j > historyLen)
                                                    {
                                                        if (foward) branches.Add(branch.ToArray());
                                                        foward = false;
                                                        history.RemoveAt(j);
                                                        branch.RemoveAt(j);
                                                        reference.RemoveAt(refPosition);
                                                        j--;
                                                        reference[refPosition - 1]++;
                                                    }
                                                    else
                                                    {
                                                        if (history[j].Handler.ExistHandles() || history[j].Components.Length == 0)
                                                            branches.Add(branch.ToArray());
                                                        break;
                                                    }
                                                }
                                            }
                                            mostMatchFullArgs.AddRange(branches);
                                            history = subHistory.ToList();
                                        }
                                        //record
                                        candidacies.Add(new Candidacies(mostMatch, mostMatchArgs.ToArray(), mostMatchFullArgs.ToArray()));
                                        if (i > 0)
                                        {
                                            for (int j = i; j > 0; j--)
                                            {
                                                history.RemoveAt(j);
                                                refe.RemoveAt(j);
                                                refe[j - 1]++;
                                                if (history[j - 1].Components.Length > refe[j - 1])
                                                {
                                                    history.Add(history[j - 1].Components[refe[j - 1]]);
                                                    refe.Add(0);
                                                    break;
                                                }
                                                i--;
                                            }
                                            if (i <= 0)
                                            {
                                                break;//no match
                                            }
                                            i -= 1;
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        mostMatchFullArgs.Add(mostMatchArgs.ToArray());
                                        //record
                                        candidacies.Add(new Candidacies(mostMatch, mostMatchArgs.ToArray(), mostMatchFullArgs.ToArray()));
                                        if (i > 0)
                                        {
                                            for (int j = i; j > 0; j--)
                                            {
                                                history.RemoveAt(j);
                                                refe.RemoveAt(j);
                                                refe[j - 1]++;
                                                if (history[j - 1].Components.Length > refe[j - 1])
                                                {
                                                    history.Add(history[j - 1].Components[refe[j - 1]]);
                                                    refe.Add(0);
                                                    break;
                                                }
                                                i--;
                                            }
                                            if (i <= 0)
                                            {
                                                break;//no match
                                            }
                                            i -= 1;
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (history[i].Components.Length != 0)
                                    {
                                        history.Add(history[i].Components[refe[i]]);
                                        refe.Add(0);
                                        continue;
                                    }
                                    else
                                    {
                                        if (i > 0)
                                        {
                                            for (int j = i; j > 0; j--)
                                            {
                                                history.RemoveAt(j);
                                                refe.RemoveAt(j);
                                                refe[j - 1]++;
                                                if (history[j - 1].Components.Length > refe[j - 1])
                                                {
                                                    history.Add(history[j - 1].Components[refe[j - 1]]);
                                                    refe.Add(0);
                                                    break;
                                                }
                                                i--;
                                            }
                                            if (i <= 0)
                                            {
                                                break;//no match
                                            }
                                            i -= 1;
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //no hits
                                if (i > 0)
                                {
                                    for (int j = i; j > 0; j--)
                                    {
                                        history.RemoveAt(j);
                                        refe.RemoveAt(j);
                                        refe[j - 1]++;
                                        if (history[j - 1].Components.Length > refe[j - 1])
                                        {
                                            history.Add(history[j - 1].Components[refe[j - 1]]);
                                            refe.Add(0);
                                            break;
                                        }
                                        i--;
                                    }
                                    if (i <= 0)
                                    {
                                        break;//no match
                                    }
                                    i -= 1;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else if (history[i].Type == CommandType.Option)
                        {
                            string[]? cd = history[i].Candidacies;
                            if (cd == null)
                            {
                                if (i == args.Length - 1)
                                {
                                    //last lap
                                    mostMatch = history[i].Name;
                                    foreach (CommandComponent commandComponent in history)
                                    {
                                        mostMatchArgs.Add(commandComponent.Name);
                                    }
                                    if (history[i].Components.Length != 0)
                                    {
                                        List<CommandComponent> subHistory = history.ToList();
                                        for (int d = 0; d < history[i].Components.Length; d++)
                                        {
                                            mostMatch = history[i].Name;
                                            int historyLen = history.Count;
                                            List<string> branch = mostMatchArgs.ToList();
                                            branch.Add(history[i].Components[d].Name);
                                            history.Add(history[i].Components[d]);//
                                            List<string[]> branches = [];
                                            List<int> reference = [0];
                                            bool foward = false;
                                            for (int j = historyLen; j > historyLen - 1;)
                                            {
                                                int refPosition = j - historyLen;
                                                if (history[j].Components.Length > reference[refPosition])
                                                {
                                                    branch.Add(history[j].Components[reference[refPosition]].Name);
                                                    history.Add(history[j].Components[reference[refPosition]]);
                                                    j++;
                                                    reference.Add(0);
                                                    foward = true;
                                                }
                                                else
                                                {
                                                    if (j > historyLen)
                                                    {
                                                        if (foward) branches.Add(branch.ToArray());
                                                        foward = false;
                                                        history.RemoveAt(j);
                                                        branch.RemoveAt(j);
                                                        reference.RemoveAt(refPosition);
                                                        j--;
                                                        reference[refPosition - 1]++;
                                                    }
                                                    else
                                                    {
                                                        if (history[j].Handler.ExistHandles() || history[j].Components.Length == 0)
                                                            branches.Add(branch.ToArray());
                                                        break;
                                                    }
                                                }
                                            }
                                            mostMatchFullArgs.AddRange(branches);
                                            history = subHistory.ToList();
                                        }
                                        //record
                                        candidacies.Add(new Candidacies(mostMatch, mostMatchArgs.ToArray(), mostMatchFullArgs.ToArray()));
                                        if (i > 0)
                                        {
                                            for (int j = i; j > 0; j--)
                                            {
                                                history.RemoveAt(j);
                                                refe.RemoveAt(j);
                                                refe[j - 1]++;
                                                if (history[j - 1].Components.Length > refe[j - 1])
                                                {
                                                    history.Add(history[j - 1].Components[refe[j - 1]]);
                                                    refe.Add(0);
                                                    break;
                                                }
                                                i--;
                                            }
                                            if (i <= 0)
                                            {
                                                break;//no match
                                            }
                                            i -= 1;
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (history[i].Components.Length != 0)
                                    {
                                        history.Add(history[i].Components[refe[i]]);
                                        refe.Add(0);
                                        continue;
                                    }
                                    else
                                    {
                                        if (i > 0)
                                        {
                                            for (int j = i; j > 0; j--)
                                            {
                                                history.RemoveAt(j);
                                                refe.RemoveAt(j);
                                                refe[j - 1]++;
                                                if (history[j - 1].Components.Length > refe[j - 1])
                                                {
                                                    history.Add(history[j - 1].Components[refe[j - 1]]);
                                                    refe.Add(0);
                                                    break;
                                                }
                                                i--;
                                            }
                                            if (i <= 0)
                                            {
                                                break;//no match
                                            }
                                            i -= 1;
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                List<string> matchList = [];
                                foreach (string s in cd)
                                {
                                    if (args[i].Length <= s.Length &&
                                    s.Substring(0, args[i].Length).ToLower() == args[i].ToLower())
                                        matchList.Add(s);
                                }
                                if (matchList.Count != 0)
                                {
                                    if (i == args.Length - 1)
                                    {
                                        //last lap
                                        mostMatch = history[i].Name;
                                        foreach (CommandComponent commandComponent in history)
                                        {
                                            mostMatchArgs.Add(commandComponent.Name);
                                        }
                                        if (history[i].Components.Length != 0)
                                        {
                                            List<CommandComponent> subHistory = history.ToList();
                                            for (int d = 0; d < history[i].Components.Length; d++)
                                            {
                                                mostMatch = history[i].Name;
                                                int historyLen = history.Count;
                                                List<string> branch = mostMatchArgs.ToList();
                                                branch.Add(history[i].Components[d].Name);
                                                history.Add(history[i].Components[d]);//
                                                List<string[]> branches = [];
                                                List<int> reference = [0];
                                                bool foward = false;
                                                for (int j = historyLen; j > historyLen - 1;)
                                                {
                                                    int refPosition = j - historyLen;
                                                    if (history[j].Components.Length > reference[refPosition])
                                                    {
                                                        branch.Add(history[j].Components[reference[refPosition]].Name);
                                                        history.Add(history[j].Components[reference[refPosition]]);
                                                        j++;
                                                        reference.Add(0);
                                                        foward = true;
                                                    }
                                                    else
                                                    {
                                                        if (j > historyLen)
                                                        {
                                                            if (foward) branches.Add(branch.ToArray());
                                                            foward = false;
                                                            history.RemoveAt(j);
                                                            branch.RemoveAt(j);
                                                            reference.RemoveAt(refPosition);
                                                            j--;
                                                            reference[refPosition - 1]++;
                                                        }
                                                        else
                                                        {
                                                            if (history[j].Handler.ExistHandles() || history[j].Components.Length == 0)
                                                                branches.Add(branch.ToArray());
                                                            break;
                                                        }
                                                    }
                                                }
                                                mostMatchFullArgs.AddRange(branches);
                                                history = subHistory.ToList();
                                            }
                                            //record
                                            candidacies.Add(new Candidacies(mostMatch, mostMatchArgs.ToArray(), mostMatchFullArgs.ToArray()));
                                            if (i > 0)
                                            {
                                                for (int j = i; j > 0; j--)
                                                {
                                                    history.RemoveAt(j);
                                                    refe.RemoveAt(j);
                                                    refe[j - 1]++;
                                                    if (history[j - 1].Components.Length > refe[j - 1])
                                                    {
                                                        history.Add(history[j - 1].Components[refe[j - 1]]);
                                                        refe.Add(0);
                                                        break;
                                                    }
                                                    i--;
                                                }
                                                if (i <= 0)
                                                {
                                                    break;//no match
                                                }
                                                i -= 1;
                                                continue;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            mostMatchFullArgs.Add(mostMatchArgs.ToArray());
                                            //record
                                            foreach (string s in matchList)
                                            {
                                                candidacies.Add(new Candidacies(s, mostMatchArgs.ToArray(), mostMatchFullArgs.ToArray()));
                                            }
                                            if (i > 0)
                                            {
                                                for (int j = i; j > 0; j--)
                                                {
                                                    history.RemoveAt(j);
                                                    refe.RemoveAt(j);
                                                    refe[j - 1]++;
                                                    if (history[j - 1].Components.Length > refe[j - 1])
                                                    {
                                                        history.Add(history[j - 1].Components[refe[j - 1]]);
                                                        refe.Add(0);
                                                        break;
                                                    }
                                                    i--;
                                                }
                                                if (i <= 0)
                                                {
                                                    break;//no match
                                                }
                                                i -= 1;
                                                continue;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (history[i].Components.Length != 0)
                                        {
                                            history.Add(history[i].Components[refe[i]]);
                                            refe.Add(0);
                                            continue;
                                        }
                                        else
                                        {
                                            if (i > 0)
                                            {
                                                for (int j = i; j > 0; j--)
                                                {
                                                    history.RemoveAt(j);
                                                    refe.RemoveAt(j);
                                                    refe[j - 1]++;
                                                    if (history[j - 1].Components.Length > refe[j - 1])
                                                    {
                                                        history.Add(history[j - 1].Components[refe[j - 1]]);
                                                        refe.Add(0);
                                                        break;
                                                    }
                                                    i--;
                                                }
                                                if (i <= 0)
                                                {
                                                    break;//no match
                                                }
                                                i -= 1;
                                                continue;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //unknown option
                                    if (i > 0)
                                    {
                                        for (int j = i; j > 0; j--)
                                        {
                                            history.RemoveAt(j);
                                            refe.RemoveAt(j);
                                            refe[j - 1]++;
                                            if (history[j - 1].Components.Length > refe[j - 1])
                                            {
                                                history.Add(history[j - 1].Components[refe[j - 1]]);
                                                refe.Add(0);
                                                break;
                                            }
                                            i--;
                                        }
                                        if (i <= 0)
                                        {
                                            break;//no match
                                        }
                                        i -= 1;
                                        continue;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        else if (history[i].Type == CommandType.BoolValue)
                        {
                            bool trueFlag = false;
                            foreach (string s in True)
                            {
                                if (args[i] == s)
                                {
                                    trueFlag = true;
                                    break;
                                }
                            }
                            bool falseFlag = false;
                            foreach (string s in False)
                            {
                                if (args[i] == s)
                                {
                                    falseFlag = true;
                                    break;
                                }
                            }
                            if (trueFlag || falseFlag)
                            {
                                if (i == args.Length - 1)
                                {
                                    //last lap
                                    mostMatch = history[i].Name;
                                    foreach (CommandComponent commandComponent in history)
                                    {
                                        mostMatchArgs.Add(commandComponent.Name);
                                    }
                                    if (history[i].Components.Length != 0)
                                    {
                                        List<CommandComponent> subHistory = history.ToList();
                                        for (int d = 0; d < history[i].Components.Length; d++)
                                        {
                                            mostMatch = history[i].Name;
                                            int historyLen = history.Count;
                                            List<string> branch = mostMatchArgs.ToList();
                                            branch.Add(history[i].Components[d].Name);
                                            history.Add(history[i].Components[d]);//
                                            List<string[]> branches = [];
                                            List<int> reference = [0];
                                            bool foward = false;
                                            for (int j = historyLen; j > historyLen - 1;)
                                            {
                                                int refPosition = j - historyLen;
                                                if (history[j].Components.Length > reference[refPosition])
                                                {
                                                    branch.Add(history[j].Components[reference[refPosition]].Name);
                                                    history.Add(history[j].Components[reference[refPosition]]);
                                                    j++;
                                                    reference.Add(0);
                                                    foward = true;
                                                }
                                                else
                                                {
                                                    if (j > historyLen)
                                                    {
                                                        if (foward) branches.Add(branch.ToArray());
                                                        foward = false;
                                                        history.RemoveAt(j);
                                                        branch.RemoveAt(j);
                                                        reference.RemoveAt(refPosition);
                                                        j--;
                                                        reference[refPosition - 1]++;
                                                    }
                                                    else
                                                    {
                                                        if (history[j].Handler.ExistHandles() || history[j].Components.Length == 0)
                                                            branches.Add(branch.ToArray());
                                                        break;
                                                    }
                                                }
                                            }
                                            mostMatchFullArgs.AddRange(branches);
                                            history = subHistory.ToList();
                                        }
                                        //record
                                        candidacies.Add(new Candidacies(mostMatch, mostMatchArgs.ToArray(), mostMatchFullArgs.ToArray()));
                                        if (i > 0)
                                        {
                                            for (int j = i; j > 0; j--)
                                            {
                                                history.RemoveAt(j);
                                                refe.RemoveAt(j);
                                                refe[j - 1]++;
                                                if (history[j - 1].Components.Length > refe[j - 1])
                                                {
                                                    history.Add(history[j - 1].Components[refe[j - 1]]);
                                                    refe.Add(0);
                                                    break;
                                                }
                                                i--;
                                            }
                                            if (i <= 0)
                                            {
                                                break;//no match
                                            }
                                            i -= 1;
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        mostMatchFullArgs.Add(mostMatchArgs.ToArray());
                                        //record
                                        candidacies.Add(new Candidacies(mostMatch, mostMatchArgs.ToArray(), mostMatchFullArgs.ToArray()));
                                        if (i > 0)
                                        {
                                            for (int j = i; j > 0; j--)
                                            {
                                                history.RemoveAt(j);
                                                refe.RemoveAt(j);
                                                refe[j - 1]++;
                                                if (history[j - 1].Components.Length > refe[j - 1])
                                                {
                                                    history.Add(history[j - 1].Components[refe[j - 1]]);
                                                    refe.Add(0);
                                                    break;
                                                }
                                                i--;
                                            }
                                            if (i <= 0)
                                            {
                                                break;//no match
                                            }
                                            i -= 1;
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (history[i].Components.Length != 0)
                                    {
                                        history.Add(history[i].Components[refe[i]]);
                                        refe.Add(0);
                                        continue;
                                    }
                                    else
                                    {
                                        if (i > 0)
                                        {
                                            for (int j = i; j > 0; j--)
                                            {
                                                history.RemoveAt(j);
                                                refe.RemoveAt(j);
                                                refe[j - 1]++;
                                                if (history[j - 1].Components.Length > refe[j - 1])
                                                {
                                                    history.Add(history[j - 1].Components[refe[j - 1]]);
                                                    refe.Add(0);
                                                    break;
                                                }
                                                i--;
                                            }
                                            if (i <= 0)
                                            {
                                                break;//no match
                                            }
                                            i -= 1;
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //illegal option
                                if (i > 0)
                                {
                                    history.RemoveAt(i);
                                    refe.RemoveAt(i);
                                    refe[i - 1]++;
                                    i -= 2;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else if (history[i].Type == CommandType.IntValue)
                        {
                            if (int.TryParse(args[i], out int parse))
                            {
                                if (i == args.Length - 1)
                                {
                                    //last lap
                                    mostMatch = history[i].Name;
                                    foreach (CommandComponent commandComponent in history)
                                    {
                                        mostMatchArgs.Add(commandComponent.Name);
                                    }
                                    if (history[i].Components.Length != 0)
                                    {
                                        List<CommandComponent> subHistory = history.ToList();
                                        for (int d = 0; d < history[i].Components.Length; d++)
                                        {
                                            mostMatch = history[i].Name;
                                            int historyLen = history.Count;
                                            List<string> branch = mostMatchArgs.ToList();
                                            branch.Add(history[i].Components[d].Name);
                                            history.Add(history[i].Components[d]);//
                                            List<string[]> branches = [];
                                            List<int> reference = [0];
                                            bool foward = false;
                                            for (int j = historyLen; j > historyLen - 1;)
                                            {
                                                int refPosition = j - historyLen;
                                                if (history[j].Components.Length > reference[refPosition])
                                                {
                                                    branch.Add(history[j].Components[reference[refPosition]].Name);
                                                    history.Add(history[j].Components[reference[refPosition]]);
                                                    j++;
                                                    reference.Add(0);
                                                    foward = true;
                                                }
                                                else
                                                {
                                                    if (j > historyLen)
                                                    {
                                                        if (foward) branches.Add(branch.ToArray());
                                                        foward = false;
                                                        history.RemoveAt(j);
                                                        branch.RemoveAt(j);
                                                        reference.RemoveAt(refPosition);
                                                        j--;
                                                        reference[refPosition - 1]++;
                                                    }
                                                    else
                                                    {
                                                        if (history[j].Handler.ExistHandles() || history[j].Components.Length == 0)
                                                            branches.Add(branch.ToArray());
                                                        break;
                                                    }
                                                }
                                            }
                                            mostMatchFullArgs.AddRange(branches);
                                            history = subHistory.ToList();
                                        }
                                        //record
                                        candidacies.Add(new Candidacies(mostMatch, mostMatchArgs.ToArray(), mostMatchFullArgs.ToArray()));
                                        if (i > 0)
                                        {
                                            for (int j = i; j > 0; j--)
                                            {
                                                history.RemoveAt(j);
                                                refe.RemoveAt(j);
                                                refe[j - 1]++;
                                                if (history[j - 1].Components.Length > refe[j - 1])
                                                {
                                                    history.Add(history[j - 1].Components[refe[j - 1]]);
                                                    refe.Add(0);
                                                    break;
                                                }
                                                i--;
                                            }
                                            if (i <= 0)
                                            {
                                                break;//no match
                                            }
                                            i -= 1;
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        mostMatchFullArgs.Add(mostMatchArgs.ToArray());
                                        //record
                                        candidacies.Add(new Candidacies(mostMatch, mostMatchArgs.ToArray(), mostMatchFullArgs.ToArray()));
                                        if (i > 0)
                                        {
                                            for (int j = i; j > 0; j--)
                                            {
                                                history.RemoveAt(j);
                                                refe.RemoveAt(j);
                                                refe[j - 1]++;
                                                if (history[j - 1].Components.Length > refe[j - 1])
                                                {
                                                    history.Add(history[j - 1].Components[refe[j - 1]]);
                                                    refe.Add(0);
                                                    break;
                                                }
                                                i--;
                                            }
                                            if (i <= 0)
                                            {
                                                break;//no match
                                            }
                                            i -= 1;
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (history[i].Components.Length != 0)
                                    {
                                        history.Add(history[i].Components[refe[i]]);
                                        refe.Add(0);
                                        continue;
                                    }
                                    else
                                    {
                                        if (i > 0)
                                        {
                                            for (int j = i; j > 0; j--)
                                            {
                                                history.RemoveAt(j);
                                                refe.RemoveAt(j);
                                                refe[j - 1]++;
                                                if (history[j - 1].Components.Length > refe[j - 1])
                                                {
                                                    history.Add(history[j - 1].Components[refe[j - 1]]);
                                                    refe.Add(0);
                                                    break;
                                                }
                                                i--;
                                            }
                                            if (i <= 0)
                                            {
                                                break;//no match
                                            }
                                            i -= 1;
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //value is not a number.
                                if (i > 0)
                                {
                                    for (int j = i; j > 0; j--)
                                    {
                                        history.RemoveAt(j);
                                        refe.RemoveAt(j);
                                        refe[j - 1]++;
                                        if (history[j - 1].Components.Length > refe[j - 1])
                                        {
                                            history.Add(history[j - 1].Components[refe[j - 1]]);
                                            refe.Add(0);
                                            break;
                                        }
                                        i--;
                                    }
                                    if (i <= 0)
                                    {
                                        break;//no match
                                    }
                                    i -= 1;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else if (history[i].Type == CommandType.StrValue)
                        {
                            if (i == args.Length - 1)
                            {
                                //last lap
                                mostMatch = history[i].Name;
                                foreach (CommandComponent commandComponent in history)
                                {
                                    mostMatchArgs.Add(commandComponent.Name);
                                }
                                if (history[i].Components.Length != 0)
                                {
                                    List<CommandComponent> subHistory = history.ToList();
                                    for (int d = 0; d < history[i].Components.Length; d++)
                                    {
                                        mostMatch = history[i].Name;
                                        int historyLen = history.Count;
                                        List<string> branch = mostMatchArgs.ToList();
                                        branch.Add(history[i].Components[d].Name);
                                        history.Add(history[i].Components[d]);//
                                        List<string[]> branches = [];
                                        List<int> reference = [0];
                                        bool foward = false;
                                        for (int j = historyLen; j > historyLen - 1;)
                                        {
                                            int refPosition = j - historyLen;
                                            if (history[j].Components.Length > reference[refPosition])
                                            {
                                                branch.Add(history[j].Components[reference[refPosition]].Name);
                                                history.Add(history[j].Components[reference[refPosition]]);
                                                j++;
                                                reference.Add(0);
                                                foward = true;
                                            }
                                            else
                                            {
                                                if (j > historyLen)
                                                {
                                                    if (foward) branches.Add(branch.ToArray());
                                                    foward = false;
                                                    history.RemoveAt(j);
                                                    branch.RemoveAt(j);
                                                    reference.RemoveAt(refPosition);
                                                    j--;
                                                    reference[refPosition - 1]++;
                                                }
                                                else
                                                {
                                                    if (history[j].Handler.ExistHandles() || history[j].Components.Length == 0)
                                                        branches.Add(branch.ToArray());
                                                    break;
                                                }
                                            }
                                        }
                                        mostMatchFullArgs.AddRange(branches);
                                        history = subHistory.ToList();
                                    }
                                    //record
                                    candidacies.Add(new Candidacies(mostMatch, mostMatchArgs.ToArray(), mostMatchFullArgs.ToArray()));
                                    if (i > 0)
                                    {
                                        for (int j = i; j > 0; j--)
                                        {
                                            history.RemoveAt(j);
                                            refe.RemoveAt(j);
                                            refe[j - 1]++;
                                            if (history[j - 1].Components.Length > refe[j - 1])
                                            {
                                                history.Add(history[j - 1].Components[refe[j - 1]]);
                                                refe.Add(0);
                                                break;
                                            }
                                            i--;
                                        }
                                        if (i <= 0)
                                        {
                                            break;//no match
                                        }
                                        i -= 1;
                                        continue;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    mostMatchFullArgs.Add(mostMatchArgs.ToArray());
                                    //record
                                    candidacies.Add(new Candidacies(mostMatch, mostMatchArgs.ToArray(), mostMatchFullArgs.ToArray()));
                                    if (i > 0)
                                    {
                                        for (int j = i; j > 0; j--)
                                        {
                                            history.RemoveAt(j);
                                            refe.RemoveAt(j);
                                            refe[j - 1]++;
                                            if (history[j - 1].Components.Length > refe[j - 1])
                                            {
                                                history.Add(history[j - 1].Components[refe[j - 1]]);
                                                refe.Add(0);
                                                break;
                                            }
                                            i--;
                                        }
                                        if (i <= 0)
                                        {
                                            break;//no match
                                        }
                                        i -= 1;
                                        continue;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (history[i].Components.Length != 0)
                                {
                                    history.Add(history[i].Components[refe[i]]);
                                    refe.Add(0);
                                    continue;
                                }
                                else
                                {
                                    if (i > 0)
                                    {
                                        for (int j = i; j > 0; j--)
                                        {
                                            history.RemoveAt(j);
                                            refe.RemoveAt(j);
                                            refe[j - 1]++;
                                            if (history[j - 1].Components.Length > refe[j - 1])
                                            {
                                                history.Add(history[j - 1].Components[refe[j - 1]]);
                                                refe.Add(0);
                                                break;
                                            }
                                            i--;
                                        }
                                        if (i <= 0)
                                        {
                                            break;//no match
                                        }
                                        i -= 1;
                                        continue;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                return candidacies.ToArray();
            }
            else
            {
                return [];
            }
        }
        public static void DisplayCurrentCommandComponents(DisplayOption option)
        {
            string outline = "";
            if (option == DisplayOption.Ladder)
            {
                List<List<string>> levelNames = [new List<string>()];
                string str = "─ ";
                string trs = "┬ ";
                string tri = "├ ";
                string lef = "└ ";
                string vrt = "│ ";
                foreach (CommandComponent component in components)
                {
                    int level = 0;
                    List<int> taskc = new List<int> { 0 };
                    CommandComponent com = component;
                    List<CommandComponent> target = new List<CommandComponent> { com };
                    levelNames[0].Add(component.Name);
                    List<string> figure = [""];
                    figure[0] += component.Name;
                    List<int> count = [component.Name.Length];
                    int downcCount = 0;
                    //CommandComponent com = new CommandComponent(component.Name, getNewId, component.Type);
                    //List<CommandComponent> target = new List<CommandComponent> { com };
                    while (true)
                    {
                        if (target[level].Components.Length > taskc[level])
                        {
                            //List<CommandComponent> lis = target[level].Components.ToList();
                            //CommandComponent c = target[level].Components[taskc[level]];
                            //lis.Add(new CommandComponent(c.Name, getNewId, c.Type));
                            //target[level].Components = lis.ToArray();
                            if (taskc[level] == 0)
                            {
                                int c = 0;
                                if (target[level].Components.Length == 1)
                                {
                                    figure[downcCount] += str + target[level].Components[taskc[level]].Name;
                                    c = (str + target[level].Components[taskc[level]].Name).Length;
                                }
                                else
                                {
                                    figure[downcCount] += trs + target[level].Components[taskc[level]].Name;
                                    c = (trs + target[level].Components[taskc[level]].Name).Length;
                                }
                                count.Add(c);
                            }
                            else
                            {
                                downcCount++;
                                string space = "";
                                for (int i = 0; i <= level; i++)
                                {
                                    for (int j = 0; j < count[i]; j++)
                                    {
                                        space += " ";
                                    }
                                }
                                figure.Add(space);
                                figure[downcCount] += lef + target[level].Components[taskc[level]].Name;
                                count.Add(space.Length + figure[taskc[level]].Length);
                                for (int i = 1; i < downcCount; i++)
                                {
                                    if (figure[i].Substring(space.Length, 2) == lef)
                                        figure[i] = figure[i].Remove(space.Length, 2).Insert(space.Length, tri);
                                    else figure[i] = figure[i].Remove(space.Length, 2).Insert(space.Length, vrt);
                                }
                            }
                            taskc.Add(0);
                            target.Add(target[level].Components[taskc[level]]);
                            //target.Add(target[level].Components[taskc[level]]);
                            level++;
                        }
                        else
                        {
                            if (level == 0) break;
                            else
                            {
                                taskc.RemoveAt(level);
                                //target.RemoveAt(level);
                                target.RemoveAt(level);
                                count.RemoveAt(level);
                                level--;
                                taskc[level]++;
                            }
                        }
                    }
                    foreach (string s in figure)
                    {
                        outline += s + "\n";
                    }
                }
                if (outline.Length != 0)
                    if (outline.Substring(outline.Length - 1, 1) == "\n")
                        outline = outline.Substring(0, outline.Length - 1);
            }
            else if (option == DisplayOption.Column)
            {
                List<List<string>> levelNames = [new List<string>()];
                string spa = " ";
                foreach (CommandComponent component in components)
                {
                    int level = 0;
                    List<int> taskc = new List<int> { 0 };
                    CommandComponent com = component;
                    List<CommandComponent> target = new List<CommandComponent> { com };
                    levelNames[0].Add(component.Name);
                    List<string> figure = [""];
                    figure[0] += component.Name;
                    List<int> count = [component.Name.Length];
                    int downcCount = 0;
                    //CommandComponent com = new CommandComponent(component.Name, getNewId, component.Type);
                    //List<CommandComponent> target = new List<CommandComponent> { com };
                    while (true)
                    {
                        if (target[level].Components.Length > taskc[level])
                        {
                            //List<CommandComponent> lis = target[level].Components.ToList();
                            //CommandComponent c = target[level].Components[taskc[level]];
                            //lis.Add(new CommandComponent(c.Name, getNewId, c.Type));
                            //target[level].Components = lis.ToArray();
                            if (taskc[level] == 0)
                            {
                                int c = 0;
                                if (target[level].Components.Length == 1)
                                {
                                    figure[downcCount] += spa + target[level].Components[taskc[level]].Name;
                                    c = target[level].Components[taskc[level]].Name.Length + spa.Length;
                                }
                                else
                                {
                                    figure[downcCount] += spa + target[level].Components[taskc[level]].Name;
                                    c = target[level].Components[taskc[level]].Name.Length + spa.Length;
                                }
                                count.Add(c);
                            }
                            else
                            {
                                downcCount++;
                                int leftCount = 0;
                                for (int i = 0; i <= level; i++)
                                {
                                    leftCount += count[i];
                                }
                                string space = figure[downcCount - 1].Substring(0, leftCount);
                                figure.Add(space);
                                figure[downcCount] += spa + target[level].Components[taskc[level]].Name;
                                count.Add(space.Length + figure[taskc[level]].Length);
                                for (int i = 1; i < downcCount; i++)
                                {
                                    if (figure[i].Substring(space.Length, spa.Length) == spa)
                                        figure[i] = figure[i].Remove(space.Length, spa.Length).Insert(space.Length, spa);
                                    else figure[i] = figure[i].Remove(space.Length, spa.Length).Insert(space.Length, spa);
                                }
                            }
                            taskc.Add(0);
                            target.Add(target[level].Components[taskc[level]]);
                            //target.Add(target[level].Components[taskc[level]]);
                            level++;
                        }
                        else
                        {
                            if (level == 0) break;
                            else
                            {
                                taskc.RemoveAt(level);
                                //target.RemoveAt(level);
                                target.RemoveAt(level);
                                count.RemoveAt(level);
                                level--;
                                taskc[level]++;
                            }
                        }
                    }
                    foreach (string s in figure)
                    {
                        outline += s + "\n";
                    }
                }
                if (outline.Length != 0)
                    if (outline.Substring(outline.Length - 1, 1) == "\n")
                        outline = outline.Substring(0, outline.Length - 1);
            }
            log.Info(outline);
        }
        public static int ArroundCallCommandComponent(string[] args)
        {
            return MainSender.Send(new CopyData(null, null, SystemId.Command, null, null, null, null, null, null, args));
        }
        public static int ArroundCallCommandComponent(string[] args,CallOption option)
        {
            return MainSender.Send(new CopyData(null, null, SystemId.Command, (int)option, null, null, null, null, null, args));
        }
        public static (int code, CommandResponse? response) GlobalCallCommandComponent(string[] args)
        {
            foreach (CommandComponent component in components)
            {
                if (args[0] != component.Name) continue;
                int level = 0;
                List<int> taskc = new List<int> { 0 };
                CommandComponent icom = component;
                List<CommandComponent> itarget = new List<CommandComponent> { icom };
                IReadOnlyCommandComponent com = new IReadOnlyCommandComponent(component.Name, component.Type, args[0] == null ? null : args[0]);
                List<IReadOnlyCommandComponent> target = [com];
                while (true)
                {
                    if (itarget[level].Components.Length > taskc[level])
                    {
                        //List<CommandComponent> lis = target[level].Components.ToList();
                        //ICommandComponent c = itarget[level].Components[taskc[level]];
                        //CommandComponent cc = new CommandComponent(c.Name, c.Expression, getNewId, c.Type);
                        //cc.Handler.SwapEventHandler(c.Handler.getEventHandler());
                        //lis.Add(cc);
                        //target[level].Components = lis.ToArray();
                        if (args.Length < level + 1)
                        {
                            if (args[level + 1] == itarget[level].Components[taskc[level]].Name)
                            {
                                if (args.Length == level + 2)
                                {
                                    if (itarget[level].Components[taskc[level]].Components.Length == 0)
                                    {
                                        return (MainSender.Send(new CopyData(null, null, SystemId.Command, null, null, null, null, null, null, args)),
                                        itarget[level].Components[taskc[level]].Handler.Call(com));
                                    }
                                }
                                taskc.Add(0);
                                itarget.Add(itarget[level].Components[taskc[level]]);
                                List<IReadOnlyCommandComponent> c = target[level].Components.ToList();
                                c.Add(new IReadOnlyCommandComponent(itarget[level + 1].Name, itarget[level + 1].Type, itarget[level + 1].Value));
                                target.Add(target[level].Components[taskc[level]]);
                                level++;
                            }
                        }
                    }
                    else
                    {
                        if (level == 0) break;
                        else
                        {
                            taskc.RemoveAt(level);
                            target.RemoveAt(level);
                            itarget.RemoveAt(level);
                            level--;
                            taskc[level]++;
                        }
                    }
                }
            }
            return (MainSender.Send(new CopyData(null, null, SystemId.Command, null, null, null, null, null, null, args)), null);
        }
        public static (int code,CommandResponse? response) GlobalCallCommandComponent(string[] args, CallOption option)
        {
            foreach (CommandComponent component in components)
            {
                if (args[0] != component.Name) continue;
                int level = 0;
                List<int> taskc = [0];
                CommandComponent icom = component;
                List<CommandComponent> itarget = [icom];
                IReadOnlyCommandComponent com = new IReadOnlyCommandComponent(component.Name, component.Type, args[0] == null ? null : args[0]);
                List<IReadOnlyCommandComponent> target = [com];
                while (true)
                {
                    if (itarget[level].Components.Length > taskc[level])
                    {
                        //List<CommandComponent> lis = target[level].Components.ToList();
                        //ICommandComponent c = itarget[level].Components[taskc[level]];
                        //CommandComponent cc = new CommandComponent(c.Name, c.Expression, getNewId, c.Type);
                        //cc.Handler.SwapEventHandler(c.Handler.getEventHandler());
                        //lis.Add(cc);
                        //target[level].Components = lis.ToArray();
                        if (args.Length < level + 1)
                        {
                            if (args[level + 1] == itarget[level].Components[taskc[level]].Name)
                            {
                                if (args.Length == level + 2)
                                {
                                    if (option == CallOption.Perfect && itarget[level].Components[taskc[level]].Components.Length == 0)
                                    {
                                        return (MainSender.Send(new CopyData(null,null,SystemId.Command,null,null,null,null,null,null,args)),
                                        itarget[level].Components[taskc[level]].Handler.Call(com));
                                    }
                                    else if (option == CallOption.PerfectStep)
                                    {
                                        return (MainSender.Send(new CopyData(null,null,SystemId.Command,null,null,null,null,null,null,args)),
                                        itarget[level].Components[taskc[level]].Handler.Call(com));
                                    }
                                }
                                taskc.Add(0);
                                itarget.Add(itarget[level].Components[taskc[level]]);
                                List<IReadOnlyCommandComponent> c = target[level].Components.ToList();
                                c.Add(new IReadOnlyCommandComponent(itarget[level + 1].Name, itarget[level + 1].Type, itarget[level + 1].Value));
                                target.Add(target[level].Components[taskc[level]]);
                                level++;
                            }
                            else if (args[level + 1].Length < itarget[level].Components[taskc[level]].Name.Length
                                        && args[level + 1] == itarget[level].Components[taskc[level]].Name.Substring(0, args[level + 1].Length))
                            {
                                if (args.Length == level + 2)
                                {
                                    if (option == CallOption.MatchStep && option == CallOption.MatchTop)
                                    {
                                        return (MainSender.Send(new CopyData(null,null,SystemId.Command,null,null,null,null,null,null,args)),
                                        itarget[level].Components[taskc[level]].Handler.Call(com));
                                    }
                                    else
                                    {
                                        return (MainSender.Send(new CopyData(null,null,SystemId.Command,null,null,null,null,null,null,args)),null);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (level == 0) break;
                        else
                        {
                            taskc.RemoveAt(level);
                            target.RemoveAt(level);
                            itarget.RemoveAt(level);
                            level--;
                            taskc[level]++;
                        }
                    }
                }
            }
            return (MainSender.Send(new CopyData(null,null,SystemId.Command,null,null,null,null,null,null,args)),null);
        }
        public static CommandResponse? LocalCallCommandComponent(string[] args)
        {
            foreach (CommandComponent component in components)
            {
                if (args[0] != component.Name) continue;
                int level = 0;
                List<int> taskc = new List<int> { 0 };
                CommandComponent icom = component;
                List<CommandComponent> itarget = new List<CommandComponent> { icom };
                IReadOnlyCommandComponent com = new IReadOnlyCommandComponent(component.Name, component.Type, args[0] == null ? null : args[0]);
                List<IReadOnlyCommandComponent> target = [com];
                while (true)
                {
                    if (itarget[level].Components.Length > taskc[level])
                    {
                        //List<CommandComponent> lis = target[level].Components.ToList();
                        //ICommandComponent c = itarget[level].Components[taskc[level]];
                        //CommandComponent cc = new CommandComponent(c.Name, c.Expression, getNewId, c.Type);
                        //cc.Handler.SwapEventHandler(c.Handler.getEventHandler());
                        //lis.Add(cc);
                        //target[level].Components = lis.ToArray();
                        if (args.Length < level + 1)
                        {
                            if (args[level + 1] == itarget[level].Components[taskc[level]].Name)
                            {
                                if (args.Length == level + 2)
                                {
                                    if (itarget[level].Components[taskc[level]].Components.Length == 0)
                                    {
                                        return itarget[level].Components[taskc[level]].Handler.Call(com);
                                    }
                                }
                                taskc.Add(0);
                                itarget.Add(itarget[level].Components[taskc[level]]);
                                List<IReadOnlyCommandComponent> c = target[level].Components.ToList();
                                c.Add(new IReadOnlyCommandComponent(itarget[level + 1].Name, itarget[level + 1].Type, itarget[level + 1].Value));
                                target.Add(target[level].Components[taskc[level]]);
                                level++;
                            }
                        }
                    }
                    else
                    {
                        if (level == 0) break;
                        else
                        {
                            taskc.RemoveAt(level);
                            target.RemoveAt(level);
                            itarget.RemoveAt(level);
                            level--;
                            taskc[level]++;
                        }
                    }
                }
            }
            return null;
        }
        public static CommandResponse? LocalCallCommandComponent(string[] args, CallOption option)
        {
            foreach (CommandComponent component in components)
            {
                if (args[0] != component.Name) continue;
                int level = 0;
                List<int> taskc = [0];
                CommandComponent icom = component;
                List<CommandComponent> itarget = [icom];
                IReadOnlyCommandComponent com = new IReadOnlyCommandComponent(component.Name, component.Type, args[0] == null ? null : args[0]);
                List<IReadOnlyCommandComponent> target = [com];
                while (true)
                {
                    if (itarget[level].Components.Length > taskc[level])
                    {
                        //List<CommandComponent> lis = target[level].Components.ToList();
                        //ICommandComponent c = itarget[level].Components[taskc[level]];
                        //CommandComponent cc = new CommandComponent(c.Name, c.Expression, getNewId, c.Type);
                        //cc.Handler.SwapEventHandler(c.Handler.getEventHandler());
                        //lis.Add(cc);
                        //target[level].Components = lis.ToArray();
                        if (args.Length < level + 1)
                        {
                            if (args[level + 1] == itarget[level].Components[taskc[level]].Name)
                            {
                                if (args.Length == level + 2)
                                {
                                    if (option == CallOption.Perfect && itarget[level].Components[taskc[level]].Components.Length == 0)
                                    {
                                        return itarget[level].Components[taskc[level]].Handler.Call(com);
                                    }
                                    else if (option == CallOption.PerfectStep)
                                    {
                                        return itarget[level].Components[taskc[level]].Handler.Call(com);
                                    }
                                }
                                taskc.Add(0);
                                itarget.Add(itarget[level].Components[taskc[level]]);
                                List<IReadOnlyCommandComponent> c = target[level].Components.ToList();
                                c.Add(new IReadOnlyCommandComponent(itarget[level + 1].Name, itarget[level + 1].Type, itarget[level + 1].Value));
                                target.Add(target[level].Components[taskc[level]]);
                                level++;
                            }
                            else if (args[level + 1].Length < itarget[level].Components[taskc[level]].Name.Length
                                        && args[level + 1] == itarget[level].Components[taskc[level]].Name.Substring(0, args[level + 1].Length))
                            {
                                if (args.Length == level + 2)
                                {
                                    if (option == CallOption.MatchStep && option == CallOption.MatchTop)
                                    {
                                        return itarget[level].Components[taskc[level]].Handler.Call(com);
                                    }
                                    else
                                    {
                                        return null;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (level == 0) break;
                        else
                        {
                            taskc.RemoveAt(level);
                            target.RemoveAt(level);
                            itarget.RemoveAt(level);
                            level--;
                            taskc[level]++;
                        }
                    }
                }
            }
            return null;
        }
    }
}