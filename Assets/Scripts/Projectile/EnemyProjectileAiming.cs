using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAiming : Projectile
{
    private void Awake()
    {
        StartCoroutine(nameof(Co_MoveDirection));
        target = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void OnEnable()
    {
        if(target.activeSelf)
        {
            moveDirection = (target.transform.position - transform.position).normalized;
        }
        base.OnEnable();
    }

    IEnumerator Co_MoveDirection()
    {
        yield return null;
        if (target.activeSelf)
        {
            moveDirection = (target.transform.position - transform.position).normalized;
        }
    }
}
