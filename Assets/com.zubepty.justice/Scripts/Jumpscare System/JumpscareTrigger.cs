using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    [Header("Jumpscare Setup")]
    public GameObject[] jumpscarePrefabs; // List of scary models
    public Transform spawnPoint;
    public AudioClip scareSound;
    public float destroyAfterSeconds = 3f;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player")) // Make sure your player has "Player" tag
        {
            hasTriggered = true;
            TriggerJumpscare();
        }
    }

    [ContextMenu("Trigger Jumpscare")]
    public void TriggerJumpscare()
    {
        // Spawn a random scary model
        int index = Random.Range(0, jumpscarePrefabs.Length);
        GameObject scare = Instantiate(jumpscarePrefabs[index], spawnPoint.position, spawnPoint.rotation);

        // Play sound
        AudioSource.PlayClipAtPoint(scareSound, spawnPoint.position);

        // Optionally destroy after delay
        Destroy(scare, destroyAfterSeconds);
    }
}
