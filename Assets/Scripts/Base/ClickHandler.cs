using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class ClickHandler : MonoBehaviour
{
    private InputHandler _inputHandler;
    private BaseCreator selectBase;

    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
    }

    private void Update()
    {
        if (_inputHandler.RightMouseButtonPressed)
        {
            GetClickObject();
        }
    }

    private void GetClickObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (selectBase != null)
                selectBase.SetSelect();

            if (hit.collider != null && hit.collider.gameObject.TryGetComponent<BaseCreator>(out selectBase))
            {
                selectBase.ShowFlag();
                selectBase.SetSelect();
            }
        }
    }
}
