namespace Abu.Tools.UI
{
    public abstract class ListBaseComponent<T> : UIComponent
    {
        protected virtual T[] Selection { get; set; }
        protected virtual int Length => Selection.Length;
        protected override void OnValidate() { }
    }
}