using UnityEngine;

public class Grid : MonoBehaviour
{
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
        for(double y = maxBound; y >= minBound; y -= 1f)
        {
            for(double x = minBound; x <= maxBound; x += 1f)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3((float)x, (float)y, 0f), Quaternion.identity, transform);
                tiles[index] = tile;

                tile.GetComponent<Tile>().index = index;
                tile.GetComponent<SpriteRenderer>().enabled = false;

                index++;
            }
        }

        return tiles;
    }

    public GameObject SnapOnGrid(Vector3 mousePos)
	{
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        Transform closestTile = FindClosestTile(worldPos);

        return closestTile.gameObject;
	}

    Transform FindClosestTile(Vector3 worldPos)
	{
        float closestDistance = 10f, oldDistance;
        int index = 0;
        for(int i = 0; i < tileSet.Length; i++)
		{
            oldDistance = Vector2.Distance(tileSet[i].transform.position, worldPos);

            if(oldDistance < closestDistance)
			{
                closestDistance = oldDistance;
                index = i;
			}
		}
        return tileSet[index].transform;
	}
    
}
