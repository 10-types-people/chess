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
    /// С���������
    /// </summary>
    public bool specialCase;

    private Animator animator; // ���ڿ��ƶ���
    private Collider2D collider; // ���ڼ����ײ

    /// <summary>
    /// ����
    /// </summary>
    public GameObject qiPan;
    public S_Qipan s_Qipan;

    /// <summary>
    /// �ƶ�ʱ�߶�,ԭʼ�߶ȣ����Ӹ߶�
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
    /// �ƶ�ʱ����ʱ��ת�Ƕ�
    /// </summary>
    public float moveAngle = -15f;
    public float eatAngle = 60f;

    /// <summary>
    /// ƽ������
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
    /// ��ʼ������
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
    /// �����ѡ������
    /// </summary>
    private void OnMouseDown()
    {
        if (chessIsActive && !chessIsSelected)
        {
            s_Qipan.PlaySelectedAnim(animator);
        }
    }
    /// <summary>
    /// �ж�Ŀ��λ���Ƿ�ɴ�
    /// </summary>
    /// <param name="wantToPos"></param>
    /// <returns></returns>
    public bool IsAvailablePos(Vector2Int wantToPos)
    {

        int cx = chessCurrentPosition.x;
        int cy = chessCurrentPosition.y;
        int chessColor = chessIsRed ? 1 : -1;
        bool answer = true;
        //���Ŀ��Ϊ�Լ��������򲻿ɴ�
        if (s_Qipan.chessState[wantToPos.x][wantToPos.y] != null && !(s_Qipan.chessState[wantToPos.x][wantToPos.y].chessIsRed ^ chessIsRed))
        {
            return false;
        }
        // ����Ŀ��λ���뵱ǰλ�õĲ�ֵ��ȷ���ƶ�����
        int dx = wantToPos.x - cx;
        int dy = wantToPos.y - cy;
        // ȷ���ƶ������������
        int signDx = Math.Sign(dx);
        int signDy = Math.Sign(dy);
        switch (chessType)
        {
            //�жϳ����ƶ�
            case 0:

                if (dx == 0 ^ dy == 0)
                {
                    // ��ʼ��Ŀ�귽���ϵ����ڸ���
                    int tx = cx + signDx; // ˮƽ��������ڸ���
                    int ty = cy + signDy; // ��ֱ��������ڸ���

                    // ��ֱ�߷��������ֱ������Ŀ��λ�û������ϰ���
                    while (tx != wantToPos.x || ty != wantToPos.y)
                    {
                        if (s_Qipan.chessState[tx][ty] != null)//�������赲
                        {
                            answer = false;
                            break;
                        }
                        else// �ƶ�����һ�����ڸ���
                        {
                            tx += signDx;
                            ty += signDy;
                        }
                    }
                }
                else answer = false;
                break;
            //�ж�����ƶ�
            case 1:
                if (System.Math.Abs(dx) == 1 && System.Math.Abs(dy) == 2)//Ʋ���
                {

                    if (s_Qipan.chessState[cx][cy + signDy] != null) answer = false;
                }
                else if (System.Math.Abs(dx) == 2 && System.Math.Abs(dy) == 1)
                {
                    if (s_Qipan.chessState[cx + signDx][cy] != null) answer = false;
                }
                else answer = false;
                break;
            //�ж�����ƶ�
            case 2:
                //�������Ҳ��ܹ���
                if (System.Math.Abs(dx) == 2 && System.Math.Abs(dy) == 2 && wantToPos.y < 5)
                {
                    if (s_Qipan.chessState[cx + signDx][cy + signDy] != null) answer = false;
                }
                else answer = false;
                break;
            //�ж�ʿ���ƶ�
            case 3:
                //�ڹ̶���Χ��
                if (System.Math.Abs(dx) == 1 && System.Math.Abs(dy) == 1 && wantToPos.y < 3 && wantToPos.x > 2 && wantToPos.x < 6)
                {

                }
                else answer = false;
                break;
            //�жϽ���˧���ƶ�
            case 4:
                //�ڹ̶���Χ��
                if (System.Math.Abs(dx) + System.Math.Abs(dy) == 1 && wantToPos.y < 3 && wantToPos.x > 2 && wantToPos.x < 6)
                {

                }
                else answer = false;
                break;
            //�ж��ڵ��ƶ�
            case 5:
                if (dx == 0 ^ dy == 0)
                {
                    int barrirCount = 0;
                    // ��ʼ��Ŀ�귽���ϵ����ڸ���
                    int tx = cx + signDx; // ˮƽ��������ڸ���
                    int ty = cy + signDy; // ��ֱ��������ڸ���

                    // ��ֱ�߷��������ֱ������Ŀ��λ�û������ϰ���
                    while (tx != wantToPos.x || ty != wantToPos.y)
                    {
                        if (s_Qipan.chessState[tx][ty] != null && barrirCount > 0)//�������赲,�����һ��������Ŀ��Ϊ�Է�����
                        {
                            answer = false;
                            break;
                        }
                        else// �ƶ�����һ�����ڸ���
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
            //�жϱ�������ƶ�
            case 6:
                //���Ӻ�������ֻ��������ǰ��
                if (specialCase)
                {
                    if (dy >= 0 && System.Math.Abs(dx) + dy == 1)
                    {
                    }
                    else answer = false;
                }
                //δ���ӵ������ֻ����ǰ��
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
    /// �ƶ�����Ӷ���
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
    /// Э�������ƶ�����
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
        //����
        float elapsed = 0;
        while (elapsed < upTime)
        {
            transform.localPosition = UnityEngine.Vector3.Lerp(startPosition, startUpPosition, elapsed / upTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = startUpPosition;
        //�ƶ�
        ///��ת��
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

        //��������
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

        //�½�
        elapsed = 0;
        //��ͨ�½�
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

        //���������ϵ�����λ��
        s_Qipan.chessState[chessCurrentPosition.x][chessCurrentPosition.y] = null;
        //���Ŀ��λ�����������ƿ�
        if (s_Qipan.chessState[targetPos.x][targetPos.y]) s_Qipan.chessState[targetPos.x][targetPos.y].transform.localPosition = new UnityEngine.Vector3(0, -1, 0);
        s_Qipan.chessState[targetPos.x][targetPos.y] = GetComponent<S_ChessAction>();

        chessCurrentPosition = targetPos;
        if (chessCurrentPosition.y > 4) specialCase = true;
        s_Qipan.selectedChess = null;
        chessIsSelected = false;

    }
}


