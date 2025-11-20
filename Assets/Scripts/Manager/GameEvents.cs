using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action OnObjectiveActivated;
    public static event Action<GameObject> OnTargetFocused;
    public static event Action<GameObject> OnTargetLost;

    public static void TriggerObjectiveActivated()
    {
        OnObjectiveActivated?.Invoke();
    }

    public static void TriggerTargetFocused(GameObject target)
    {
        OnTargetFocused?.Invoke(target);
    }

    public static void TriggerTargetLost(GameObject target)
    {
        OnTargetLost?.Invoke(target);
    }
}