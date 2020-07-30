using System.Linq;
using Abu.Tools.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Data.Scripts.ScreensScripts
{
    public class CollectionScrollList : VerticalScrollListComponent<CollectionListView>
    {
        protected override void Start()
        {
            Selection = Account.CollectionItems.Select(item => new CollectionListView(item)).ToArray();
            base.Start();
        }

        protected override void AddElement(IListElement listElement)
        {
            base.AddElement(listElement);
            
            Content.offsetMin -= new Vector2(0, ((FlexibleLayoutGroup) Layout).spacing.y);
            int rows = Layout.transform.childCount / 3 + 1;

            float rowsOffset = -rows * (((FlexibleLayoutGroup) Layout).spacing.y + listElement.Size.y) - layout.padding.top -layout.padding.bottom;
            
            ((RectTransform) Layout.transform).offsetMin = new Vector2(0, rowsOffset);
            Content.offsetMin = new Vector2(0, rowsOffset);
        }
    }
}