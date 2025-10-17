using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool IsFilled {  get; private set; }

    public void Put()
    {
        IsFilled = true;
    }

    public void Pull()
    {
        IsFilled = false;
    }
}
