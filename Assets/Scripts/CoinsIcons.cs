using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinsIcons : MonoBehaviour
{
    [SerializeField] private GameObject CoinFrame;

    private int numberIcons;
    void Start()
    {
        numberIcons = transform.childCount;
        StartCoroutine(moveIcons());
    }

    IEnumerator moveIcons() 
    {
        for (int i = 0; i < numberIcons; i++) 
        {
            GameObject icon = transform.GetChild(i).gameObject;
            icon.SetActive(true);
            icon.transform.DOMove(CoinFrame.transform.position, 1f).OnComplete(() => { Destroy(icon); }); 
            yield return new WaitForSeconds(0.1f);
        }
    }
}
