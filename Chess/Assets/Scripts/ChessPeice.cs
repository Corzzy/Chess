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

    const int rookRepeat = 8;
    const int queenRepeat = 8;
    const int pawnRepeat = 1;
    const int knightRepeat = 8;
    const int kingRepeat = 1;
    const int horseRepeat = 1;

    /*
     * Peice specific variables
     */
    bool pawnMoved;
    const int pawnDoubleRepeat = 2;
    const int horseMaxMove = 2;
    const int horseMinMove = 1;

    [Header("Peice")]
    public bool white;
    public Peice peice;
    public int startingTileIndex;

    [Header("GraveYard")]
    public GraveyardManager graveyard;

    //EndState
    EndStateManager endState;

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
        endState = GameObject.Find("EndState").GetComponent<EndStateManager>();
        pawnMoved = false;

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
                    ChessPeice peice = snappedTile.GetComponent<Tile>().occupant.GetComponent<ChessPeice>();
                    graveyard.Bury(peice.peice, peice.white);
                    Destroy(peice.gameObject);

                    if(peice.peice == Peice.King)
                    {
                        endState.EndGame(peice.white);
                    }
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

    void CalculateMoves(int[] behavior, int maxRepeatTotal)
    {
        int oneDimensionalIndex = currentTile.GetComponent<Tile>().index % 8;

        int[] bounds = MapBounds(oneDimensionalIndex);

        for (int offset = 0; offset < behavior.Length; offset++)
        {
            GameObject tileObject;
            bool nextClear = true;
            int repeats = 1;
            int repeatTotal = maxRepeatTotal;

            /*
             * Binding peices behaviors
             */

            //Prevents peice from going across the screen
            if (peice == Peice.Knight || peice == Peice.Queen || peice == Peice.Rook)
            {
                if (!EqualsOffsets(behavior[offset], new int[] { -8, 8}))
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

            //Pawn behavior restrictions
            if (peice == Peice.Pawn)
            {
                //Prevent pawns from going across the screen
                if (bounds[0] == 0)
                {
                    if (EqualsOffsets(behavior[offset], new int[] { -9, 7 })/*behavior[offset] == -9 || behavior[offset] == 7*/)
                    {
                        continue;
                    }
                }
                if(bounds[1] == 0)
				{
                    if (EqualsOffsets(behavior[offset], new int[] { -7, 9 })/*behavior[offset] == -7 || behavior[offset] == 9*/)
                    {
                        continue;
                    }
                }

                //Pawn double move in the begining
                if(startingTileIndex == currentTile.GetComponent<Tile>().index && !pawnMoved)
				{
                    if(EqualsOffsets(behavior[offset], new int[] { -8, 8 }))
					{
                        repeatTotal = pawnDoubleRepeat;
					}
				}
            }

            //House behavior restrictions
            if(peice == Peice.Horse)
			{
                if(EqualsOffsets(behavior[offset], new int[] { -10, 6 }))
				{
                    if (bounds[0] < horseMaxMove)
                    {
                        continue;
                    }
                }
                if(EqualsOffsets(behavior[offset], new int[] {10, -6}))
                {
                    if (bounds[1] < horseMaxMove)
                    {
                        continue;
                    }
                }
                if (bounds[0] < horseMinMove)
                {
                    if(EqualsOffsets(behavior[offset], new int[] {15, -17}))
                    {
                        continue;
                    }
                }
                else if (bounds[1] < horseMinMove)
                {
                    if (EqualsOffsets(behavior[offset], new int[] { -15, 17 }))
                    {
                        continue;
                    }
                }
			}
            if(peice == Peice.King)
            {
                if (bounds[0] == 0)
                {
                    if (EqualsOffsets(behavior[offset], new int[] { -9, 7, -1})/*behavior[offset] == -9 || behavior[offset] == 7*/)
                    {
                        continue;
                    }
                }
                if (bounds[1] == 0)
                {
                    if (EqualsOffsets(behavior[offset], new int[] { -7, 9, 1 })/*behavior[offset] == -7 || behavior[offset] == 9*/)
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
                    else if(EqualsOffsets(behavior[offset], new int[] { -8, 8 }))
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
                    else
					{
                        if (!EqualsOffsets(behavior[offset], new int[] { -8, 8 }))
                        {
                            possibleMoves.Add(tileObject);
                            legalMoves.Add(tileObject);
                            tile.SetTileColor(Occupants.enemy);
                            nextClear = false;
                        }
                        else
                        {
                            nextClear = false;
                        }
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

    bool EqualsOffsets(int offset, int[] check)
	{
        foreach(int possibleOffset in check)
		{
            if(possibleOffset == offset)
			{
                return true;
			}
		}
        return false;
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