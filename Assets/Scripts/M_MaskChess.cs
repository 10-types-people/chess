using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_MaskChess : MonoBehaviour
{
    public float dropDuration = 3f;
    public Vector3 dropRawPos=new Vector3(0f,1.1f,-9f);
    public Vector3 dropTargetPos= new Vector3(0f, -2.4f, -9f);
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DropDown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 遮挡棋子下落动画
    /// </summary>
    /// <returns></returns>
    IEnumerator DropDown()
    {
        float elapsedTime = 0;// 记录开始时间

        while (elapsedTime < dropDuration)
        {
            float t = elapsedTime / dropDuration;

            // 移动物体到目标位置
            transform.localPosition = Vector3.Lerp(dropRawPos, dropTargetPos, t);

            elapsedTime += Time.deltaTime;
            // 等待下一帧
            yield return null;
        }
        transform.localPosition = dropTargetPos;
    }
}
