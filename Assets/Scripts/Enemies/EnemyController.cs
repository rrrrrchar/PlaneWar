using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //move
    [Header("移动相关")]
    [SerializeField]float paddingx;
    [SerializeField] float paddingy;
    [SerializeField] float movespeed = 2f;
    [SerializeField] float moveRotationAngle = 25f;
    [Header("开火相关")]
    //fire
    [SerializeField]float minFireTime=1;
    [SerializeField]float maxFireTime=3;
    [SerializeField]GameObject[] projectile;
    [SerializeField] Transform waist;
    private void OnEnable()
    {
        //Debug.Log("run??");
        StartCoroutine(nameof(Co_RandomMove));
        StartCoroutine(nameof(Co_RandomFIre));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    //移动
    IEnumerator Co_RandomMove()
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingx, paddingy);
        Vector3 targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingx, paddingy);
        while(gameObject.active)
        {
            if(Vector3.Distance(transform.position,targetPosition)>Mathf.Epsilon)
            {
                //Debug.Log("why not move?");
                transform.position=Vector3.MoveTowards(transform.position, targetPosition, movespeed * Time.deltaTime);
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);
            }
            else
            {
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingx, paddingy);
            }
            yield return null;
        }
        
    }

    //开火
    IEnumerator Co_RandomFIre()
    {
        //可以弄个hash表随机子弹间隔
        while(gameObject.activeSelf)
        {

            yield return new WaitForSeconds(Random.Range(minFireTime,maxFireTime));

            foreach(var value in projectile)
            {
                PoolManager.release(value, waist.position);
            }
        }
    }
}
