using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//泛型单例
public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance
    {
        //protected 
        get;
        private set;
    }
    protected virtual void Awake()
    {
        Instance = this as T;
    }
}
