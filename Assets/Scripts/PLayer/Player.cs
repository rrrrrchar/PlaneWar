using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//玩家类
//通过接收 playerInput发送过来的事件 与 刚体一起实现移动

//添加2D刚体
[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    //下面没有序列化直接再脚本写死的都是懒得调
    [SerializeField] bool regenerateHealth = true;
    [SerializeField] float healthRegenerateTime;
    [SerializeField ,Range(0f,1f)] float healthRegeneratePercent;
    WaitForSeconds waitHealthRegenerateTime;
    Coroutine TempCoHealthRegenerate;


    [Header("----------移动")]
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



    //开火
    //子弹对象
    [SerializeField] GameObject projectileUp;
    [SerializeField] GameObject projectileMid;
    [SerializeField] GameObject projectileDown;
    //武器威力
    [SerializeField,Range(0,2)] int weaponPower = 0;
    //瞄准点
    [SerializeField] Transform projectilePositionUp;
    [SerializeField] Transform projectilePositionMid;
    [SerializeField] Transform projectilePositionDown;
    //[SerializeField]
    //攻击间隔
    float fireInerval=0.1f;
    private WaitForSeconds waitForSeconds;



    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigidbody.gravityScale = 0f;
        //激活动作表
        input.EnableGamePlayInput();
        //攻击间隔初始化
        waitForSeconds = new WaitForSeconds(fireInerval);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
    }



    //订阅事件方便 移动/停止移动的实现
    protected override void OnEnable()
    {
        base.OnEnable();
        input.OnMoveEvent += MoveFunc;
        input.OnStopMoveEvent += StopMoveFunc;
        input.OnFireEvent += FireFunc;
        input.OnStopFireEvent += StopFireFunc;
    }
    //取消订阅
    private void OnDisable()
    {
        input.OnMoveEvent -= MoveFunc;
        input.OnStopMoveEvent -= StopMoveFunc;
        input.OnFireEvent -= FireFunc;
        input.OnStopFireEvent -= StopFireFunc;
    }

    #region 移动
    void MoveFunc(Vector2 moveinput)
    {
        //Vector2 moveAmount = moveinput.normalized * movespeed;
        //rigidbody.velocity = moveinput.normalized * movespeed;
        
        if (TempCoMove != null)
        {
            StopCoroutine(TempCoMove);
        }
        //用一个四元数表示绕哪个轴旋转
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

    #endregion


    #region 开火
    void FireFunc()
    {
        StartCoroutine("Co_Fire");
    }

    void StopFireFunc()
    {
        StopCoroutine("Co_Fire");
    }
    IEnumerator Co_Fire()
    {
        
        while (true)
        {
            switch (weaponPower)
            {
                case 0:

                    PoolManager.release(projectileMid, projectilePositionMid.position);
                    //Instantiate(projectileMid, projectilePositionMid.position, Quaternion.identity);
                    break;
                case 1:
                    PoolManager.release(projectileUp, projectilePositionMid.position);
                    PoolManager.release(projectileMid, projectilePositionMid.position);
                    //Instantiate(projectileUp, projectilePositionUp.position, Quaternion.identity);
                   // Instantiate(projectileMid, projectilePositionMid.position, Quaternion.identity);
                    break;
                case 2:
                    PoolManager.release(projectileUp, projectilePositionMid.position);
                    PoolManager.release(projectileMid, projectilePositionMid.position);
                    PoolManager.release(projectileDown, projectilePositionMid.position);
                    //Instantiate(projectileUp, projectilePositionUp.position, Quaternion.identity);
                    //Instantiate(projectileMid, projectilePositionMid.position, Quaternion.identity);
                    //Instantiate(projectileDown, projectilePositionDown.position, Quaternion.identity);
                    break;
                default:
                    break;
            }
            
            yield return waitForSeconds;
        }
        
    }

    #endregion


    #region 受伤

    public override void TakeDamege(float damage)
    {
        base.TakeDamege(damage);

        if(gameObject.activeSelf)
        {
            if(regenerateHealth)
            {
                if (TempCoHealthRegenerate != null)
                    StopCoroutine(TempCoHealthRegenerate);
                TempCoHealthRegenerate=StartCoroutine(CO_HeathRegenerate(waitHealthRegenerateTime, healthRegeneratePercent));
            }
        }
    }

    #endregion
}
