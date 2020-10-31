using System;
using System.Collections;
using System.Linq;
using Abu.Tools;
using System.Collections.Generic;
using UnityEngine;

public class StarsManager : MonoBehaviour
{
    StarView[] StarsView;

    #region factory

    static StarsManager prefab;
    static StarsManager Prefab
    {
        get
        {
            if (prefab == null)
                prefab = Resources.Load<StarsManager>("Prefabs/StarsView");
            return prefab;
        }
    }
    
    public static StarsManager Create(Transform parent, StarView starView)
    {
        StarsManager starsManager = new GameObject("stars_manager").AddComponent<StarsManager>();
        starsManager.transform.SetParent(parent);
        
        List<StarView> starViews = new List<StarView>();
        
        for (int i = 0; i < 3; i++)
            starViews.Add(Instantiate(starView, starsManager.transform));
        
        starsManager.Initialize(starViews);
            
        return starsManager;
    }
        
    #endregion
    
    public void Initialize(IEnumerable<StarView> starViews)
    {
        StarsView = starViews.ToArray();
        
        //Calculate scale for single star as if it's a single element and then use it for whole view
        float singleImageScale =
            ScreenScaler.FitHorizontalPart(StarsView.First().Background, 0.3f);

        transform.localScale = singleImageScale * Vector3.one;
        
        transform.localPosition = new Vector3(0, ScreenScaler.PartOfScreen(0.25f).y, 0);

        Vector2 camSize = ScreenScaler.CameraSize;

        StarsView[0].transform.localPosition = new Vector3(-0.25f * camSize.x / transform.lossyScale.x,0.125f * camSize.y);
        StarsView[1].transform.localPosition = new Vector3(0, 0.25f * camSize.y);
        StarsView[2].transform.localPosition = new Vector3(0.25f * camSize.x / transform.lossyScale.x, 0.125f * camSize.y);
    }

    public void ShowStarsTogether(int stars)
    {
        stars = Mathf.Clamp(stars, 0, 3);
        
        for (int i = 0; i < 3; i++)
            StarsView[i].Show(i < stars);
    }
    
    public void ShowStarsInstant(int stars)
    {
        stars = Mathf.Clamp(stars, 0, 3);

        for (int i = 0; i < 3; i++)
            StarsView[i].Show(i < stars, true);
    }
    
    public void ShowStarsAnimation(int stars, Action finished = null)
    {
        stars = Mathf.Clamp(stars, 0, 3);
        
        int count = stars;

        void Finished()
        {
            count--;
            if (count > 0) 
                return;
        
            finished?.Invoke();
        }

        for (int i = 0; i < 3; i++)
            StarsView[i].Show(i < stars, finished: Finished);
    }

    public void HideStars(Action finished = null)
    {
        int count = StarsView.Length;

        foreach (StarView star in StarsView)
        {
            star.Hide(() =>
            {
                count--;
                if(count > 0)
                    return;
                finished?.Invoke();
            });
        }
    }
    
#if UNITY_EDITOR
    
    [ContextMenu("Hide stars")]
    public void EditorHideStars()
    {
        if (Application.IsPlaying(this))
            HideStars( () => Debug.LogError("Hide finished"));
    }
    
    [ContextMenu("3 stars")]
    public void EditorCallThreeStars()
    {
        if (Application.IsPlaying(this))
            ShowStarsAnimation(3, () => Debug.LogError("Animation finished"));
    }
    
    [ContextMenu("2 stars")]
    public void EditorCallTwoStars()
    {
        if (Application.IsPlaying(this))
            ShowStarsAnimation(2, () => Debug.LogError("Animation finished"));
    }
    
    [ContextMenu("1 star")]
    public void EditorCallOneStar()
    {
        if (Application.IsPlaying(this))
            ShowStarsAnimation(1, () => Debug.LogError("Animation finished"));
    }
    
    [ContextMenu("3 star instant")]
    public void EditorCallThreeStarsInstant()
    {
        if (Application.IsPlaying(this))
            ShowStarsInstant(3);
    }
    
    [ContextMenu("2 star instant")]
    public void EditorCallTwoStarsInstant()
    {
        if (Application.IsPlaying(this))
            ShowStarsInstant(2);
    }
    
    [ContextMenu("1 star instant")]
    public void EditorCallStarInstant()
    {
        if (Application.IsPlaying(this))
            ShowStarsInstant(1);
    }
#endif
}