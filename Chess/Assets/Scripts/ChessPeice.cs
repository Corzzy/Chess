using UnityEngine;

public enum Peice { Rook, Queen, Pawn, Knight, King, Horse}

public class ChessPeice : MonoBehaviour
{
    //Peice Behavior offsets
    int[] rookBehavior = { -8, 1, 8, -1 };
    int[] queenBehavior = { -9, -8, -7, 1, 9, 8, 7, -1 };
    int[] pawnBehavior = { -9, -8, -7, 9, 8, 7 };
    int[] knightBehavior = { -9, -7, 7, 9 };
    int[] kingBehavior = { -9, -8, -7, 1, 9, 8, 7, -1 };
    int[] horseBehavior = { -17, -15, -6, 10, 17, 15, 6, -10 };

    [Header("Peice")]
    public bool white;
    public Peice peice;
    public int startingTileIndex;

    //Peice components
    SpriteRenderer renderer;
    BoxCollider2D collider;

    //Highlight Moves and current position
    Grid grid;
    GameObject currentTile;
    GameObject[] moves;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }

    private void Update()
    {
        if(currentTile == null)
        {
            currentTile = grid.tileSet[startingTileIndex];
            currentTile.GetComponent<Tile>().occupant = this;
        }

        if(white && Board.turn == Turn.Blackturn || !white && Board.turn == Turn.Whiteturn)
        {
            collider.enabled = false;
        }
        if(white && Board.turn == Turn.Whiteturn || !white && Board.turn == Turn.Blackturn)
        {
            collider.enabled = true;
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
        GameObject snappedTile;

        renderer.color = new Color32(255, 255, 255, 255);
        

        snappedTile = grid.SnapOnGrid(Input.mousePosition);

        if (snappedTile == currentTile)
        {
            transform.position = currentTile.transform.position;
        }
        else
        {
            Tile tile = snappedTile.GetComponent<Tile>();

            if(tile.CheckEnemy(this) == true)
            {
                Debug.Log("take");
                TakePeice(tile.occupant);
            }
            else if(tile.CheckEnemy(this) == false)
            {
                Debug.Log("Friendly");
                SnapBack();
            }
            else
            {
                Debug.Log("Empty");
                tile.occupant = this;
                Snap(snappedTile);
            }

            Board.NextTurn();
        }
    }

    void TakePeice(ChessPeice peice)
    {
        Destroy(peice.gameObject);
        
    }

    void Snap(GameObject tile)
    {
        currentTile = tile;
        transform.position = currentTile.transform.position;
    }

    void SnapBack()
    {
        transform.position = currentTile.transform.position;
    }
}
