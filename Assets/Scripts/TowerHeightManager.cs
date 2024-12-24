using UnityEngine;

public class TowerHeightManager : MonoBehaviour
{
    public static TowerHeightManager Instance;
    public float CurrentTowerHeight { get; private set; } = 0f;

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
