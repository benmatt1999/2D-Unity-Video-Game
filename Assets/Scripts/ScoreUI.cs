using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;


    [Header("Count Up")]
    [SerializeField] private float countSpeed = 500f;

    [Header("Pop")]
    [SerializeField] private float popScale = 1.3f;
    [SerializeField] private float popReturnSpeed = 5f;

    private float displayScore;
    private int targetScore;

    void Start()
    {
        scoreText.text = "0";
        ScoreScript.Instance.OnScoreChanged += HandleScoreChanged;
        
    }

    private void OnDestroy()
    {
        if (ScoreScript.Instance != null)
        {
            ScoreScript.Instance.OnScoreChanged -= HandleScoreChanged;

        }
    }

    private void HandleScoreChanged(int newScore)
    {
        targetScore = newScore;
        transform.localScale = Vector3.one * popScale;
    }

    private void Update()
    {
        if (displayScore < targetScore)
        {
            displayScore = Mathf.MoveTowards(displayScore, targetScore, countSpeed * Time.deltaTime);
            scoreText.text = Mathf.RoundToInt(displayScore).ToString();
        }

        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, popReturnSpeed * Time.deltaTime);
    }
}
