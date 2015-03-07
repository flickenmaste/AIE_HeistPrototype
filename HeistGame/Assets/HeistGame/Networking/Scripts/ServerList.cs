using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ServerList : MonoBehaviour {

    public List<ServerEntry> ServerEntries = new List<ServerEntry>();
    List<GameObject> serverButtons = new List<GameObject>();

    public GameObject ServerButtonPrefab;
    public ScrollRect ServerPanel;

    public NetworkedMenu networkMenu;

    void debugEntries()
    {
        foreach (var server in ServerEntries)
        {
            server.printServerDesc();
        }
    }

    void cleanUpEntries()
    {
        // Destroy all of the buttons
        foreach (var server in serverButtons)
        {
            Destroy(server);
        }
    }

    public void PopulateEntries()
    {
        Bolt.Zeus.RequestSessionList(); // Get server list

        // Create entries for each server in the session list
        foreach (var server in BoltNetwork.SessionList)
        {
            ServerEntries.Add(new ServerEntry(server.Value));
        }
    }

    public void PopulatePanel()
    {
        Transform contentTrans = ServerPanel.content.transform;

        bool firstRun = true;

        int contentCounter = 0;
        int contentHeight = (int)contentTrans.GetComponent<RectTransform>().rect.height;

        // Create a button for each server
        foreach (var server in ServerEntries)
        {
            // Create the object
            GameObject newButton = Instantiate(ServerButtonPrefab) as GameObject;
            serverButtons.Add(newButton);
            

            // Assign it to the content panel
            newButton.GetComponent<Transform>().SetParent(contentTrans);

            // Assign data
            ServerEntryView dataView = newButton.GetComponent<ServerEntryView>();
            dataView.serverEntryInformation = server;
            dataView.masterMenu = networkMenu;

            // Calculate position
            RectTransform rectTrans = newButton.GetComponent<RectTransform>();
            rectTrans.offsetMin = new Vector2(0, 0 - contentCounter);
            rectTrans.offsetMax = new Vector2(0, 0 - contentCounter);

            contentCounter += (int)rectTrans.rect.height;   // increment calculator

            rectTrans.localScale = Vector3.one; // reset scale

            Debug.Log(contentCounter);
            // fuck me
        }
    }
    
    public void ResetBrowser()
    {
        cleanUpEntries();
        PopulateEntries();
        PopulatePanel();
    }

    void OnEnable()
    {
        ResetBrowser();
    }
}
