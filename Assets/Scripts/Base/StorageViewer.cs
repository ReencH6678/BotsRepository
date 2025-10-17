using TMPro;
using UnityEngine;

public class StorageViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void UpdateCount(int count)
    {
        _text.text = count.ToString();
    }
}
