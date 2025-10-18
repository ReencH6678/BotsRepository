using UnityEngine;
using UnityEngine.Events;

public class Releaser : MonoBehaviour
{
    public event UnityAction<int> ItemsRelesased;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Unit>(out Unit unit))
               ItemsRelesased?.Invoke(unit.ReleaseItems(transform.position));
    }
}
