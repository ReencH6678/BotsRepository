using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSpawner : Spawner<Item>
{
    [SerializeField] private float _spawnrate;
    [SerializeField] private List<BoxCollider> _spawnArea;

    private bool _isOn = true;

    public event Action<Item> Spawned;
    public event Action<Item> Released;

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
        base.Get(obj);

        obj.DeactivationRequested += Release;
        Spawned?.Invoke(obj);
    }

    public override void Release(IPoolable obj)
    {
        if (obj is Item item)
            Released?.Invoke(item);

        base.Release(obj);
        obj.DeactivationRequested -= Release;
    }

    protected override Vector3 GetRandomPosition()
    {
        Bounds bounds = _spawnArea[UnityEngine.Random.Range(0, _spawnArea.Count)].bounds;

        return new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), transform.position.y, UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
    }
}
