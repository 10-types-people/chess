using UnityEngine;
using TMPro;
using System.Collections;
public class DisableTextMeshProRaycast : MonoBehaviour
{
    public TextMeshProUGUI textMeshProComponent;
    public TMP_InputField inputField;

    public float moveTime = 0.3f; // �ƶ�����ʱ��
    public float moveDistance = 6f;
    public Vector3 originalPosition;
    public Vector3 targetPosition;
    public Color originalColor;
    public Color targetColor;
    void Start()
    {
        if (textMeshProComponent != null)
        {
            // ����TextMeshPro������ɵ��
            textMeshProComponent.raycastTarget = false;
        }
        originalPosition = textMeshProComponent.transform.position;
        targetPosition = originalPosition + new Vector3(0, moveDistance, 0);

        originalColor = textMeshProComponent.color;
        targetColor = Color.white;
    }

    /// <summary>
    /// ��ʾ���ֵ������ƶ�
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

        // ��ʼ��ʱ��
        float timeElapsed = 0;

        while (timeElapsed < moveTime)
        {
            // �����ֵ��t = timeElapsed / moveDuration��
            float t = timeElapsed / moveTime;
            // ��ֵ�ƶ�������ɫ
            transform.position = Vector3.Lerp(position1, position2, t);
            textMeshProComponent.color=Color.Lerp(color1, color2, t);
            yield return null;
            // ����ʱ��
            timeElapsed += Time.deltaTime;
        }

        // ȷ������λ����Ŀ��λ��
        transform.position = position2;
        textMeshProComponent.color = color2;
    }
}