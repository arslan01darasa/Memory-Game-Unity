using StackTheBlockArslan;
using UnityEngine;
using UnityEngine.Advertisements;

using UnityEngine.Events;



public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{

    public string GAME_ID = "5437555"; //replace with your gameID from dashboard. note: will be different for each platform.
    public string GAME_ID_ANDROID = "";



    private const string VIDEO_PLACEMENT = "video";
    private const string REWARDED_VIDEO_PLACEMENT = "rewardedVideo";

    private bool testMode = true;
    public bool RemoveAds;


    public UnityEvent successCallback;

    public UnityEvent StartCallBack;


    public static AdsManager mInstance;

    public void Awake()
    {
        if (mInstance == null)
            mInstance = this;
        else if (mInstance != this)
            Destroy(gameObject);

        
            Debug.Log(Application.platform + " supported by Advertisement");
            Initialize();
        
        

        DontDestroyOnLoad(this.gameObject);
        StartCallBack.Invoke();
    }

    public void Initialize()
    {
        if (Advertisement.isSupported)
        {
            if(RemoveAds==false)
            {
            Debug.Log(Application.platform + " supported by Advertisement");
            }
        }
#if UNITY_IOS
        Advertisement.Initialize(GAME_ID, testMode, this);
#else
        Advertisement.Initialize(GAME_ID_ANDROID, testMode, this);
#endif
    }

    public void LoadRewardedAd()
    {
        Advertisement.Load(REWARDED_VIDEO_PLACEMENT, this);
    }

    public void ShowRewardedAd()
    {
        AudioListener.pause = true;
        Advertisement.Show(REWARDED_VIDEO_PLACEMENT, this);
    }

    public void LoadInterstitial()
    {
        Advertisement.Load(VIDEO_PLACEMENT, this);
    }

    public void ShowInterstitial()
    {
        Debug.Log("Show ad");
        AudioListener.pause = true;
        Advertisement.Show(VIDEO_PLACEMENT, this);
    }

    #region Interface Implementations
    public void OnInitializationComplete()
    {
        Debug.Log("Init Success");
        LoadInterstitial();
        LoadRewardedAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Init Failed: [{error}]: {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Load Success: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Load Failed: [{error}:{placementId}] {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"OnUnityAdsShowFailure: [{error}]: {message}");
        AudioListener.pause = false;
        if (placementId.Equals(REWARDED_VIDEO_PLACEMENT))
        {            
            LoadRewardedAd();
        }
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"OnUnityAdsShowStart: {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"OnUnityAdsShowClick: {placementId}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"OnUnityAdsShowComplete: [{showCompletionState}]: {placementId}");
        AudioListener.pause = false;
        if (placementId.Equals(REWARDED_VIDEO_PLACEMENT) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            GameplayController.Instance.ContinueGame();
            successCallback.Invoke();
        }
    }
    #endregion

    public void ToggleTestMode(bool isOn)
    {
        testMode = isOn;
    }




}
