using System.Collections.Generic;
using UnityEngine;

public enum Peice { Rook, Queen, Pawn, Knight, King, Horse}

public class ChessPeice : MonoBehaviour
{
    //Peice Behavior offsets
    readonly int[] rookBehavior = { -8, 1, 8, -1 };
    readonly int[] queenBehavior = { -9, -8, -7, 1, 9, 8, 7, -1 };
    readonly int[] blackPawnBehavior = { 9, 8, 7 };
    readonly int[] whitePawnBehavior = { -9, -8, -7 };
    readonly int[] knightBehavior = { -9, -7, 7, 9 };
    readonly int[] kingBehavior = { -9, -8, -7, 1, 9, 8, 7, -1 };
    readonly int[] horseBehavior = { -17, -15, -6, 10, 17, 15, 6, -10 };

    readonly int rookRepeat = 8;
    readonly int queenRepeat = 8;
    readonly int pawnRepeat = 1;
    readonly int knightRepeat = 8;
    readonly int kingRepeat = 1;
    readonly int horseRepeat = 1;

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
    List<GameObject> legalMoves = new List<GameObject>();
    List<GameObject> possibleMoves = new List<GameObject>();

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
            currentTile.GetComponent<Tile>().occupant = gameObject;
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
        int repeat;

        switch(peice)
        {
            case Peice.Rook:
                behavior = rookBehavior;
                repeat = rookRepeat;
                break;
            case Peice.Queen:
                behavior = queenBehavior;
                repeat = queenRepeat;
                break;
            case Peice.Pawn:
                if(white)
				{
                    behavior = whitePawnBehavior;
				}
                else
				{
                    behavior = blackPawnBehavior;
                }
                repeat = pawnRepeat;
                break;
            case Peice.Knight:
                behavior = knightBehavior;
                repeat = knightRepeat;
                break;
            case Peice.King:
                behavior = kingBehavior;
                repeat = kingRepeat;
                break;
            case Peice.Horse:
                behavior = horseBehavior;
                repeat = horseRepeat;
                break;
            default:
                behavior = null;
                repeat = 0;
                break;
        }
        CalculateMoves(behavior, repeat);
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
            UnHighlightMoves();
        }
        else
        {
            if(legalMoves.Contains(snappedTile))
			{
                currentTile.GetComponent<Tile>().occupant = null;

                if(snappedTile.GetComponent<Tile>().occupant != null)
				{
                    Destroy(snappedTile.GetComponent<Tile>().occupant);
				}

                currentTile = snappedTile;
                transform.position = currentTile.transform.position;
                currentTile.GetComponent<Tile>().occupant = gameObject;

                UnHighlightMoves();
                possibleMoves.Clear();
                legalMoves.Clear();
                Board.NextTurn();
            }
            else
			{
                transform.position = currentTile.transform.position;
                UnHighlightMoves();
            }
        }
    }

    void CalculateMoves(int[] behavior, int repeatTotal)
    {
        int oneDimensionalIndex = currentTile.GetComponent<Tile>().index % 8;

        int[] bounds = MapBounds(oneDimensionalIndex);

        for (int offset = 0; offset < behavior.Length; offset++)
        {
            GameObject tileObject;
            bool nextClear = true;
            int repeats = 1;

            //Check if move teleports throught the grid
            if (peice == Peice.Knight || peice == Peice.Queen || peice == Peice.Rook)
            {
                if (Mathf.Abs(behavior[offset]) != 8)
                {
                    switch (behavior[offset])
                    {
                        case -9:
                            repeatTotal = bounds[0];
                            break;
                        case -7:
                            repeatTotal = bounds[1];
                            break;
                        case 1:
                            repeatTotal = bounds[1];
                            break;
                        case 9:
                            repeatTotal = bounds[1];
                            break;
                        case 7:
                            repeatTotal = bounds[0];
                            break;
                        case -1:
                            repeatTotal = bounds[0];
                            break;
                    }
                } 
            }
            if (peice == Peice.Pawn)
            {
                if (bounds[0] == 0)
                {
                    if (behavior[offset] == -9 || behavior[offset] == 7)
                    {
                        continue;
                    }
                }
                if(bounds[1] == 0)
				{
                    if (behavior[offset] == -7 || behavior[offset] == 9)
                    {
                        continue;
                    }
                }
            }

            while (repeats <= repeatTotal && nextClear)
            {
                int gridNum = currentTile.GetComponent<Tile>().index + (behavior[offset] * repeats);

                //Check if move is within the grid
                if (gridNum >= grid.tileSet.Length || gridNum < 0)
				{
                    break;
				}

                tileObject = grid.tileSet[gridNum];

                Tile tile = tileObject.GetComponent<Tile>();
                
                if(tile.occupant == null)
				{
                    if(peice != Peice.Pawn)
					{
                        possibleMoves.Add(tileObject);
                        legalMoves.Add(tileObject);
                        tile.SetTileColor(Occupants.clear);
                    } 
                    else if(Mathf.Abs(behavior[offset]) != 9 && Mathf.Abs(behavior[offset]) != 7)
					{
                        possibleMoves.Add(tileObject);
                        legalMoves.Add(tileObject);
                        tile.SetTileColor(Occupants.clear);
                    }
				}
                else if(IsEnemy(tile.occupant))
				{
                    if (peice != Peice.Pawn)
                    {
                        possibleMoves.Add(tileObject);
                        legalMoves.Add(tileObject);
                        tile.SetTileColor(Occupants.enemy);
                        nextClear = false;
                    }
                    else if (Mathf.Abs(behavior[offset]) == 9 || Mathf.Abs(behavior[offset]) == 7)
                    {
                        possibleMoves.Add(tileObject);
                        legalMoves.Add(tileObject);
                        tile.SetTileColor(Occupants.enemy);
                        nextClear = false;
                    }
                    
				}
                else
				{
                    possibleMoves.Add(tileObject);
                    tile.SetTileColor(Occupants.friendly);
                    nextClear = false;
				}
                repeats++;
			}
            
		}
        
    }

    int[] MapBounds(int index)
	{
        int[] bounds = new int[2];

        bounds[0] = index;
        bounds[1] = Mathf.Abs(index - 7);

        return bounds;
	} 

    bool IsEnemy(GameObject peice)
	{
        bool otherWhite = peice.GetComponent<ChessPeice>().white;
        if(otherWhite && white || !otherWhite && !white)
		{
            return false;
		}
        return true;
	}

    void HighlightMoves()
    {
        foreach(GameObject tile in possibleMoves)
        {
            tile.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void UnHighlightMoves()
	{
        foreach (GameObject tile in possibleMoves)
        {
            tile.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
