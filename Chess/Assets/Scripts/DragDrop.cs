using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public bool white;

    SpriteRenderer renderer;
    Grid grid;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        grid = GetComponent<Grid>();
    }

    private void OnMouseDown()
    {
        renderer.color = new Color32(255, 255, 255, 120);

    }

    private void OnMouseDrag()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = mousePos;
    }
    private void OnMouseUp()
    {
        renderer.color = new Color32(255, 255, 255, 255);
        Vector2 mouseUpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = grid.FindVector(mouseUpPos);
    }
}
