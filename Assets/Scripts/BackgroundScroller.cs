﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    //偏移速度
    [SerializeField]Vector2 scrollVelocity;
    Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material; 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //匀速转动 
        material.mainTextureOffset += scrollVelocity * Time.deltaTime;   
    }
}
