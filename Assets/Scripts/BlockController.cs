using UnityEngine;

public class BlockController : MonoBehaviour
{
    public float speed = 5f; // Скорость движения блока
    private bool isMoving = true; // Флаг движения
    private Vector3 direction; // Направление движения блока
    private Rigidbody rb; // Компонент Rigidbody

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Физика отключена до остановки блока

        float cameraWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        float spawnX = Random.value > 0.5f ? cameraWidth + 5f : -cameraWidth - 5f;
        float spawnZ = 10f;
        float spawnY = 5f;

        transform.position = new Vector3(spawnX, spawnY, spawnZ);

        Vector3 lookAtPosition = new Vector3(0f, spawnY, 0f);
        transform.LookAt(lookAtPosition);

        direction = (lookAtPosition - transform.position).normalized;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isMoving)
        {
            isMoving = false;
            rb.isKinematic = false; // Включаем физику
            this.enabled = false; // Отключаем скрипт после остановки
        }
    }
}
