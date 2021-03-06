﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour {

    public Text PlayerText;

    // Managers
    public GameObject Player;
    public GameObject Shooting;
    
    // Use this for initialization
	void Start () 
    {
        Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdatePlayerText();
	}

    void UpdatePlayerText()
    {
        PlayerText.text = "Health: " + Player.GetComponent<Player>().Health + " Bullets: " + Shooting.GetComponent<C_Shoot>().MaxShots;
    }
}
