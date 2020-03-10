using System.Linq;
using Boo.Lang;

namespace Abu.Console
{
    public class Console
    {
        private const string Prefix = "/";
        public readonly List<IConsoleCommand> Commands;

        public Console()
        {
            Commands = new List<IConsoleCommand>
            {
                new HelpConsoleCommand(),
                new ClearConsoleCommand()
            };
        }

        public string Process(string cmd)
        {
            if (!cmd.StartsWith(Prefix))
                return "Commands start with /";

            object[] args = cmd.Split();
            string commandName = args[0] as string;

            if (commandName == null)
                return "Error. Can't read command";
            
            commandName = commandName.Replace("/", "");
            
            IConsoleCommand command = Commands.FirstOrDefault(c => c.Command == commandName);

            if (command == null)
                return $"Command {commandName} not found";

            return command.Process(args, this);

        }
    }
}