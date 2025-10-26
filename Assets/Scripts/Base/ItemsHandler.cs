using UnityEngine;
using System.Collections.Generic;

public class ItemsHandler : MonoBehaviour
{
    [SerializeField] private ItemsSpawner _itemSpawner;

    private HashSet<Item> _items = new HashSet<Item>();
    private HashSet<Item> _reserveItems = new HashSet<Item>();

    private void OnEnable()
    {
        _itemSpawner.Spawned += AddItem;
        _itemSpawner.Released += RemoveItem;
    }

    public bool CanTake(Item item)
    {
        return _reserveItems.Contains(item) == false && _items.Contains(item);
    }

    private void AddItem(Item item)
    {
        _items.Add(item);
    }

    public void ReserveItem(Item item)
    {
        _items.Remove(item);
        _reserveItems.Add(item);
    }

    private void RemoveItem(Item item)
    {
        _items.Remove(item);
        _reserveItems.Remove(item);
    }
}
