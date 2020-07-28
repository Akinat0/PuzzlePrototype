using System;
using System.Net.Mime;
using Abu.Tools.UI;
using Puzzle;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Data.Scripts.ScreensScripts
{
    public class AchievementView : IListElement
    {
        public AchievementView(Achievement achievement)
        {
            this.achievement = achievement;
        }

        Achievement achievement;
        
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
            entity = Object.Instantiate(Prefab);
            entity.Text = achievement.Name;

            return entity.transform;
        }
        
    }
}