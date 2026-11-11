using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartsUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image heartPrefab;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Sprite fullHeart;
    private List<Image> hearts = new List<Image>();

    void Start()
    {
        for (int i = 0; i < playerHealth.MaxHealth; i++)
        {
            Image heart = Instantiate(heartPrefab, transform);
            hearts.Add(heart);
        }
        playerHealth.OnHealthChanged += Redraw;
        Redraw();
    }

    private void Redraw()
    {
        Debug.Log("Redraw: health = " + playerHealth.CurrentHealth);
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].sprite = i < playerHealth.CurrentHealth ? fullHeart : emptyHeart;
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= Redraw;
        }
    }
}
