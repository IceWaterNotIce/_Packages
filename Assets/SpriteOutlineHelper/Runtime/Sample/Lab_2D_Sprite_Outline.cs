using UnityEngine;

public class Lab_2D_Sprite_Outline : MonoBehaviour
{
    public Color outlineColor = Color.red; // 外框顏色
    public float outlineSize = 0.1f; // 外框大小
    public GameObject spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpriteOutlineHelper.AddMouseOverHighlight(spriteRenderer, outlineColor, outlineSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
