using UnityEngine;

namespace Abu.Tools.UI
{
    public class ShardsWalletComponent : WalletComponent
    {
        [SerializeField] Rarity rarity;
        
        protected override Wallet Wallet => Account.GetShards(rarity);
        protected override string IconText => EmojiHelper.GetShardEmojiText(rarity);

        void Start()
        {
            Alpha = 0;
        }
    }
}