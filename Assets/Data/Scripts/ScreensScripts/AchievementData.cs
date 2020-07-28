using System;
using System.Net.Mime;
using Abu.Tools.UI;
using TMPro;
using UnityEngine;

namespace Data.Scripts.ScreensScripts
{
    [CreateAssetMenu(fileName = "New PuzzleAchievementItem", menuName = "Puzzle/CreateAchievementItem", order = 51)]
    public class AchievementData : ScriptableObject, IListElement
    {
        [SerializeField] string Description;

        public Vector2 Size => entity.RectTransform.rect.size;
        
        TextButtonComponent entity;

        const string pathToPrefab = "UI/TextButton Variant";

        static TextButtonComponent prefab;
        
        static TextButtonComponent Prefab
        {
            get
            {
                if (prefab == null)
                    prefab = Resources.Load<TextButtonComponent>(pathToPrefab);

                return prefab;
            }        
        }

        public Transform Create()
        {
            entity = Instantiate(Prefab);
            entity.Text = Description;

            return entity.transform;
        }

        
    }
}