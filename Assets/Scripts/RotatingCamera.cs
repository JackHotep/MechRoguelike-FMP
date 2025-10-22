using UnityEngine;

public class RotatingCamera : MonoBehaviour
{
    public float rotateSpeed = 5f;
    
    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
