using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Huisstofmijt : MonoBehaviour
{
    private Transform _child;
    private SpriteRenderer _visuals;

    private void Start()
    {
        _visuals = GetComponentInChildren<SpriteRenderer>();
        _child = GetComponentInChildren<Transform>();
    }

    private void Update()
    {
        _child.rotation = Quaternion.identity;
        _visuals.flipX = _rb.transform.forward.x > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player p = other.gameObject.GetComponent<Player>();
            p.TakeDustOff(3);
        }
    }
}
