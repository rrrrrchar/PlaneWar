using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    
    
    //玩家子弹池子
    [SerializeField] Pool[] playerProjectilePool;
    //敌人子弹
    [SerializeField] Pool[] enemyProjectilePool;
     static Dictionary<GameObject, Pool> dict;

    private void Start()
    {
        dict = new Dictionary<GameObject, Pool>();
        init(playerProjectilePool);
        init(enemyProjectilePool);
    }
    //实时检测池子应有的大小
    #if UNITY_EDITOR
    private void OnDestroy()
    {
        checkPoolSize(playerProjectilePool);
        checkPoolSize(enemyProjectilePool);
    }
    #endif
    void checkPoolSize(Pool[] pools)
    {
        foreach(var val in pools)
        {
            if( val.runtimeSize > val.Size)
            {
                Debug.LogWarning(string.Format("Pool:{0} has runtimeSize {1} bigger than size {2}", val.Prefab.name, val.runtimeSize, val.Size));
            }
        }
    }

    void init(Pool[] pools)
    {
        foreach(var poolPrefab in pools)
        {
            if(!dict.ContainsKey(poolPrefab.Prefab))
                dict.Add(poolPrefab.Prefab, poolPrefab);
            Transform valParent = new GameObject("Pool: " + poolPrefab.Prefab).transform;
            valParent.parent = transform;
            poolPrefab.InitPool(valParent) ;
        }
    }
#region
    /// <summary>
    /// 
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject release(GameObject prefab)
    {
        if (dict.ContainsKey(prefab))
            return dict[prefab].useObj();
        else
            return null;
    }

    public static GameObject release(GameObject prefab,Vector3 position)
    {
        if (dict.ContainsKey(prefab))
            return dict[prefab].useObj(position);
        else
            return null;
    }


    public static GameObject release(GameObject prefab,Vector3 position,Quaternion rotation)
    {
        if (dict.ContainsKey(prefab))
            return dict[prefab].useObj(position,rotation);
        else
            return null;
    }

    public static GameObject release(GameObject prefab, Vector3 position, Quaternion rotation,Vector3 localScale)
    {
        if (dict.ContainsKey(prefab))
            return dict[prefab].useObj(position,rotation,localScale);
        else
            return null;
    }
#endregion
}
