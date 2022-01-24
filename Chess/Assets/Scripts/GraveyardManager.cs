using UnityEngine;

public class GraveyardManager : MonoBehaviour
{
    [Header("Piece Sprites")]
    public Sprite whiteRook;
    public Sprite whitePawn;
    public Sprite whiteQueen;
    public Sprite whiteKing;
    public Sprite whiteKnight;
    public Sprite whiteHorse;
    public Sprite blackRook;
    public Sprite blackPawn;
    public Sprite blackQueen;
    public Sprite blackKing;
    public Sprite blackKnight;
    public Sprite blackHorse;

    [Header("Graveyard Lists")]
    public GameObject[] whitePieceGraves;
    public GameObject[] blackPieceGraves;

    //How many pieces are in the graveyard
    int whiteFilled;
    int blackFilled;

	private void Start()
	{
        whiteFilled = 0;
        blackFilled = 0;
	}

    //Take a killed piece and if its white and bury the correct piece
	public void Bury(Piece piece, bool white)
	{
        if(white)
		{
            SpriteRenderer whiteRenderer = whitePieceGraves[whiteFilled].GetComponent<SpriteRenderer>();
            switch (piece)
			{
                case Piece.Rook:
                    whiteRenderer.sprite = whiteRook;
                    whiteRenderer.enabled = true;
                    whiteFilled++;
                    break;
                case Piece.Queen:
                    whiteRenderer.sprite = whiteQueen;
                    whiteRenderer.enabled = true;
                    whiteFilled++;
                    break;
                case Piece.Pawn:
                    whiteRenderer.sprite = whitePawn;
                    whiteRenderer.enabled = true;
                    whiteFilled++;
                    break;
                case Piece.Knight:
                    whiteRenderer.sprite = whiteKnight;
                    whiteRenderer.enabled = true;
                    whiteFilled++;
                    break;
                case Piece.King:
                    whiteRenderer.sprite = whiteKing;
                    whiteRenderer.enabled = true;
                    whiteFilled++;
                    break;
                case Piece.Horse:
                    whiteRenderer.sprite = whiteHorse;
                    whiteRenderer.enabled = true;
                    whiteFilled++;
                    break;
            }
		}
        else
		{
            SpriteRenderer blackRenderer = blackPieceGraves[blackFilled].GetComponent<SpriteRenderer>();
            switch (piece)
            {
                case Piece.Rook:
                    blackRenderer.sprite = blackRook;
                    blackRenderer.enabled = true;
                    blackFilled++;
                    break;
                case Piece.Queen:
                    blackRenderer.sprite = blackQueen;
                    blackRenderer.enabled = true;
                    blackFilled++;
                    break;
                case Piece.Pawn:
                    blackRenderer.sprite = blackPawn;
                    blackRenderer.enabled = true;
                    blackFilled++;
                    break;
                case Piece.Knight:
                    blackRenderer.sprite = blackKnight;
                    blackRenderer.enabled = true;
                    blackFilled++;
                    break;
                case Piece.King:
                    blackRenderer.sprite = blackKing;
                    blackRenderer.enabled = true;
                    blackFilled++;
                    break;
                case Piece.Horse:
                    blackRenderer.sprite = blackHorse;
                    blackRenderer.enabled = true;
                    blackFilled++;
                    break;
            }
        }
	}
}
