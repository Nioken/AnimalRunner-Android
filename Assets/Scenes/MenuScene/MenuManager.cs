using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text RecordText;
    [SerializeField]
    private TMP_Text GemsText;
    public static int nowCharacter = 1;
    [SerializeField]
    private GameObject Characters;
    [SerializeField]
    private Button LeftArrow;
    [SerializeField]
    private Button RightArrow;
    [SerializeField]
    private Button PlayButton;
    public List<GameObject> pgObjects;
    public GameObject pgUI;

    public List<GameObject> catObjects;
    public GameObject catUI;
    public Material normalMat;


    public void UpdateStats()
    {
        RecordText.text = "your record: " + PlayerPrefs.GetInt("RecordScore");
        GemsText.text = PlayerPrefs.GetInt("Gems").ToString();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        UpdateStats();
        PlayerPrefs.GetInt("CatUnlocked");
        PlayerPrefs.GetInt("PinguinUnlocked");
        nowCharacter = 1;
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void NextCharecter()
    {
        Characters.transform.DOMoveX(Characters.transform.position.x + 6.78f, 1f);
        nowCharacter++;
        if(nowCharacter > 1)
        {
            RightArrow.interactable = false;
        }
        if (nowCharacter == 1)
        {
            LeftArrow.interactable = true;
        }
        Debug.Log(nowCharacter);
        CheckUnlock();
    }
    public void PrevCharecter()
    {
        nowCharacter--;
        Characters.transform.DOMoveX(Characters.transform.position.x - 6.78f, 1f);
        if (nowCharacter > 1)
        {
            RightArrow.interactable = false;
        }
        if(nowCharacter == 1)
        {
            RightArrow.interactable = true;
        }
        if (nowCharacter < 1)
        {
            LeftArrow.interactable = false;
        }
        Debug.Log(nowCharacter);
        CheckUnlock();
    }
    public void CheckUnlock()
    {
                if(PlayerPrefs.GetInt("RecordScore") >= 200)
                {
                    PlayerPrefs.SetInt("PinguinUnlocked", 1);
            PlayerPrefs.Save();
                }
        if (PlayerPrefs.GetInt("RecordScore") >= 300)
        {
            PlayerPrefs.SetInt("CatUnlocked", 1);
            PlayerPrefs.Save();
        }
        if (nowCharacter == 0)
        {
            Debug.Log("PG Unlocked: " + PlayerPrefs.GetInt("PinguinUnlocked"));
            if(PlayerPrefs.GetInt("PinguinUnlocked") != 1)
            {
                PlayButton.interactable = false;
                pgUI.GetComponent<RectTransform>().DOScale(1, 1);
            }
            else
            {
                for(int i = 0; i < pgObjects.Count; i++)
                {
                    pgObjects[i].GetComponent<Renderer>().material = normalMat;
                }

                    pgUI.SetActive(false);
                PlayButton.interactable = true;
            }
        }
        
        if(nowCharacter == 1)
        {
            pgUI.GetComponent<RectTransform>().DOScale(0, 1);
            catUI.GetComponent<RectTransform>().DOScale(0, 1);
            PlayButton.interactable = true;
        }

        if (nowCharacter == 2)
        {
            Debug.Log(PlayerPrefs.GetInt("CatUnlocked"));
            if (PlayerPrefs.GetInt("CatUnlocked") != 1)
            {
                PlayButton.interactable = false;
                catUI.GetComponent<RectTransform>().DOScale(1, 1);
            }
            else
            {
                for (int i = 0; i < catObjects.Count; i++)
                {
                    catObjects[i].GetComponent<Renderer>().material = normalMat;
                }
                    catUI.SetActive(false);
                PlayButton.interactable = true;
            }
        }
    }

    public void UnlockPenguin()
    {
        if(PlayerPrefs.GetInt("Gems") >= 400)
        {
            PlayerPrefs.SetInt("PinguinUnlocked", 1);
            PlayerPrefs.Save();
            CheckUnlock();
            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") - 400);
            PlayerPrefs.Save();
            UpdateStats();
        }
    }

    public void UnlockCat()
    {
        if (PlayerPrefs.GetInt("Gems") >= 600)
        {
            PlayerPrefs.SetInt("CatUnlocked", 1);
            PlayerPrefs.Save();
            CheckUnlock();
            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") - 400);
            PlayerPrefs.Save();
            UpdateStats();
        }
    }
}
