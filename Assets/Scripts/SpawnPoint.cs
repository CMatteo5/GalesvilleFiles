using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool playerStartsLookingRight;
    private void Start()
    {
        
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 0.8f);
    }
}