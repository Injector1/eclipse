public class SavedInput<T>
{
    public bool IsUpdated { get; private set; }

    private T _value;
    public T Value
    {
        get
        {
            IsUpdated = false;
            return _value;
        }
        set
        {
            IsUpdated = true;
            _value = value;
        }
    }
}