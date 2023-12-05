using UnityEngine;

[CreateAssetMenu(fileName = "AchieveJobActionSO", menuName = "SO/Quest/JobActionSO")]
public class AchieveJobAction : JobAction
{
    public override int Run(bool condition, int currentProgress)
    {
        if (condition)
        {
            return ++currentProgress;
        }
        else
        {
            return currentProgress;
        }
    }
}
