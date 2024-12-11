using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject blockPrefab; // Префаб блока
    private GameObject currentBlock; // Текущий блок

    void Start()
    {
        // Генерируем первый блок
        SpawnNewBlock();
    }

    void Update()
    {
        // Проверяем, завершилось ли движение текущего блока
        if (currentBlock == null || !currentBlock.GetComponent<BlockController>().enabled)
        {
            SpawnNewBlock();
        }
    }

    void SpawnNewBlock()
    {
        // Создаем новый блок
        currentBlock = Instantiate(blockPrefab);
    }
}
