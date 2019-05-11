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
                stream.SendNext(win);
                stream.SendNext(finish);
            }
            else
            {
                this.win = (bool)stream.ReceiveNext();
                finish = (bool)stream.ReceiveNext();
            }
        }
        #endregion

        #region Public Fields

        public static GameObject LocalPlayerInstance;


        public float slashpowermax = 3;
        public float slashpowermin = 1.5f;

        public bool win=true;
        public static bool finish;

        

        #endregion

        #region Private Fields
        private bool Slash;

        [Tooltip("The Player's UI GameObject Prefab")]
        [SerializeField]
        private GameObject playerUiPrefab;

        private int PlayerCount = 0;


        private Vector3 acceleration;
        private Vector3 accelerationbef;
        private float kakudo;
        private float time;
        private bool slash;
        private Animator animator;
        private GameObject GameManager;
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
            /*if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }*/
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

            Input.gyro.enabled = true;
            acceleration = Input.acceleration;
            animator = this.GetComponent<Animator>();

            Input.compass.enabled = true;
            Input.location.Start();

            GameManager = GameObject.Find("GameManager");

            PlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            if (PlayerCount == 1&&photonView.IsMine)
            {
                this.transform.position = new Vector3(0, 0, -0.6f);
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                
            }
            else if (PlayerCount == 2&&photonView.IsMine)
            {
                this.transform.position = new Vector3(0, 0, 0.6f);
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
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
                if (Input.GetMouseButtonDown(0))
                {
                    Slash = true;
                    this.GetComponent<Animator>().SetTrigger("Attack");
                }


                if ((acceleration - accelerationbef).magnitude > slashpowermax)
                {
                    slash = true;
                }
                if (slash)
                {
                    kakudo += Input.gyro.rotationRateUnbiased.z;
                    if ((acceleration - accelerationbef).magnitude < slashpowermin && kakudo < 0 )
                    {
                        this.GetComponent<Animator>().SetTrigger("Attack");
                        //sword.GetComponent<Animator>().ResetTrigger("SlashR");
                        slash = false;
                        time = 0;
                        kakudo = 0;
                        //Vibration.Vibrate(1000);
                    }
                    else if ((acceleration - accelerationbef).magnitude < slashpowermin && kakudo > 0 )
                    {
                        this.GetComponent<Animator>().SetTrigger("Attack");
                        //animator.ResetTrigger("HitR");
                        //sword.GetComponent<Animator>().SetTrigger("SlashR");
                        //sword.GetComponent<Animator>().ResetTrigger("SlashL");
                        slash = false;
                        time = 0;
                        kakudo = 0;
                    }
                }
                accelerationbef = acceleration;

            }

            GameManager.GetComponent<GameManagerSamurai>().finish = finish;
            GameManager.GetComponent<GameManagerSamurai>().win = win;

        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.LogWarning("Collision");
            if (other.tag == "Weapon"&& photonView.IsMine)
            {
                Debug.LogWarning("Death");
                win = false;
                finish = true;
            }
        }



        #endregion

        #region Custom

        /// <summary>
        /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
        /// </summary>


        #endregion
    }
}