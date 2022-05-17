using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//玩家类
//通过接收 playerInput发送过来的事件 与 刚体一起实现移动

//添加2D刚体
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    new Rigidbody2D rigidbody;

    [SerializeField]float movespeed = 10f;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigidbody.gravityScale = 0f;
        //激活动作表
        input.EnableGamePlayInput();
    }

    //订阅事件方便 移动/停止移动的实现
    private void OnEnable()
    {
        input.OnMoveEvent += MoveFunc;
        input.OnStopMoveEvent += StopMoveFunc;
    }
    //取消订阅
    private void OnDisable()
    {
        input.OnMoveEvent -= MoveFunc;
        input.OnStopMoveEvent -= StopMoveFunc;
    }

    void MoveFunc(Vector2 moveinput)
    {
        //Vector2 moveAmount = moveinput * movespeed;
        rigidbody.velocity = moveinput * movespeed;
    }

    void StopMoveFunc()
    {
        rigidbody.velocity = Vector2.zero;
    }

}
