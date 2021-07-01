using System;
using Abu.Tools.UI;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ChestRewardView : UIComponent
{
    public static ChestRewardView Create(RectTransform container, Rarity rarity, int amount)
    {
        //TODO: When badge system will be ready use badge for amount
        ChestRewardView rewardView = Instantiate(Resources.Load<ChestRewardView>("UI/ChestRewardView"), container);
        rewardView.Rarity = rarity;
        return rewardView;
    }

    void OnEnable()
    {
        Animator.SetInteger(RarityID, (int) Rarity);
    }

    Rarity Rarity { get; set; }

    Animator animator;
    Animator Animator => animator ? animator : animator = GetComponent<Animator>();

    static readonly int RarityID = Animator.StringToHash("rarity");

}
