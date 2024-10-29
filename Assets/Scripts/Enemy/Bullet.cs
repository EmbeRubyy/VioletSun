using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) 
    {
        Transform hitTransform = collision.transform;
        string hitTag = hitTransform.tag;

        switch (hitTag)
        {
            case "Player":
                Debug.Log("Hit Player");
                hitTransform.GetComponent<PlayerHealth>().TakeDamage(10);
                break;
            case "Enemy":
                Debug.Log("Hit Enemy");
                Enemy enemy = hitTransform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.EnemyDamage();
                }
                break;
            case "Untagged":
                Debug.Log("Hit Untagged");
                break;
            default:
                Debug.Log($"Hit {hitTag}");
                break;
        }

        // Destroy the bullet regardless of what it hits
        Destroy(gameObject);
    }
}
