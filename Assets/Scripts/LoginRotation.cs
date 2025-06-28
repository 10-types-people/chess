using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class LoginRotation : MonoBehaviour
{
    public static bool startRotationFlag=true;
    
    public Transform rotationCenter;
    public float rotationSpeed = -20f; // 旋转速度，单位为度/秒
    Vector3 startPosition;
    public Vector3 targetPos =new Vector3(0,-1.3f,0);
    public float duration = 0.3f; // 移动持续时间
    public float moveRatio = 0.8f;

    /// <summary>
    /// 遮挡视野时动画
    /// </summary>
    public Vector3 targetPosMask;
    public float targetYMask = 9.5f;
    // 目标Y坐标
    public float durationMask = 0.6f; // 动画持续时间
    public float rotationTimeMask = 3f;
    private float rotationSpeedMask;
    public float outRatio = 5f;

    public float offsetValue = 180f;
    private void Start()
    {
        targetPosMask = new Vector3(targetPos.x, targetYMask, targetPos.z);
        rotationSpeedMask = (rotationTimeMask * 360f + offsetValue) / durationMask;
        StartCoroutine(StartRotation());
    }
    void Update()
    {
        
    }
    /// <summary>
    /// 初始时，棋子开始转动
    /// </summary>
    /// <returns></returns>
    IEnumerator StartRotation()
    {
        while (startRotationFlag)
        {
            Quaternion rotation = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
            transform.localRotation = transform.localRotation * rotation;
            // 等待下一帧
            yield return null;
        }
    }
    /// <summary>
    /// 登录时，棋子向中间移动
    /// </summary>
    public void LoginAnim(bool flag)
    {
        // 启动协程，开始移动
        StartCoroutine(MoveTowardsTarget(flag));
    }
    IEnumerator MoveTowardsTarget(bool flag)
    {
        float elapsedTime = 0; // 记录经过的时间
        startPosition = transform.position; // 记录起始位置
        float movetime = flag ? duration : moveRatio * duration;
        // 当经过的时间小于持续时间时，继续执行
        while (elapsedTime < movetime)
        {
            // 计算插值比例，从0到1
            float t = elapsedTime / duration;

            // 计算当前的插值位置
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPos, t);

            // 更新物体的位置
            transform.position = currentPosition;

            // 等待下一帧
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保物体到达目标位置
        //transform.position = targetPos;
    }


    /// <summary>
    /// 成功登录后，场景遮挡
    /// </summary>
    public void MaskAnim()
    {
        
        StartCoroutine(AnimateMask());
    }
    IEnumerator AnimateMask()
    {
        float elapsedTime = 0;// 记录开始时间

        while (elapsedTime < durationMask)
        {
            
            float t = elapsedTime / durationMask;

            // 计算当前应该旋转的角度，并应用旋转
            float rotateAngle = rotationSpeedMask * Time.deltaTime;
            Quaternion rotation = Quaternion.Euler(rotateAngle, 0, 0);
            transform.localRotation = transform.localRotation * rotation;
            // 移动物体到目标位置
            transform.localPosition = Vector3.Lerp(targetPos, targetPosMask, t);            

            elapsedTime += Time.deltaTime;
            // 等待下一帧
            yield return null;
        }

        // 确保物体到达目标位置和旋转角度
         transform.localPosition = targetPosMask;
        
    }

    public void OutAnim()
    {
        StartCoroutine(AnimateOut());
    }
    IEnumerator AnimateOut()
    {
        float elapsedTime = 0;// 记录开始时间
        startPosition = startPosition * outRatio;

        while (elapsedTime < durationMask)
        {

            float t = elapsedTime / durationMask;

            // 计算当前应该旋转的角度，并应用旋转
            float rotateAngle = rotationSpeedMask * Time.deltaTime;
            Quaternion rotation = Quaternion.Euler(rotateAngle, 0, 0);
            transform.localRotation = transform.localRotation * rotation;
            // 移动物体到目标位置
            transform.localPosition = Vector3.Lerp(targetPos, startPosition, t);

            elapsedTime += Time.deltaTime;
            // 等待下一帧
            yield return null;
        }

        // 确保物体到达目标位置和旋转角度
        transform.localPosition = startPosition;
    }
}
