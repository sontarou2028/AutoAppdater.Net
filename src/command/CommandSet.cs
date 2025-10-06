namespace AutoAppdater.Command
{
    /// <summary>
    /// Legacy wrapper class for backward compatibility.
    /// All methods delegate to the new modular command system.
    /// </summary>
    [Obsolete("Use specific command classes (CommandRegistry, CommandExecutor, CommandCandidates, CommandDisplay) instead. This class will be removed in a future version.")]
    public static class CommandSet
    {
        // ===== Registration Methods =====

        /// <summary>
        /// Registers a command component. Delegates to CommandRegistry.
        /// </summary>
        public static void RegistCommandComponent(ICommandComponent component)
            => CommandRegistry.RegistCommandComponent(component);

        /// <summary>
        /// Registers multiple command components. Delegates to CommandRegistry.
        /// </summary>
        public static void RegistCommandComponent(ICommandComponent[] components)
            => CommandRegistry.RegistCommandComponent(components);

        // ===== Execution Methods =====

        /// <summary>
        /// Calls a command remotely via the main process. Delegates to CommandExecutor.
        /// </summary>
        public static int ArroundCallCommandComponent(string[] args)
            => CommandExecutor.ArroundCallCommandComponent(args);

        /// <summary>
        /// Calls a command remotely with options. Delegates to CommandExecutor.
        /// </summary>
        public static int ArroundCallCommandComponent(string[] args, CallOption option)
            => CommandExecutor.ArroundCallCommandComponent(args, option);

        /// <summary>
        /// Calls a command globally (local + remote). Delegates to CommandExecutor.
        /// </summary>
        public static (int code, CommandResponse? response) GlobalCallCommandComponent(string[] args)
            => CommandExecutor.GlobalCallCommandComponent(args);

        /// <summary>
        /// Calls a command globally with options. Delegates to CommandExecutor.
        /// </summary>
        public static (int code, CommandResponse? response) GlobalCallCommandComponent(string[] args, CallOption option)
            => CommandExecutor.GlobalCallCommandComponent(args, option);

        /// <summary>
        /// Calls a command locally only. Delegates to CommandExecutor.
        /// </summary>
        public static CommandResponse? LocalCallCommandComponent(string[] args)
            => CommandExecutor.LocalCallCommandComponent(args);

        /// <summary>
        /// Calls a command locally with options. Delegates to CommandExecutor.
        /// </summary>
        public static CommandResponse? LocalCallCommandComponent(string[] args, CallOption option)
            => CommandExecutor.LocalCallCommandComponent(args, option);

        // ===== Candidate Methods =====

        /// <summary>
        /// Gets command candidates for auto-completion. Delegates to CommandCandidates.
        /// </summary>
        public static Candidacies[] GetCandidacies(string[] args)
            => CommandCandidates.GetCandidacies(args);

        // ===== Display Methods =====

        /// <summary>
        /// Displays all registered command components. Delegates to CommandDisplay.
        /// </summary>
        public static void DisplayCurrentCommandComponents(DisplayOption option)
            => CommandDisplay.DisplayCurrentCommandComponents(option);
    }
}
