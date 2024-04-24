using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class Dice
{
    public int DiceValue = 0;
    public int DiceValueMax = 6;

    public void LaunchDice()
    {
        DiceValue = UnityEngine.Random.Range(0, DiceValueMax + 1);
    }
}

public class GameManager : MonoBehaviour
{
    public Dice Dice1;
    public Dice Dice2;
    public Dice Dice3;

    public TextMeshProUGUI TextDice1;
    public TextMeshProUGUI TextDice2;
    public TextMeshProUGUI TextDice3;

    private void Start()
    {
        AffDicesValues();
    }

    public void LaunchDices()
    {
        Dice1.LaunchDice();
        Dice2.LaunchDice();
        Dice3.LaunchDice();

        AffDicesValues();
    }

    void AffDicesValues()
    {
        TextDice1.text = $"Dice 1 : {Dice1.DiceValue.ToString()}";
        TextDice2.text = $"Dice 2 : {Dice2.DiceValue.ToString()}";
        TextDice3.text = $"Dice 3 : {Dice3.DiceValue.ToString()}";
    }
}
