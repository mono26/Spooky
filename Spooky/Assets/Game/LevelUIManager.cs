using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager
{
    public GameObject topUI;
    public GameObject bottomUI;

    public Image cropUIBar;
    public Image gameWaveBar;
    public Text gameMoneyText;
    public int startingMoney = 400;
    public int currentMoney;
    public int maxCrop = 800;
    public float currentCrop;

    public LevelUIManager(Image _cropUIBar, Image _waveBar, Text _gameMoneyText, GameObject _topUi, GameObject _bottomUi)
    {
        topUI = _topUi;
        bottomUI = _bottomUi;
        cropUIBar = _cropUIBar;
        gameWaveBar = _waveBar;
        gameMoneyText = _gameMoneyText;

        HideUI();
        GameManager.Instance.StartGame += ShowUI;
    }

    public void OnDisable()
    {
        WaveSpawner.Instance.SpawnStart -= IncreaseWave;
    }

    public void Start ()
    {
        WaveSpawner.Instance.SpawnStart += IncreaseWave;

        currentCrop = maxCrop;
        currentMoney = startingMoney;
        cropUIBar.fillAmount = currentCrop / maxCrop;
        gameMoneyText.text = "" + currentMoney;

        IncreaseWave();
    }

    public void LoseCrop(int _stole)
    {
        currentCrop -= _stole;
        cropUIBar.fillAmount = currentCrop / maxCrop;
        if (currentCrop <= 0)
        {
            //GameOver Code
            //GameOver();
        }
    }

    public void GiveMoney(int reward)
    {
        currentMoney += reward;
        gameMoneyText.text = "$:" + currentMoney;
    }

    public void TakeMoney(int money)
    {
        currentMoney -= money;
        gameMoneyText.text = "$:" + currentMoney;
    }

    private void HideUI()
    {
        topUI.SetActive(false);
        bottomUI.SetActive(false);
    }

    private void ShowUI()
    {
        topUI.SetActive(true);
        bottomUI.SetActive(true);
    }

    private void IncreaseWave()
    {
        gameWaveBar.fillAmount = (float)(WaveSpawner.Instance.nextWave) / (float)(WaveSpawner.Instance.waves.Length);
    }
}
