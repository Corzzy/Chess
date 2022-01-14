using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector] public ChessPeice occupant;
    [HideInInspector] public int index;

    public Color clear;
    public Color friendly;
    public Color enemy;

    public bool CheckEnemy(ChessPeice attacker)
    {
        if(attacker.white == occupant.white)
        {
            return false;
        }
        return true;
    }
}
