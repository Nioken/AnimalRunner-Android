using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInstantiate : MonoBehaviour
{
    private bool IsRotated = true;
    private GameObject PlayerPref;
    private Quaternion StartQuaternion = Quaternion.identity;
    private Quaternion SideQuaternion = Quaternion.Euler(0,90,0);
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        Debug.Log(transform.parent.tag);
        if (other.tag == "Player")
        {
            PlayerControl.IsMove = false;
            if (transform.parent.tag == "RotateBlockRight")
            {
                if (GameManager.IsRotatedLeft)
                {
                    GameManager.IsRotatedLeft = false;
                    GameManager.IsRotatedRight = false;
                }
                else
                {
                    GameManager.IsRotatedRight = true;
                }
                IsRotated = false;
                PlayerPref = other.gameObject;
                if (GameManager.IsRotatedRight)
                {
                    int RandomIndex = Random.Range(0, GameManager._GameManager.BlocksPrefabs.Count);
                    while (RandomIndex == 1)
                    {
                        RandomIndex = Random.Range(0, GameManager._GameManager.BlocksPrefabs.Count);
                    }
                    GameManager._GameManager.SpawnedBlocks.Add(Instantiate(GameManager._GameManager.BlocksPrefabs[RandomIndex], transform.parent.GetChild(1).position + new Vector3(5, -15, 0), Quaternion.Euler(0, 90, 0)));
                }
                if (!GameManager.IsRotatedLeft && !GameManager.IsRotatedRight)
                {
                    GameManager._GameManager.SpawnedBlocks.Add(Instantiate(GameManager._GameManager.BlocksPrefabs[Random.Range(0, GameManager._GameManager.BlocksPrefabs.Count)], transform.parent.GetChild(1).position + new Vector3(0, -15, 6), Quaternion.identity));
                }
            }
            if (transform.parent.tag == "RotateBlockLeft")
            {
                if (GameManager.IsRotatedRight)
                {
                    GameManager.IsRotatedLeft = false;
                    GameManager.IsRotatedRight = false;
                }
                else
                {
                GameManager.IsRotatedLeft = true;
                }
                IsRotated = false;
                PlayerPref = other.gameObject;
                if (GameManager.IsRotatedLeft)
                {
                    int RandomIndex = Random.Range(0, GameManager._GameManager.BlocksPrefabs.Count);
                    while (RandomIndex == 3)
                    {
                        RandomIndex = Random.Range(0, GameManager._GameManager.BlocksPrefabs.Count);
                    }
                    GameManager._GameManager.SpawnedBlocks.Add(Instantiate(GameManager._GameManager.BlocksPrefabs[RandomIndex], transform.parent.parent.GetChild(1).position + new Vector3(-5, -15, 0), Quaternion.Euler(0, -90, 0)));
                }
                if(!GameManager.IsRotatedLeft && !GameManager.IsRotatedRight)
                {
                    GameManager._GameManager.SpawnedBlocks.Add(Instantiate(GameManager._GameManager.BlocksPrefabs[Random.Range(0, GameManager._GameManager.BlocksPrefabs.Count)], transform.parent.position + new Vector3(0, -15, 6), Quaternion.identity));
                }
            }
            if (transform.parent.tag == "Block")
            {
                int RandomIndex = Random.Range(0, GameManager._GameManager.BlocksPrefabs.Count);
                if (!GameManager.IsRotatedRight && !GameManager.IsRotatedLeft)
                {
                    GameManager._GameManager.SpawnedBlocks.Add(Instantiate(GameManager._GameManager.BlocksPrefabs[Random.Range(0, GameManager._GameManager.BlocksPrefabs.Count)], transform.parent.position + new Vector3(0, -15, 6), Quaternion.identity));
                }
                else
                {
                    if (GameManager.IsRotatedRight)
                    {
                        while (RandomIndex == 1)
                        {
                            RandomIndex = Random.Range(0, GameManager._GameManager.BlocksPrefabs.Count);
                        }
                        GameManager._GameManager.SpawnedBlocks.Add(Instantiate(GameManager._GameManager.BlocksPrefabs[RandomIndex], transform.parent.GetChild(1).position + new Vector3(5, -15, 0), Quaternion.Euler(0, 90, 0)));
                    }
                    else
                    {
                        while (RandomIndex == 3)
                        {
                            RandomIndex = Random.Range(0, GameManager._GameManager.BlocksPrefabs.Count);
                        }
                        GameManager._GameManager.SpawnedBlocks.Add(Instantiate(GameManager._GameManager.BlocksPrefabs[RandomIndex], transform.parent.GetChild(1).position + new Vector3(-5, -15, 0), Quaternion.Euler(0, -90, 0)));
                    }
                }
            }
            transform.GetComponent<BoxCollider>().isTrigger = false;
            transform.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void Update()
    {
        if (!IsRotated)
        {
            if (GameManager.IsRotatedLeft || GameManager.IsRotatedRight)
            {
                if (GameManager.IsRotatedRight)
                {
                PlayerPref.transform.rotation = Quaternion.Lerp(PlayerPref.transform.rotation, SideQuaternion, 5 * Time.fixedDeltaTime);
                }
                if (GameManager.IsRotatedLeft)
                {
                    PlayerPref.transform.rotation = Quaternion.Lerp(PlayerPref.transform.rotation, Quaternion.Inverse(SideQuaternion), 5 * Time.fixedDeltaTime);
                }
            }
            if(!GameManager.IsRotatedRight && !GameManager.IsRotatedLeft)
            {
                PlayerPref.transform.rotation = Quaternion.Lerp(PlayerPref.transform.rotation, StartQuaternion, 5 * Time.fixedDeltaTime);
            }
        }
    }
}
