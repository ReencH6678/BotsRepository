using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Taker), typeof(Backpack))]
[RequireComponent(typeof(Rigidbody))]
public class Unit : MonoBehaviour
{
    [SerializeField] private GameObject _base;
    [SerializeField] private float _reachDistance;

    private NavMeshAgent _mover;
    private Taker _taker;
    private Backpack _backpack;
    private Rigidbody _rigidbody;

    private List<Item> _targetItems = new List<Item>();

    public int MaxItemCount => _backpack.MaxItemsCount;
    public bool HaveTargetItems => _targetItems.Count > 0;

    public bool IsWaiting { get; private set; }

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
        IsWaiting = false;

        for (int i = 0; i < _targetItems.Count; i++)
        {
            Item item = _targetItems[i];

            _taker.SetTargetItem(item);
            yield return MoveTo(item.transform.position);
        }

        _targetItems.Clear();

        yield return MoveTo(releaser.position);
        yield return MoveTo(GetWaitPoint(waitArea));


        IsWaiting = true;
    }

    public IEnumerator MoveTo(Vector3 target)
    {
        _mover.SetDestination(target);

        yield return new WaitUntil(() => (transform.position - target).sqrMagnitude < _reachDistance);
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
        return new Vector3(Random.Range(bounds.min.x, bounds.max.x), transform.position.y, Random.Range(bounds.min.z, bounds.max.z));
    }
}
