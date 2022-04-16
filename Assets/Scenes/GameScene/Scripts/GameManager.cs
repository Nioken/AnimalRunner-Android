using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _GameManager;
    public List<GameObject> PlayersPrefabs;
    public Canvas UI;
    public GameObject DeathPanel;
    public Button ToMenuButton;
    public Button RestartButton;
    [SerializeField]
    private Image TapFinger;
    [SerializeField]
    private TMP_Text TapText;
    private bool IsStarted = false;
    [SerializeField]
    private GameObject FirstBlockAnchor;
    [SerializeField]
    private GameObject StartBlockAnchor;
    [SerializeField]
    public List<GameObject> BlocksPrefabs = new List<GameObject>();
    public List<GameObject> SpawnedBlocks = new List<GameObject>();
    public static bool IsRotatedRight = false;
    public static bool IsRotatedLeft = false;
    public static int Score = 0;
    public static int Gems = 0;
    [SerializeField]
    private TMP_Text ScoreText;
    [SerializeField]
    private TMP_Text GemsText;
    [SerializeField]
    private TMP_Text ComboText;
    public static float GameSpeedMultipluer;
    public static int comboCounter;

    private void Start()
    {
        switch (MenuManager.nowCharacter)
        {
            case 0:
                Debug.Log("Penguin");
                for (int i = 0; i < PlayersPrefabs.Count; i++)
                {
                    if(i == MenuManager.nowCharacter)
                    {
                        
                        PlayersPrefabs[i].SetActive(true);
                        UI.worldCamera = PlayersPrefabs[i].transform.GetChild(1).GetComponent<Camera>();
                        Debug.Log(PlayersPrefabs[i].transform.GetChild(1).name);
                       
                    }
                    else
                    {
                        Destroy(PlayersPrefabs[i]);
                    }
                }
                break;
                case 1:
                Debug.Log("Dog");
                for (int i = 0; i < PlayersPrefabs.Count; i++)
                {
                    if (i == MenuManager.nowCharacter)
                    {
                        PlayersPrefabs[i].SetActive(true);
                        UI.worldCamera = PlayersPrefabs[i].transform.GetChild(8).GetComponent<Camera>();
                        //RestartButton.onClick.AddListener(() => PlayersPrefabs[i].GetComponent<PlayerControl>().RestartGame());
                        //ToMenuButton.onClick.AddListener(() => PlayersPrefabs[i].GetComponent<PlayerControl>().ToMenu());
                    }
                    else
                    {
                        Destroy(PlayersPrefabs[i]);
                    }
                }
                break;
            case 2:
                Debug.Log("Cat");
                for (int i = 0; i < PlayersPrefabs.Count; i++)
                {
                    if (i == MenuManager.nowCharacter)
                    {
                        PlayersPrefabs[i].SetActive(true);
                        UI.worldCamera = PlayersPrefabs[i].transform.GetChild(6).GetComponent<Camera>();
                        //RestartButton.onClick.AddListener(() => PlayersPrefabs[i].GetComponent<PlayerControl>().RestartGame());
                        //ToMenuButton.onClick.AddListener(() => PlayersPrefabs[i].GetComponent<PlayerControl>().ToMenu());
                    }
                    else
                    {
                        Destroy(PlayersPrefabs[i]);
                    }
                }
                break;
        }
        GameSpeedMultipluer = 1;
        _GameManager = this;
        Application.targetFrameRate = 60;
        TapFinger.rectTransform.DOScale(1.5f, 0.5f).OnComplete(() => TapFinger.rectTransform.DOScale(1.3f, 0.5f)).SetLoops(-1).SetEase(Ease.Linear);
        TapText.rectTransform.DOScale(3f, 0.5f).OnComplete(() => TapText.rectTransform.DOScale(2.8f, 0.5f)).SetLoops(-1).SetEase(Ease.Linear);
    }

    void Update()
    {
        //andoroid
        if (!IsStarted)
            if (Input.touches.Length == 1)
                if (Input.touches[0].phase == TouchPhase.Ended)
                    StartGame();
        if (IsStarted)
            FirstBlockAnchor.transform.position = Vector3.Lerp(FirstBlockAnchor.transform.position, StartBlockAnchor.transform.position, 7 * Time.fixedDeltaTime);
        //pc
        //if (!IsStarted)
        //    if (Input.GetMouseButtonDown(0))
        //        StartGame();
        //if (IsStarted)
        //    FirstBlockAnchor.transform.position = Vector3.Lerp(FirstBlockAnchor.transform.position, StartBlockAnchor.transform.position, 7 * Time.fixedDeltaTime);

    }

    private void StartGame()
    {
        TapFinger.rectTransform.DOScale(0f, 1).OnComplete(() => TapFinger.gameObject.SetActive(false));
        TapText.rectTransform.DOScale(0f, 1).OnComplete(() => TapText.gameObject.SetActive(false));
        IsStarted = true;
        PlayerControl.IsMove = true;
    }
    public void UnpdateScore(int _Score)
    {
        Score += _Score;
        GameSpeedMultipluer += (float)_Score/100;
        Debug.Log("GameManager: " + GameSpeedMultipluer);
        Debug.Log("GameManager: " + (float)_Score / 100);
        ScoreText.text = "Score: " + Score.ToString();
        ScoreText.gameObject.transform.DOScale(1.35f,0.15f).OnComplete(()=> ScoreText.gameObject.transform.DOScale(1f, 0.15f));
    }
    public void UpdateGems()
    {
        GemsText.text = Gems.ToString();
        GemsText.gameObject.transform.DOScale(1.35f, 0.15f).OnComplete(() => GemsText.gameObject.transform.DOScale(1f, 0.15f));
    }
    public void UpdateCombo()
    {
        comboCounter ++;
        ComboText.text = "Combo X" + comboCounter.ToString();
        ComboText.GetComponent<RectTransform>().DOLocalRotate(new Vector3(0, 0, UnityEngine.Random.Range(1, 30)), 0.1f,RotateMode.Fast);
        ComboText.GetComponent<RectTransform>().DOScale(3, 0.1f).OnComplete(()=>StartCoroutine(WaitToHideCombo()));
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        DeathPanel.SetActive(false);
        PlayerControl.IsDead = false;
        IsRotatedLeft = false;
        IsRotatedRight = false;
        Gems = 0;
        Score = 0;
    }
    public void ToMenu()
    {
        PlayerControl.IsDead = false;
        IsRotatedLeft = false;
        IsRotatedRight = false;
        Gems = 0;
        Score = 0;
        SceneManager.LoadScene("MenuScene");
    }

    IEnumerator WaitToHideCombo()
    {
        yield return new WaitForSeconds(1);
        ComboText.GetComponent<RectTransform>().DOLocalRotate(new Vector3(0, 0, 0), 0.1f, RotateMode.Fast);
        ComboText.GetComponent<RectTransform>().DOScale(0, 0.1f);
    }
}
