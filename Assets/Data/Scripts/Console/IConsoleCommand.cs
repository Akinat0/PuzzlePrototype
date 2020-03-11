using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
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
            //TODO fix it
            Text output =  Object.FindObjectOfType<ConsoleView>().GetComponentInChildren<Text>();
            output.text = "";
            return "";
        }
    }

    public class WalletConsoleCommand : IConsoleCommand
    {
        public string Command => "wallet";

        public string Process(object[] args, Console console)
        {
            var walletManager = Object.FindObjectOfType<WalletManager>();
            string result = "Success";
            
            if (args.Length < 2)
                return "Fail. First arg should be integer amount of coins" ;
            
            try
            {
                walletManager.WalletData.coins = Convert.ToInt32(args[1]);
            }
            catch (Exception e)
            {
                result = $"Fail. {e.Message}";
            }
            
            return result;
        }
    }
}