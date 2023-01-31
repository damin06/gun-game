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


    [SerializeField] private bool isTest = true;
    public bool isSlowMotion = false;
    public static GameManager instance { get; private set; }

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
        fiextime = Time.fixedDeltaTime;
        notime = Time.timeScale;
    }

    void Update()
    {
        // Time.timeScale += (1f / 2) * Time.unscaledDeltaTime;
        // Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    public void NextLevel()
    {
        SotpSlowMotion();
        AD();
        currentLevelTXT.text = "LEVEL" + SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLEVEL()
    {
        SotpSlowMotion();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void AD()
    {
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        this.interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
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
}
