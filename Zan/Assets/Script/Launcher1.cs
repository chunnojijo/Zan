using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{

    public class Launcher1 : MonoBehaviourPunCallbacks
    {
        [Tooltip("The maxim number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [SerializeField]
        private GameObject ConnectPanel;

        [SerializeField]
        private GameObject EnterPanel;

        [SerializeField]
        private GameObject EnterButton;

        

        
        #region Private Serializable Fields

        #endregion

        #region Private Fields

        /// <summary>
        /// This client's verion number. Users are separated from each other by game Version (which allows you to make breaking changes).
        /// </summary>

        string gameVersion = "1";

        #endregion

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();

            PhotonNetwork.GameVersion = gameVersion;
            //Connect();
            ConnectPanel.SetActive(true);
            EnterButton.SetActive(false);
            EnterPanel.SetActive(false);
        }


        public override void OnConnectedToMaster()
        {
            //PhotonNetwork.JoinRandomRoom();
            Debug.Log("Pun Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            ConnectPanel.SetActive(false);
            EnterButton.SetActive(true);
            EnterPanel.SetActive(false);
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom(9 called by PUN. Now this client is in a room.");
            
            Debug.Log("We load the BattleRoom");
        
            PhotonNetwork.LoadLevel("BattleRoom");
            

        }


        #endregion
        // Update is called once per frame
        #region Public Methods
        public void Connect()
        {
            
            PhotonNetwork.JoinRandomRoom();
            ConnectPanel.SetActive(false);
            EnterButton.SetActive(false);
            EnterPanel.SetActive(true);

        }
        #endregion


    }
}