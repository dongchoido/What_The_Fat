using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioController : MonoBehaviour
{
    public float targetAspectRatio = 16f / 9f;

    void Start()
    {
        UpdateViewport();
    }

    void Update()
    {
        // Nếu người chơi resize cửa sổ, cập nhật lại viewport
        if (Mathf.Abs((float)Screen.width / Screen.height - targetAspectRatio) > 0.01f)
        {
            UpdateViewport();
        }
    }

    void UpdateViewport()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspectRatio;

        Camera cam = GetComponent<Camera>();

        if (scaleHeight < 1f)
        {
            // Letterbox (viền đen trên/dưới)
            Rect rect = new Rect(0, (1f - scaleHeight) / 2f, 1f, scaleHeight);
            cam.rect = rect;
        }
        else
        {
            // Pillarbox (viền đen trái/phải)
            float scaleWidth = 1f / scaleHeight;
            Rect rect = new Rect((1f - scaleWidth) / 2f, 0, scaleWidth, 1f);
            cam.rect = rect;
        }
    }
}

