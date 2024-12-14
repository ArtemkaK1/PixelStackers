using UnityEngine;

public class BlockController : MonoBehaviour
{
    public float speed = 5f; // Скорость движения блока
    private bool isMoving = true; // Флаг движения
    private Vector3 direction; // Направление движения блока
    private Rigidbody rb; // Компонент Rigidbody
    private bool hasLanded = false; // Флаг, если блок "пришел на место"
    private bool hasSupport = false; // Флаг наличия устойчивой опоры
    private bool checkedStability = false; // Флаг, что устойчивость уже проверена
    private static bool isFirstBlock = true; // Проверка первого блока

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Физика отключена до остановки блока

        float cameraWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        float spawnX = Random.value > 0.5f ? cameraWidth + 5f : -cameraWidth - 5f; // Левый или правый край
        float spawnZ = 10f; // Появление за башней
        float spawnY = 5f; // Высота появления блока

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
        if (!isMoving && hasLanded && !checkedStability)
        {
            checkedStability = true; // Устанавливаем флаг, чтобы проверка выполнялась только один раз

            if (!hasSupport)
            {
                DestroyBlock(); // Удаляем блок, если нет поддержки
            }
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
                    this.enabled = false; // Отключаем скрипт
                    return;
                }
            }

            // Проверяем устойчивость на другом блоке
            if (collision.gameObject.CompareTag("Block"))
            {
                // Проверяем, находится ли центр текущего блока над блоком, на который он падает
                Bounds otherBounds = collision.collider.bounds;

                if (transform.position.x >= otherBounds.min.x &&
                    transform.position.x <= otherBounds.max.x &&
                    transform.position.z >= otherBounds.min.z &&
                    transform.position.z <= otherBounds.max.z)
                {
                    hasSupport = true; // Блок устойчив
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
}
