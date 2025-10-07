namespace AutoAppdater.Command
{
    /// <summary>
    /// Manages registration and storage of command components.
    /// </summary>
    public static class CommandRegistry
    {
        private static List<CommandComponent> components = new List<CommandComponent>();
        private static int idStatus = 0;
        private static int getNewId { get { int ret = idStatus; idStatus++; return ret; } }

        /// <summary>
        /// Gets all registered command components as a read-only collection.
        /// </summary>
        /// <returns>Read-only list of all registered command components.</returns>
        internal static IReadOnlyList<CommandComponent> GetAllComponents()
        {
            return components.AsReadOnly();
        }

        /// <summary>
        /// Registers a new command component to the command system.
        /// </summary>
        /// <param name="component">The command component to register.</param>
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

        /// <summary>
        /// Registers multiple command components at once.
        /// </summary>
        /// <param name="components">Array of command components to register.</param>
        public static void RegistCommandComponent(ICommandComponent[] components)
        {
            foreach (ICommandComponent component in components)
            {
                RegistCommandComponent(component);
            }
        }
    }
}
