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
    //padding 的大小
    //[SerializeField] float paddingx;
    //[SerializeField] float paddingy;
    //加减速时间
    //[SerializeField] float accTime=5f;
    //[SerializeField] float decelerdTime=5f;
    float accTime = 0.25f;
    float decelerdTime = 0.25f;
    //移动时的旋转角度
    //[SerializeField] float rotationAngle=28f;
    float rotationAngle = 28f;
    //移动逻辑要用到的临时协程
    Coroutine TempCoMove;
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
        //Vector2 moveAmount = moveinput.normalized * movespeed;
        //rigidbody.velocity = moveinput.normalized * movespeed;

        if (TempCoMove != null)
        {
            StopCoroutine(TempCoMove);
        }
        //Quaternion moveRotation = Quaternion.AngleAxis( rotationAngle * moveinput.y,Vector3.right);
        TempCoMove=StartCoroutine(Co_Move(moveinput.normalized * movespeed,accTime , Quaternion.AngleAxis(rotationAngle * moveinput.y, Vector3.right)));
        StartCoroutine(Co_MovePositionLimit());
    }

    void StopMoveFunc()
    {
        //rigidbody.velocity = Vector2.zero;
        if (TempCoMove != null)
        {
            StopCoroutine(TempCoMove);
        }

        TempCoMove=StartCoroutine(Co_Move(Vector2.zero,decelerdTime,Quaternion.identity));
        StopCoroutine(Co_MovePositionLimit());
    }

    //移动协程
    IEnumerator Co_Move(Vector2 moveVelocity,float type,Quaternion rotation)
    {
        float time = 0f;
        while(time<type)
        {
            
            time += Time.fixedDeltaTime / type;
            //加减速
            rigidbody.velocity=Vector2.Lerp(rigidbody.velocity, moveVelocity, time / type);
            //旋转
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, time / type);

            yield return null;
        }

    }


    //移动限制协程
    IEnumerator Co_MovePositionLimit()
    {
        while(true)
        {
            transform.position = Viewport.Instance.PlayerMoveablePostion(transform.position);

            yield return null;
        }
    }
}
