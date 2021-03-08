using System;
using System.Linq;
using Puzzle;
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
            ConsoleView consoleView = Object.FindObjectOfType<ConsoleView>();
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
                    achievement.ResetAchievement();
                
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
    
 
    public class BoosterCommand : IConsoleCommand
    {
        public string Command => "booster";

        public string Process(object[] args, Console console)
        {
            if (args.Length > 1)
            {
                switch (args[1])
                {
                    case "list":
                        return List;
                    case "add":
                        return Add(args[2], args[3]);
                    case "activate":
                        return Activate(args[2]);
                    default:
                        return Help;
                }
            }
            else
                return Help;
            
        }

        string List
        {
            get
            {
                string result = string.Empty;

                foreach (Booster booster in Account.Boosters)
                    result += booster.Name + Environment.NewLine;

                return result;
            }
        }

        string Add(object name, object amount)
        {
            Booster booster;
            try
            {
                // ReSharper disable once PossibleNullReferenceException
                booster = Account.Boosters.FirstOrDefault(b =>
                        String.Equals(b.Name.Replace(" ", ""), name.ToString(), StringComparison.InvariantCultureIgnoreCase));

                booster.Amount += int.Parse(amount.ToString());
            }
            catch (Exception e)
            {
                return $"Fail. {e.Message}";
            }

            return $"Success. Now amount of {booster.Name} is {booster.Amount}";
        }

        string Activate(object name)
        {
            Booster booster;
            try
            {
                // ReSharper disable once PossibleNullReferenceException
                booster = Account.Boosters.FirstOrDefault(b =>
                    String.Equals(b.Name.Replace(" ", ""), name.ToString(), StringComparison.InvariantCultureIgnoreCase));

                if (booster.Amount <= 0)
                    booster.Amount++;
                
                booster.Activate();
            }
            catch (Exception e)
            {
                return $"Fail. {e.Message}";
            }

            return $"Success. Now {booster.Name} is active";
        }

        string Help => $"\"list\" to print all boosters. {Environment.NewLine}" +
                       $"\"add\" \"name\" \"amount\" to add this amount of boosters {Environment.NewLine}" +
                       $"\"activate\" \"name\" to add this amount of boosters {Environment.NewLine}";
        
    }
    
    public class SetStarsCommand : IConsoleCommand
    {
        public string Command => "stars";

        public string Process(object[] args, Console console)
        {
            if (args.Length == 3)
                return SetStars(args[1], args[2]);
            else
                return Help;
        }
        
        string SetStars(object level, object stars)
        {
            try
            {
                // ReSharper disable once PossibleNullReferenceException
                Account.LevelConfigs.FirstOrDefault(_level =>
                        String.Equals(_level.Name.Replace(" ", ""), level.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    .StarsAmount = int.Parse(stars.ToString());
            }
            catch (Exception e)
            {
                return $"Fail. {e.Message}";
            }

            return "Success.";
        }


        string Help => $"stars \"level name\" (without whitespaces) \"progress\" to stars amount. ";
    }
    
    public class DebugImageCommand : IConsoleCommand
    {
        public string Command => "debugimage";

        public string Process(object[] args, Console console)
        {
            ConsoleView.ToggleDebugImage();
            return "Success.";
        }
        
    }

    public class AnalyticsCommand : IConsoleCommand
    {
        public string Command => "analytics";
        
        public string Process(object[] args, Console console)
        {
            if (args.Length == 2)
                return Parse(args[1]);
            else
                return Help;
        }
        

        string Parse(object command)
        {
            try
            {
                string parsedCommand = command.ToString();
                switch (parsedCommand.Trim().ToLowerInvariant())
                {
                    case "enable":
                        Account.Analytics.Enable();
                        return "Analytics enabled";
                    case "disable":
                        Account.Analytics.Disable();
                        return "Analytics disabled";
                    default:
                        return Help;
                }
            }
            catch (Exception e)
            {
                return $"Fail. {e.Message}";
            }
        }
        
        string Help => "Analytics has commands \"enable\" and \"disable\"";
    }
    
    public class DeleteLocalCommand : IConsoleCommand
    {
        public string Command => "deletelocal";

        public string Process(object[] args, Console console)
        {
            PlayerPrefs.DeleteAll();
            return "All local data deleted. Restart the application";
        }

    }
    
    
    public class HapticCommand : IConsoleCommand
    {
        public string Command => "haptic";
        
        public string Process(object[] args, Console console)
        {
            ConsoleView.ToggleHapticMenu();
            return "success";
        }
    }
    
}