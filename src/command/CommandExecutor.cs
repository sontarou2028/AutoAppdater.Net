using AutoAppdater.MainSenderHost;

namespace AutoAppdater.Command
{
    /// <summary>
    /// Provides functionality to execute registered commands.
    /// </summary>
    public static class CommandExecutor
    {
        /// <summary>
        /// Sends command arguments to the main AutoAppdater process for execution.
        /// </summary>
        /// <param name="args">Command arguments.</param>
        /// <returns>Error code from the send operation.</returns>
        public static int ArroundCallCommandComponent(string[] args)
        {
            return MainSender.Send(new CopyData(null, null, SystemId.Command, null, null, null, null, null, null, args));
        }

        /// <summary>
        /// Sends command arguments to the main AutoAppdater process with specific call option.
        /// </summary>
        /// <param name="args">Command arguments.</param>
        /// <param name="option">Call matching option.</param>
        /// <returns>Error code from the send operation.</returns>
        public static int ArroundCallCommandComponent(string[] args, CallOption option)
        {
            return MainSender.Send(new CopyData(null, null, SystemId.Command, (int)option, null, null, null, null, null, args));
        }

        /// <summary>
        /// Executes a command both locally and sends it to the main process.
        /// </summary>
        /// <param name="args">Command arguments.</param>
        /// <returns>Tuple containing the send error code and command response.</returns>
        public static (int code, CommandResponse? response) GlobalCallCommandComponent(string[] args)
        {
            var components = CommandRegistry.GetAllComponents();

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

        /// <summary>
        /// Executes a command both locally and sends it to the main process with specific call option.
        /// </summary>
        /// <param name="args">Command arguments.</param>
        /// <param name="option">Call matching option.</param>
        /// <returns>Tuple containing the send error code and command response.</returns>
        public static (int code, CommandResponse? response) GlobalCallCommandComponent(string[] args, CallOption option)
        {
            var components = CommandRegistry.GetAllComponents();

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
                        if (args.Length < level + 1)
                        {
                            if (args[level + 1] == itarget[level].Components[taskc[level]].Name)
                            {
                                if (args.Length == level + 2)
                                {
                                    if (option == CallOption.Perfect && itarget[level].Components[taskc[level]].Components.Length == 0)
                                    {
                                        return (MainSender.Send(new CopyData(null, null, SystemId.Command, null, null, null, null, null, null, args)),
                                        itarget[level].Components[taskc[level]].Handler.Call(com));
                                    }
                                    else if (option == CallOption.PerfectStep)
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
                            else if (args[level + 1].Length < itarget[level].Components[taskc[level]].Name.Length
                                        && args[level + 1] == itarget[level].Components[taskc[level]].Name.Substring(0, args[level + 1].Length))
                            {
                                if (args.Length == level + 2)
                                {
                                    if (option == CallOption.MatchStep && option == CallOption.MatchTop)
                                    {
                                        return (MainSender.Send(new CopyData(null, null, SystemId.Command, null, null, null, null, null, null, args)),
                                        itarget[level].Components[taskc[level]].Handler.Call(com));
                                    }
                                    else
                                    {
                                        return (MainSender.Send(new CopyData(null, null, SystemId.Command, null, null, null, null, null, null, args)), null);
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
            return (MainSender.Send(new CopyData(null, null, SystemId.Command, null, null, null, null, null, null, args)), null);
        }

        /// <summary>
        /// Executes a command locally without sending to the main process.
        /// </summary>
        /// <param name="args">Command arguments.</param>
        /// <returns>Command response or null if no matching command found.</returns>
        public static CommandResponse? LocalCallCommandComponent(string[] args)
        {
            var components = CommandRegistry.GetAllComponents();

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

        /// <summary>
        /// Executes a command locally with specific call option.
        /// </summary>
        /// <param name="args">Command arguments.</param>
        /// <param name="option">Call matching option.</param>
        /// <returns>Command response or null if no matching command found.</returns>
        public static CommandResponse? LocalCallCommandComponent(string[] args, CallOption option)
        {
            var components = CommandRegistry.GetAllComponents();

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
