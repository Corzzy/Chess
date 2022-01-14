using UnityEngine;

public class ChessPeice : MonoBehaviour
{
    [SerializeField] bool white;

    SpriteRenderer renderer;
    BoxCollider2D collider;

    Grid grid;

    [HideInInspector] public GameObject currentTile;
    GameObject[] moves;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }

    private void Update()
    {
        if(white && Board.turn == Turn.Blackturn)
        {
            collider.enabled = false;
        }
        if(white && Board.turn == Turn.Whiteturn)
        {
            collider.enabled = true;
        }
        if (!white && Board.turn == Turn.Blackturn)
        {
            collider.enabled = true;
        }
        if (!white && Board.turn == Turn.Whiteturn)
        {
            collider.enabled = false;
        }
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
        transform.position = grid.SnapOnGrid(Input.mousePosition);

        Board.NextTurn();
    }
}
