using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Photon.Pun;
using Shooter;
using UnityEngine;

public class Pioupiou : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject pioupiouMesh;
    [SerializeField] private GameObject pfBulletProjectile;
    [SerializeField] private Transform bulletOrigin;
    public AllGenericTypes.Team playerTeam;

    private float _gunHeat;

    private void Start()
    {
        Debug.Log($"pioupiou start with team {playerTeam}");
    }

    public void Update()
    {
        if (_gunHeat > 0)
        {
            _gunHeat -= Time.deltaTime;
        }
    }

    public void OnShot()
    {
        photonView.RPC("ShootGun", RpcTarget.AllViaServer);
        //ShootGun();
    }

    [PunRPC]
    public void ShootGun()
    {
        if (_gunHeat <= 0)
        {
            Quaternion aimRotateDirection = pioupiouMesh.transform.rotation.normalized;
            _gunHeat = GameDataManager.Instance.data.DelayShot; // this is the interval between firing.
            Debug.Log("instantiate bullet by piou piou");
            GameObject bullet = Instantiate(pfBulletProjectile, bulletOrigin.position, aimRotateDirection);
            BulletProjectile bulletScript = bullet.GetComponent<BulletProjectile>();
            bulletScript.teamToAvoid = playerTeam;
        }
    }
}