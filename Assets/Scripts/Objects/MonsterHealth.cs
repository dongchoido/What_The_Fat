using System.Collections.Generic;
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

    [Header("Reward")]
    public GameObject rewards;
    public GameObject currentParent;
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
        ShowReward();
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

    void ShowReward()
    {
        if(rewards != null)
        {
            currentParent = transform.parent.gameObject;
            Vector3 offset = new Vector3(0, transform.localScale.y/2,0);
            GameObject reward = Instantiate(rewards,transform.position - offset,Quaternion.identity);
            reward.transform.SetParent(currentParent.transform, worldPositionStays: true);
        }
    }
}