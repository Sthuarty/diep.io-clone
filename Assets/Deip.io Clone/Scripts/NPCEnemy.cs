using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NPCEnemy : Character {
    [SerializeField] private List<GunScriptableObject> _guns;
    [SerializeField] private float _attackCullDown = 0.5f;
    private float _nextAttackTime;
    private Gun _gunController;


    public override void Awake() {
        base.Awake();
        CurrentGun = _guns[Random.Range(0, _guns.Count)];
        _gunController = GetComponentInChildren<Gun>();
    }

    #region SHOOT
    public void ShootHandler() {
        if (!PhotonNetwork.IsConnected || (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)) {
            if (!IsInCullDownTime) {
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient) PhotonView.RPC("RPC_GunFire", RpcTarget.All);
                else _gunController.Fire();

                _nextAttackTime = Time.time + _attackCullDown;
            }
        }
    }

    [PunRPC] public void RPC_GunFire() => _gunController.Fire();

    #endregion

    public bool IsInCullDownTime => Time.time < _nextAttackTime;
}
