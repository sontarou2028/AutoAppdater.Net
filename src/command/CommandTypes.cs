namespace AutoAppdater.Command
{
    /// <summary>
    /// Defines the type of command component.
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        /// A keyword or command word.
        /// </summary>
        Word,

        /// <summary>
        /// An integer value parameter.
        /// </summary>
        IntValue,

        /// <summary>
        /// A boolean value parameter.
        /// </summary>
        BoolValue,

        /// <summary>
        /// A string value parameter.
        /// </summary>
        StrValue,

        /// <summary>
        /// An option parameter with predefined candidates.
        /// </summary>
        Option,
    }

    /// <summary>
    /// Defines how commands should be matched when called.
    /// </summary>
    public enum CallOption : byte
    {
        /// <summary>
        /// Match each step of the command path.
        /// </summary>
        MatchStep,

        /// <summary>
        /// Match only the top-level command.
        /// </summary>
        MatchTop,

        /// <summary>
        /// Perfect match required for each step.
        /// </summary>
        PerfectStep,

        /// <summary>
        /// Perfect match required for the entire command.
        /// </summary>
        Perfect,
    }

    /// <summary>
    /// Defines the display format for command components.
    /// </summary>
    public enum DisplayOption
    {
        /// <summary>
        /// Display commands in a tree-like ladder format.
        /// </summary>
        Ladder,

        /// <summary>
        /// Display commands in a column format.
        /// </summary>
        Column,
    }

    /// <summary>
    /// Represents command candidates for auto-completion.
    /// </summary>
    public class Candidacies
    {
        /// <summary>
        /// Gets the most matching command string.
        /// </summary>
        public string mostMatch { get; }

        /// <summary>
        /// Gets the most matching command arguments.
        /// </summary>
        public string[] mostMatchArgs { get; }

        /// <summary>
        /// Gets all possible full command argument combinations.
        /// </summary>
        public string[][] mostMatchFullArgs { get; }

        internal Candidacies(string mostMatch, string[] mostMatchArgs, string[][] mostMatchFullArgs)
        {
            this.mostMatch = mostMatch;
            this.mostMatchArgs = mostMatchArgs;
            this.mostMatchFullArgs = mostMatchFullArgs;
        }

        /// <summary>
        /// Creates a copy of this Candidacies instance.
        /// </summary>
        /// <returns>A new Candidacies instance with the same values.</returns>
        public Candidacies ToCandidacies()
        {
            return new Candidacies(mostMatch, mostMatchArgs, mostMatchFullArgs);
        }
    }

    /// <summary>
    /// Represents a response from command execution.
    /// </summary>
    public class CommandResponse
    {
        // Reserved for future implementation
    }
}
