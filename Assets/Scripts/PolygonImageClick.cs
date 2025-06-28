using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PolygonCollider2D))]
public class PolygonImageClick : MonoBehaviour, ICanvasRaycastFilter
{
    /// <summary>
    /// 2D多边形碰撞器
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
        //将屏幕上的点转换为世界坐标中的点，考虑到了矩形（RectTransform）的本地坐标系
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_image.rectTransform, screenPoint, eventCamera, out worldPos);
        return m_polygonCollider2D.OverlapPoint(worldPos);
    }
}
