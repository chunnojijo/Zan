using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class GameManagerSamurai : MonoBehaviourPunCallbacks
    {
        [Tooltip("The preafab to user for representing the player")]
        public GameObject playerPrefab;

        private void Start()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Read><a>Missing</a></Color> player Prefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                if (SamuraiManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);

                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
                
            }
        }

        #region Photon Callbacks

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }
        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        #endregion

        #region Private Methods

        void LoadArea()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwor :Trying to Load a level but we are not the master Client");

            }
            Debug.LogFormat("PhotonNetwork:Loading Level :{0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Room for" + PhotonNetwork.CurrentRoom.PlayerCount);
        }
        #endregion

        #region Photon Callbacks

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", newPlayer.NickName);
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                //LoadArea();
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient{0}", PhotonNetwork.IsMasterClient);
                //LoadArea();
            }
        }
        #endregion
        // Update is called once per frame
        void Update()
        {

        }
    }

}
