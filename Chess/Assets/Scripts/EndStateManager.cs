using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class EndStateManager : MonoBehaviour
{
    public Canvas canvas;

    [Space]
    public TextMeshProUGUI winText;
    public TextMeshProUGUI timeText;

    public Button playAgain;
    Text playAgainText;
    public Button quit;
    Text quitText;

    const string blackWin = "Black Wins!";
    const string whiteWin = "White Wins!";

    //Timer
    string totalTimeText;
    int minute;
    int second;
    bool timer;

    const float targetSecond = 60.0f;
    float timePast;

    private void Start()
    {
        playAgainText = playAgain.GetComponentInChildren<Text>(true);
        quitText = quit.GetComponentInChildren<Text>(true);

        canvas.enabled = false;
        
        timePast = 0.0f;
        minute = 0;
        second = 0;
        totalTimeText = "";

        StartTimer();
    }

	private void Update()
	{
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

    string LeadingZero(int t)
	{
        return t.ToString().PadLeft(2, '0');
	}

    void EndTimer()
	{
        timer = false;
	}

	public void EndGame(bool winner)
    {
        EndTimer();
        DisablePeices();
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

    void DisablePeices()
    {
        GameObject[] peices = GameObject.FindGameObjectsWithTag("Peice");

        foreach(GameObject peice in peices)
        {
            peice.GetComponent<ChessPeice>().enabled = false;
            peice.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    //Button functions
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
