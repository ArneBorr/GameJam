using UnityEngine;
using UnityEngine.AI;

public class Huisstofmijt : MonoBehaviour
{
    private Transform _child;
    private SpriteRenderer _visuals;
    private NavMeshAgent _agent;

    private void Start()
    {
        _visuals = GetComponentInChildren<SpriteRenderer>();
        _child = GetComponentInChildren<Transform>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _child.rotation = Quaternion.identity;
        _visuals.flipX = ;
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
