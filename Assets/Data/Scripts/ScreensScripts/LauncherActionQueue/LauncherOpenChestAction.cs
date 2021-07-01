public class LauncherOpenChestAction : LauncherAction
{
    public LauncherOpenChestAction(Rarity rarity) : base(LauncherActionOrder.Chest)
    {
        Rarity = rarity;
    }

    Rarity Rarity { get; }

    public override void Start()
    {
        StartCoroutine(Coroutines.FramesDelay(1, OpenChest));
    }

    void OpenChest()
    {
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
