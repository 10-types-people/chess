using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMask : MonoBehaviour
{

    public Vector3 rawPos; // 记录物体的初始Y坐标
    public Vector3 targetPos;
    public float targetY = 10.5f;
     // 目标Y坐标
    float duration = 2.0f; // 动画持续时间
    float rotationTime= 3f ;

    private float rotationSpeed;




    public LoginRotation loginRotation;
    void Start()
    {
      
        rawPos = transform.position;
        targetPos = new Vector3(rawPos.x, targetY, rawPos.z);
        rotationSpeed = (rotationTime * 360 + 180) / duration;

        // 启动协程
        // StartCoroutine(AnimateObject());
        loginRotation.MaskAnim();
        Debug.Log("12345");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator AnimateObject()
    {
        float elapsedTime = 0;// 记录开始时间

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // 移动物体到目标位置
            //transform.localPosition = Vector3.Lerp(rawPos, targetPos, t);

            // 计算当前应该旋转的角度，并应用旋转
           float rotateAngle = rotationSpeed * Time.deltaTime;
            Quaternion rotation = Quaternion.Euler(rotateAngle,0, 0);
            transform.localRotation = transform.localRotation * rotation;

            // 等待下一帧
            yield return null;
        }

        // 确保物体到达目标位置和旋转角度
       // transform.localPosition = targetPos;
        transform.Rotate(0, rotationSpeed * rotationTime, 0);
    }
}
