using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;                       
    public PlayerController playerController;

    [SerializeField] private GameObject LosePanel;      // Lose Screen
    [SerializeField] private GameObject WinPanel;       // Win Screen

    [HideInInspector] public int TotalCoins;            // Total coins of the player
    [HideInInspector] public int currentGameScore;      // coins gained during last game

    [SerializeField] private GameObject[] AllCubes;     // Cubes at the end (score multiplicators)
    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        else 
        {
            Destroy(this.gameObject);
        }
        LoadData();                     
    }


    public void StartGame() 
    {
        playerController.StartGame();                   
    }
    public IEnumerator WinGame(int sizePlayer)                          
    {
        Camera.main.GetComponent<CameraFollow>().LookAtCubes();         // Move Camera to look at cubes
        yield return new WaitForSeconds(1);
        StartCoroutine(EnableCubesParticles(AllCubes, sizePlayer));     // Explode cubes according to size of the player
        currentGameScore *= (sizePlayer + 1);                           // multiply the earned coins
        TotalCoins += currentGameScore;
        SaveData();
    }

    private IEnumerator EnableCubesParticles(GameObject[] cubes, int max) 
    {
        for (int i = 0; i< max; i++) 
        {
            for (int j = 0; j < cubes[i].transform.childCount; j++)
            {
                cubes[i].transform.GetChild(j).GetComponent<Cubes>().EnableParticles();
            }
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(1f);
        WinPanel.SetActive(true);
    }
    public IEnumerator LoseGame()                   
    {
        yield return new WaitForSeconds(1f);
        LosePanel.SetActive(true);
        TotalCoins += currentGameScore;
        SaveData();
    }

    private void LoadData() 
    {
        if (PlayerPrefs.HasKey("TotalCoins"))
        {
            TotalCoins = PlayerPrefs.GetInt("TotalCoins");
        }
        else {
            PlayerPrefs.SetInt("TotalCoins", 0);
        }
    }

    public void SaveData() 
    {
        PlayerPrefs.SetInt("TotalCoins", TotalCoins);
    }
}
