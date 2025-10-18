using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour, IPoolable
{
    [SerializeField] private T _prefabe;

    protected ObjectPool<T> _pool;

    private int _poolCapacity = 20;
    private int _poolSize = 100;

    private void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: () => Instantiate(_prefabe, GetRandomPosition(), Quaternion.identity),
            actionOnGet: (obj) => Get(obj),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolSize
            );
    }

    public virtual void Get(T obj)
    {
        obj.gameObject.transform.position = GetRandomPosition();
    }

    public virtual void Release(IPoolable obj)
    {
        _pool.Release((T)obj);  
    }

    protected virtual Vector3 GetRandomPosition()
    {
        return Vector3.zero;
    }
}
