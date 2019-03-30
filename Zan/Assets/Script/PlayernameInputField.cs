using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


namespace Com.MyCompany.MyGame
{
    [RequireComponent(typeof(InputField))]
    public class PlayernameInputField : MonoBehaviour
    {
        #region Private Constants

        //Store Player Pref key to avoid typos

        const string playerNamePrefKey = "Playername";
        #endregion

        #region MonoBehavior CallBacks


        // Start is called before the first frame update
        void Start()
        {
            string defaultName = string.Empty;
            InputField _InputField = this.GetComponent<InputField>();
            if (_InputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _InputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        #endregion

        #region Public Methods

        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;

            PlayerPrefs.SetString(playerNamePrefKey, value);
        }

        #endregion
    }
}