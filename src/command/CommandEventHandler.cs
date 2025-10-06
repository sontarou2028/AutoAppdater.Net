namespace AutoAppdater.Command
{
    /// <summary>
    /// Manages event handlers for command execution.
    /// </summary>
    public class EventHandler
    {
        /// <summary>
        /// Delegate for command execution without parameters.
        /// </summary>
        /// <returns>Command response or null.</returns>
        public delegate CommandResponse? baseDelegate0();

        /// <summary>
        /// Event handler for parameterless command execution.
        /// </summary>
        public event baseDelegate0? CallStepEventHandler0;

        /// <summary>
        /// Delegate for command execution with value array parameters.
        /// </summary>
        /// <param name="value">Array of string values from the command.</param>
        /// <returns>Command response or null.</returns>
        public delegate CommandResponse? baseDelegate1(string?[] value);

        /// <summary>
        /// Event handler for command execution with value array.
        /// </summary>
        public event baseDelegate1? CallStepEventHandler1;

        /// <summary>
        /// Delegate for command execution with component structure.
        /// </summary>
        /// <param name="components">The command component structure.</param>
        /// <returns>Command response or null.</returns>
        public delegate CommandResponse? baseDelegate2(IReadOnlyCommandComponent components);

        /// <summary>
        /// Event handler for command execution with component structure.
        /// </summary>
        public event baseDelegate2? CallStepEventHandler2;

        /// <summary>
        /// Delegate for command execution with string arguments.
        /// </summary>
        /// <param name="args">Array of string arguments.</param>
        /// <returns>Command response or null.</returns>
        public delegate CommandResponse? baseDelegate3(string[] args);

        /// <summary>
        /// Event handler for command execution with string arguments.
        /// </summary>
        public event baseDelegate2? CallStepEventHandler3;

        /// <summary>
        /// Gets all registered event handlers as a tuple.
        /// </summary>
        /// <returns>Tuple containing all four event handler delegates.</returns>
        public (baseDelegate0? del0, baseDelegate1? del1, baseDelegate2? del2, baseDelegate2? del3) getEventHandler()
        {
            return (CallStepEventHandler0, CallStepEventHandler1, CallStepEventHandler2, CallStepEventHandler3);
        }

        /// <summary>
        /// Replaces all event handlers with new ones.
        /// </summary>
        /// <param name="callStepEventHandler">Tuple containing new event handler delegates.</param>
        public void SwapEventHandler((baseDelegate0? del0, baseDelegate1? del1, baseDelegate2? del2, baseDelegate2? del3) callStepEventHandler)
        {
            CallStepEventHandler0 = callStepEventHandler.del0;
            CallStepEventHandler1 = callStepEventHandler.del1;
            CallStepEventHandler2 = callStepEventHandler.del2;
            CallStepEventHandler3 = callStepEventHandler.del3;
        }

        /// <summary>
        /// Checks if any event handlers are registered.
        /// </summary>
        /// <returns>True if at least one event handler is registered.</returns>
        internal bool ExistHandles()
        {
            if (CallStepEventHandler0 != null || CallStepEventHandler1 != null ||
            CallStepEventHandler2 != null || CallStepEventHandler3 != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Invokes all registered event handlers and returns the last non-null response.
        /// </summary>
        /// <param name="component">The command component to pass to handlers.</param>
        /// <returns>The last non-null command response, or null if all handlers returned null.</returns>
        internal CommandResponse? Call(IReadOnlyCommandComponent component)
        {
            CommandResponse? res = null;

            // Call handler 0: No parameters
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
            }

            // Call handler 1: Value array
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
            }

            // Call handler 2: Component structure
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
            }

            // Call handler 3: String array
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
            }

            return res;
        }
    }
}
