using UnityEngine;
using System.Collections;

public class GameManagerVik : Photon.MonoBehaviour {

    public string loginOk = "";
    // this is a object name (must be in any Resources folder) of the prefab to spawn as player avatar.
    // read the documentation for info how to spawn dynamically loaded game objects at runtime (not using Resources folders)
    public string playerPrefabName = "Charprefab";

    void OnJoinedRoom()
    {
        StartGame();
        LoginCheck();
    }

    public void LoginCheck()
    {
        byte evCode = 1;
        //string contentMessage = "PlayerName=" + PhotonNetwork.playerName + ",PlayerPassword=" + PlayerPrefs.GetString("playerPassword");
       // Debug.Log(contentMessage);
        //byte[] content = System.Text.Encoding.UTF8.GetBytes(contentMessage);
        bool reliable = true;
        LoginInfo newInfo = new LoginInfo();
        newInfo.Username = PhotonNetwork.playerName;
        newInfo.Password = PlayerPrefs.GetString("playerPassword");
        DataPasser.Instance.playerPassword = newInfo.Password;

        PhotonNetwork.RaiseEvent(evCode, LoginInfo.Serialize(newInfo), reliable, null);
    }


    IEnumerator OnLeftRoom()
    {
        //Easy way to reset the level: Otherwise we'd manually reset the camera

        //Wait untill Photon is properly disconnected (empty room, and connected back to main server)
        while(PhotonNetwork.room!=null || PhotonNetwork.connected==false)
            yield return 0;

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

    }

    void StartGame()
    {
        Camera.main.farClipPlane = 1000; //Main menu set this to 0.4 for a nicer BG    

        //prepare instantiation data for the viking: Randomly diable the axe and/or shield
        bool[] enabledRenderers = new bool[2];
        enabledRenderers[0] = Random.Range(0,2)==0;//Axe
        enabledRenderers[1] = Random.Range(0, 2) == 0; ;//Shield
        
        object[] objs = new object[1]; // Put our bool data in an object array, to send
        objs[0] = enabledRenderers;

        // Spawn our local player
        DataPasser.Instance.character = PhotonNetwork.Instantiate("Charprefab", transform.position, Quaternion.identity, 0, objs);
        DataPasser.Instance.pet = PhotonNetwork.Instantiate("Pet", transform.position, Quaternion.identity, 0, objs);
    }

    void OnGUI()
    {
        if (PhotonNetwork.room == null) return; //Only display this GUI when inside a room

        if (loginOk == "WrongPassword")
        {
            PhotonNetwork.LeaveRoom();
            GamesparksManager.instance.GetComponent<GamesparksLoginUI>().loggedIn = false;
        }

        if (GUILayout.Button("Leave Room"))
        {
            PhotonNetwork.LeaveRoom();
            GamesparksManager.instance.GetComponent<GamesparksLoginUI>().loggedIn = false;
        }
    }

    void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("OnDisconnectedFromPhoton");
    }    
}
