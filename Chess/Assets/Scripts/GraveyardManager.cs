using UnityEngine;

public class GraveyardManager : MonoBehaviour
{
    [Header("Peice Sprites")]
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
    public GameObject[] whitePeiceGraves;
    public GameObject[] blackPeiceGraves;

    int whiteFilled;
    int blackFilled;

	private void Start()
	{
        whiteFilled = 0;
        blackFilled = 0;
	}

	public void Bury(Peice peice, bool white)
	{
        if(white)
		{
            SpriteRenderer whiteRenderer = whitePeiceGraves[whiteFilled].GetComponent<SpriteRenderer>();
            switch (peice)
			{
                case Peice.Rook:
                    whiteRenderer.sprite = whiteRook;
                    whiteRenderer.enabled = true;
                    whiteFilled++;
                    break;
                case Peice.Queen:
                    whiteRenderer.sprite = whiteQueen;
                    whiteRenderer.enabled = true;
                    whiteFilled++;
                    break;
                case Peice.Pawn:
                    whiteRenderer.sprite = whitePawn;
                    whiteRenderer.enabled = true;
                    whiteFilled++;
                    break;
                case Peice.Knight:
                    whiteRenderer.sprite = whiteKnight;
                    whiteRenderer.enabled = true;
                    whiteFilled++;
                    break;
                case Peice.King:
                    whiteRenderer.sprite = whiteKing;
                    whiteRenderer.enabled = true;
                    whiteFilled++;
                    break;
                case Peice.Horse:
                    whiteRenderer.sprite = whiteHorse;
                    whiteRenderer.enabled = true;
                    whiteFilled++;
                    break;
            }
		}
        else
		{
            SpriteRenderer blackRenderer = blackPeiceGraves[blackFilled].GetComponent<SpriteRenderer>();
            switch (peice)
            {
                case Peice.Rook:
                    blackRenderer.sprite = blackRook;
                    blackRenderer.enabled = true;
                    blackFilled++;
                    break;
                case Peice.Queen:
                    blackRenderer.sprite = blackQueen;
                    blackRenderer.enabled = true;
                    blackFilled++;
                    break;
                case Peice.Pawn:
                    blackRenderer.sprite = blackPawn;
                    blackRenderer.enabled = true;
                    blackFilled++;
                    break;
                case Peice.Knight:
                    blackRenderer.sprite = blackKnight;
                    blackRenderer.enabled = true;
                    blackFilled++;
                    break;
                case Peice.King:
                    blackRenderer.sprite = blackKing;
                    blackRenderer.enabled = true;
                    blackFilled++;
                    break;
                case Peice.Horse:
                    blackRenderer.sprite = blackHorse;
                    blackRenderer.enabled = true;
                    blackFilled++;
                    break;
            }
        }
	}
}
