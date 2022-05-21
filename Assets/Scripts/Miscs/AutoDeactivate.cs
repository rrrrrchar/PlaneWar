using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//自动禁用/销毁 子弹
public class AutoDeactivate : MonoBehaviour
{
    //是否要禁用
    //[SerializeField] 
    bool destroyGameObj=true;
    //强制销毁子弹的时间
    //[SerializeField] 
    float leftTime = 3f;
    WaitForSeconds waitForSeconds;
    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(leftTime);
    }

    private void OnEnable()
    {
        StartCoroutine("Co_Deactivate");
    }

    //销毁子弹协程
    IEnumerator Co_Deactivate()
    {
        yield return waitForSeconds;
        //如果要禁用
        if(destroyGameObj)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
