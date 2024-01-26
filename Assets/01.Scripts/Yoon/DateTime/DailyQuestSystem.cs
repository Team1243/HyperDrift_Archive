using UnityEngine;

public class DailyQuestSystem : MonoBehaviour
{
    private PlayerQuestCheck _playerQuestCheck;

    private void Awake()
    {
        _playerQuestCheck = transform.parent.parent.GetComponentInChildren<PlayerQuestCheck>();
    }

    private void OnEnable()
    {
        RealDailyTimeSystem.OnDayHasPassed += _playerQuestCheck.AllQuestJobInit;
    }

    private void OnDisable()
    {
        RealDailyTimeSystem.OnDayHasPassed -= _playerQuestCheck.AllQuestJobInit;
    }
}
