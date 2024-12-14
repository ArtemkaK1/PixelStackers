using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 1f; // Скорость подъёма камеры
    public float verticalOffset = 1f; // Насколько камера поднимается вверх после установки блока
    private float targetYPosition; // Целевая позиция камеры по оси Y

    void Start()
    {
        // Устанавливаем начальную целевую позицию камеры
        targetYPosition = transform.position.y;
    }

    void Update()
    {
        // Движение камеры к целевой позиции по Y
        Vector3 targetPosition = new Vector3(transform.position.x, targetYPosition, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraSpeed);
    }

    public void RaiseCamera()
    {
        // Поднимаем целевую позицию камеры
        targetYPosition += verticalOffset;
    }
}
