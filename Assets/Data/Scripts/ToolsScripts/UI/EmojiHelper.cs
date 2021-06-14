namespace Abu.Tools.UI
{
    public static class EmojiHelper
    {
        const string EMOJI_TEXT = "<sprite={0}>";

        public static string StarEmoji => string.Format(EMOJI_TEXT, 0);
        
    }
}