using UnityEngine;

[RequireComponent(typeof(Releaser), typeof(StorageViewer))]
public class Storage : MonoBehaviour
{
    private Releaser _releaser;
    private StorageViewer _storageViewer;

    private int _itemsCount;

    private void Awake()
    {
        _releaser = GetComponent<Releaser>();
        _storageViewer = GetComponent<StorageViewer>();
    }

    private void OnEnable()
    {
        _releaser.ItemsRelesased += AddItems;
    }

    private void OnDisable()
    {
        _releaser.ItemsRelesased -= AddItems;
    }

    private void AddItems(int count)
    {
        _itemsCount += count;
        _storageViewer.UpdateCount(_itemsCount);
    }
}
