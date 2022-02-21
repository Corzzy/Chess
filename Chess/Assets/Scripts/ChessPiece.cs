using System.Collections.Generic;
using UnityEngine;

public enum Piece { Rook, Queen, Pawn, Knight, King, Horse}

public class ChessPiece : MonoBehaviour
{
    /*
     *The behavior offsets are derived from a 2d array of numbers from 0 to 63 that are assigned to each board position
     *starting from 0 in the top left to 63 in the bottom right. 
     *
     *For example the computer sees
     *{ 0, 1, 2, 3, 4, 5, 6, 7,
     *8, 9, 10, 11, 12, 13, 14, 15,
     *16, 17, 18, 19, 20, 21, 22, 23,
     *24, 25, 26, 27, 28, 29, 30, 31,
     *32, 33, 34, 35, 36, 37, 38, 39,
     *40, 41, 42, 43, 44, 45, 46, 47,
     *48, 49, 50, 51, 52, 53, 54, 55,
     *56, 57, 58, 59, 60, 61, 62, 63 }
     *Each position on the chess board has a number assigned to it
     *
     *For example lets place a rook on the position with an index of 36. The rook can move left, right, up, and down. So
     *to move left we subtract 1 from our index of 36, to move right we add 1 to our index of 36, to move up we subtract 8,
     *to move down we add 8. So the rook offsets would be { -1, 1, -8, 8 } these offsets stay the same 
     *if we put it on any number besides 36.
     */

    //Piece Behavior offsets
    readonly int[] rookBehavior = { -8, 1, 8, -1 };
    readonly int[] queenBehavior = { -9, -8, -7, 1, 9, 8, 7, -1 };
    readonly int[] blackPawnBehavior = { 9, 8, 7 };
    readonly int[] whitePawnBehavior = { -9, -8, -7 };
    readonly int[] knightBehavior = { -9, -7, 7, 9 };
    readonly int[] kingBehavior = { -9, -8, -7, 1, 9, 8, 7, -1 };
    readonly int[] horseBehavior = { -17, -15, -6, 10, 17, 15, 6, -10 };

    //How many times should a piece be able to more in a direction
    const int rookRepeat = 8;
    const int queenRepeat = 8;
    const int pawnRepeat = 1;
    const int knightRepeat = 8;
    const int kingRepeat = 1;
    const int horseRepeat = 1;

    /*
     * Piece specific variables
     */
    const int pawnDoubleRepeat = 2;
    const int horseMaxMove = 2;
    const int horseMinMove = 1;

    [Header("Piece")]
    public bool white;
    public Piece piece;
    public int startingTileIndex;

    [Header("GraveYard")]
    public GraveyardManager graveyard;

    //EndState
    EndStateManager endState;

    //Piece components
    SpriteRenderer renderer;
    BoxCollider2D collider;

    //Highlight Moves and current position
    Grid grid;
    GameObject currentTile;

    /*
     * PossibleMoves is all possible moves a piece can make with its behavior
     * LegalMoves is all the moves in possibleMoves except for moves that already contain a friendly piece in them
     */
    List<GameObject> legalMoves = new List<GameObject>();
    List<GameObject>    possibleMoves = new List<GameObject>();

    private void Start()
    {
        endState = GameObject.Find("EndState").GetComponent<EndStateManager>();

        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }

    private void Update()
    {
        //When the first update is called current tile is null
        if(currentTile == null)
        {
            currentTile = grid.tileSet[startingTileIndex];
            currentTile.GetComponent<Tile>().occupant = gameObject;
        }

        //Control colliders of piece sets based on turn
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
        //Make the piece more transparent
        renderer.color = new Color32(255, 255, 255, 120);

        int[] behavior;
        int repeat;

        //Identify what piece we are holding and what is behavior is
        switch(piece)
        {
            case Piece.Rook:
                behavior = rookBehavior;
                repeat = rookRepeat;
                break;
            case Piece.Queen:
                behavior = queenBehavior;
                repeat = queenRepeat;
                break;
            case Piece.Pawn:
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
            case Piece.Knight:
                behavior = knightBehavior;
                repeat = knightRepeat;
                break;
            case Piece.King:
                behavior = kingBehavior;
                repeat = kingRepeat;
                break;
            case Piece.Horse:
                behavior = horseBehavior;
                repeat = horseRepeat;
                break;
            default:
                behavior = null;
                repeat = 0;
                break;
        }
        //Calculate possible moves for piece we are holding
        CalculateMoves(behavior, repeat);
        //Highlight the possible moves
        HighlightMoves();
    }

