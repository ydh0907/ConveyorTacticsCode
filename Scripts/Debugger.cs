using UnityEngine;

public static class Debugger
{
    public static bool DebugLogIsNull(Object obj, string message)
    {
        if (obj == null)
        {
            Debug.Log($"<color=red>Obj is null</color>");
            return false;
        }

        Debug.Log($"<color=green>{obj.GetType().Name} : {message}</color>");
        return true;
    }
}