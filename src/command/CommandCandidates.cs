namespace AutoAppdater.Command
{
    /// <summary>
    /// Provides functionality to generate command candidates for auto-completion.
    /// </summary>
    public static class CommandCandidates
    {
        public static Candidacies[] GetCandidacies(string[] args)
        {
            string[] True = { "true", "True" };
            string[] False = { "false", "False" };
            if (CommandRegistry.GetAllComponents().Count != 0 && args.Length != 0)
            {
                List<Candidacies> candidacies = [];
                for (int c = 0; c < CommandRegistry.GetAllComponents().Count; c++)
                {
                    List<CommandComponent> history = [CommandRegistry.GetAllComponents()[c]];
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
    }
}