    private void OnMouseDrag()
    {
        //Piece follows mouse around
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = mousePos;
    }
    private void OnMouseUp()
    {
        //Restore pieces full color
        renderer.color = new Color32(255, 255, 255, 255);

        //Snap piece to the tile the mouse is on
        GameObject snappedTile;
        
        snappedTile = grid.SnapOnGrid(Input.mousePosition);

        if (snappedTile == currentTile)
        {
            //If piece returns to tile it was on before put it back
            transform.position = currentTile.transform.position;
            UnHighlightMoves();
        }
        else
        {
            //Check if the move is legal to the pieces behavior
            if(legalMoves.Contains(snappedTile))
			{
                currentTile.GetComponent<Tile>().occupant = null;

                //Check if the tile the piece has landed on has a enemy piece on it
                if(snappedTile.GetComponent<Tile>().occupant != null)
				{
                    ChessPiece piece = snappedTile.GetComponent<Tile>().occupant.GetComponent<ChessPiece>();
                    graveyard.Bury(piece.piece, piece.white);
                    Destroy(piece.gameObject);

                    //If a piece takes the enemy king end the game
                    if(piece.piece == Piece.King)
                    {
                        endState.EndGame(piece.white);
                    }
				}

                //Change the pieces position to the new tile
                currentTile = snappedTile;
                transform.position = currentTile.transform.position;
                currentTile.GetComponent<Tile>().occupant = gameObject;

                //Unhighlight the possible moves
                UnHighlightMoves();
                //Other players turn now
                Board.NextTurn();
            }
            else
			{
                //If the piece moves to a spot that is not legal with the pieces behavior snap it back to the tile it started at
                transform.position = currentTile.transform.position;
                UnHighlightMoves();
            }
        }
        //Clear the lists that contain the moves for pieces
        possibleMoves.Clear();
        legalMoves.Clear();
    }

    void CalculateMoves(int[] behavior, int maxRepeatTotal)
    {
        //OneDimensionalIndex is a number between 0 and 7 and its the index of the piece from left to right ignoring what y coord the piece is at
        int oneDimensionalIndex = currentTile.GetComponent<Tile>().index % 8;

        //How far to the left and right is the piece
        int[] bounds = MapBounds(oneDimensionalIndex);

        //Calculate each possible move for each possible offset the piece can move on the boards grid
        for (int offset = 0; offset < behavior.Length; offset++)
        {
            GameObject tileObject;
            bool nextClear = true; //Does the next tile have a piece on it
            int repeats = 1; //How many tiles has the offset been checked in a direction
            int repeatTotal = maxRepeatTotal; //How many times should it check in a direction

            /*
             * Binding pieces behaviors
             */

            //Prevents piece from going across the screen
            if (piece == Piece.Knight || piece == Piece.Queen || piece == Piece.Rook)
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
            if (piece == Piece.Pawn)
            {
                //Prevent pawns from going across the screen
                if (bounds[0] == 0)
                {
                    if (EqualsOffsets(behavior[offset], new int[] { -9, 7 }))
                    {
                        continue;
                    }
                }
                if(bounds[1] == 0)
				{
                    if (EqualsOffsets(behavior[offset], new int[] { -7, 9 }))
                    {
                        continue;
                    }
                }

                //Pawn double move in the begining
                if(startingTileIndex == currentTile.GetComponent<Tile>().index)
				{
                    if(EqualsOffsets(behavior[offset], new int[] { -8, 8 }))
					{
                        repeatTotal = pawnDoubleRepeat;
					}
				}
            }

            //House behavior restrictions
            if(piece == Piece.Horse)
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
            //Prevents king from going accross the screen
            if(piece == Piece.King)
            {
                if (bounds[0] == 0)
                {
                    if (EqualsOffsets(behavior[offset], new int[] { -9, 7, -1}))
                    {
                        continue;
                    }
                }
                if (bounds[1] == 0)
                {
                    if (EqualsOffsets(behavior[offset], new int[] { -7, 9, 1 }))
                    {
                        continue;
                    }
                }
            }
            /*
             * For a single move or offset at a tile check if the next possible tile is occupied
             * If its occupied assign the tile color the correct color
             * Also add it to the correct list
             */

            while (repeats <= repeatTotal && nextClear)
            {
                //What tile index should we check
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
                    //If piece is pawn don't let it move diagonal when no piece is their
                    if(piece != Piece.Pawn)
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
                    //If piece is pawn don't let it attack enemys from infront of them
                    if (piece != Piece.Pawn)
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

    //Takes a number and checks if that number is equal to any number in the array given
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

    //Finds the distance from the right and left sides
    int[] MapBounds(int index)
	{
        int[] bounds = new int[2];

        bounds[0] = index;
        bounds[1] = Mathf.Abs(index - 7);

        return bounds;
	} 

    //Checks if the piece give is a enemy
    bool IsEnemy(GameObject piece)
	{
        bool otherWhite = piece.GetComponent<ChessPiece>().white;
        if(otherWhite && white || !otherWhite && !white)
		{
            return false;
		}
        return true;
	}

    //Highlights the moves in the possible moves list
    void HighlightMoves()
    {
        foreach(GameObject tile in possibleMoves)
        {
            tile.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    //Unhighlights the moves in the possible moves list
    void UnHighlightMoves()
	{
        foreach (GameObject tile in possibleMoves)
        {
            tile.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}