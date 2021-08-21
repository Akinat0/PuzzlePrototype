using Puzzle;

public class TutorialPauseScreen : PauseScreen
{
    protected override void OnMenuClick()
    {
        if (Account.TutorialCompleted)
        {
            base.OnMenuClick();
            return;
        }

        Window window = null;
        
        void ExitTutorial()
        {
            GameSceneManager.Instance.LevelConfig.StarsAmount = 3;
            base.OnMenuClick();
        }

        void Continue()
        {
            if(window != null)
                window.Hide();
        }

        const string title       = "Are you sure?"; 
        const string ok          = "Continue";
        const string exit        = "Exit";
        const string description = "Do you want to exit tutorial?";

        window = CancelableWindow.Create(description, Continue, ExitTutorial, title, ok, exit, RectTransform);
    }
}
