using UnityEngine;
using UnityEngine.AI;

public class Huisstofmijt : MonoBehaviour
{
    [SerializeField] private Vector2 min = Vector2.one * -10;
    [SerializeField] private Vector2 max = Vector2.one * 10;
    [SerializeField] private float _directionSearchFreq = 5f;

    private Transform _child;
    private SpriteRenderer _visuals;
    private NavMeshAgent _agent;

    private float _timer = 1000;
    private GameObject _dustToMoveTo;

    private void Start()
    {
        _visuals = GetComponentInChildren<SpriteRenderer>();
        _child = GetComponentInChildren<Transform>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _child.rotation = Quaternion.identity;
        _visuals.flipX = _agent.velocity.x > 0;

        if (_dustToMoveTo == null)
        {
            _timer += Time.deltaTime;

            if (_timer >= _directionSearchFreq)
            {
                // Search Dust and move to
                GameObject[] dusts = GameObject.FindGameObjectsWithTag("Dust");
                if (dusts.Length > 0)
                {
                    _dustToMoveTo = dusts[Random.Range(0, dusts.Length - 1)];
                    NavMeshHit navHit;
                    NavMesh.SamplePosition(_dustToMoveTo.transform.position, out navHit, 1000, -1);
                    _agent.SetDestination(navHit.position);
                }
                // Wander
                else
                {
                    GameObject obj = this.gameObject;

                    Vector3 sphereCenter = obj.transform.position + obj.transform.forward * 2;
                    Vector3 circlePos = Random.insideUnitSphere * 2f;
                    circlePos.y = 0;

                    Vector3 newDestination = sphereCenter + circlePos;

                    if (newDestination.x > max.x || newDestination.x < min.x)
                        newDestination.x = 0;
                    if (newDestination.z > max.y || newDestination.x < min.y)
                        newDestination.z = 0;

                    NavMeshHit navHit;
                    NavMesh.SamplePosition(newDestination, out navHit, 1000, -1);

                    _agent.SetDestination(navHit.position);
                }

                _timer = 0;
            }
        }
    }

    public void ReachedDust()
    {
        _dustToMoveTo = null;
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
