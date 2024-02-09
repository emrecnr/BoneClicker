using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] T prefab;
    [SerializeField] int poolSize = 5;

    Queue<T> pool = new Queue<T>();

    private void Awake()
    {
        SingletonObject();
    }
    private void Start() {
        GrowPoolPrefab();
    }

    protected abstract void SingletonObject();

    public T Get()
    {
        if(pool.Count == 0)
        {
            GrowPoolPrefab();
        }
        return pool.Dequeue();
    }

    private void GrowPoolPrefab()
    {
        for (int i = 0; i < poolSize; i++)
        {
            T  poolObject = Instantiate(prefab);
            poolObject.gameObject.transform.SetParent(this.gameObject.transform);
            poolObject.gameObject.SetActive(false);
            pool.Enqueue(poolObject);
        }
    }
    public void Set(T poolObject)
    {
        poolObject.gameObject.SetActive(false);
        pool.Enqueue(poolObject);
    }
}
