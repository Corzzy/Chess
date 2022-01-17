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

	public void Bury(Peice peice)
	{

	}
}
