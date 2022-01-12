using UnityEngine;

public enum Occupant { White, Black, Clear};

public class Tile : MonoBehaviour
{
    Occupant occupant;

    [HideInInspector] public int index;

    public Color clear;
    public Color friendly;
    public Color enemy;

	
}
