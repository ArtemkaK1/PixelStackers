using UnityEngine;

public class TowerHeightManager : MonoBehaviour
{
    public static TowerHeightManager Instance; // Синглтон
    public float CurrentTowerHeight { get; private set; } = 0f; // Текущая высота башни

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateTowerHeight(float newHeight)
    {
        if (newHeight > CurrentTowerHeight)
        {
            CurrentTowerHeight = newHeight;
        }
    }
}
