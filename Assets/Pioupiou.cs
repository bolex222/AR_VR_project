using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Photon.Pun;
using UnityEngine;

public class Pioupiou : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject pioupiouMesh;
    [SerializeField] private GameObject pfBulletProjectile;
    [SerializeField] private Transform bulletOrigin;
    public AllGenericTypes.Team playerTeam;

    private float _gunHeat;

    public void Update()
    {
        if (_gunHeat > 0)
        {
            _gunHeat -= Time.deltaTime;
        }
    }

    public void OnShot()
    {
        //photonView.RPC("ShootGun", RpcTarget.All);
        ShootGun();
    }

    //[PunRPC]
    public void ShootGun()
    {
        Debug.Log("here");
        if (_gunHeat <= 0)
        {
            Quaternion aimRotateDirection = pioupiouMesh.transform.rotation.normalized;
            _gunHeat = GameDataManager.Instance.data.DelayShot; // this is the interval between firing.
            GameObject bullet = PhotonNetwork.Instantiate("Prefabs/"+ pfBulletProjectile.name, bulletOrigin.position, aimRotateDirection);
            BulletProjectile bulletScript = bullet.GetComponent<BulletProjectile>();
            bulletScript.teamToAvoid = playerTeam;
        }
    }
}