using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class GameManagerSamurai : MonoBehaviourPunCallbacks
    {
        [Tooltip("The preafab to user for representing the player")]
        public GameObject playerPrefab;

        [SerializeField] GameObject LosePanel;
        [SerializeField] GameObject WinPanel;
        [SerializeField] Camera camera;
        [SerializeField] GameObject Panel;
        [SerializeField] GameObject MatchingText;
        [SerializeField] float FadeOutSpeed=1;

        public bool finish;
        public bool win;
        public bool StartEventEnd = false;
        private bool first = true;
        private bool firstPerson = false;
        public static bool StartEventSwitch = false;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(StartEventSwitch);
            }
            else
            {
                StartEventSwitch= (bool)stream.ReceiveNext();
            }
        }



        private void Start()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1) firstPerson = true;
                LosePanel.SetActive(false);
            WinPanel.SetActive(false);
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
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                camera.gameObject.GetComponent<Animator>().SetTrigger("StartEvent");
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
            //finish = SamuraiManager.finish;
            //win = SamuraiManager.LocalPlayerInstance.GetComponent<SamuraiManager>().win;

            if (PhotonNetwork.CurrentRoom.PlayerCount == 2&&!firstPerson )
            {
                StartEventSwitch = true;
                StartCoroutine("StartEvent");
            }

            if(StartEventSwitch)StartCoroutine("StartEvent");
            if (SamuraiManager.finish&&first)
            {
                first = false;
                Debug.LogWarning("Finish");
                if (win)
                {
                    Debug.LogWarning("Win");
                    WinPanel.SetActive(true);
                }
                else
                {
                    LosePanel.SetActive(true);
                    Debug.LogWarning("Lose");
                }
            }

            if (camera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("CameraEventEnd"))
            {
                StartEventEnd = true;
            }
        }

        private IEnumerator StartEvent()
        {
            while (Panel.GetComponent<Image>().color.a >= 0)
            {
                Panel.GetComponent<Image>().color -= new Color(0, 0, 0, FadeOutSpeed * Time.deltaTime);
                yield return null;
                MatchingText.GetComponent<Text>().color -= new Color(0, 0, 0, FadeOutSpeed * Time.deltaTime);
                yield return null;
            }
            camera.GetComponent<Animator>().SetTrigger("StartEvent");
            
        }



    }

}
