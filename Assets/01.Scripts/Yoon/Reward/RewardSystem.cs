using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [SerializeField] private HeartSystem heartSystem;
    [SerializeField] private CoinSystem coinSystem;

    private void Start()
    {
        // ���ڰ� false�̸� �޽��� ������
        Debug.Assert(heartSystem, "HeartSystem is not in RewardSystem");
        Debug.Assert(coinSystem, "CoinSystem is not in RewardSystem");
    }

    private void OnEnable()
    {
        DailyBonusPopup.OnRecieveReward += GiveReward;
        QuestUI.OnRecieveReward += GiveReward;
    }

    private void OnDisable()
    {
        DailyBonusPopup.OnRecieveReward -= GiveReward;
        QuestUI.OnRecieveReward -= GiveReward;
    }

    public void GiveReward(RewardInfo info)
    {
        if (info.RewardType == RewardType.Heart)
        {
            heartSystem.GenerateHeart(info.RewardAmount, isReward: true);
        }
        else if (info.RewardType == RewardType.Coin)
        {
            coinSystem.RecieveCoin(info.RewardAmount);
        }
    }

}
