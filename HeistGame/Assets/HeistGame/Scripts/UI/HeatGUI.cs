using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeatGUI : MonoBehaviour {

    public Text HeatText;

    // Managers
    public GameObject EscaMan;

    // Use this for initialization
    void Start()
    {
        EscaMan = GameObject.FindGameObjectWithTag("EscalationManager");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHeatText();
    }

    void UpdateHeatText()
    {
        if (EscaMan.GetComponent<EscalationManager>().PhaseQueue.Count <= 4 && EscaMan.GetComponent<EscalationManager>().PhaseQueue.Count >= 1)
        {
            if (EscaMan.GetComponent<EscalationManager>().PhaseQueue.Peek().ToString() == "PhaseOne")
                HeatText.text = "Heat Level 1";
            if (EscaMan.GetComponent<EscalationManager>().PhaseQueue.Peek().ToString() == "PhaseTwo")
                HeatText.text = "Heat Level 2";
            if (EscaMan.GetComponent<EscalationManager>().PhaseQueue.Peek().ToString() == "PhaseThree")
                HeatText.text = "Heat Level 3";
        }
    }
}
