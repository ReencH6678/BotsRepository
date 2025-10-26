using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _searchAreaRadius;
    [SerializeField] private float _searchRate;

    private bool _isOn = true;

    public event UnityAction<List<Item>> Found;

    private void Start()
    {
        StartCoroutine(SearchItems());
    }

    public IEnumerator SearchItems()
    {
        var waitForSeconds = new WaitForSeconds(_searchRate);

        while (_isOn)
        {
            yield return new WaitForFixedUpdate();
            Collider[] colliders = Physics.OverlapSphere(transform.position, _searchAreaRadius);
            List<Item> items = new List<Item>();

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.TryGetComponent<Item>(out Item item))
                    items.Add(item);
            }

            if (items.Count > 0)
                Found?.Invoke(items);

            items.Clear();
            yield return waitForSeconds;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, _searchAreaRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _searchAreaRadius);

    }
}
