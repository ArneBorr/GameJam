using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Huisstofmijt : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player p = other.gameObject.GetComponent<Player>();
            p.TakeDustOff(3);
        }
    }
}
