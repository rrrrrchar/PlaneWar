using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//对象池基类
[System.Serializable]public class Pool
{
    public GameObject Prefab => prefab;

    //对象池要维护的对象
    [SerializeField] GameObject prefab;
    //用队列维护
    Queue<GameObject> queue;
    //对象池大小
    [SerializeField] int poolSize;
    //初始化对象池

    //
    Transform parent;
    public void InitPool(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.parent = parent;
        for (var i = 0; i < poolSize; i++)
        {
            queue.Enqueue(CreateObjForPool());
        }
    }
    //预先生成对象
    GameObject CreateObjForPool()
    {
        //
        var res = GameObject.Instantiate(prefab,parent);

        res.SetActive(false);
        return res;
    }

    //从对象池中取出对象
    GameObject getObjForPool()
    {
        GameObject res = null;
        if (queue.Count > 0 && !queue.Peek().active)
            res = queue.Dequeue();
        else
            res = CreateObjForPool();
        //启用完就加回队列，省去了失活函数的调用，但是对象池只有一个时不能这样子做
        queue.Enqueue(res);

        return res;
    }
    #region 使用对象
    //将可用的对象启用
    public GameObject useObj()
    {
        GameObject res = getObjForPool();
        res.SetActive(true);
        return res;
    }

    public GameObject useObj(Vector3 position)
    {
        GameObject res = getObjForPool();
        res.SetActive(true);
        res.transform.position = position;

        return res;
    }


    public GameObject useObj(Vector3 position,Quaternion rotation)
    {
        GameObject res = getObjForPool();
        res.SetActive(true);
        res.transform.position = position;
        res.transform.rotation = rotation;
        return res;
    }

    public GameObject useObj(Vector3 position, Quaternion rotation,Vector3 localScale)
    {
        GameObject res = getObjForPool();
        res.SetActive(true);
        res.transform.position = position;
        res.transform.rotation = rotation;
        res.transform.localScale = localScale;
        return res;
    }

    #endregion
    /* 将使用过的对象失活
   public void deleteObj(GameObject gameObject)
    {
        queue.Enqueue(gameObject);
    }
    */
}
