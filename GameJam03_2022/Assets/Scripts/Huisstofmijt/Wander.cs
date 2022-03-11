using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.AI;

public class Wander : ActionNode
{
    public Vector2 min = Vector2.one * -10;
    public Vector2 max = Vector2.one * 10;
    public float _directionSearchFreq = 5f;

    private NavMeshAgent _agent;
    private float _timer = 1000;
    private GameObject _dustToMoveTo;

    protected override void OnStart() 
    {
        _agent = context.gameObject.GetComponent<NavMeshAgent>();
        GameObject[] dusts = GameObject.FindGameObjectsWithTag("Dust");
        if (dusts.Length > 0 && dusts.Length != this.blackboard.dustsMovingTo.Count)
        { 
            _dustToMoveTo = dusts[Random.Range(0, dusts.Length - 1)];
            while (this.blackboard.dustsMovingTo.Contains(_dustToMoveTo))
            {
                _dustToMoveTo = dusts[Random.Range(0, dusts.Length - 1)];
            }
        }
    }

    protected override void OnStop() 
    {

    }

    //https://subscription.packtpub.com/book/game-development/9781783553570/1/ch01lvl1sec14/wandering-around
    protected override State OnUpdate() 
    {
        if (_dustToMoveTo == null)
        {
            _timer += Time.deltaTime;

            if (_timer >= _directionSearchFreq)
            {
                GameObject obj = context.gameObject;

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

                _timer = 0;
            }

            return State.Success;
        }
        else
        {
            NavMeshHit navHit;
            NavMesh.SamplePosition(_dustToMoveTo.transform.position, out navHit, 1000, -1);
            _agent.SetDestination(navHit.position);

            return State.Running;
        }
    }
}
