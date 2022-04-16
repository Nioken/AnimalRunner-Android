using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{

    private bool IsUpper = false;
    [SerializeField]
    private Vector3 maxOffsetX = Vector3.right * 3;
    [SerializeField]
    private Vector3 MinOffsetX = Vector3.left * -3;
    [SerializeField]
    private Vector3 MaxOffsetZ = Vector3.forward * 3;
    [SerializeField]
    private Vector3 MinOffsetZ = Vector3.back * -3;
    private bool IsLefted = false;
    private bool IsToched = false;
    [SerializeField]
    private Transform PrevBlock;
    [SerializeField]
    private List<AudioClip> BlockAudios = new List<AudioClip>();
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PrevBlock = GameManager._GameManager.SpawnedBlocks[GameManager._GameManager.SpawnedBlocks.Count - 2].transform.GetChild(1);
        maxOffsetX = transform.position + maxOffsetX;
        MinOffsetX = transform.position - MinOffsetX;
        MaxOffsetZ = transform.position + MaxOffsetZ;
        MinOffsetZ = transform.position - MinOffsetZ;
        audioSource.PlayOneShot(BlockAudios[Random.Range(4, 5)]);
    }

    void Update()
    {
        MoveBlock();
        if (IsToched)
        {
            AttachBlock();
        }
    }

    void MoveBlock()
    {
        
        if (!IsUpper)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 0, transform.position.z), 12 * GameManager.GameSpeedMultipluer * Time.fixedDeltaTime);
            if (Vector3.Distance(transform.position, new Vector3(transform.position.x, 0, transform.position.z)) < 0.1)
            {
                IsUpper = true;
            }
        }
        else
        {
            if (!IsToched)
            {
                if (!IsLefted)
                {
                    transform.Translate(Vector3.left * 4 * GameManager.GameSpeedMultipluer * Time.fixedDeltaTime);
                    if (!GameManager.IsRotatedRight && !GameManager.IsRotatedLeft)
                    {
                        if (transform.position.x <= MinOffsetX.x)
                        {
                            IsLefted = true;
                        }
                    }
                    if (GameManager.IsRotatedRight)
                    {
                        if (transform.position.z >= MaxOffsetZ.z)
                        {
                            IsLefted = true;
                        }
                    }
                    if (GameManager.IsRotatedLeft)
                    {
                        if (transform.position.z <= MinOffsetZ.z)
                        {
                            IsLefted = true;
                        }
                    }
                }
                if (IsLefted)
                {
                    transform.Translate(Vector3.right * 4 * GameManager.GameSpeedMultipluer * Time.fixedDeltaTime);
                    if (!GameManager.IsRotatedRight && !GameManager.IsRotatedLeft)
                    {
                        if (transform.position.x >= maxOffsetX.x)
                        {
                            IsLefted = false;
                        }
                    }
                    if (GameManager.IsRotatedRight)
                    {
                        if (transform.position.z <= MinOffsetZ.z)
                        {
                            IsLefted = false;
                        }
                    }
                    if (GameManager.IsRotatedLeft)
                    {
                        if (transform.position.z >= MaxOffsetZ.z)
                        {
                            IsLefted = false;
                        }
                    }
                }

                if (Input.touches.Length == 1)
                    if (Input.touches[0].phase == TouchPhase.Began)
                    {
                        IsToched = true;
                        audioSource.PlayOneShot(BlockAudios[Random.Range(0, 3)]);
                    }


                //if (Input.GetMouseButtonDown(0))
                //{
                //    IsToched = true;
                //    audioSource.PlayOneShot(BlockAudios[Random.Range(0, 3)]);
                //}
            }
        }
    }

    void AttachBlock()
    {
        if (!GameManager.IsRotatedRight && !GameManager.IsRotatedLeft)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 0, PrevBlock.position.z), 8 * Time.fixedDeltaTime);
            if (transform.position.z <= PrevBlock.position.z + 0.01f)
            {
                Debug.Log(FindDelta(transform.position.x, PrevBlock.position.x));
                if(FindDelta(transform.position.x, PrevBlock.position.x) < 0.085f)
                {
                    transform.position = new Vector3(PrevBlock.transform.position.x, 0, PrevBlock.position.z);
                    audioSource.PlayOneShot(BlockAudios[6]);
                    GameManager._GameManager.UnpdateScore(5);
                    GameManager._GameManager.UpdateCombo();
                }
                else
                {
                    GameManager._GameManager.UnpdateScore(3);
                    GameManager.comboCounter = 0;
                }
                this.enabled = false;
            }
        }
        if (GameManager.IsRotatedRight)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(PrevBlock.position.x, 0, transform.position.z), 8 * Time.fixedDeltaTime);
            if (transform.position.x <= PrevBlock.position.x + 0.01f)
            {
                Debug.Log(FindDelta(transform.position.z, PrevBlock.position.z));
                if (FindDelta(transform.position.z, PrevBlock.position.z) < 0.085f)
                {
                    transform.position = new Vector3(PrevBlock.transform.position.x, 0, PrevBlock.position.z);
                    audioSource.PlayOneShot(BlockAudios[6]);
                    GameManager._GameManager.UnpdateScore(5);
                    GameManager._GameManager.UpdateCombo();
                }
                else
                {
                    GameManager.comboCounter = 0;
                    GameManager._GameManager.UnpdateScore(3);
                }
                this.enabled = false;
            }
        }
        if (GameManager.IsRotatedLeft)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(PrevBlock.position.x, 0, transform.position.z), 8 * Time.fixedDeltaTime);
            if (transform.position.x >= PrevBlock.position.x - 0.01f)
            {
                Debug.Log(FindDelta(transform.position.z, PrevBlock.position.z));
                if (FindDelta(transform.position.z, PrevBlock.position.z) < 0.085f)
                {
                    transform.position = new Vector3(PrevBlock.transform.position.x, 0, PrevBlock.position.z);
                    audioSource.PlayOneShot(BlockAudios[6]);
                    GameManager._GameManager.UnpdateScore(5);
                    GameManager._GameManager.UpdateCombo();
                }
                else
                {
                    GameManager.comboCounter = 0;
                    GameManager._GameManager.UnpdateScore(3);
                }
                this.enabled = false;
            }
        }
        PlayerControl.IsMove = true;
    }

    float FindDelta(float x, float y)
    {
        if(x > y)
        {
            return x - y;
        }
        else
        {
            return y - x;
        }
    }
}
