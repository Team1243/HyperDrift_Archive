using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [SerializeField] private HeartSystem heartSystem;
    [SerializeField] private CoinSystem coinSystem;

    private void Start()
    {
        // 인자가 false이면 메시지 보내줌
        Debug.Assert(heartSystem, "HeartSystem is not in RewardSystem");
        Debug.Assert(coinSystem, "CoinSystem is not in RewardSystem");
    }

    private void OnEnable()
    {
        DailyBonusPopup.OnRecieveReward += GiveReward;
        // 나중에 여기에 퀘스트 보상 수령 버튼 눌렀을 때 액션 넣어주어야 함
    }

    private void OnDisable()
    {
        DailyBonusPopup.OnRecieveReward -= GiveReward;
        // 나중에 여기에 퀘스트 보상 수령 버튼 눌렀을 때 액션 넣어주어야 함
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
