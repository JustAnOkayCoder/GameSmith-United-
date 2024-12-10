using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    void Update()
    {
        if (GameManager.Instance != null)
        {
            healthText.text = $"Health: {GameManager.Instance.playerCurrentHealth}";
        }
    }
}