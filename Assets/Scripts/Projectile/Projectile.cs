using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//子弹的基类
public class Projectile : MonoBehaviour
{
    [SerializeField]private float moveSpeed = 10f;
    [SerializeField]protected Vector2 moveDirection;

    private void OnEnable()
    {
        StartCoroutine(Co_MoveDirectly());
    }
    IEnumerator Co_MoveDirectly()
    {
        while(gameObject.activeSelf)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

}
