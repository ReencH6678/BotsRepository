using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private const int RightMouseIndex = 0;
    public bool RightMouseButtonPressed { get; private set; }

    private void Update()
    {
        RightMouseButtonPressed = Input.GetMouseButtonDown(RightMouseIndex);
    }
}
