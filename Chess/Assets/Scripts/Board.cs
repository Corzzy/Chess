using UnityEngine;

public enum Turn { Whiteturn, Blackturn };
public class Board : MonoBehaviour
{
    [Header("BoardGeneration")]
    public int size = 8;
    public int worldWidth = 512, worldHeight = 512;
    int localWidth, localHeight;
    
    [Space]
    public Color colorOdd;
    public Color colorEven;

    //[Header("TurnSystem")]
    public static Turn turn;
    

    private void Start()
    {
        turn = Turn.Whiteturn;
        Camera.main.backgroundColor = Color.white;

        localWidth = worldWidth / size;
        localHeight = worldHeight / size;

        GetComponent<Renderer>().material.mainTexture = GenerateTexture();
    }

    public static void NextTurn()
    {
        if(turn == Turn.Whiteturn)
        {
            turn = Turn.Blackturn;
            Camera.main.backgroundColor = Color.black;
        }
        else
        {
            turn = Turn.Whiteturn;
            Camera.main.backgroundColor = Color.white;
        }
    }

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

    Color GenerateColor(int x, int y)
    {
        bool odd = true;
        int widthUnit, heightUnit;

        if(x == 0)
        {
            widthUnit = 0;
        } 
        else
        {
            widthUnit = x / localWidth;
        }
        if(y == 0)
        {
            heightUnit = 0;
        }
        else
        {
            heightUnit = y / localHeight;
        }

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
