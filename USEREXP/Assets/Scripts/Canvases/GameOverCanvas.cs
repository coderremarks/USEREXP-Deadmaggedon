using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameOverCanvas : MenuCanvas
{
    [SerializeField] private Button _tryAgain;
    [SerializeField] private Button _toTitleScreen;
    public TextMeshProUGUI CauseOfDeathText;
    public TextMeshProUGUI waveScore;
    public TextMeshProUGUI waveHighScore;

    public override void Preinitialize()
    {
        base.Preinitialize();
        _tryAgain.interactable = true;
        _toTitleScreen.interactable = true;
    }
    public void UpdateScores(int score, int highscore)
    {
        waveScore.text = score.ToString();
        waveHighScore.text = highscore.ToString();
    }
    // Start is called before the first frame update
    public void RetryAgain()
    {
        _tryAgain.interactable = false;
        GameManager.instance.RetryGame();
        Time.timeScale = 1f;
    }

    public void ToTitleScreen()
    {
        _toTitleScreen.interactable = false;
        GameManager.instance.RestartGame();
        Time.timeScale = 1f;
        //GameManager.instance.OnRestartGame();
    }
}
