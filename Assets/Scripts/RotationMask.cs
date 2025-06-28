using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMask : MonoBehaviour
{

    public Vector3 rawPos; // ��¼����ĳ�ʼY����
    public Vector3 targetPos;
    public float targetY = 10.5f;
     // Ŀ��Y����
    float duration = 2.0f; // ��������ʱ��
    float rotationTime= 3f ;

    private float rotationSpeed;




    public LoginRotation loginRotation;
    void Start()
    {
      
        rawPos = transform.position;
        targetPos = new Vector3(rawPos.x, targetY, rawPos.z);
        rotationSpeed = (rotationTime * 360 + 180) / duration;

        // ����Э��
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
        float elapsedTime = 0;// ��¼��ʼʱ��

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // �ƶ����嵽Ŀ��λ��
            //transform.localPosition = Vector3.Lerp(rawPos, targetPos, t);

            // ���㵱ǰӦ����ת�ĽǶȣ���Ӧ����ת
           float rotateAngle = rotationSpeed * Time.deltaTime;
            Quaternion rotation = Quaternion.Euler(rotateAngle,0, 0);
            transform.localRotation = transform.localRotation * rotation;

            // �ȴ���һ֡
            yield return null;
        }

        // ȷ�����嵽��Ŀ��λ�ú���ת�Ƕ�
       // transform.localPosition = targetPos;
        transform.Rotate(0, rotationSpeed * rotationTime, 0);
    }
}
