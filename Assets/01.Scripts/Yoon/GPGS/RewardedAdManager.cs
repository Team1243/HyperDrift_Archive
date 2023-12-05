using System.Net.NetworkInformation;
using GoogleMobileAds.Api;
using UnityEngine;
using System;

public class RewardedAdManager : MonoBehaviour
{
    public static RewardedAdManager Instance = null;

    private RewardedAd rewardedAd;

    public Action onUserEarnedRewardAction; // 보상형 광고 실행 후 광고창을 닫았을 때 실행

    private readonly string rewardAdID = "ca-app-pub-5714181718235393~3436478902"; // 보상형 광고 ID
    private readonly string rewardTestAdID = "ca-app-pub-5714181718235393~3436478902"; //보상형 테스트 광고 ID

    private void Awake()
    {
        if (Instance != null)
        {
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (IsNetworkAvailable())
        {
            Debug.Log(IsNetworkAvailable());
        }

        onUserEarnedRewardAction += () => Debug.Log("Player successfull watching");

        LoadAd();
    }

    // 광고를 보기 위한 준비 단계
    private void LoadAd()
    {
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd = new RewardedAd(rewardTestAdID); // 지금은 샘플 아이디로
        rewardedAd.LoadAd(request);

        #region Events

        // 보상형 광고 실행 후 광고창을 닫았을 때 호출
        rewardedAd.OnUserEarnedReward += (sender, reward) => onUserEarnedRewardAction();

        // 광고 로드 중에 오류가 발생했을 때 호출
        rewardedAd.OnAdFailedToLoad += (sender, args) => HandleAdFailedToLoad(sender, args);

        // 광고가 전체 화면으로 표시되지 않을 때 호출
        rewardedAd.OnAdFailedToShow += (sender, args) => ShowAd();

        #endregion
    }

    // 광고가 잘 로딩되었는지 확인하고 광고 재생
    public void ShowAd()
    {
        if (rewardedAd != null && rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
            LoadAd();
        }
        else
        {
            Debug.Log("Ad is not ready");
        }
    }

    private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("ad loading is failure");

        if (IsNetworkAvailable())
        {
            Debug.Log("network is connect");
        }
        else
        {
            Debug.Log("network is not connect");
        }
    }

    // 나중에 게임 매니저 시스템쪽으로 빼야 할 거 같음
    private bool IsNetworkAvailable()
    {
        return NetworkInterface.GetIsNetworkAvailable();
    }
}
