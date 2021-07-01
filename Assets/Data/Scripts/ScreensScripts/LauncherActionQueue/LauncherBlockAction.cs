
using Puzzle;

public class LauncherBlockAction : LauncherAction
{
    public LauncherBlockAction() : base(LauncherActionOrder.Block) { }
    
    public override void Start()
    {
        if(GameSceneManager.Instance == null)
            Pop();
    }

    public override void Update()
    {
        if(GameSceneManager.Instance == null)
            Pop();
    }
}
