using UnityEngine;

public class SaturnDistance : MonoBehaviour
{
    // This is in order to make the planet look stationary in the sky
    
    [SerializeField] private GameObject player;
    private Vector3 currentDist;
    private Vector3 originalDist;
    private Vector3 diff;
    
    void Start()
    {
        originalDist = transform.position - player.transform.position;
    }

    void Update()
    {
        currentDist = transform.position - player.transform.position;
        diff = originalDist - currentDist;
        transform.position += diff;
    }
}
