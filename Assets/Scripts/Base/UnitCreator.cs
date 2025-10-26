using UnityEngine;

public class UnitCreator : MonoBehaviour
{
    [SerializeField] private Unit _prefabe;
    [SerializeField] private Transform _spawnPoint;

    public Unit CreateUnit()
    {
        return Instantiate(_prefabe, _spawnPoint.position, _prefabe.transform.rotation);
    }
}
