using UnityEngine;

public enum Occupant { White, Black, Clear};

public class Tile : MonoBehaviour
{
    Occupant occupant;
    public int index { get; private set; }

    public Color clear;
    public Color friendly;
    public Color enemy;

	public void Create(Occupant _occupant, int _index)
	{
        occupant = _occupant;
        index = _index;
	}
}
