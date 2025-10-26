using System;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public void Move()
    {
        Ray mousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mousePosition, out RaycastHit hit))
            transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
    }
}
