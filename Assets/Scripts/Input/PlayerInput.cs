using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
//玩家输入动作表管理类，
//inputsystem信号的接收与
//发送事件给player
[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, PlayerInputActions.IGamePlayActions
{
    //动作表的生成类
    PlayerInputActions inputActions;

    //初始化时给空委托
    // 移动和停止移动事件
    public event UnityAction<Vector2> OnMoveEvent = delegate { };
    public event UnityAction OnStopMoveEvent = delegate { };
    private void OnEnable()
    {
        //初始化
        inputActions = new PlayerInputActions();
        //登记回调函数
        inputActions.GamePlay.SetCallbacks(this);

    }
    private void OnDisable()
    {
        DisableGamePlayInput();
    }

    //方便其他类启用与关闭动作表
    public void EnableGamePlayInput()
    {
        //
        inputActions.GamePlay.Enable();
        //隐藏鼠标
        Cursor.visible = false;
    }
    public void DisableGamePlayInput()
    {
        inputActions.GamePlay.Disable();
    }



    public void OnMove(InputAction.CallbackContext context)
    {
        
        //按住按键时调用 onMove事件
        if(context.phase ==InputActionPhase.Performed)
        {
            OnMoveEvent.Invoke( context.ReadValue<Vector2>() );
        }
        
        //松开按键时调用 onStopMove事件
        if (context.phase ==InputActionPhase.Canceled)
        {
            OnStopMoveEvent.Invoke();
        }
        
    }
}
