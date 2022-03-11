using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningProjectile : MonoBehaviour
{
    [SerializeField] private float _movementspeed = 0.5f;
    [SerializeField] private float _lifeTime = 5f;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(this.transform.forward * _movementspeed);
        Destroy(gameObject, _lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player p =other.gameObject.GetComponent<Player>();
            p.TakeDustOff(1);
            Destroy(this.gameObject);
        }
    }
}
