using UnityEngine;

public class BlockController : MonoBehaviour
{
    public float speed = 5f; // Скорость движения блока
    private bool isMoving = true; // Флаг движения
    private Vector3 direction; // Направление движения блока
    private Rigidbody rb; // Компонент Rigidbody
    private bool hasLanded = false; // Флаг, если блок "пришел на место"
    private bool hasSupport = false; // Флаг наличия устойчивой опоры
    private bool hasRaisedCamera = false; // Флаг, чтобы камера поднималась только один раз
    private static bool isFirstBlock = true; // Проверка первого блока

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Физика отключена до остановки блока

        float cameraWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        float spawnX = Random.value > 0.5f ? cameraWidth + 5f : -cameraWidth - 5f; // Левый или правый край
        float spawnZ = 12f; // Появление за башней
        float spawnY = TowerHeightManager.Instance.CurrentTowerHeight + 2f; // Выше текущей башни

        transform.position = new Vector3(spawnX, spawnY, spawnZ);

        // Устанавливаем направление движения блока
        float angle = spawnX > 0 ? -135f : 135f; // Угол зависит от стороны появления
        direction = Quaternion.Euler(0, angle, 0) * Vector3.forward; // Направление движения
    }

    void Update()
    {
        if (isMoving)
        {
            // Движение блока по направлению
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isMoving)
        {
            // Останавливаем блок
            isMoving = false;
            rb.isKinematic = false; // Включаем физику
        }

        // Если блок падает за пределы сцены
        if (transform.position.y < -10f)
        {
            DestroyBlock(); // Удаляем блок
        }
    }

    void FixedUpdate()
    {
        // Проверяем устойчивость после остановки, если она ещё не была проверена
        if (!isMoving && hasLanded && !hasSupport && !hasRaisedCamera)
        {
            hasRaisedCamera = true; // Помечаем, что камера поднята
            DestroyBlock(); // Удаляем блок, если нет поддержки
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Проверяем, если блок соприкоснулся с другим объектом
        if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Base"))
        {
            hasLanded = true; // Блок завершил движение

            // Если блок касается базы
            if (collision.gameObject.CompareTag("Base"))
            {
                if (isFirstBlock)
                {
                    // Если это первый блок, он остаётся на базе
                    hasSupport = true;
                    isFirstBlock = false; // Сбрасываем флаг первого блока
                    RaiseCameraOnce(); // Поднимаем камеру
                    this.enabled = false; // Отключаем скрипт
                    return;
                }
            }

            // Проверяем устойчивость на другом блоке
            if (collision.gameObject.CompareTag("Block"))
            {
                Bounds otherBounds = collision.collider.bounds;

                if (transform.position.x >= otherBounds.min.x &&
                    transform.position.x <= otherBounds.max.x &&
                    transform.position.z >= otherBounds.min.z &&
                    transform.position.z <= otherBounds.max.z)
                {
                    hasSupport = true; // Блок устойчив
                    TowerHeightManager.Instance.UpdateTowerHeight(transform.position.y); // Обновляем высоту башни
                    RaiseCameraOnce(); // Поднимаем камеру
                    this.enabled = false; // Отключаем скрипт
                }
                else
                {
                    hasSupport = false; // Устойчивости нет
                }
            }
        }
    }

    private void DestroyBlock()
    {
        Destroy(gameObject); // Удаляем только текущий блок
    }

    private void RaiseCameraOnce()
    {
        // Поднимаем камеру только один раз
        if (!hasRaisedCamera)
        {
            var cameraController = Camera.main?.GetComponent<CameraController>();
            if (cameraController != null)
            {
                cameraController.RaiseCamera();
            }
            hasRaisedCamera = true;
        }
    }
}
