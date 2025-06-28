using UnityEngine;
using TMPro;
using System.Collections;
public class DisableTextMeshProRaycast : MonoBehaviour
{
    public TextMeshProUGUI textMeshProComponent;
    public TMP_InputField inputField;

    public float moveTime = 0.3f; // 移动持续时间
    public float moveDistance = 6f;
    public Vector3 originalPosition;
    public Vector3 targetPosition;
    public Color originalColor;
    public Color targetColor;
    void Start()
    {
        if (textMeshProComponent != null)
        {
            // 设置TextMeshPro组件不可点击
            textMeshProComponent.raycastTarget = false;
        }
        originalPosition = textMeshProComponent.transform.position;
        targetPosition = originalPosition + new Vector3(0, moveDistance, 0);

        originalColor = textMeshProComponent.color;
        targetColor = Color.white;
    }

    /// <summary>
    /// 提示文字的上下移动
    /// </summary>
    public void moveUp()
    {
        if (string.IsNullOrWhiteSpace(inputField.text))
        {
            StartCoroutine(MoveTextMeshProCoroutine(originalPosition, targetPosition, originalColor, targetColor));
        }
        
    }
    public void moveDown()
    {
        if (string.IsNullOrWhiteSpace(inputField.text))
        {
            StartCoroutine(MoveTextMeshProCoroutine(targetPosition, originalPosition, targetColor, originalColor));
        }
    }
    private IEnumerator MoveTextMeshProCoroutine(Vector3 position1, Vector3 position2,Color color1,Color color2)
    {

        // 初始化时间
        float timeElapsed = 0;

        while (timeElapsed < moveTime)
        {
            // 计算插值（t = timeElapsed / moveDuration）
            float t = timeElapsed / moveTime;
            // 插值移动更改颜色
            transform.position = Vector3.Lerp(position1, position2, t);
            textMeshProComponent.color=Color.Lerp(color1, color2, t);
            yield return null;
            // 更新时间
            timeElapsed += Time.deltaTime;
        }

        // 确保最终位置是目标位置
        transform.position = position2;
        textMeshProComponent.color = color2;
    }
}