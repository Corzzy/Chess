using UnityEngine;

public class RookBehavior : MonoBehaviour
{
    Board board;

    int[] offsets = { -8, 1, 8, -1 };

    int indexPos;


    private void Start()
    {
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();


    }
}
