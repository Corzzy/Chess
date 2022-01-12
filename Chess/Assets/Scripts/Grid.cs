using UnityEngine;

public class Grid : MonoBehaviour
{
    static readonly float[] coords = { -3.5f, -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f, 3.5f };

    public GameObject tilePrefab;
    public int gridSize = 64;
    public double maxBound = 3.5;
    public double minBound = -3.5;
    [HideInInspector] public GameObject[] tileSet;

    private void Start()
    {
        tileSet = GenerateTiles(maxBound, minBound, gridSize);
    }

    GameObject[] GenerateTiles(double maxBound, double minBound, int size)
    {
        GameObject[] tiles = new GameObject[size];
        int index = 0;
        for(double x = maxBound; x >= minBound; x -= 1f)
        {
            for(double y = maxBound; y >= minBound; y -= 1f)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3((float)x, (float)y, 0f), Quaternion.identity, transform);
                tiles[index] = tile;
                tile.GetComponent<Tile>().index = index;
                index++;
            }
        }

        return tiles;
    }

    public static Vector2 SnapOnGrid(Vector3 mousePosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        float closestX = 0, closestY = 0;
        float oldDistanceX = 2, oldDistanceY = 2;

        for(int x = 0; x < coords.Length; x++)
        {
            float newDistance = Mathf.Abs(worldPosition.x - coords[x]);

            if(oldDistanceX > newDistance)
            {
                closestX = coords[x];
                oldDistanceX = newDistance;
            }
        }

        for (int y = 0; y < coords.Length; y++)
        {
            float newDistance = Mathf.Abs(worldPosition.y - coords[y]);

            if (oldDistanceY > newDistance)
            {
                closestY = coords[y];
                oldDistanceY = newDistance;
            }
        }

        return new Vector2(closestX, closestY);
    }

    
}
