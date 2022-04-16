using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    public float MoveSpeed;
    public static bool IsMove = false;
    public static bool IsDead = false;
    Animator Anim;
    [SerializeField]
    private AudioClip DeadClip;
    [SerializeField]
    private GameObject DeathPanel;
    private bool StopCorutine = false;

    void Start()
    {
        Anim = GetComponent<Animator>();
        StartCoroutine(WaitSeconds());
    }
    void Update()
    {
        MoveForward();
        //CheckRayCheckCollision();
    }

    public void MoveForward()
    {
        if (IsMove)
        {
            transform.Translate(Vector3.forward * MoveSpeed * GameManager.GameSpeedMultipluer/1.5f * Time.deltaTime);
            Anim.SetInteger("Walk", 1);
        }
        else
        {
            Anim.SetInteger("Walk", 0);
        }
    }

    public void CheckRayCheckCollision()
    {
        Debug.DrawRay(gameObject.transform.position + new Vector3(0, 0.5f, 0), Vector3.down);
        if (!Physics.Raycast(gameObject.transform.position + new Vector3(0,0.5f,0), Vector3.down))
        {
            IsDead = true;
        }
        if (IsDead)
        {
            StopCorutine = true;
            IsMove = false;
            Debug.Log(gameObject.transform.childCount - 1);
            gameObject.transform.GetChild(gameObject.transform.childCount-1).parent = null;
            DeathPanel.SetActive(true);
            if(GameManager.Score > PlayerPrefs.GetInt("RecordScore"))
            {
                PlayerPrefs.SetInt("RecordScore", GameManager.Score);
                PlayerPrefs.Save();
                DeathPanel.transform.GetChild(4).GetComponent<TMP_Text>().text = "Record: " + PlayerPrefs.GetInt("RecordScore").ToString() + "\n NEW RECORD!";
            }
            else
            {
                DeathPanel.transform.GetChild(4).GetComponent<TMP_Text>().text = "Record: " + PlayerPrefs.GetInt("RecordScore").ToString();
            }
            PlayerPrefs.SetInt("Gems", PlayerPrefs.GetInt("Gems") + GameManager.Gems);
            PlayerPrefs.Save();
            //Debug.Log(PlayerPrefs.GetInt("Gems"));
            gameObject.transform.DOScale(0, 1).OnComplete(()=>DeathPanel.GetComponent<RectTransform>().DOScale(new Vector3(5.6f,7.5f,4.3f), 0.5f));
            GameManager._GameManager.GetComponent<AudioSource>().PlayOneShot(DeadClip);
            return;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        DeathPanel.SetActive(false);
        IsDead = false;
        GameManager.IsRotatedLeft = false;
        GameManager.IsRotatedRight = false;
        GameManager.Gems = 0;
        GameManager.Score = 0;
    }
    public void ToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    IEnumerator WaitSeconds()
    {
        while (true)
        {
            if (!StopCorutine)
            {
                CheckRayCheckCollision();
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                break;
            }
        }
    }
}
