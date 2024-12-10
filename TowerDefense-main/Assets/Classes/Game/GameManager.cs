using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int playerMaxHealth = 100;
    public int playerCurrentHealth;
    public TextMeshProUGUI healthText;

    [SerializeField] private int minDamage = 5;
    [SerializeField] private int maxDamage = 15;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            playerCurrentHealth = playerMaxHealth;
            UpdateHealthUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DamagePlayer(int baseDamage)
    {
        int actualDamage = Random.Range(minDamage, maxDamage + 1);
        playerCurrentHealth -= actualDamage;
        UpdateHealthUI();

        if (playerCurrentHealth <= 0)
        {
            Time.timeScale = 0;
            Debug.Log("Game Over!");
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {playerCurrentHealth}";
        }
    }
}