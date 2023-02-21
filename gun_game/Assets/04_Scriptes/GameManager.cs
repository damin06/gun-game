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

    [Space]
    [SerializeField] private bool isTest = false;

    public bool isSlowMotion = false;
    public bool isGamePause = false;
    public static GameManager instance;


    private InterstitialAd interstitial;
    private float fiextime;
    private float notime;
    public Action OndieKing;
    public Action isntKing;
    Cinemachine.CinemachineVirtualCamera c_VirtualCamera;
    private JSON json;
    private Data PlayerData;

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

        //json = GetComponent<JSON>();
        //PlayerData = json.playerData;
        //json.LoadPlayerDataToJson();
    }

    private void Start()
    {
        OndieKing += OnEndGameUI;
        OndieKing += SaveScene;

        isntKing += SotpSlowMotion;
        isntKing += SwichCamToGun;

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

        StartCoroutine(BugBannerAd());
    }

    void Update()
    {

        // Time.timeScale += (1f / 2) * Time.unscaledDeltaTime;
        // Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    private void OnEndGameUI()
    {
        StartCoroutine(ONEndGame());
    }

    private IEnumerator ONEndGame()
    {
        //yield return new WaitForSeconds(1);
        //PlayerData.currentStage++;
        //json.SavePlayerDataToJson();

        isGamePause = true;
        SotpSlowMotion();
        EndGameUI.SetActive(true);
        ShowFrontAd();
        yield return null;
    }

    public void ChangeNextLevel()
    {
        EndGameUI.SetActive(false);
        isGamePause = false;
        StartCoroutine(BugBannerAd());

        if (SceneManager.GetActiveScene().name == "End")
        {
            SceneManager.LoadScene(0);
            StartCoroutine(ChangeLevelTXT());
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(ChangeLevelTXT());
    }

    private IEnumerator ChangeLevelTXT()
    {
        yield return new WaitForSeconds(0.1f);
        int currenScene = SceneManager.GetActiveScene().buildIndex + 1;
        currentLevelTXT.text = "LEVEL " + currenScene;
    }

    public void RestartLEVEL()
    {
        if (!isGamePause)
        {
            SotpSlowMotion();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void SlowMotion()
    {
        SwichCamToBullet();
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

    private void SwichCamToBullet()
    {
        c_VirtualCamera = GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        Transform s = GameObject.FindGameObjectWithTag("killKing").transform;
        c_VirtualCamera.m_Follow = s.transform;
        c_VirtualCamera.m_LookAt = s.transform;
    }

    private void SwichCamToGun()
    {
        c_VirtualCamera = GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        Transform s = GameObject.FindGameObjectWithTag("Gun").transform;
        c_VirtualCamera.m_Follow = s.transform;
        c_VirtualCamera.m_LookAt = null;
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

    private IEnumerator BugBannerAd()
    {
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(ChangeLevelTXT());
        ToggleBannerAd(true);

    }

    private void SaveScene()
    {
        if (SceneManager.GetActiveScene().name == "End")
        {
            JSON.instance.playerData.currentStage = 0;
            JSON.instance.SavePlayerDataToJson();
            return;
        }
        string currenScene = SceneManager.GetActiveScene().name;
        int num = Int32.Parse(currenScene);
        JSON.instance.playerData.currentStage = num;
        JSON.instance.SavePlayerDataToJson();
    }

}
