using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PhaseGUI : MonoBehaviour {

    public Text PhaseText;

    // Managers
    public GameObject PhaseMan;

    // Use this for initialization
    void Start()
    {
        PhaseMan = GameObject.FindGameObjectWithTag("PhaseManager");
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePhaseText();
    }

    void UpdatePhaseText()
    {
        if (PhaseMan.GetComponent<PhaseManager>().PhaseQueue.Peek().ToString() == "Casing")
            PhaseText.text = "Casing Stage: Press F to continue";
        if (PhaseMan.GetComponent<PhaseManager>().PhaseQueue.Peek().ToString() == "Planning")
            PhaseText.text = "Pre-planning Stage: Press F to continue";
        if (PhaseMan.GetComponent<PhaseManager>().PhaseQueue.Peek().ToString() == "Execution")
            PhaseText.text = "";
    }
}
