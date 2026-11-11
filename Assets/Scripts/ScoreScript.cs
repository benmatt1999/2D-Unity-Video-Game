using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    public static ScoreScript Instance { get; private set; }
    public event System.Action<int> OnScoreChanged;
    private int score;
    public int Score => score;
    void Awake()
    {
        score = 0;
        Instance = this;
    }

    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke(score);
    }
}
