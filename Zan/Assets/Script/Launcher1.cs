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

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;
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
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            //Connect();
        }


        public override void OnConnectedToMaster()
        {
            //PhotonNetwork.JoinRandomRoom();
            Debug.Log("Pun Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            PhotonNetwork.CreateRoom(null, new RoomOptions());
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom(9 called by PUN. Now this client is in a room.");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the BattleRoom");

                PhotonNetwork.LoadLevel("BattleRoom");
            }

        }


        #endregion
        // Update is called once per frame
        #region Public Methods
        public void Connect()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        #endregion


    }
}