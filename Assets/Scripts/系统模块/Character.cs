using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] GameObject deathVFX; 
    [SerializeField] protected float maxHealth;
    protected float health;

    protected virtual void OnEnable()
    {
        health = maxHealth;
    }

    public virtual void TakeDamege(float damage)
    {
        health -= damage;

        if(health <=0f)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        health = 0f;
        PoolManager.release(deathVFX, transform.position);
        gameObject.SetActive(false);
    }

    public virtual void RestoreHealth(float value)
    {
        if (health == maxHealth)
            return;
        //health += value;
        //health = Mathf.Clamp(health , 0f, maxHealth);
        health = Mathf.Clamp(health + value, 0f, maxHealth);
    }

    //持续回血/扣血
    protected IEnumerator CO_HeathRegenerate(WaitForSeconds waitTime, float percent)
    {
        while(health<maxHealth)
        { 
            yield return waitTime;

            RestoreHealth(maxHealth * percent);
        }
    }
    protected IEnumerator CO_DamegeOverTime(WaitForSeconds waitTime, float percent)
    {
        while (health >0f)
        {
            yield return waitTime;

            RestoreHealth(maxHealth * percent);
        }
    }
}
