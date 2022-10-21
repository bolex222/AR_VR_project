using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Cinemachine;

public class UserManagerVR : MonoBehaviourPunCallbacks
{
    public static GameObject UserMeInstance;

    /// <summary>
    /// Represents the GameObject on which to change the color for the local player
    /// </summary>
    //public GameObject GameObjectLocalPlayerColor;

    /// <summary>
    /// The FreeLookCameraRig GameObject to configure for the UserMe
    // /// </summary>
    public Camera CameraPlayer;
    bool CursorLockedVar;

    void Awake()
    {
        if (photonView.IsMine)
        {
            //Debug.LogFormat("Avatar UserMe created for userId {0}", photonView.ViewID);
            UserMeInstance = gameObject; 
        }

        Debug.Log( "is mine ?" + photonView.IsMine);
        CameraPlayer.enabled = true;
        

        //DontDestroyOnLoad(gameObject);



    }

    // Start is called before the first frame update
    // void Start()
    // {
    //     //updateGoFreeLookCameraRig();
    //     //followLocalPlayer();
    //     // activateLocalPlayer();
    //
    //     // Cursor.lockState = CursorLockMode.Locked;
    //     // Cursor.visible = (false);
    //     // CursorLockedVar = (true);
    // }

    // void Update()
    // {
    //     if (Input.GetKeyDown("escape") && !CursorLockedVar)
    //     {
    //
    //         Cursor.lockState = CursorLockMode.Locked;
    //         Cursor.visible = (false);
    //         CursorLockedVar = (true);
    //
    //     }
    //     else if (Input.GetKeyDown("escape") && CursorLockedVar)
    //     {
    //         Cursor.lockState = CursorLockMode.None;
    //         Cursor.visible = (true);
    //         CursorLockedVar = (false);
    //     }
    // }

    protected void activateLocalPlayer()
    {
        // enable the ThirdPersonUserControl if it is a Loacl player = UserMe
        // disable the ThirdPersonUserControl if it is not a Loacl player = UserOther
        //GetComponent<CharacterController>().enabled = photonView.IsMine;
        //GetComponent<Rigidbody>().isKinematic = !photonView.IsMine;
        if (photonView.IsMine)
        {
            try
            {
                // Change the material of the Ethan Glasses
                //GameObjectLocalPlayerColor.GetComponent<Renderer>().material = PlayerLocalMat;
            }
            catch (System.Exception)
            {

            }
        }
    }
}