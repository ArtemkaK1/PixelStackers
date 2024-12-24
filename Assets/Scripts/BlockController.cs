using UnityEngine;

public class BlockController : MonoBehaviour
{
    public static bool isFirstBlock = true;

    public float speed = 5f;
    private bool isMoving = true;
    private Vector3 direction;
    private Rigidbody rb;
    private bool hasLanded = false;
    private bool hasSupport = false;
    private bool hasRaisedCamera = false;
    private bool hasScored = false;
    private ScoreManager scoreManager;

    private Color[] neonColors = new Color[]
    {
        new Color(0.0f, 1.0f, 1.0f), // Ярко-голубой
        new Color(1.0f, 0.0f, 1.0f), // Пурпурный
        new Color(1.0f, 1.0f, 0.0f), // Жёлтый
        new Color(1.0f, 0.5f, 0.0f), // Оранжевый
        new Color(0.0f, 1.0f, 0.5f), // Насыщенный зелёный
        new Color(1.0f, 0.0f, 0.5f), // Ярко-розовый
        new Color(0.5f, 0.0f, 1.0f), // Фиолетовый
        new Color(0.0f, 0.5f, 1.0f), // Синий
        new Color(1.0f, 0.25f, 0.25f), // Коралловый
        new Color(0.0f, 1.0f, 0.25f)  // Салатовый
    };

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        float blockScale = Random.Range(1.5f, 2.5f);
        float scaleX = blockScale;
        float scaleY = 0.5f;
        float scaleZ = blockScale;
        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = neonColors[Random.Range(0, neonColors.Length)];
        }

        float cameraWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        float spawnX = Random.value > 0.5f ? cameraWidth + 0.5f : -cameraWidth - 0.5f;
        float spawnZ = 8f;
        float spawnY = TowerHeightManager.Instance.CurrentTowerHeight + 2f;

        transform.position = new Vector3(spawnX, spawnY, spawnZ);

        float angle = spawnX > 0 ? -135f : 135f;
        direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

        scoreManager = FindObjectOfType<ScoreManager>();
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
            rb.isKinematic = false;
        }

        if (transform.position.y < -10f)
        {
            DestroyBlock();
        }
    }

    void FixedUpdate()
    {
        if (!isMoving && hasLanded && !hasSupport && !hasRaisedCamera)
        {
            hasRaisedCamera = true;
            DestroyBlock();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Base"))
        {
            hasLanded = true;

            if (collision.gameObject.CompareTag("Base"))
            {
                if (isFirstBlock)
                {
                    hasSupport = true;
                    isFirstBlock = false;
                    RaiseCameraOnce();

                    if (!hasScored)
                    {
                        scoreManager.AddScore(1);
                        hasScored = true;
                    }
                    
                    this.enabled = false;
                    return;
                }
            }

            if (collision.gameObject.CompareTag("Block"))
            {
                Bounds otherBounds = collision.collider.bounds;

                if (transform.position.x >= otherBounds.min.x &&
                    transform.position.x <= otherBounds.max.x &&
                    transform.position.z >= otherBounds.min.z &&
                    transform.position.z <= otherBounds.max.z)
                {
                    hasSupport = true;
                    TowerHeightManager.Instance.UpdateTowerHeight(transform.position.y);
                    
                    if (!hasScored)
                    {
                        scoreManager.AddScore(1);
                        hasScored = true;
                    }

                    RaiseCameraOnce();
                    this.enabled = false;
                }
                else
                {
                    hasSupport = false;
                }
            }
        }
    }

    private void DestroyBlock()
    {
        Destroy(gameObject);

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.RestartGame();
        }
    }

    private void RaiseCameraOnce()
    {
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
