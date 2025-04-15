using UnityEngine;
using UnityEngine.UI;

public class CustomToggleVisual : MonoBehaviour
{
    public Toggle toggle;
    public GameObject onVisual;   // Nút sáng ở bên trái
    public GameObject offVisual;  // Nút sáng ở bên phải

    void Start()
    {
        toggle.onValueChanged.AddListener(UpdateVisual);
        UpdateVisual(toggle.isOn);
    }

    void UpdateVisual(bool isOn)
    {
        onVisual.SetActive(isOn);
        offVisual.SetActive(!isOn);
    }
}
