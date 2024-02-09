using Unity.VisualScripting;
using UnityEngine;

public class AutoEarningsSpawner : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject dogPrefab;

    [Header(" Settings ")]
    [SerializeField] private float rotatorRadius;

    private void Start() 
    {
        SpawnDogs((int)DataManager.Instance.AutoEarningLevel);
    }

    private void OnEnable()
    {
        AutoEarningsManager.OnAutoEarningLevelIncreased += OnAutoEarningLevelIncreasedHandler;
    }

    private void OnDisable()
    {
        AutoEarningsManager.OnAutoEarningLevelIncreased -= OnAutoEarningLevelIncreasedHandler;
    }

    public void SpawnDogs(int autoEarningLevel)
    {
        // Destroy all of the dogs
        while(transform.childCount > 0)
        {
            Transform dog = transform.GetChild(0);
            dog.SetParent(null);
            Destroy(dog.gameObject);
        }

        int dogCount = Mathf.Min(autoEarningLevel, 36);

        for (int i = 0; i < dogCount; i++)
        {
            float angle = i * 10;

            Vector2 position = new Vector2();
            position.x = rotatorRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            position.y = rotatorRadius * Mathf.Sin(angle * Mathf.Deg2Rad);

            GameObject dogInstance = Instantiate(dogPrefab, position, Quaternion.identity, this.transform);

            dogInstance.transform.up =  position.normalized;
        }
    }

    private void OnAutoEarningLevelIncreasedHandler(int autoEarningLevel)
    {
        SpawnDogs(autoEarningLevel);
    }
}