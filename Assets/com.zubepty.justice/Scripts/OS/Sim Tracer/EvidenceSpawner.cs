using UnityEngine;

public class EvidenceSpawner : MonoBehaviour
{
    public GameObject evidencePrefab;
    public Transform spawnArea;

    public void SpawnEvidence(string referenceID)
    {
        GameObject go = Instantiate(evidencePrefab, spawnArea);
        go.GetComponent<EvidenceItem>().referenceID = referenceID;
    }
}
