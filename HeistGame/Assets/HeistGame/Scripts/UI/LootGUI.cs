using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LootGUI : MonoBehaviour {

    public Text LootText;

    public GameObject Player;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePhaseText();
    }

    void UpdatePhaseText()
    {
        if (Player.GetComponent<Player>().CarryingGold)
            LootText.text = "Carrying Gold";
        else
            LootText.text = "";
    }
}
