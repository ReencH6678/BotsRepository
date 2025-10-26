using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Scanner), typeof(BaseCreator))]
public class Base : MonoBehaviour
{
    [SerializeField] private List<Unit> _units;

    [SerializeField] private BoxCollider _waitArea;
    [SerializeField] private Storage _storage;

    [SerializeField] private int _itemsCountForUnit;
    [SerializeField] private int _itemsCountForCreateBase;
    [SerializeField] private int _minUnitCountForCreateBase;

    [SerializeField] private ItemsHandler _itemsHandler;
    [SerializeField] private float _reachDistanceForCreateBase;

    private Scanner _itemFinder;
    private UnitCreator _unitCreator;
    private BaseCreator _baseCreator;

    private bool _isUnitCreating = true;

    private HashSet<Item> _foundItems = new HashSet<Item>();
    private HashSet<Item> _collectingItems = new HashSet<Item>();

    private void Awake()
    {

        _itemFinder = GetComponent<Scanner>();
        _unitCreator = GetComponent<UnitCreator>();
        _baseCreator = GetComponent<BaseCreator>();
    }

    private void OnEnable()
    {
        foreach (Unit unit in _units)
            unit.ItemsCollected += UpdateCollectingItems;

        _baseCreator.FlagPlaced += SetCreateBasePriority;
        _storage.CountChanged += TryAddUnit;
        _itemFinder.Found += SetFoundItems;
    }

    private void OnDisable()
    {
        foreach (Unit unit in _units)
            unit.ItemsCollected -= UpdateCollectingItems;

        _storage.CountChanged -= TryAddUnit;
        _itemFinder.Found -= SetFoundItems;
        _baseCreator.FlagPlaced -= SetCreateBasePriority;
    }

    private void TryAddUnit(int itemsCount)
    {
        if (itemsCount >= _itemsCountForUnit && _isUnitCreating)
        {
            Unit newUnit = _unitCreator.CreateUnit();
            _storage.TakeItems(_itemsCountForUnit);

            _units.Add(newUnit);
            SendUnitToCollect(newUnit);
        }
    }

    public void AddUnit(Unit unit)
    {
        _units = new List<Unit>();
        _units.Add(unit);

        unit.ItemsCollected += UpdateCollectingItems;
    }

    private void SetFoundItems(List<Item> items)
    {
        foreach (Item item in items)
        {
            if (_itemsHandler.CanTake(item))
            {
                if (_collectingItems.Contains(item) == false)
                {
                    _foundItems.Add(item);
                    _itemsHandler.ReserveItem(item);
                }
            }
        }

        foreach (Unit unit in _units)
            if (unit.IsWaiting && _foundItems.Count > 0)
                SendUnitToCollect(unit);
    }

    private void SendUnitToCollect(Unit unit)
    {
        SetUnitItems(unit);
        StartCoroutine(unit.Collect(_storage.transform, _waitArea));
    }

    private void SetUnitItems(Unit unit)
    {
        List<Item> unitItems = new List<Item>();

        for (int i = 0; i < unit.MaxItemCount; i++)
        {
            if (_foundItems.Count > 0)
            {
                Item currentItem = _foundItems.First();

                unitItems.Add(currentItem);

                _foundItems.Remove(currentItem);
                _collectingItems.Add(currentItem);
            }
        }

        unit.SetCollectItems(unitItems);
    }

    private void UpdateCollectingItems(List<Item> collectedItems)
    {
        foreach (Item item in collectedItems)
        {
            _collectingItems.Remove(item);
        }
    }

    private void SetCreateBasePriority()
    {
        _isUnitCreating = false;
        StartCoroutine(CreateBase(_baseCreator.FlapPosition));
    }

    private void SetItemsHandler(ItemsHandler itemsHandler)
    {
        _itemsHandler = itemsHandler;
    }

    private IEnumerator CreateBase(Vector3 flagPosition)
    {
        Base newBase;
        IEnumerator<Unit> enumerator = GetWaitingUnit();

        yield return new WaitUntil(() => _storage.ItemsCount >= _itemsCountForCreateBase && _units.Count > _minUnitCountForCreateBase);
        yield return new WaitUntil(() => enumerator.MoveNext() == false);

        Unit unit = enumerator.Current;

        _storage.TakeItems(_itemsCountForCreateBase);

        yield return unit.MoveTo(flagPosition, _reachDistanceForCreateBase);

        newBase = _baseCreator.CreateBase();
        _storage.TakeItems(_itemsCountForCreateBase);

        newBase.SetItemsHandler(_itemsHandler);

        unit.ItemsCollected -= UpdateCollectingItems;

        newBase.AddUnit(unit);

        _units.Remove(unit);

        _isUnitCreating = true;
    }

    private IEnumerator<Unit> GetWaitingUnit()
    {
        Unit unit = null;

        while (unit == null)
        {
            foreach (Unit currentUnit in _units)
            {
                if (currentUnit.IsWaiting)
                    unit = currentUnit;
            }

            yield return null;
        }

        yield return unit;
    }
}
