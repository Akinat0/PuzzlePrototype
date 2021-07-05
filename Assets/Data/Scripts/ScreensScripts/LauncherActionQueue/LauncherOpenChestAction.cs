using Data.Scripts.Tools.Input;

public class LauncherOpenChestAction : LauncherAction
{
    public LauncherOpenChestAction(Rarity rarity) : base(LauncherActionOrder.Chest)
    {
        Rarity = rarity;
    }

    Rarity Rarity { get; }

    public override void Start()
    {
        //hack to disable input
        MobileInput.Condition = false;
        
        StartCoroutine(Coroutines.Delay(0.3f, OpenChest));
    }

    void OpenChest()
    {
        //hack to disable input
        MobileInput.Condition = true;
        
        Chest chest = Account.GetChest(Rarity); 

        if (chest.Count <= 0)
        {
            Pop();
            return;
        }

        Reward[] rewards = chest.Open();
        OpenChestWindow.Create(rewards, Rarity, Pop);
    }
}
