using System.Collections;
using UnityEngine;

public class ItemsSpawner : Spawner<Item>
{
    [SerializeField] private float _spawnrate;
    [SerializeField] private BoxCollider _spawnArea;

    private bool _isOn = true;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        var waitForSeconds = new WaitForSeconds(_spawnrate);

        while (_isOn)
        {
            _pool.Get();
            yield return waitForSeconds;
        }
    }

    public override void Get(Item obj)
    {
        obj.DeactivationRequested += Release;
        base.Get(obj);
    }

    public override void Release(IPoolable obj)
    {
        base.Release(obj);
        obj.DeactivationRequested -= Release;
    }

    protected override Vector3 GetRandomPosition()
    {
        Bounds bounds = _spawnArea.bounds;

        return new Vector3(Random.Range(bounds.min.x, bounds.max.x), transform.position.y, Random.Range(bounds.min.z, bounds.max.z));
    }
}
