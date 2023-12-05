using UnityEngine;

public abstract class JobAction : ScriptableObject
{
    public abstract int Run(bool condition, int currentProgress);
}
