using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;
using Photon.Pun;

namespace Com.MyCompany.MyGame
{
    /// <summary>
    /// Player manager.
    /// Handles fire Input and Beams.
    /// </summary>
    public class SamuraiManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(Slash);
            }
            else
            {
                this.Slash = (bool)stream.ReceiveNext();
            }
        }
        #endregion

        #region Public Fields

        public static GameObject LocalPlayerInstance;

        #endregion

        #region Private Fields
        
        private bool Slash;
        

        [Tooltip("The Player's UI GameObject Prefab")]
        [SerializeField]
        private GameObject playerUiPrefab;

        private int PlayerCount = 0;
        #endregion


        #region MonoBehaviour CallBacks
#if !UNITY_5_4_OR_NEWER
        /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
#endif


        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
        }
        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
           
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }
            DontDestroyOnLoad(this.gameObject);
        }


        private void Start()
        {
            PlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            if (PlayerCount == 1)
            {
                this.transform.position = new Vector3(0, 0, -1);
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                if (photonView.IsMine)
                {

                }
            }
            else if (PlayerCount == 2)
            {
                this.transform.position = new Vector3(0, 0, 1);
                this.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            

            if (playerUiPrefab != null)
            {
                GameObject _uiGo = Instantiate(playerUiPrefab);
                _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }
#if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
            {
                this.CalledOnLevelWasLoaded(scene.buildIndex);
            };
#endif

        }
        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void Update()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected) return;
            if (photonView.IsMine)
            {
                ProcessInputs();
            }
            // trigger Beams active state
            
        }

        

        #endregion

        #region Custom

        /// <summary>
        /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
        /// </summary>
       

        #endregion
    }
}