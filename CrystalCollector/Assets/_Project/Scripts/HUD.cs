using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameManager game;

    [Header("Texts")]
    [SerializeField] private TMP_Text crystalsText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text loseReasonText;

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    void OnEnable()
    {
        game.CrystalsChanged += OnCrystalsChanged;
        game.TimeChanged += OnTimeChanged;
        game.StateChanged += OnStateChanged;
    }

    void OnDisable()
    {
        game.CrystalsChanged -= OnCrystalsChanged;
        game.TimeChanged -= OnTimeChanged;
        game.StateChanged -= OnStateChanged;
    }

    private void OnCrystalsChanged(int collected, int total) =>
        crystalsText.text = $"{collected} / {total}";

    private void OnTimeChanged(float seconds)
    {
        int s = Mathf.CeilToInt(seconds);
        timerText.text = $"{s / 60:00}:{s % 60:00}";
        // Last 10 seconds in red
        timerText.color = seconds <= 10f ? new Color(1f, .35f, .3f)
                                         : Color.white;
    }

    private void OnStateChanged(GameState state, string message)
    {
        pausePanel.SetActive(state == GameState.Paused);
        winPanel.SetActive(state == GameState.Won);
        losePanel.SetActive(state == GameState.Lost);
        if (state == GameState.Lost)
            loseReasonText.text = message;
    }
}