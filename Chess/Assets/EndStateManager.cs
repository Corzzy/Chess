using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndStateManager : MonoBehaviour
{
    public Canvas canvas;

    [Space]
    public TextMeshProUGUI winText;
    public TextMeshProUGUI time;

    public Button playAgain;
    Text playAgainText;
    public Button quit;
    Text quitText;

    const string blackWin = "Black Wins!";
    const string whiteWin = "White Wins!";

    private void Start()
    {
        playAgainText = playAgain.GetComponentInChildren<Text>(true);
        quitText = quit.GetComponentInChildren<Text>(true);

        canvas.enabled = false;
    }

    public void EndGame(bool winner)
    {
        if (!winner)
        {
            //White wins
            winText.text = whiteWin;
            winText.color = Color.white;
            winText.outlineColor = Color.black;
            //time.text = Board.Time;
            time.color = Color.white;
            time.outlineColor = Color.black;

            canvas.enabled = true;
        }
        else
        {
            //Black wins
            winText.text = blackWin;
            winText.color = Color.black;
            winText.outlineColor = Color.white;
            //time.text = Board.Time;
            time.color = Color.black;
            time.outlineColor = Color.white;

            canvas.enabled = true;
        }
    }

    //Button functions
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
