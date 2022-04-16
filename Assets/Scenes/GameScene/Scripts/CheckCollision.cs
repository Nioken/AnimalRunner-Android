using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Coll " + other.tag);
        if (other.tag == "Block")
        {
            Debug.Log("Dead");
            PlayerControl.IsDead = true;
        }
    }
}
