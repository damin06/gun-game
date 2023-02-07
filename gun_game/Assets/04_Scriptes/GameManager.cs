using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using Cinemachine;

public class GameManager : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI currentLevelTXT;
    [SerializeField] private GameObject EndGameUI;


    [SerializeField] private bool isTest = true;
    public bool isSlowMotion = false;
    public bool isGamePause = false;
    public static GameManager instance { get; private set; }
    public Action OndieKing;


    private InterstitialAd interstitial;
    private float fiextime;
    private float notime;
    Cinemachine.CinemachineVirtualCamera c_VirtualCamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }

    private void Start()
    {

        OndieKing += ONEndGame;

        fiextime = Time.fixedDeltaTime;
        notime = Time.timeScale;

        //string adUnitId = "ca-app-pub-3940256099942544/1033173712";

        var requestConfiguration = new RequestConfiguration
               .Builder()
               .SetTestDeviceIds(new List<string>() { "1DF7B7CC05014E8" }) // test Device ID
               .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        LoadFrontAd();
        LoadBannerAd();
        ToggleBannerAd(true);
    }

    void Update()
    {
        // Time.timeScale += (1f / 2) * Time.unscaledDeltaTime;
        // Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    public void ONEndGame()
    {
        isGamePause = true;
        SotpSlowMotion();
        EndGameUI.SetActive(true);
        LoadAD();
    }

    public void ChangeNextLevel()
    {
        EndGameUI.SetActive(false);
        isGamePause = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        currentLevelTXT.text = "LEVEL " + SceneManager.GetActiveScene().buildIndex;
    }

    public void RestartLEVEL()
    {
        if (!isGamePause)
        {
            SotpSlowMotion();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // AdRequest GetAdRequest()
    // {
    //     return new AdRequest.Builder().AddTestDevice("1DF7B7CC05014E8").Build();
    // }

    private void LoadAD()
    {
        Debug.Log("AD");
        ShowFrontAd();
        // this.rewardedAd = new RewardedAd("ca-app-pub-3940256099942544/1033173712");
        // //this.interstitial = new InterstitialAd(adUnitId);
        // AdRequest request = new AdRequest.Builder().Build();
        // this.rewardedAd.LoadAd(request);
        // #if UNITY_ANDROID
        //         string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        // #elif UNITY_IPHONE
        //         string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        // #else
        //         string adUnitId = "unexpected_platform";
        // #endif

        //         this.interstitial = new InterstitialAd(adUnitId);
        //         AdRequest request = new AdRequest.Builder().Build();
        //         this.interstitial.LoadAd(request);

        // frontAd = new InterstitialAd("ca-app-pub-3940256099942544/1033173712");
        // frontAd.LoadAd(GetAdRequest());
        // frontAd.OnAdClosed += (sender, e) =>
        // {

        // };
    }

    public void SlowMotion()
    {
        SwichCam();
        isSlowMotion = true;
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = fiextime * .02f;
    }

    public void SotpSlowMotion()
    {
        isSlowMotion = false;
        Time.timeScale = notime;
        Time.fixedDeltaTime = fiextime;
    }

    private void SwichCam()
    {
        c_VirtualCamera = GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        Transform sibal = GameObject.FindGameObjectWithTag("killKing").transform;
        c_VirtualCamera.m_Follow = sibal.transform;
        c_VirtualCamera.m_LookAt = sibal.transform;
    }


    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    const string frontTestID = "ca-app-pub-3940256099942544/8691691433";
    const string frontID = "ca-app-pub-5866814658384804/7439071934";
    InterstitialAd frontAd;


    void LoadFrontAd()
    {
        frontAd = new InterstitialAd(isTest ? frontTestID : frontID);
        frontAd.LoadAd(GetAdRequest());
        frontAd.OnAdClosed += (sender, e) =>
        {
            //LogText.text = "전면광고 성공";
        };
    }

    public void ShowFrontAd()
    {
        frontAd.Show();
        LoadFrontAd();
    }


    const string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
    const string bannerID = "ca-app-pub-5866814658384804~8085939196";
    BannerView bannerAd;


    void LoadBannerAd()
    {
        bannerAd = new BannerView(isTest ? bannerTestID : bannerID,
            AdSize.SmartBanner, AdPosition.Bottom);
        bannerAd.LoadAd(GetAdRequest());
        ToggleBannerAd(false);
    }

    public void ToggleBannerAd(bool b)
    {
        if (b) bannerAd.Show();
        else bannerAd.Hide();
    }
}
