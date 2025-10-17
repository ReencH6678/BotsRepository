using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;

    public void Move(Vector3 taget)
    {
            transform.position = Vector3.MoveTowards(transform.position, taget, _speed);
    }
}
