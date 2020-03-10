using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Abu.Console
{
    public interface IConsoleCommand
    {
        
        string Command { get; }
        string Process(object[] args, Console console);
    }

    public class HelpConsoleCommand : IConsoleCommand
    {
        public string Command => "help";

        public string Process(object[] args, Console console)
        {
            string result = "List of commands available " + Environment.NewLine;
            
            foreach (var command in console.Commands)
            {
                result += $"{command.Command}{Environment.NewLine}";
            }
            
            return result;
        }
    }
    public class ClearConsoleCommand : IConsoleCommand
    {
        public string Command => "clear";

        public string Process(object[] args, Console console)
        {
            Text output =  Object.FindObjectOfType<ConsoleView>().GetComponentInChildren<Text>();
            output.text = "";
            return "";
        }
    }
}