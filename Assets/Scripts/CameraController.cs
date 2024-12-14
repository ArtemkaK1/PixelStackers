using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 1f; // Скорость движения камеры
    public float verticalOffset = 0.5f; // Насколько камера поднимается вверх после установки блока
    public float zOffsetFactor = 0.25f; // Насколько изменяется позиция по Z с ростом башни
    private float targetYPosition; // Целевая позиция камеры по оси Y
    private float targetZPosition; // Целевая позиция камеры по оси Z

    void Start()
    {
        // Устанавливаем начальные целевые позиции камеры
        targetYPosition = transform.position.y;
        targetZPosition = transform.position.z;
    }

    void Update()
    {
        // Движение камеры к целевым позициям
        Vector3 targetPosition = new Vector3(transform.position.x, targetYPosition, targetZPosition);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraSpeed);
    }

    public void RaiseCamera()
    {
        // Поднимаем целевую позицию камеры по оси Y
        targetYPosition += verticalOffset;

        // Отдаляем камеру по оси Z
        targetZPosition -= zOffsetFactor;
    }
}
