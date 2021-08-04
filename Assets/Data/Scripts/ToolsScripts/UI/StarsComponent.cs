using ScreensScripts;

namespace Abu.Tools.UI
{
    public class StarsComponent : WalletComponent
    {

        protected override Wallet Wallet => Account.Stars;
        protected override string IconText => EmojiHelper.StarEmoji;


        protected override void OnEnable()
        {
            base.OnEnable();
            
            LauncherUI.LevelChangedEvent += OnLevelChangedHandler;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            LauncherUI.LevelChangedEvent -= OnLevelChangedHandler;
        }

        void OnLevelChangedHandler(LevelChangedEventArgs args)
        {
            Text.Color = args.LevelConfig.ColorScheme.TextColorLauncher;
        }

        
    }
}
