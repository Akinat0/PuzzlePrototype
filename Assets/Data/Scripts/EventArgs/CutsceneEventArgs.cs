
public class CutsceneEventArgs
{
    public string SceneID { get; private set; }
    public SceneTransitionType SceneTransition { get; private set; }
    

    public CutsceneEventArgs(string sceneID, SceneTransitionType sceneTransitionType)
    {
        SceneID = sceneID;
        SceneTransition = sceneTransitionType;
    }
    
}
