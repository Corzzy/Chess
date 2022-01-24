using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndStateManager : MonoBehaviour
{
    public Canvas canvas;

    [Space]
    public TextMeshProUGUI winText;
    public TextMeshProUGUI timeText;

    const string blackWin = "Black Wins!";
    const string whiteWin = "White Wins!";

    //Timer
    string totalTimeText;
    int minute;
    int second;
    bool timer;

    //Counting seconds
    const float targetSecond = 60.0f;
    float timePast;

    private void Start()
    {
        canvas.enabled = false;
        
        timePast = 0.0f;
        minute = 0;
        second = 0;
        totalTimeText = "";

        //Begins counting the time
        StartTimer();
    }

	private void Update()
	{
        //Counts how much time has past
        if(timer)
		{
            timePast += Time.deltaTime;

            second = Mathf.FloorToInt(timePast);

            if(timePast >= targetSecond)
			{
                minute += 1;
                timePast = 0;
			}

            string minuteString = minute.ToString();
            
            totalTimeText = minuteString + ":" + LeadingZero(second);
        }
	}

	void StartTimer()
	{
        timer = true;
	}

    //Adds a zero to an into below 10 and converts it to a string
    string LeadingZero(int t)
	{
        return t.ToString().PadLeft(2, '0');
	}

    void EndTimer()
	{
        timer = false;
	}

    //Ends the game a displays who won
	public void EndGame(bool winner)
    {
        EndTimer();
        DisablePieces();
        if (!winner)
        {
            //White wins
            winText.text = whiteWin;
            winText.color = Color.white;
            winText.outlineColor = Color.black;
            timeText.text = totalTimeText;
            timeText.color = Color.white;
            timeText.outlineColor = Color.black;

            canvas.enabled = true;
        }
        else
        {
            //Black wins
            winText.text = blackWin;
            winText.color = Color.black;
            winText.outlineColor = Color.white;
            timeText.text = totalTimeText;
            timeText.color = Color.black;
            timeText.outlineColor = Color.white;

            canvas.enabled = true;
        }
    }

    //Disables picking up the pieces
    void DisablePieces()
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");

        foreach(GameObject piece in pieces)
        {
            piece.GetComponent<ChessPiece>().enabled = false;
            piece.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    //Button functions
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
