using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields

    /// <summary>
    /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    /// </summary>
    [Tooltip(
        "The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    [SerializeField] private TMP_InputField ipInputField;
    [SerializeField] private TMP_InputField portInputField;
    [SerializeField] private TMP_InputField nickNameInputField;
    //[SerializeField] private TMP_Button nickNameInputField;


    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;

    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;


    /// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
    /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// this parameter is true when we click on the button Start and false if we come back from a game to the Lobby.
    /// </summary>
    bool isConnecting;

    #endregion


    #region Private Fields

    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    string gameVersion = "1";

    #endregion


    #region MonoBehaviour CallBacks

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        // try connecting to the PUN server
        //Connect();
        // Now called by the UI.

        progressLabel.SetActive(false);
        controlPanel.SetActive(true);

        isConnecting = false;
        //ipInputField.text = "10.169.129.241";
        // ipInputField.text = "192.168.1.18";
        ipInputField.text = "10.169.130.167";
        // ipInputField.text = "10.169.129.241";
        // ipInputField.text = "10.188.191.49";
        portInputField.text = "5055";
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Start the connection process when click on the Start button.
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        if (string.IsNullOrEmpty(nickNameInputField.text))
        {
            Debug.LogError("Nickname must be set to a value");
            nickNameInputField.placeholder.GetComponent<Text>().text = "!! Nickname Empty!!";
            return;
        }

        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        isConnecting = true;
        PhotonNetwork.NickName = nickNameInputField.text;

        // Tells to the user that we are trying to connect
        isConnecting = true;
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room.
            // If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {

            // UDP port 5056 for local server, 5055 for cloud server
            string ip = ipInputField.text;
            int port = int.Parse(portInputField.text);



            if (!string.IsNullOrWhiteSpace(ip) && port > 0)
            {
                // Use local server OnPremise
                // See this thread for more details https://forum.photonengine.com/discussion/comment/43218/#Comment_43218
                PhotonNetwork.PhotonServerSettings.AppSettings.UseNameServer = false;
                PhotonNetwork.PhotonServerSettings.AppSettings.Server = ip;
                PhotonNetwork.PhotonServerSettings.AppSettings.Port = port;
                PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = null;
                PhotonNetwork.PhotonServerSettings.AppSettings.Protocol =
                    ExitGames.Client.Photon.ConnectionProtocol.Udp;
            }
            else
            {
                // Used for cloud Server.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.PhotonServerSettings.AppSettings.UseNameServer = true;
                //PhotonNetwork.ConnectUsingSettings();
            }

            PhotonNetwork.ConnectUsingSettings();

        }
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        // VHD attention, cet événement est appelé lorsque on quitte une room et que l'on revient sur le Lobby.
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            // #Critical
            // Load the Room Level.
            PhotonNetwork.LoadLevel("Matchmaking");
        }
    }

    #endregion
}