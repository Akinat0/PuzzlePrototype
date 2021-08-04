namespace Abu.Tools.UI
{
    public static class EmojiHelper
    {
        const string EMOJI_TEXT = "<sprite name={0}>";

        public static string StarEmoji => string.Format(EMOJI_TEXT, "casual_star");
        public static string ShardCommonEmoji => string.Format(EMOJI_TEXT, "puzzle_shard_common");
        public static string ShardRareEmoji => string.Format(EMOJI_TEXT, "puzzle_shard_rare");
        public static string ShardEpicEmoji => string.Format(EMOJI_TEXT, "puzzle_shard_epic");

        public static string GetShardEmojiText(Rarity rarity)
        {
            switch (rarity)
            {
                case Rarity.Common:
                    return ShardCommonEmoji;
                case Rarity.Rare:
                    return ShardRareEmoji;
                case Rarity.Epic:
                    return ShardEpicEmoji;
                default:
                    return string.Empty;
            }
        }
        
    }
}