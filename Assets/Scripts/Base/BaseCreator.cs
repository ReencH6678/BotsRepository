using System;
using UnityEngine;

public class BaseCreator : MonoBehaviour
{
    [SerializeField] private Flag _flag;
    [SerializeField] private Base _prefabe;

    private bool _isSelect = false;

    public Vector3 FlapPosition => _flag.transform.position;

    public event Action FlagPlaced;

    private void Awake()
    {
        _flag.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isSelect)
            _flag.Move();
    }

    public void ShowFlag()
    {
        _flag.gameObject.SetActive(true);
    }

    public void SetSelect()
    {
        _isSelect = !_isSelect;

        if (_isSelect == false)
            FlagPlaced?.Invoke();
    }

    public Base CreateBase()
    {
        _flag.gameObject.SetActive(false);
        return Instantiate(_prefabe, _flag.transform.position, _prefabe.transform.rotation);
    }
}
