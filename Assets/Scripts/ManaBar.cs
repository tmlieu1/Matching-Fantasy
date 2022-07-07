using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;

    PlayerManager playManager;
    private void Start()
    {
        playManager = GetComponentInParent<PlayerManager>();
    }

    void Update(){
        slider.value = playManager.mana/playManager.maxMana;
        //slider.value = GameManager.currPlayerHp / GameManager.maxPlayerHp;
    }
}
