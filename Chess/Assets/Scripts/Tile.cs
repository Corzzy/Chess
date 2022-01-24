using UnityEngine;

//Enum for what could occupy a tile
public enum Occupants { friendly, enemy, clear }
public class Tile : MonoBehaviour
{
    [HideInInspector] public GameObject occupant;
    [HideInInspector] public int index;

    //Colors to display what piece is in a tile
    Color active;
    public Color clear;
    public Color friendly;
    public Color enemy;

    //Sets tile color according to what occupant is passed in
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

    //Updates the components color
    void UpdateColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = active;
    }
}
