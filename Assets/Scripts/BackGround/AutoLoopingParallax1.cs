using UnityEngine;

public class AutoLoopingParallax1 : MonoBehaviour
{
    public float parallaxFactor = 0.5f;

    private Transform mainPart;
    private Transform clonePart;
    private float spriteWidth;

    void Start()
    {
        // Gán phần gốc
        mainPart = transform;

        // Tính chiều rộng sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;

        // Tạo bản sao
        clonePart = Instantiate(gameObject, transform.position + Vector3.right * spriteWidth, Quaternion.identity, transform.parent).transform;

        // Xoá script ở bản sao để tránh loop đệ quy
        Destroy(clonePart.GetComponent<AutoLoopingParallax>());
    }

    void Update()
    {
        float speed = 3 * parallaxFactor;
        mainPart.position += Vector3.left * speed * Time.deltaTime;
        clonePart.position += Vector3.left * speed * Time.deltaTime;

        if (mainPart.position.x <= Camera.main.transform.position.x - spriteWidth)
        {
            mainPart.position = new Vector3(clonePart.position.x + spriteWidth, mainPart.position.y, mainPart.position.z);
            SwapParts();
        }
        else if (clonePart.position.x <= Camera.main.transform.position.x - spriteWidth)
        {
            clonePart.position = new Vector3(mainPart.position.x + spriteWidth, clonePart.position.y, clonePart.position.z);
            SwapParts();
        }
    }

    void SwapParts()
    {
        // Hoán đổi vị trí giữa mainPart và clonePart
        Transform temp = mainPart;
        mainPart = clonePart;
        clonePart = temp;
    }
}
