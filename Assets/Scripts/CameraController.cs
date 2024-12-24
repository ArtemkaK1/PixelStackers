using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 1f;
    public float verticalOffset = 0.5f;
    private float targetYPosition;

    void Start()
    {
        targetYPosition = transform.position.y;
    }

    void Update()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, targetYPosition, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraSpeed);
    }

    public void RaiseCamera()
    {
        targetYPosition += verticalOffset;
    }
}
