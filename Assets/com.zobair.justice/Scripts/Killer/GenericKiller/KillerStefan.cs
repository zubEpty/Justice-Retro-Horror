using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;

public class KillerStefan : KillerBase
{
    

    public override void OnEnable()
    {
        base.OnEnable();        
    }
    protected override void Start()
    {
        base.Start();
        StateMachineIgnition(PatrolState);         
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
