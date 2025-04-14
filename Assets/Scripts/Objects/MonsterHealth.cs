using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    [Header("HP")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Sprites")]
    public Sprite strong;
    public Sprite normal;
    public Sprite weak;
    private SpriteRenderer sprite;
    void Start()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        UpdateSprite();
    }

    void Die()
    {
        // Xử lý khi quái vật chết
        Destroy(gameObject);
    }

    void UpdateSprite()
    {
        float healthPercent = (float)currentHealth/maxHealth;
        if (healthPercent > 0.75f)
        {
            sprite.sprite = strong;
        }
        else if (healthPercent > 0.45f)
        {
            sprite.sprite = normal;
        }
        else
        {
            sprite.sprite = weak;
        }
    }
}