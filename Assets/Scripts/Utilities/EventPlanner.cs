using System;
using System.Threading.Tasks;
using UnityEngine;

public static class EventPlanner
{
    public static async void PostponeAnEvent(Action action, int delay, bool ignoreNullableException = true)
    {
        await Task.Delay(delay);
        if (!ignoreNullableException)
            action?.Invoke();
        else
        {
            try { action?.Invoke(); }
            catch(NullReferenceException) { /* ignored */ }
            catch(ArgumentNullException) { /* ignored */ }
            catch(MissingReferenceException) { /* ignored */ }
            catch(MissingComponentException) { /* ignored */ }
            catch(MissingFieldException) { /* ignored */ }
            catch(MissingMemberException) { /* ignored */ }
            catch(InvalidOperationException) { /* ignored */ }
        }
    }
}