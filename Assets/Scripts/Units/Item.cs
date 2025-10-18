
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour, IPoolable
{
    public event UnityAction<IPoolable> DeactivationRequested;

    public void Relese()
    {
        transform.SetParent(null);
        DeactivationRequested?.Invoke(this);
    }
}
