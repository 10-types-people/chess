using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PolygonCollider2D))]
public class PolygonImageClick : MonoBehaviour, ICanvasRaycastFilter
{
    /// <summary>
    /// 2D�������ײ��
    /// </summary>
    protected PolygonCollider2D m_polygonCollider2D;
    protected Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
        m_polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector3 worldPos;
        //����Ļ�ϵĵ�ת��Ϊ���������еĵ㣬���ǵ��˾��Σ�RectTransform���ı�������ϵ
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_image.rectTransform, screenPoint, eventCamera, out worldPos);
        return m_polygonCollider2D.OverlapPoint(worldPos);
    }
}
