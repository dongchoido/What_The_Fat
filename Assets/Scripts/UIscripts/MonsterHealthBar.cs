using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    public Image healthFill; // Gán Image Fill vào đây
    private MonsterHealth monster;

    void Start()
    {
        monster = GetComponentInParent<MonsterHealth>();
    }

    void Update()
    {
        if (monster != null && healthFill != null)
        {
            healthFill.fillAmount = (float)monster.currentHealth / monster.maxHealth;
        }
        else Debug.Log("fsf");
    }
}
