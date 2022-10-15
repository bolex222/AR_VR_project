using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 10f;
    public float currentHealth;

    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;

    public HealthBar healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        healthBar.SetHealth(currentHealth);
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Die();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    public void Die()
    {
        player.transform.position = respawnPoint.transform.position;
        Physics.SyncTransforms();
    }
}
