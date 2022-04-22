using System;
using System.Threading.Tasks;

public static class EventPlanner
{
    public static async void PostponeAnEvent(Action action, int delay)
    {
        await Task.Delay(delay);
        action?.Invoke();
    }
}