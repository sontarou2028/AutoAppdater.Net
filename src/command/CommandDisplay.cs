using AutoAppdater.Common;

namespace AutoAppdater.Command
{
    /// <summary>
    /// Provides functionality to display registered command components.
    /// </summary>
    public static class CommandDisplay
    {
        private static Log.Log log = Common.Common.DefaultLogHost;

        /// <summary>
        /// Displays all currently registered command components in the specified format.
        /// </summary>
        /// <param name="option">The display format (Ladder or Column).</param>
        public static void DisplayCurrentCommandComponents(DisplayOption option)
        {
            var components = CommandRegistry.GetAllComponents();
            string outline = "";

            if (option == DisplayOption.Ladder)
            {
                outline = RenderLadderFormat(components);
            }
            else if (option == DisplayOption.Column)
            {
                outline = RenderColumnFormat(components);
            }

            log.Info(outline);
        }

        private static string RenderLadderFormat(IReadOnlyList<CommandComponent> components)
        {
            List<List<string>> levelNames = [new List<string>()];
            string str = "─ ";
            string trs = "┬ ";
            string tri = "├ ";
            string lef = "└ ";
            string vrt = "│ ";
            string outline = "";

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

                while (true)
                {
                    if (target[level].Components.Length > taskc[level])
                    {
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
                        level++;
                    }
                    else
                    {
                        if (level == 0) break;
                        else
                        {
                            taskc.RemoveAt(level);
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

            return outline;
        }

        private static string RenderColumnFormat(IReadOnlyList<CommandComponent> components)
        {
            List<List<string>> levelNames = [new List<string>()];
            string spa = " ";
            string outline = "";

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

                while (true)
                {
                    if (target[level].Components.Length > taskc[level])
                    {
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
                        level++;
                    }
                    else
                    {
                        if (level == 0) break;
                        else
                        {
                            taskc.RemoveAt(level);
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

            return outline;
        }
    }
}
