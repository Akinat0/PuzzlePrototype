using Puzzle;
using UnityEngine;

public class HeartsHealthManager : HealthManagerBase
{
    [SerializeField] private GameObject[] hearts;
    
    private void Start()
    { 
        Hp = hearts.Length - 1;
    }

    protected override void LoseHeart(int _Hp)
    {
        if (Hp < 0)
        {
          Debug.LogError("Current HP is " + Hp);
           return;
        }
        
        hearts[Hp].SetActive(false);
        Hp--;
        
        if(_Hp != Hp)
            Debug.LogWarning("Health manager hp doesn't match with player's hp");

//        Debug.Log("ASD:");
//        if (phase < 5)
//        {
//            phase = 0;
//        }
//        
//            damagableSprites = GameObject.FindGameObjectsWithTag("DamagableView");
//            for (int i = 0; i < damagableSprites.Length; i++)
//            {
//                Debug.Log("LENGTH OF DAMAGABLE = " + damagableSprites.Length);
//                damagableSprites[i].GetComponent<SkinContainer>().IncrementPhase();
//            }
//            phase++;
//        
    }

    protected override void Reset()
    {
        foreach (var heart in hearts)
            heart.SetActive(true);
    }
}