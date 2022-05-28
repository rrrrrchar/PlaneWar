using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    
    
    //Íæ¼Ò×Óµ¯³Ø×Ó
    [SerializeField] Pool[] playerProjectilePool;

    private void Start()
    {
        init(playerProjectilePool);
    }

    void init(Pool[] pools)
    {
        foreach(var val in pools)
        {
            Transform valParent = new GameObject("Pool: " + val.Prefab).transform;
            valParent.parent = transform;
            val.InitPool(valParent) ;
        }
    }
}
