using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager<T> where T : Component
{
    private readonly GameObject prefab;
    private readonly Transform parent;
    private readonly ObjectPool<T> pool;

    public ObjectPoolManager(GameObject prefab, Transform parent = null, int defaultCapacity = 10, int maxSize = 50)
    {
        this.prefab = prefab;
        this.parent = parent;

        pool = new ObjectPool<T>(
            CreateFunc,
            OnTake,
            OnRelease,
            OnDestroyObject,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    private T CreateFunc()
    {
        var obj = Object.Instantiate(prefab, parent);
        obj.SetActive(false);
        return obj.GetComponent<T>();
    }

    private void OnTake(T obj) => obj.gameObject.SetActive(true);
    private void OnRelease(T obj) => obj.gameObject.SetActive(false);
    private void OnDestroyObject(T obj) => Object.Destroy(obj.gameObject);

    public T Get() => pool.Get();
    public void Release(T obj) => pool.Release(obj);
}