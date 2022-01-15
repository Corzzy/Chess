using UnityEngine;

public enum Occupants { friendly, enemy, clear }
public class Tile : MonoBehaviour
{
    [HideInInspector] public GameObject occupant;
    [HideInInspector] public int index;

    Color active;
    public Color clear;
    public Color friendly;
    public Color enemy;

    public void SetTileColor(Occupants occupant)
	{
        switch(occupant)
		{
            case Occupants.clear:
                active = clear;
                break;
            case Occupants.enemy:
                active = enemy;
                break;
            case Occupants.friendly:
                active = friendly;
                break;
		}
        UpdateColor();
	}
    void UpdateColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = active;
    }
}
