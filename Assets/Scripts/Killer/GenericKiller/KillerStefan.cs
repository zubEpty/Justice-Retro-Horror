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
        StateMachineIgnition(CaughtPlayerState_);
    }


    #region NavMesh
    Vector3 GetRandomPositionFromNavMesh()
    {
        // Get NavMesh data
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        // Pick a random triangle
        int triangleIndex = Random.Range(0, navMeshData.indices.Length / 3);
        int vertexIndex = triangleIndex * 3;

        // Get the vertices of the triangle
        Vector3 point1 = navMeshData.vertices[navMeshData.indices[vertexIndex]];
        Vector3 point2 = navMeshData.vertices[navMeshData.indices[vertexIndex + 1]];
        Vector3 point3 = navMeshData.vertices[navMeshData.indices[vertexIndex + 2]];

        // Calculate a random point on the triangle
        float r1 = Random.value;
        float r2 = Random.value;

        return Vector3.Lerp(
            Vector3.Lerp(point1, point2, r1),
            point3,
            r2
        );
    }
    
    public override void ExecuteNavMeshAction()
    {
        base.ExecuteNavMeshAction();

        if (GetNavAgent.velocity.magnitude <= 0)
        {
            GetNavAgent.destination = GetRandomPositionFromNavMesh();// GetPatrolPoints[Random.RandomRange(0, GetPatrolPoints.Count - 1)].transform.position;
        }
    }
    #endregion
}
