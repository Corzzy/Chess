using UnityEngine;

public enum Peice { Rook, Queen, Pawn, Knight, King, Horse}

public class ChessPeice : MonoBehaviour
{
    //Peice Behavior offsets
    readonly int[] rookBehavior = { -8, 1, 8, -1 };
    readonly int[] queenBehavior = { -9, -8, -7, 1, 9, 8, 7, -1 };
    readonly int[] pawnBehavior = { -9, -8, -7, 9, 8, 7 };
    readonly int[] knightBehavior = { -9, -7, 7, 9 };
    readonly int[] kingBehavior = { -9, -8, -7, 1, 9, 8, 7, -1 };
    readonly int[] horseBehavior = { -17, -15, -6, 10, 17, 15, 6, -10 };

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
        }


        //Control colliders of peice sets based on turn
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

        int[] behavior;

        switch(peice)
        {
            case Peice.Rook:
                behavior = rookBehavior;
                break;
            case Peice.Queen:
                behavior = queenBehavior;
                break;
            case Peice.Pawn:
                behavior = pawnBehavior;
                break;
            case Peice.Knight:
                behavior = kingBehavior;
                break;
            case Peice.King:
                behavior = kingBehavior;
                break;
            case Peice.Horse:
                behavior = horseBehavior;
                break;
            default:
                behavior = null;
                break;
        }
        moves = CalculateMoves(behavior);
        HighlightMoves();
    }

    private void OnMouseDrag()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = mousePos;
    }
    private void OnMouseUp()
    {
        renderer.color = new Color32(255, 255, 255, 255);

        GameObject snappedTile;
        
        snappedTile = grid.SnapOnGrid(Input.mousePosition);

        if (snappedTile == currentTile)
        {
            transform.position = currentTile.transform.position;
        }
        else
        {
            currentTile = snappedTile;
            transform.position = currentTile.transform.position;

            Board.NextTurn();
        }
    }

    GameObject[] CalculateMoves(int[] behavior)
    {
        return moves;
    }

    void HighlightMoves()
    {
        foreach(GameObject tile in moves)
        {
            tile.GetComponent<Tile>();
        }
    }
}
