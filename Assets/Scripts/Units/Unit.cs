using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Taker), typeof(Backpack))]
[RequireComponent(typeof(Rigidbody))]
public class Unit : MonoBehaviour
{
    private const float DefoultReachDistance = 1;

    [SerializeField] private float _itemReachDistance;
    [SerializeField] private float _releaserReachDistance;

    private NavMeshAgent _mover;
    private Taker _taker;
    private Backpack _backpack;
    private Rigidbody _rigidbody;

    private List<Item> _targetItems = new List<Item>();

    public int MaxItemCount => _backpack.MaxItemsCount;

    public bool IsWaiting { get; private set; }
    public bool IsRunning { get; private set; }

    public event Action<List<Item>> ItemsCollected;

    private void Start()
    {
        IsWaiting = true;
    }

    private void Awake()
    {
        _mover = GetComponent<NavMeshAgent>();
        _taker = GetComponent<Taker>();
        _backpack = GetComponent<Backpack>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetCollectItems(List<Item> items)
    {
        _targetItems.AddRange(items);
    }

    public IEnumerator Collect(Transform releaser, BoxCollider waitArea)
    {
        for (int i = 0; i < _targetItems.Count; i++)
        {
            Item item = _targetItems[i];

            _taker.SetTargetItem(item);
            yield return MoveTo(item.transform.position, _itemReachDistance);
        }


        yield return MoveTo(releaser.position, _releaserReachDistance);

        ItemsCollected?.Invoke(_targetItems);
        _targetItems.Clear();

        yield return MoveTo(GetWaitPoint(waitArea));
    }

    public IEnumerator MoveTo(Vector3 target, float reachDistance = DefoultReachDistance)
    {
        IsWaiting = false;

        _mover.SetDestination(target);

        yield return new WaitUntil(() => (transform.position - target).sqrMagnitude < reachDistance);

        IsWaiting = true;
    }

    public int ReleaseItems(Vector3 releasePoint)
    {
        int itemsCount = _backpack.ItemsCount;
        _backpack.Pull();
        
        return itemsCount;
    }

    public Vector3 GetWaitPoint(BoxCollider waitArea)
    {
        Bounds bounds = waitArea.bounds;
        return new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), transform.position.y, UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
    }
}
