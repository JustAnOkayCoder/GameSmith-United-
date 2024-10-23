using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MoneyDisplayText;
    [SerializeField] private int StartingMoney;
    private int CurrentMoney;
    private void Start()
    {
        CurrentMoney = StartingMoney;
        MoneyDisplayText.SetText($"${StartingMoney}");
    }

    public void AddMoney(int MoneyToAdd)
    {
        CurrentMoney += MoneyToAdd;
        MoneyDisplayText.SetText($"${CurrentMoney}");//shows the money updating on screen from purchases
    }

    public int GetMoney()
    {
        return CurrentMoney;
        
    }

    
}
