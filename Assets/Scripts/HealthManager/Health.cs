using Interfaces;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class Health : MonoBehaviourPunCallbacks, IPunObservable
{
    public float maxHealth;
    public float currentHealth;
    public GameObject AudioManager;
    public AudioSource PlayerDeath;
    public AudioClip playerDeathSound;
    public AudioClip playerSpawnSound;
    public AudioClip playerHurtSound;

    [SerializeField] private Transform player;
    [SerializeField] private GameObject deathScreen;

    public HealthBar healthBar;

    private float respawnTime;
    private bool _timerOn = false;
    private float _timeLeft;

    private void Start()
    {
        respawnTime = GameDataManager.Instance.data.DelayRespawn;

        maxHealth = GameDataManager.Instance.data.LifeNumber;
        _timeLeft = respawnTime;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        RespawnTimer();
    }

    //[PunRPC]
    public void TakeDamage(float damage)
    {
        Debug.Log(photonView.IsMine);
        if (photonView.IsMine)
        {
            Play(playerHurtSound);
        }

        if (!photonView.IsMine) return;

        //healthBar.SetHealth(currentHealth);
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Die();
            Play(playerDeathSound);
            currentHealth = maxHealth;

            healthBar.SetMaxHealth(maxHealth);
        }
    }

    public void Play(AudioClip clip)
    {
        PlayerDeath.clip = clip;
        PlayerDeath.Play();
    }

    [PunRPC]
    private void PlayerVisibilityVR(bool state)
    {
        transform.GetChild(0).gameObject.SetActive(state);
        transform.GetChild(1).gameObject.SetActive(state);
        healthBar.gameObject.SetActive(state);
        gameObject.GetComponent<InputActionManager>().enabled = state;
        gameObject.GetComponent<Collider>().enabled = state;
    }

    public void Die()
    {
        photonView.RPC(
            UserDeviceManager.GetDeviceUsed() == UserDeviceType.PC ? "PlayerVisibility" : "PlayerVisibilityVR",
            RpcTarget.AllViaServer, false);

        _timerOn = true;

        deathScreen.gameObject.SetActive(true);
        if (photonView.IsMine)
        {
            GetSpawn();
        }
    }


    private void GetSpawn()
    {
        AllGenericTypes.Team team = MatchMakingNetworkManager.playersTeamA.Contains(PhotonNetwork.LocalPlayer)
            ? AllGenericTypes.Team.TeamA
            : AllGenericTypes.Team.TeamB;

        Transform spawn =
            team == AllGenericTypes.Team.TeamA
                ? SpawnerManager.instance.GetTeamSpawn(0)
                : SpawnerManager.instance.GetTeamSpawn(1);

        player.transform.position = spawn.position;
        player.transform.rotation = spawn.rotation;
        Physics.SyncTransforms();
        if (photonView.IsMine)
        {
            Play(playerSpawnSound);
        }
    }

    private void RespawnTimer()
    {
        if (_timerOn)
        {
            if (_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
            }
            else
            {
                _timerOn = false;

                photonView.RPC("ResetHealth", RpcTarget.AllViaServer);

                if (UserDeviceManager.GetDeviceUsed() == UserDeviceType.PC)
                {
                    photonView.RPC("PlayerVisibility", RpcTarget.AllViaServer, true);
                }
                else
                {
                    photonView.RPC("PlayerVisibilityVR", RpcTarget.AllViaServer, true);
                }

                deathScreen.gameObject.SetActive(false);
                _timeLeft = respawnTime;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            healthBar.SetHealth(currentHealth);
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (float)stream.ReceiveNext();
            healthBar.SetHealth(currentHealth);
        }
    }
}