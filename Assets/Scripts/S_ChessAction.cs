using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public class S_ChessAction : MonoBehaviour
{
    // Start is called before the first frame update

    public bool chessIsRed;

    public int chessType;
    public bool chessIsActive;
    public bool chessIsSelected;
    public Vector2Int chessCurrentPosition;
    public float chessValue;
    public Vector<Vector2Int> availablePos;
    /// <summary>
    /// 小兵过河情况
    /// </summary>
    public bool specialCase;

    private Animator animator; // 用于控制动画
    private Collider2D collider; // 用于检测碰撞

    /// <summary>
    /// 棋盘
    /// </summary>
    public GameObject qiPan;
    public S_Qipan s_Qipan;

    /// <summary>
    /// 移动时高度,原始高度，吃子高度
    /// </summary>
    public float moveY = 0.3f;
    public float zeroY = 0f;
    public float eatY = 1.5f;
    public float deadY = -1f;
    public float moveSpeed = 0.5f;
    public float upTime = 0.1f;
    public float moveTime = 0.4f;
    public float downTime = 0.1f;
    public float eatTime = 0.3f;
    /// <summary>
    /// 移动时吃子时旋转角度
    /// </summary>
    public float moveAngle = -15f;
    public float eatAngle = 60f;

    /// <summary>
    /// 平均步伐
    /// </summary>
    public float averageFeet = 0.8f;

    void Start()
    {
        animator = GetComponent<Animator>();
        qiPan = GameObject.FindWithTag("QiPan");
        s_Qipan = qiPan.GetComponent<S_Qipan>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 初始化属性
    /// </summary>
    /// <param name="isRed"></param>
    /// <param name="type"></param>
    /// <param name="isActive"></param>
    /// <param name="currentPosition"></param>
    /// <param name="value"></param>
    public void InitialProperties(bool isRed, int type, bool isActive, Vector2Int currentPosition, float value)
    {
        chessIsRed = isRed;
        chessType = type;
        chessIsActive = isActive;
        chessIsSelected = false;
        chessCurrentPosition = currentPosition;
        chessValue = value;
        specialCase = false;
    }

    /// <summary>
    /// 鼠标点击选中棋子
    /// </summary>
    private void OnMouseDown()
    {
        if (chessIsActive && !chessIsSelected)
        {
            s_Qipan.PlaySelectedAnim(animator);
        }
    }
    /// <summary>
    /// 判断目标位置是否可达
    /// </summary>
    /// <param name="wantToPos"></param>
    /// <returns></returns>
    public bool IsAvailablePos(Vector2Int wantToPos)
    {

        int cx = chessCurrentPosition.x;
        int cy = chessCurrentPosition.y;
        int chessColor = chessIsRed ? 1 : -1;
        bool answer = true;
        //如果目标为自己的棋子则不可达
        if (s_Qipan.chessState[wantToPos.x][wantToPos.y] != null && !(s_Qipan.chessState[wantToPos.x][wantToPos.y].chessIsRed ^ chessIsRed))
        {
            return false;
        }
        // 计算目标位置与当前位置的差值，确定移动方向
        int dx = wantToPos.x - cx;
        int dy = wantToPos.y - cy;
        // 确定移动方向的正负号
        int signDx = Math.Sign(dx);
        int signDy = Math.Sign(dy);
        switch (chessType)
        {
            //判断车的移动
            case 0:

                if (dx == 0 ^ dy == 0)
                {
                    // 初始化目标方向上的相邻格子
                    int tx = cx + signDx; // 水平方向的相邻格子
                    int ty = cy + signDy; // 垂直方向的相邻格子

                    // 沿直线方向遍历，直到到达目标位置或遇到障碍物
                    while (tx != wantToPos.x || ty != wantToPos.y)
                    {
                        if (s_Qipan.chessState[tx][ty] != null)//有棋子阻挡
                        {
                            answer = false;
                            break;
                        }
                        else// 移动到下一个相邻格子
                        {
                            tx += signDx;
                            ty += signDy;
                        }
                    }
                }
                else answer = false;
                break;
            //判断马的移动
            case 1:
                if (System.Math.Abs(dx) == 1 && System.Math.Abs(dy) == 2)//撇马脚
                {

                    if (s_Qipan.chessState[cx][cy + signDy] != null) answer = false;
                }
                else if (System.Math.Abs(dx) == 2 && System.Math.Abs(dy) == 1)
                {
                    if (s_Qipan.chessState[cx + signDx][cy] != null) answer = false;
                }
                else answer = false;
                break;
            //判断象的移动
            case 2:
                //填象心且不能过河
                if (System.Math.Abs(dx) == 2 && System.Math.Abs(dy) == 2 && wantToPos.y < 5)
                {
                    if (s_Qipan.chessState[cx + signDx][cy + signDy] != null) answer = false;
                }
                else answer = false;
                break;
            //判断士的移动
            case 3:
                //在固定范围内
                if (System.Math.Abs(dx) == 1 && System.Math.Abs(dy) == 1 && wantToPos.y < 3 && wantToPos.x > 2 && wantToPos.x < 6)
                {

                }
                else answer = false;
                break;
            //判断将、帅的移动
            case 4:
                //在固定范围内
                if (System.Math.Abs(dx) + System.Math.Abs(dy) == 1 && wantToPos.y < 3 && wantToPos.x > 2 && wantToPos.x < 6)
                {

                }
                else answer = false;
                break;
            //判断炮的移动
            case 5:
                if (dx == 0 ^ dy == 0)
                {
                    int barrirCount = 0;
                    // 初始化目标方向上的相邻格子
                    int tx = cx + signDx; // 水平方向的相邻格子
                    int ty = cy + signDy; // 垂直方向的相邻格子

                    // 沿直线方向遍历，直到到达目标位置或遇到障碍物
                    while (tx != wantToPos.x || ty != wantToPos.y)
                    {
                        if (s_Qipan.chessState[tx][ty] != null && barrirCount > 0)//有棋子阻挡,最多有一个棋子且目标为对方棋子
                        {
                            answer = false;
                            break;
                        }
                        else// 移动到下一个相邻格子
                        {
                            if (s_Qipan.chessState[tx][ty] != null) barrirCount++;
                            tx += signDx;
                            ty += signDy;
                        }
                    }
                    if (tx == wantToPos.x && ty == wantToPos.y)
                    {
                        if (barrirCount == 0 ^ s_Qipan.chessState[tx][ty] == null) answer = false;

                    }
                }
                else answer = false;
                break;
            //判断兵，卒的移动
            case 6:
                //过河后的情况，只能往左右前走
                if (specialCase)
                {
                    if (dy >= 0 && System.Math.Abs(dx) + dy == 1)
                    {
                    }
                    else answer = false;
                }
                //未过河的情况，只能往前走
                else
                {
                    if (dx == 0 && dy == 1)
                    {
                    }
                    else answer = false;
                }
                break;
        }

        return answer;
    }
    /// <summary>
    /// 移动或吃子动作
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="moveType"></param>
    public void Move(Vector2Int targetPos, int moveType)
    {
        Debug.Log("MoveType: " + moveType);

        UnityEngine.Vector3 startPosition = new UnityEngine.Vector3(s_Qipan.zeroX - chessCurrentPosition.x * s_Qipan.feetX, zeroY, s_Qipan.zeroZ - chessCurrentPosition.y * s_Qipan.feetZ);
        UnityEngine.Vector3 endPosition = new UnityEngine.Vector3(s_Qipan.zeroX - targetPos.x * s_Qipan.feetX, zeroY, s_Qipan.zeroZ - targetPos.y * s_Qipan.feetZ);
        StartCoroutine(MoveChessCoroutine(startPosition, endPosition, moveType, targetPos));
    }
    /// <summary>
    /// 协程用于移动动画
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="endPosition"></param>
    /// <returns></returns>
    IEnumerator MoveChessCoroutine(UnityEngine.Vector3 startPosition, UnityEngine.Vector3 endPosition, int moveType, Vector2Int targetPos)
    {
        UnityEngine.Vector3 startUpPosition = startPosition;
        startUpPosition.y = moveY;
        UnityEngine.Vector3 endUpPosition = endPosition;
        endUpPosition.y = moveY;
        UnityEngine.Vector3 eatPosition = endPosition;
        eatPosition.y = eatY;

        float distance = UnityEngine.Vector3.Distance(startUpPosition, endUpPosition);
        if (distance <= averageFeet * 2)
        {
            moveTime = 0.2f;
        }
        else if (distance < averageFeet * 6)
        {
            moveTime = 0.3f;
        }
        else moveTime = 0.4f;
        //上升
        float elapsed = 0;
        while (elapsed < upTime)
        {
            transform.localPosition = UnityEngine.Vector3.Lerp(startPosition, startUpPosition, elapsed / upTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = startUpPosition;
        //移动
        ///旋转轴
        UnityEngine.Vector2 aVector = new UnityEngine.Vector2(startUpPosition.z - endUpPosition.z, endUpPosition.x - startUpPosition.x);
        UnityEngine.Vector3 verticalVector = new UnityEngine.Vector3(aVector.x, 0, aVector.y);

        Debug.Log(startUpPosition + ",,,," + endUpPosition + ",,,," + verticalVector);
        UnityEngine.Quaternion fromRotation = transform.localRotation;
        UnityEngine.Quaternion endRotation = UnityEngine.Quaternion.AngleAxis(moveAngle, verticalVector) * fromRotation;
        elapsed = 0;
        while (elapsed < moveTime)
        {
            transform.localPosition = UnityEngine.Vector3.Lerp(startUpPosition, endUpPosition, elapsed / moveTime);
            if (moveType == 1 && elapsed < moveTime / 5)
            {
                transform.localRotation = UnityEngine.Quaternion.Lerp(fromRotation, endRotation, elapsed / (moveTime / 5));
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (moveType == 1) transform.rotation = endRotation;
        transform.localPosition = startUpPosition;

        //吃子上升
        UnityEngine.Quaternion eatRotation = UnityEngine.Quaternion.AngleAxis(eatAngle, verticalVector) * fromRotation;
        if (moveType == 1)
        {
            elapsed = 0;
            while (elapsed < eatTime)
            {
                transform.localPosition = UnityEngine.Vector3.Lerp(endUpPosition, eatPosition, elapsed / eatTime);
                transform.localRotation = UnityEngine.Quaternion.Lerp(endRotation, eatRotation, elapsed / eatTime);

                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.localRotation = eatRotation;
            transform.localPosition = startUpPosition;
        }

        //下降
        elapsed = 0;
        //普通下降
        if (moveType == 0)
        {
            while (elapsed < downTime)
            {
                transform.localPosition = UnityEngine.Vector3.Lerp(endUpPosition, endPosition, elapsed / downTime);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (elapsed < downTime)
            {
                transform.localPosition = UnityEngine.Vector3.Lerp(eatPosition, endPosition, elapsed / downTime);
                transform.localRotation = UnityEngine.Quaternion.Lerp(eatRotation, fromRotation, elapsed / downTime);

                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.localRotation = fromRotation;
        }
        transform.localPosition = endPosition;
        moveFinishedEvent(targetPos);
    }
    void moveFinishedEvent(Vector2Int targetPos)
    {

        //更新棋盘上的棋子位置
        s_Qipan.chessState[chessCurrentPosition.x][chessCurrentPosition.y] = null;
        //如果目标位置有棋子则移开
        if (s_Qipan.chessState[targetPos.x][targetPos.y]) s_Qipan.chessState[targetPos.x][targetPos.y].transform.localPosition = new UnityEngine.Vector3(0, -1, 0);
        s_Qipan.chessState[targetPos.x][targetPos.y] = GetComponent<S_ChessAction>();

        chessCurrentPosition = targetPos;
        if (chessCurrentPosition.y > 4) specialCase = true;
        s_Qipan.selectedChess = null;
        chessIsSelected = false;

    }
}


