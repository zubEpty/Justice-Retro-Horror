using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;

public class KillerStefan : KillerBase
{
    public Transform player;
    private NavMeshAgent agent;
    private float detectionRadius = 10f;

    public override void OnEnable()
    {
        base.OnEnable();        
    }
    protected override void Start()
    {
        base.Start();
        //StateMachineIgnition(PatrolState);
        player = PlayerManager.Instance.player.transform;
    }

    protected override void Update()
    {
        base.Update();

        if (IsPlayerOnNavMesh(player.position, detectionRadius))
        {
            // Player is on NavMesh - chase them!
            agent.SetDestination(player.position);
        }
    }

    bool IsPlayerOnNavMesh(Vector3 targetPosition, float radius)
    {
        NavMeshHit hit;

        // Check if the target position is within the detection radius on the NavMesh
        if (NavMesh.SamplePosition(targetPosition, out hit, radius, NavMesh.AllAreas))
        {
            // Additional check to verify the point is actually on the NavMesh
            if (NavMesh.FindClosestEdge(hit.position, out hit, NavMesh.AllAreas))
            {
                return true;
            }
        }
        return false;
    }

    #region NavMesh


    public override void ExecuteNavMeshAction()
    {
        base.ExecuteNavMeshAction();
         
        if(Vector3.Distance(PatrolPoints[currentPatrolIndex].transform.position, transform.position) < 0.5f 
            || GetNavAgent.velocity.magnitude <= 0)
        {
            currentPatrolIndex++;

            if (currentPatrolIndex == PatrolPoints.Count)
                currentPatrolIndex = 0;

            GetNavAgent.destination = PatrolPoints[currentPatrolIndex].transform.position;
        }        
    }

    //public void GotoTargetPosition
    #endregion
}
