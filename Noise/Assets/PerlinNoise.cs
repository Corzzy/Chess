using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

    public float scale = 20;

    public float xOffset = 100;
    public float yOffset = 100;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
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
        float sample = Mathf.PerlinNoise(x, y);

        return new Color(sample, sample, sample);
    }
}
