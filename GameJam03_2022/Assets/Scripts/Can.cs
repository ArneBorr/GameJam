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

        Debug.DrawLine(_rb.transform.position, _rb.velocity * 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        float mag = _rb.velocity.magnitude;

        Vector2 rbVel = new Vector2(_rb.velocity.x, _rb.velocity.z);
        Vector2 dir = new Vector2(this.transform.position.x - other.transform.position.x, this.transform.position.z - other.transform.position.z);

        if ((mag > 0.5f && Vector2.Dot(rbVel, dir) > 0) || mag > 1.5f)
        {
            if (other.tag == "Player")
            {
                Debug.Log("ER");
                other.GetComponent<Player>().TakeDustOff(_dustToTakeOffPlayer);
            }
            else if (other.tag == "Dust")
            {
                Destroy(other.gameObject);
            }
            else if (other.tag == "Huisstofmijt")
            {
                GameObject.FindObjectOfType<HuisstofmijtManager>().SpawnMijt();
                Destroy(other.gameObject);
            }
        }     
    }
}
