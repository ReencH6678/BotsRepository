using System;
using UnityEngine;

[RequireComponent(typeof(Releaser), typeof(StorageViewer))]
public class Storage : MonoBehaviour
{
    private Releaser _releaser;
    private StorageViewer _storageViewer;

    public int ItemsCount { get; private set; }

    public event Action<int> CountChanged;

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

    public void TakeItems(int count)
    {
        ItemsCount -= count;
    }

    private void AddItems(int count)
    {
        ItemsCount += count;
        _storageViewer.UpdateCount(ItemsCount);
        CountChanged?.Invoke(ItemsCount);
    }
}
