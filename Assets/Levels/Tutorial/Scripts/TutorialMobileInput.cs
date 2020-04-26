using Abu.Tools;
using Puzzle;
using UnityEngine;

public class TutorialMobileInput : MobileGameInput
{
    [SerializeField] private SkinContainer Lock;

    public override bool Condition {
        get => base.Condition;
        protected set
        {
            Lock.Skin = value ? 1 : 0;
            base.Condition = value;
        }
    }

    private void Start()
    {
        Lock.gameObject.SetActive(true);
        float offset = ScreenScaler.PartOfScreen(0.05f).y;
        Lock.transform.position = new Vector3(-ScreenScaler.PartOfScreen(0.5f).x + offset , -ScreenScaler.PartOfScreen(0.5f).y + offset);
        Lock.transform.localScale = Vector3.one * ScreenScaler.ScaleToFillPartOfScreen(Lock.GetComponent<SpriteRenderer>(), 0.05f);
        //Disable input on start
        Condition = false;
    }

    protected override void OnEnable()
    {
        TutoriaScenelManager.OnTutorialInputEnabled += OnTutorialInputEnabled_Handler;
        TutoriaScenelManager.OnTutorialInputDisabled += OnTutorialInputDisabled_Handler;
        GameSceneManager.ResetLevelEvent += OnRestartLevel_Handler;
    }
    
    protected override void OnDisable()
    {
        TutoriaScenelManager.OnTutorialInputEnabled -= OnTutorialInputEnabled_Handler;
        TutoriaScenelManager.OnTutorialInputDisabled -= OnTutorialInputDisabled_Handler;
        GameSceneManager.ResetLevelEvent -= OnRestartLevel_Handler;
    }

    void OnTutorialInputEnabled_Handler()
    {
        Condition = true;
    }
    
    void OnTutorialInputDisabled_Handler()
    {
        Condition = false;
    }
    
    
    void OnRestartLevel_Handler()
    {
        Condition = false;
    }
}
