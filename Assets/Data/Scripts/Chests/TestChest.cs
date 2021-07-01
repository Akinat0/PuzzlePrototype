using UnityEngine;

public class TestChest : MonoBehaviour
{
    [ContextMenu("Test")]
    void Test() 
    {
        Chest epicChest = Account.GetChest(Rarity.Epic);
        epicChest.Add(1);
        Reward[] rewards = epicChest.Open();
        OpenChestWindow.Create(rewards, Rarity.Epic);
    }
}
