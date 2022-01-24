using UnityEngine;

//Enum for whose turn
public enum Turn { Whiteturn, Blackturn };
public class Board : MonoBehaviour
{

    [Header("BoardGeneration")]
    public int size = 8; //How many tiles the board will be split into
    public int worldWidth = 512, worldHeight = 512; //Resolution of the board
    int localWidth, localHeight; //Dimensions of each idividual tile
    
    [Space]
    public Color colorOdd;
    public Color colorEven;

    [Header("TurnSystem")]
    public static Turn turn;
    public static SpriteRenderer turnRenderer;
    

    private void Start()
    {
        turnRenderer = GameObject.FindGameObjectWithTag("Turn").GetComponent<SpriteRenderer>();
        turn = Turn.Whiteturn;
        turnRenderer.color = Color.white;

        localWidth = worldWidth / size;
        localHeight = worldHeight / size;

        GetComponent<Renderer>().material.mainTexture = GenerateTexture();
    }

    //Changes turn and the box around the boards color to whoevers turn it is
    public static void NextTurn()
    {
        if(turn == Turn.Whiteturn)
        {
            turn = Turn.Blackturn;
            turnRenderer.color = Color.black;
        }
        else
        {
            turn = Turn.Whiteturn;
            turnRenderer.color = Color.white;
        }
    }

    //Iterates through every pixel in the board
    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(worldWidth,worldHeight);

        //Generate board
        for(int x = 0; x < worldWidth; x++)
        {
            for(int y = 0; y < worldHeight; y++)
            {
                Color color = GenerateColor(x, y);
                
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    //Finds which pixels should be which colors
    Color GenerateColor(int x, int y)
    {
        bool odd = true;
        int widthUnit, heightUnit;

        /*
         * Finds which tile the pixel is appart of
         */

        //Changes the x pixel coord into a number from 1 to size
        if(x == 0)
        {
            widthUnit = 0;
        } 
        else
        {
            widthUnit = x / localWidth;
        }
        //Changes the y pixel coord into a number from 1 to size
        if(y == 0)
        {
            heightUnit = 0;
        }
        else
        {
            heightUnit = y / localHeight;
        }

        /*
         * Finds if the tile which the pixel is appart of is even or odd
         */
        if(heightUnit % 2 != 0)
        {
            if(widthUnit % 2 == 0)
            {
                odd = false;
            }
            else
            {
                odd = true;
            }
        }
        else
        {
            if (widthUnit % 2 != 0)
            {
                odd = false;
            }
            else
            {
                odd = true;
            }
        }

        /*
         * Sets the color of the pixel based on if its in an even or odd tile
         */
        if (!odd)
        {
            return colorEven;
        }
        else
        {
            return colorOdd;
        }
    }
}
