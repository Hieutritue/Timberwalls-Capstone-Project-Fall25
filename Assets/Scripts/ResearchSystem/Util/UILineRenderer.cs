using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UISquareLineRenderer : Graphic
{
    public RectTransform from;
    public RectTransform to;
    public float thickness = 5f;
    public float minHorizontalOffset = 50f; // optional extra spacing

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if (from == null || to == null) return;

        // Get edge positions
        Vector2 start = WorldToLocal(GetEdgePosition(from, true));
        Vector2 end = WorldToLocal(GetEdgePosition(to, false));

        float midX = (start.x + end.x) / 2f;

        if (Mathf.Abs(end.x - start.x) < minHorizontalOffset * 2f)
        {
            if (end.x > start.x)
                midX = start.x + minHorizontalOffset;
            else
                midX = start.x - minHorizontalOffset;
        }

        // Define corner points
        Vector2 corner1 = new Vector2(midX, start.y);
        Vector2 corner2 = new Vector2(midX, end.y);

        // Draw 3 orthogonal segments
        DrawSegment(vh, start, corner1);
        DrawSegment(vh, corner1, corner2);
        DrawSegment(vh, corner2, end);
    }

    private void DrawSegment(VertexHelper vh, Vector2 a, Vector2 b)
    {
        Vector2 dir = (b - a).normalized;
        Vector2 normal = new Vector2(-dir.y, dir.x) * (thickness / 2f);

        UIVertex v1 = UIVertex.simpleVert;
        UIVertex v2 = UIVertex.simpleVert;
        UIVertex v3 = UIVertex.simpleVert;
        UIVertex v4 = UIVertex.simpleVert;

        v1.color = v2.color = v3.color = v4.color = color;

        v1.position = a + normal;
        v2.position = a - normal;
        v3.position = b - normal;
        v4.position = b + normal;

        vh.AddUIVertexQuad(new[] { v1, v2, v3, v4 });
    }

    private Vector2 WorldToLocal(Vector3 worldPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            RectTransformUtility.WorldToScreenPoint(null, worldPos),
            null,
            out Vector2 localPos);
        return localPos;
    }

    private Vector3 GetEdgePosition(RectTransform rect, bool rightEdge)
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        return rightEdge
            ? (corners[2] + corners[3]) * 0.5f
            : (corners[0] + corners[1]) * 0.5f;
    }

    private void Update()
    {
        SetVerticesDirty();
    }
}
