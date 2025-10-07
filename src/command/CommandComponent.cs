namespace AutoAppdater.Command
{
    /// <summary>
    /// Use this to setup command component.
    /// </summary>
    public class ICommandComponent
    {
        /// <summary>
        /// Gets the name of this command component.
        /// </summary>
        public string Name { get { return name; } }

        /// <summary>
        /// Gets the regular expression pattern for validation (optional).
        /// </summary>
        public string? Expression { get { return expression; } }

        /// <summary>
        /// Gets the candidate values for this component (optional).
        /// </summary>
        public string[]? Candidacies { get { return candidacies; } }

        /// <summary>
        /// Gets the type of this command component.
        /// </summary>
        public CommandType Type { get { return type; } }

        /// <summary>
        /// Gets the child components of this command.
        /// </summary>
        public ICommandComponent[] Components { get { return components; } }

        /// <summary>
        /// Gets the event handler for this command component.
        /// </summary>
        public EventHandler Handler { get { return handler; } }

        string name;
        string? expression;
        string[]? candidacies;
        CommandType type;
        ICommandComponent[] components;
        EventHandler handler = new EventHandler();

        /// <summary>
        /// Initializes a new instance of the ICommandComponent class.
        /// </summary>
        /// <param name="name">The name of the command component.</param>
        /// <param name="type">The type of the command component.</param>
        public ICommandComponent(string name, CommandType type)
        {
            this.name = name;
            expression = null;
            this.type = type;
            components = [];
        }

        /// <summary>
        /// Initializes a new instance of the ICommandComponent class with child components.
        /// </summary>
        /// <param name="name">The name of the command component.</param>
        /// <param name="type">The type of the command component.</param>
        /// <param name="components">The child components.</param>
        public ICommandComponent(string name, CommandType type, ICommandComponent[] components)
        {
            this.name = name;
            expression = null;
            this.type = type;
            this.components = components;
        }

        /// <summary>
        /// Initializes a new instance of the ICommandComponent class with an expression.
        /// </summary>
        /// <param name="name">The name of the command component.</param>
        /// <param name="expression">The regular expression for validation.</param>
        /// <param name="type">The type of the command component.</param>
        public ICommandComponent(string name, string expression, CommandType type)
        {
            this.name = name;
            this.expression = expression;
            this.type = type;
            components = [];
        }

        /// <summary>
        /// Initializes a new instance of the ICommandComponent class with an expression and child components.
        /// </summary>
        /// <param name="name">The name of the command component.</param>
        /// <param name="expression">The regular expression for validation.</param>
        /// <param name="type">The type of the command component.</param>
        /// <param name="components">The child components.</param>
        public ICommandComponent(string name, string expression, CommandType type, ICommandComponent[] components)
        {
            this.name = name;
            this.expression = expression;
            this.type = type;
            this.components = components;
        }

        /// <summary>
        /// Initializes a new instance of the ICommandComponent class with candidate values.
        /// </summary>
        /// <param name="name">The name of the command component.</param>
        /// <param name="candidacies">The candidate values.</param>
        /// <param name="type">The type of the command component.</param>
        public ICommandComponent(string name, string[]? candidacies, CommandType type)
        {
            this.name = name;
            expression = null;
            this.candidacies = candidacies;
            this.type = type;
            components = [];
        }

        /// <summary>
        /// Initializes a new instance of the ICommandComponent class with candidate values and child components.
        /// </summary>
        /// <param name="name">The name of the command component.</param>
        /// <param name="candidacies">The candidate values.</param>
        /// <param name="type">The type of the command component.</param>
        /// <param name="components">The child components.</param>
        public ICommandComponent(string name, string[]? candidacies, CommandType type, ICommandComponent[] components)
        {
            this.name = name;
            expression = null;
            this.candidacies = candidacies;
            this.type = type;
            this.components = components;
        }

        /// <summary>
        /// Initializes a new instance of the ICommandComponent class with an expression and candidate values.
        /// </summary>
        /// <param name="name">The name of the command component.</param>
        /// <param name="expression">The regular expression for validation.</param>
        /// <param name="candidacies">The candidate values.</param>
        /// <param name="type">The type of the command component.</param>
        public ICommandComponent(string name, string expression, string[]? candidacies, CommandType type)
        {
            this.name = name;
            this.expression = expression;
            this.candidacies = candidacies;
            this.type = type;
            components = [];
        }

        /// <summary>
        /// Initializes a new instance of the ICommandComponent class with an expression, candidate values, and child components.
        /// </summary>
        /// <param name="name">The name of the command component.</param>
        /// <param name="expression">The regular expression for validation.</param>
        /// <param name="candidacies">The candidate values.</param>
        /// <param name="type">The type of the command component.</param>
        /// <param name="components">The child components.</param>
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
    /// Use this for internal computing of command components.
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
    }

    /// <summary>
    /// Use this for read-only command component access.
    /// </summary>
    public class IReadOnlyCommandComponent
    {
        /// <summary>
        /// Gets the name of this command component.
        /// </summary>
        public string Name { get { return name; } }

        /// <summary>
        /// Gets the type of this command component.
        /// </summary>
        public CommandType Type { get { return type; } }

        /// <summary>
        /// Gets the value of this command component.
        /// </summary>
        public object? Value { get { return value; } }

        /// <summary>
        /// Gets the child components of this command.
        /// </summary>
        public IReadOnlyCommandComponent[] Components { get { return components; } }

        string name;
        CommandType type;
        object? value;
        IReadOnlyCommandComponent[] components;

        /// <summary>
        /// Initializes a new instance of the IReadOnlyCommandComponent class.
        /// </summary>
        /// <param name="name">The name of the command component.</param>
        /// <param name="type">The type of the command component.</param>
        /// <param name="value">The value of the command component.</param>
        public IReadOnlyCommandComponent(string name, CommandType type, object? value)
        {
            this.name = name;
            this.type = type;
            this.value = value;
            this.components = [];
        }

        /// <summary>
        /// Initializes a new instance of the IReadOnlyCommandComponent class with child components.
        /// </summary>
        /// <param name="name">The name of the command component.</param>
        /// <param name="type">The type of the command component.</param>
        /// <param name="value">The value of the command component.</param>
        /// <param name="components">The child components.</param>
        public IReadOnlyCommandComponent(string name, CommandType type, object? value, IReadOnlyCommandComponent[] components)
        {
            this.name = name;
            this.type = type;
            this.value = value;
            this.components = components;
        }
    }
}
