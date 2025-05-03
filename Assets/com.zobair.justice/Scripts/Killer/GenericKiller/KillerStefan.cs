using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;

public class KillerStefan : KillerBase
{
    public Transform player;
    private NavMeshAgent agent;
   [SerializeField] private float detectionRadius = 5f;

    public override void OnEnable()
    {
        base.OnEnable();        
    }
    protected override void Start()
    {
        base.Start();
        StateMachineIgnition(IdleState);
        player = PlayerManager.Instance.player.transform;
    }

    protected override void Update()
    {
        base.Update();        
    }

    [ContextMenu("Start chasing")]
    public void SpawnKillerState()
    {
        KillerStateMachine.ChangeState(PatrolState);
    }

    #region NavMesh


    public override void ExecuteNavMeshAction()
    {
        base.ExecuteNavMeshAction();

        CheckForPlayer();

        if (Vector3.Distance(PatrolPoints[currentPatrolIndex].transform.position, transform.position) < 0.5f 
            || GetNavAgent.velocity.magnitude <= 0)
        {
            currentPatrolIndex++;

            if (currentPatrolIndex == PatrolPoints.Count)
                currentPatrolIndex = 0;

            GetNavAgent.destination = PatrolPoints[currentPatrolIndex].transform.position;
        }        
    }

    public void CheckForPlayer()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(player.position, out hit, detectionRadius, NavMesh.AllAreas))
        {
            KillerStateMachine.ChangeState(CaughtPlayerState_);

            Debug.Log("Player is on the NavMesh.");            
        }
        else
        {
            Debug.Log("Player is NOT on the NavMesh.");
        }
    }
    #endregion


}
