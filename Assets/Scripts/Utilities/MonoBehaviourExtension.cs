using System.Collections;
using UnityEngine;

public static class MonoBehaviourExtension
{
    public static Coroutine StartCoroutine(this MonoBehaviour behaviour, System.Action action, float delay)
    {
        return behaviour != null ? behaviour.StartCoroutine(WaitAndDo(behaviour, delay, action)) : null;
    }
 
    private static IEnumerator WaitAndDo(MonoBehaviour behaviour, float time, System.Action action)
    {
        yield return new WaitForSeconds(time);
        if (behaviour != null)
            action();
    }
}


/*if (!ignoreNullableException)
            action?.Invoke();
        else
        {
            try { action?.Invoke(); }
            catch(NullReferenceException) { /* ignored #1# }
catch(ArgumentNullException) { /* ignored #1# }
catch(MissingReferenceException) { /* ignored #1# }
catch(MissingComponentException) { /* ignored #1# }
catch(MissingFieldException) { /* ignored #1# }
catch(MissingMemberException) { /* ignored #1# }
catch(InvalidOperationException) { /* ignored #1# }
}*/
