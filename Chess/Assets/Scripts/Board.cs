using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("BoardGeneration")]
    public int size = 8;
    public int worldWidth = 512, worldHeight = 512;
    int localWidth, localHeight;

    [Header("BoardIndex")]
    public int totalBoardSize = 64;
    int[] boardIndices;
    Vector2[,] boardCords;

    private void Start()
    {
        boardIndices = new int[totalBoardSize];
        boardCords = new Vector2[(int) Mathf.Sqrt(totalBoardSize), (int) Mathf.Sqrt(totalBoardSize)];

        InitializeBoardArrays();

        localWidth = worldWidth / size;
        localHeight = worldHeight / size;

        Print2DArray();

        GetComponent<Renderer>().material.mainTexture = GenerateTexture();
    }

    void InitializeBoardArrays()
    {
        for(int i = 0; i < boardIndices.Length; i++)
        {
            boardIndices[i] = i;
        }

        float x = 4.0f, y = 4.0f;
        /*for(int i = 0; i < boardCords.GetLength(0); i++)
        {
            y -= .5f;
            for(int k = 0; i < boardCords.GetLength(1); k++)
            {
                x -= .5f;

                boardCords[i, k] = new Vector2(x, y);
            }
        }*/
    }

    void Print2DArray()
    {
        boardCords.ToString();
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
            return new Color32(139, 69, 19, 255);
        }
        else
        {
            return new Color32(244, 164, 96, 255);
        }
    }
}
