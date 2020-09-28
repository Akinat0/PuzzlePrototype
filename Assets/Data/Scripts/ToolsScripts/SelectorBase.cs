using System;
using Abu.Tools.UI;
using UnityEngine;

namespace Abu.Tools
{
    public abstract class SelectorBase : MonoBehaviour
    {
        [SerializeField] protected ButtonComponent RightArrow;
        [SerializeField] protected ButtonComponent LeftArrow;

        protected virtual void Start()
        {
            ItemNumber = 0;
        }

        protected virtual void OnEnable()
        {
            RightArrow.OnClick += OnRightBtnClick;
            LeftArrow.OnClick += OnLeftBtnClick;
        }
        
        protected virtual void OnDisable()
        {
            RightArrow.OnClick -= OnRightBtnClick;
            LeftArrow.OnClick -= OnLeftBtnClick;
        }

        protected abstract int Length { get;}
        public int ItemNumber { get; protected set;} //Index representing current item in the selection

        public virtual void OnRightBtnClick()
        {
            ItemNumber++;
            DisplayItem(ItemNumber, -1);
        }
    
        //Called when left btn clicks
        public virtual void OnLeftBtnClick()
        {
            ItemNumber--;
            DisplayItem(ItemNumber, 1);
        }
    
        protected virtual void DisplayItem(int _Index, int _Direction = 0)
        {
            //Managing right button
            RightArrow.SetActive(_Index + 1 != Length);

            //Managing left button
            LeftArrow.SetActive(_Index != 0);
            
            CreateItem(_Index);
            
            if (Length == 0)
            {
                LeftArrow.SetActive(false);
                RightArrow.SetActive(false);
            }
        }

        protected abstract void CreateItem(int _Index);
    }
    
}