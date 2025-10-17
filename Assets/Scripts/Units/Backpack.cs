using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    [SerializeField] private List<Slot> _slots;
    [SerializeField] private int _maxItemsCount;

    private List<Item> _items = new List<Item>();

    public bool IsFull => _items.Count >= _maxItemsCount;
    public int ItemsCount => _items.Count;
    public int MaxItemsCount => _maxItemsCount;

    public void Pack(Item item)
    {
        if (IsFull == false)
        {
            _items.Add(item);
            item.transform.SetParent(transform);
            item.transform.position = GetFreeSlotPosition();
        }
    }

    public void Pull()
    {
        List<Item> items = new List<Item>();

        if (_items.Count > 0)
            foreach (Item item in _items)
                items.Add(item);
  
        SetSlotsFree(items);
    }

    private Vector3 GetFreeSlotPosition()
    {
        foreach (Slot slot in _slots)
        {
            if (slot.IsFilled == false)
            {
                slot.Put();
                return slot.transform.position;
            }
        }

        return Vector3.zero;
    }

    private void SetSlotsFree(List<Item> items)
    {
        foreach(Item item in items)
        {
            _items.Remove(item);
            item.Relese(); 
        }

        foreach (Slot slot in _slots)
            slot.Pull();
    }
}
