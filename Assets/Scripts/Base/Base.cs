using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private List<Unit> _units;
    [SerializeField] private BoxCollider _waitArea;
    [SerializeField] private Storage _storage;

    private Scanner _itemFinder;

    private HashSet<Item> _foundItems = new HashSet<Item>();
    private HashSet<Item> _collectingItems = new HashSet<Item>();

    private void Awake()
    {
        _itemFinder = GetComponent<Scanner>();
    }

    private void OnEnable()
    {
        _itemFinder.Found += SetFoundItems;
    }

    private void OnDisable()
    {
        _itemFinder.Found -= SetFoundItems;
    }

    private void SetFoundItems(List<Item> items)
    {
        foreach (Item item in items)
        {
            if (_collectingItems.Contains(item) == false)
                _foundItems.Add(item);
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
}
