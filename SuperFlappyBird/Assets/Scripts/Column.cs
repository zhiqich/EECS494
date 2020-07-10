using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Bird>() != null)
        {
            if (other.CompareTag("Bird"))
            {
                GameControl.instance.BirdScored(1);
            }
            if (other.CompareTag("Bird2"))
            {
                GameControl.instance.BirdScored(2);
            }
        }
    }
}
