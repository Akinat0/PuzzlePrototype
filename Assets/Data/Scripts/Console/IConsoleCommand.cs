using System;
using System.Linq;
using System.Runtime.InteropServices;
using Boo.Lang;
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
            MonoBehaviour consoleView = Object.FindObjectOfType<ConsoleView>();
            Text text = consoleView.GetComponentInChildren<Text>();
            text.text = "";
            return "";
        }
    }

    public class WalletConsoleCommand : IConsoleCommand
    {
        public string Command => "wallet";

        public string Process(object[] args, Console console)
        {
            string result = "Success";
            
            if (args.Length < 2)
                return "Fail. First arg should be integer amount of coins" ;
            
            try
            {
                Account.AddCoins(int.Parse(args[1].ToString()));;
            }
            catch (Exception e)
            {
                result = $"Fail. {e.Message}";
            }
            
            return result;
        }
    }

    public class DefaultCollectionItemCommand : IConsoleCommand
    {
        public string Command => "defcolitem";

        public string Process(object[] args, Console console)
        {
            var collectionManager = Object.FindObjectOfType<CollectionManager>();
            string result;

            try
            {
                result = $"Success. Default item id is {collectionManager.DefaultItemID}, " +
                          $"object has name {collectionManager.DefaultItem.name}";
            }
            catch (Exception e)
            {
                result = $"Fail. {e.Message}";
            }
            
            return result;
        }
    }

    public class AchievementsListCommand : IConsoleCommand
    {
        public string Command => "achievements";

        public string Process(object[] args, Console console)
        {
            if (args.Length == 2)
            {
                switch (args[1].ToString())
                {
                    case "list":
                        return List;
                    case "reset":
                        return Reset;
                    default:
                        return Help;
                }
            }
            else
            {
                return Help;
            }

        }

        string List
        {
            get
            {
                string result = String.Empty;

                foreach (var achievement in Account.Achievements)
                    result += achievement.Name + Environment.NewLine;

                return result;
            }
        }

        string Reset
        {
            get
            {
                foreach (var achievement in Account.Achievements)
                    achievement.Progress = 0;

                return "All achievements have been reset";
            }
        }

        string Help => $"\"list\" to print all achievements. {Environment.NewLine}" +
                       $"\"reset\" to reset all achievements {Environment.NewLine}";

    }
    
    public class AchievementsSetProgressCommand : IConsoleCommand
    {
        public string Command => "setprogress";

        public string Process(object[] args, Console console)
        {
            if (args.Length == 3)
            {
                return SetProgress(args[1], args[2]);
            }
            else
            {
                return Help;
            }

        }


        string SetProgress(object achievement, object progress)
        {
            try
            {
                // ReSharper disable once PossibleNullReferenceException
                Account.Achievements.FirstOrDefault(a =>
                        String.Equals(a.Name.Replace(" ", ""), achievement.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    .Progress = float.Parse(progress.ToString());
            }
            catch (Exception e)
            {
                return $"Fail. {e.Message}";
            }

            return "Success.";
        }


        string Help => $"\"achievement name\" (without whitespaces) \"progress\" to to set achievement progress. ";


    }
    
    
}