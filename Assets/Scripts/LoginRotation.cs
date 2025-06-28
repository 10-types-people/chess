using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class LoginRotation : MonoBehaviour
{
    public static bool startRotationFlag=true;
    
    public Transform rotationCenter;
    public float rotationSpeed = -20f; // ��ת�ٶȣ���λΪ��/��
    Vector3 startPosition;
    public Vector3 targetPos =new Vector3(0,-1.3f,0);
    public float duration = 0.3f; // �ƶ�����ʱ��
    public float moveRatio = 0.8f;

    /// <summary>
    /// �ڵ���Ұʱ����
    /// </summary>
    public Vector3 targetPosMask;
    public float targetYMask = 9.5f;
    // Ŀ��Y����
    public float durationMask = 0.6f; // ��������ʱ��
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
    /// ��ʼʱ�����ӿ�ʼת��
    /// </summary>
    /// <returns></returns>
    IEnumerator StartRotation()
    {
        while (startRotationFlag)
        {
            Quaternion rotation = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
            transform.localRotation = transform.localRotation * rotation;
            // �ȴ���һ֡
            yield return null;
        }
    }
    /// <summary>
    /// ��¼ʱ���������м��ƶ�
    /// </summary>
    public void LoginAnim(bool flag)
    {
        // ����Э�̣���ʼ�ƶ�
        StartCoroutine(MoveTowardsTarget(flag));
    }
    IEnumerator MoveTowardsTarget(bool flag)
    {
        float elapsedTime = 0; // ��¼������ʱ��
        startPosition = transform.position; // ��¼��ʼλ��
        float movetime = flag ? duration : moveRatio * duration;
        // ��������ʱ��С�ڳ���ʱ��ʱ������ִ��
        while (elapsedTime < movetime)
        {
            // �����ֵ��������0��1
            float t = elapsedTime / duration;

            // ���㵱ǰ�Ĳ�ֵλ��
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPos, t);

            // ���������λ��
            transform.position = currentPosition;

            // �ȴ���һ֡
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ȷ�����嵽��Ŀ��λ��
        //transform.position = targetPos;
    }


    /// <summary>
    /// �ɹ���¼�󣬳����ڵ�
    /// </summary>
    public void MaskAnim()
    {
        
        StartCoroutine(AnimateMask());
    }
    IEnumerator AnimateMask()
    {
        float elapsedTime = 0;// ��¼��ʼʱ��

        while (elapsedTime < durationMask)
        {
            
            float t = elapsedTime / durationMask;

            // ���㵱ǰӦ����ת�ĽǶȣ���Ӧ����ת
            float rotateAngle = rotationSpeedMask * Time.deltaTime;
            Quaternion rotation = Quaternion.Euler(rotateAngle, 0, 0);
            transform.localRotation = transform.localRotation * rotation;
            // �ƶ����嵽Ŀ��λ��
            transform.localPosition = Vector3.Lerp(targetPos, targetPosMask, t);            

            elapsedTime += Time.deltaTime;
            // �ȴ���һ֡
            yield return null;
        }

        // ȷ�����嵽��Ŀ��λ�ú���ת�Ƕ�
         transform.localPosition = targetPosMask;
        
    }

    public void OutAnim()
    {
        StartCoroutine(AnimateOut());
    }
    IEnumerator AnimateOut()
    {
        float elapsedTime = 0;// ��¼��ʼʱ��
        startPosition = startPosition * outRatio;

        while (elapsedTime < durationMask)
        {

            float t = elapsedTime / durationMask;

            // ���㵱ǰӦ����ת�ĽǶȣ���Ӧ����ת
            float rotateAngle = rotationSpeedMask * Time.deltaTime;
            Quaternion rotation = Quaternion.Euler(rotateAngle, 0, 0);
            transform.localRotation = transform.localRotation * rotation;
            // �ƶ����嵽Ŀ��λ��
            transform.localPosition = Vector3.Lerp(targetPos, startPosition, t);

            elapsedTime += Time.deltaTime;
            // �ȴ���һ֡
            yield return null;
        }

        // ȷ�����嵽��Ŀ��λ�ú���ת�Ƕ�
        transform.localPosition = startPosition;
    }
}
