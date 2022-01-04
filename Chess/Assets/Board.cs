using UnityEngine;

public class Board : MonoBehaviour
{
    public int size = 8;
    public int worldWidth = 512, worldHeight = 512;
    int width, height;

    private void Update()
    {
        width = worldWidth / size;
        height = worldHeight / size;

        GetComponent<Renderer>().material.mainTexture = GenerateTexture();
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
            widthUnit = x / width;
        }
        if(y == 0)
        {
            heightUnit = 0;
        }
        else
        {
            heightUnit = y / height;
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
            return Color.black;
        }
        else
        {
            return Color.white;
        }
    }
}
