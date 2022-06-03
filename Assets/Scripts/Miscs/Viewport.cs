using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//单例即可，限制玩家的移动范围
public class Viewport : Singleton<Viewport>
{
    float minX,maxX;
    float minY,maxY;

    float middleX;
    private void Start()
    {
        Camera camera =Camera.main;
        //左下角坐标
        Vector2 bottomleft = camera.ViewportToWorldPoint(new Vector3(0f,0f));
        minX = bottomleft.x;
        minY = bottomleft.y;
        Vector2 topright = camera.ViewportToWorldPoint(new Vector3(1f, 1f));
        maxX = topright.x;
        maxY = topright.y;


        middleX=camera.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0f)).x;
    }

    //玩家可移动的距离
    //padding 是模型本身的大小，如果要复用在调用的协程直接传入相对应的参数即可
    public Vector3 PlayerMoveablePostion(Vector3 curPos, float paddingx=0.8f,float paddingy=0.22f)
    {
        Vector3 res = Vector3.zero;
        //限制坐标大小
        res.x =Mathf.Clamp(curPos.x, minX + paddingx, maxX - paddingx);
        res.y=Mathf.Clamp(curPos.y, minY + paddingy, maxY - paddingy);
        return res;
    }

    //enemy出生点随机
    public Vector3 RandomEnemySpawnPosition(float paddingx = 0.8f, float paddingy = 0.22f)
    {
        Vector3 res = Vector3.zero;
        res.x = maxX+paddingx;
        res.y = Random.Range(minY+paddingy, maxY-paddingy);

        return res;
    }

    
    public Vector3 RandomRightHalfPosition(float paddingx = 0.8f, float paddingy = 0.22f)
    {
        Vector3 res = Vector3.zero;
        res.x = Random.Range(middleX, maxX - paddingx);
        res.y = Random.Range(minY + paddingy, maxY - paddingy);

        return res;
    }

}
