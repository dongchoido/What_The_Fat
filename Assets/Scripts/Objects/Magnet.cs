using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour

{
    void FixedUpdate()
    {
        transform.Rotate(new Vector3 (0,5,0));
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.setMagnetTimer();
            Destroy(gameObject);
        }
            SoundManager.Instance.PlayMagnetSound();

    }
}
