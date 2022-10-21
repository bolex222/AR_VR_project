using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using Photon.Pun;
using Shooter;

public class ThirdPersonShooterController : MonoBehaviourPunCallbacks
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private BulletProjectile pfBulletProjectile;
    [SerializeField] private Material pfBulletProjectileMaterial;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private PlayerTeam playerTeam;


    public ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    public AudioSource Shoot;
    public AudioClip ShootSound;

    private float gunHeat;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (gunHeat > 0)
        {
            gunHeat -= Time.deltaTime;
        }

        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        }

        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        } else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
        if (starterAssetsInputs.fire && starterAssetsInputs.aim)
        {
            photonView.RPC("FireBullet", RpcTarget.AllViaServer, mouseWorldPosition);
        }
    }

    [PunRPC]
    private void FireBullet(Vector3 mouseWorldPos)
    {
        Vector3 aimRotateDirection = (mouseWorldPos - spawnBulletPosition.position).normalized;
        if (gunHeat <= 0)
        {
            gunHeat = GameDataManager.Instance.data.DelayShot;  // this is the interval between firing.
            Debug.Log("TPS player instantiate ONLY ONE shitty bullet");

            
            

            BulletProjectile bullet = Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimRotateDirection, Vector3.up)) as BulletProjectile;

            if (playerTeam.team == Interfaces.AllGenericTypes.Team.TeamA)
            {
                ColorUtility.TryParseHtmlString(GameDataManager.Instance.data.ColorShotKMS, out Color bulletColor);
                pfBulletProjectileMaterial.color = bulletColor;
                bullet.pfBulletProjectileTrail.startColor = bulletColor;
                pfBulletProjectileMaterial.SetColor("_EmissionColor", bulletColor);
            }
            else
            {
                ColorUtility.TryParseHtmlString(GameDataManager.Instance.data.ColorShotVirus, out Color bulletColor);
                pfBulletProjectileMaterial.color = bulletColor;
                bullet.pfBulletProjectileTrail.startColor = bulletColor;
                pfBulletProjectileMaterial.SetColor("_EmissionColor", bulletColor);
            }
            BulletProjectile bulletScript = bullet.GetComponent<BulletProjectile>();
            bulletScript.teamToAvoid = playerTeam.team;
            Play(ShootSound);

        }
        starterAssetsInputs.fire = false;
    }
    
    public void Play(AudioClip clip)
    {
        Shoot.clip = clip;
        Shoot.Play();
    }
}
