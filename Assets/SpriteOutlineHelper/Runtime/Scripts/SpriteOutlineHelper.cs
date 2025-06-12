using UnityEngine;

public static class SpriteOutlineHelper
{
    // 靜態方法：為指定 GameObject 添加外框
    public static void AddOutline(GameObject target, Color outlineColor, float outlineSize)
    {
        if (target == null)
        {
            Debug.LogWarning("Target GameObject is null!");
            return;
        }

        SpriteRenderer spriteRenderer = target.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("Target has no SpriteRenderer component!");
            return;
        }

        GameObject outline = new GameObject("Outline");
        outline.transform.parent = target.transform;
        outline.transform.localScale = Vector3.one * (1 + outlineSize);
        outline.transform.localPosition = Vector3.zero;

        SpriteRenderer outlineRenderer = outline.AddComponent<SpriteRenderer>();
        outlineRenderer.sprite = spriteRenderer.sprite;
        outlineRenderer.color = outlineColor;
        outlineRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
    }

    // 靜態方法：移除所有外框
    public static void RemoveAllOutlines(GameObject target)
    {
        if (target == null) return;

        foreach (Transform child in target.transform)
        {
            if (child.name == "Outline" || child.name == "TempOutline")
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    // 靜態方法：為 Sprite 添加滑鼠懸停高亮效果
    public static void AddMouseOverHighlight(GameObject target, Color highlightColor, float outlineSize = 0.1f)
    {
        if (target == null) return;

        // 確保目標有 Collider2D
        if (target.GetComponent<Collider2D>() == null)
        {
            Debug.LogWarning("Target needs a Collider2D for mouse interaction!");
            target.AddComponent<BoxCollider2D>();
        }

        // 添加或取得 HighlightBehaviour 組件
        HighlightBehaviour behaviour = target.GetComponent<HighlightBehaviour>();
        if (behaviour == null)
        {
            behaviour = target.AddComponent<HighlightBehaviour>();
        }

        behaviour.SetHighlightSettings(highlightColor, outlineSize);
        behaviour.enabled = true; // 確保組件啟用
    }

    // 新增靜態方法：移除滑鼠懸停高亮效果
    public static void RemoveMouseOverHighlight(GameObject target)
    {
        if (target == null) return;

        HighlightBehaviour behaviour = target.GetComponent<HighlightBehaviour>();
        if (behaviour != null)
        {
            // 強制清除當前可能存在的臨時外框
            behaviour.ForceRemoveOutline();
            
            // 禁用組件（保留組件但不再響應滑鼠事件）
            behaviour.enabled = false;
            
            // 若想完全移除組件，改用以下行：
            // GameObject.Destroy(behaviour);
        }
    }
}

// 內部類別：處理滑鼠事件的 MonoBehaviour
internal class HighlightBehaviour : MonoBehaviour
{
    private Color highlightColor;
    private float outlineSize;
    private GameObject currentOutline;

    public void SetHighlightSettings(Color color, float size)
    {
        highlightColor = color;
        outlineSize = size;
    }

    private void OnMouseEnter()
    {
        currentOutline = new GameObject("TempOutline");
        currentOutline.transform.parent = transform;
        currentOutline.transform.localScale = Vector3.one * (1 + outlineSize);
        currentOutline.transform.localPosition = Vector3.zero;

        SpriteRenderer outlineRenderer = currentOutline.AddComponent<SpriteRenderer>();
        outlineRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
        outlineRenderer.color = highlightColor;
        outlineRenderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
    }

    private void OnMouseExit()
    {
        if (currentOutline != null)
        {
            Destroy(currentOutline);
            currentOutline = null;
        }
    }

    // 新增方法：強制移除外框（供外部呼叫）
    public void ForceRemoveOutline()
    {
        if (currentOutline != null)
        {
            Destroy(currentOutline);
            currentOutline = null;
        }
    }
}