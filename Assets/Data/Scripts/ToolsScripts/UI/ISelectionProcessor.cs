
public interface ISelectionProcessor<in T>
{
    void ProcessIndex(int index, T[] selection);

    void ProcessOffset(float offset, int index, T[] selection);
}
