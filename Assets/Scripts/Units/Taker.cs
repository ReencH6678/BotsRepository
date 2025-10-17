using UnityEngine;

[RequireComponent(typeof(Backpack))]
public class Taker : MonoBehaviour
{
    private Backpack _backpack;
    private Item _targetItem;

    private void Awake()
    {
        _backpack = GetComponent<Backpack>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Item>(out Item item) && item == _targetItem)
        {
            _backpack.Pack(item);
        }
    }

    public void SetTargetItem(Item item)
    {
        _targetItem = item;
    }
}
