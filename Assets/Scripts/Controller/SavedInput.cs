public class SavedInput<T>
{
    public bool IsUpdated;
    public T Value;

    public void UpdateValue(T value)
    {
        Value = value;
        IsUpdated = true;
    }
    
    public T TakeValue()
    {
        IsUpdated = false;
        return Value;
    }
}