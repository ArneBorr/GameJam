using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Can : MonoBehaviour
{
    [SerializeField] private int _dustToTakeOffPlayer = 3;

    private Rigidbody _rb = null;
    public bool _beingSucked = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_beingSucked && _rb.velocity.sqrMagnitude <= 0.5f)
        {
            _rb.drag = 10000;
            _rb.angularDrag = 1000;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_rb.velocity.magnitude > 1 && Vector3.Dot(_rb.velocity, this.transform.position - other.transform.position) > 0)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Player>().TakeDustOff(_dustToTakeOffPlayer);
            }
            else if (other.tag == "Dust")
            {
                Destroy(other.gameObject);
            }
            else if (other.tag == "Huisstofmijt")
            {
                Destroy(other.gameObject);
            }
        }     
    }
}
