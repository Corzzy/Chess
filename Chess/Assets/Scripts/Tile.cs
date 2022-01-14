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

    private void FixedUpdate()
    {
        gameObject.GetComponent<SpriteRenderer>().color = active;
    }
}
