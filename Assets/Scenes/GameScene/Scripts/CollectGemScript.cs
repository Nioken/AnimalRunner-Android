using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectGemScript : MonoBehaviour
{
    private void Start()
    {
        transform.DORotate(new Vector3(0,360,0), 5,RotateMode.FastBeyond360).SetRelative(true).SetLoops(-1).SetEase(Ease.Linear);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.Gems++;
            GameManager._GameManager.GetComponent<AudioSource>().Play();
            GameManager._GameManager.UpdateGems();
            Destroy(gameObject);
        }
    }
}
