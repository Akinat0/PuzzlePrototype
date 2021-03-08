using System.Linq;
using System.Collections.Generic;

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
                new ClearConsoleCommand(),
                new WalletConsoleCommand(),
                new DefaultCollectionItemCommand(),
                new AchievementsListCommand(),
                new AchievementsSetProgressCommand(),
                new BoosterCommand(),
                new SetStarsCommand(),
                new DebugImageCommand(),
                new DeleteLocalCommand(),
                new AnalyticsCommand(),
                new HapticCommand(),
            };
        }

        public string Process(string cmd)
        {
            if (!cmd.StartsWith(Prefix))
                return "Commands start with /";

            object[] args = cmd.Split(' ').Select(arg => (object)arg).ToArray();

            if (!(args[0] is string commandName))
                return "Error. Can't read command";
            
            commandName = commandName.Replace("/", "");
            
            IConsoleCommand command = Commands.FirstOrDefault(c => c.Command == commandName);

            if (command == null)
                return $"Command {commandName} not found";

            return command.Process(args, this);

        }
    }
}