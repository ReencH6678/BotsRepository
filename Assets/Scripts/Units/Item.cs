
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour, IPoolable
{
    public event UnityAction<IPoolable> DeactivationRequested;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Base>(out Base collisedBase))
            DeactivationRequested?.Invoke(this);
    }

    public void Relese()
    {
        transform.SetParent(null);
        DeactivationRequested?.Invoke(this);
    }
}
