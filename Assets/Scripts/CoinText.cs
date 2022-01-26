using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    private void Start()
    {
        if(!gameObject.CompareTag("CoinTextEnd"))
            coinsText.text = GameManager.gm.TotalCoins.ToString();
        else
            coinsText.text = (GameManager.gm.TotalCoins - GameManager.gm.currentGameScore).ToString();
    }

    public void AddCoins()      // if coins of finish screen (used when collect buton clicked
    {
        coinsText.text = GameManager.gm.TotalCoins.ToString();
    }
}
