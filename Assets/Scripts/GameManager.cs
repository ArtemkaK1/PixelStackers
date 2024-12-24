using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject blockPrefab;
    private GameObject currentBlock;

    void Start()
    {
        SpawnNewBlock();
    }

    void Update()
    {
        if (currentBlock == null || !currentBlock.GetComponent<BlockController>().enabled)
        {
            SpawnNewBlock();
        }
    }

    void SpawnNewBlock()
    {
        currentBlock = Instantiate(blockPrefab);
    }

    public void RestartGame()
    {
        BlockController.isFirstBlock = true;
        // ScoreManager.Instance.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
