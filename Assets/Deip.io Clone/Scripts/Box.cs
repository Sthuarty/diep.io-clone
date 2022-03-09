using Photon.Pun;
using UnityEngine;

public class Box : MonoBehaviour, IDamageable {
    [SerializeField] private int _maxCondition = 40;
    [SerializeField] private int _ammoOnBreak = 10;
    [SerializeField] private int _scoreOnBreak = 10;

    [SerializeField] private int _condition;


    private void Awake() {
        _condition = _maxCondition;
        if (PhotonNetwork.IsConnected)
            DontDestroyOnLoad(this.gameObject);
    }

    private void OnParticleCollision(GameObject other) {
        if (string.Equals(other.tag, "Bullet"))
            TakeDamage(other.GetComponentInParent<Character>());
    }

    public void TakeDamage(Character whoDamaged) {
        _condition -= whoDamaged.CurrentGun.damageAmount;
        if (_condition <= 0) Break(whoDamaged);
    }

    private void Break(Character whoBroke) {
        whoBroke.IncreaseScore(_scoreOnBreak);
        whoBroke.IncreaseAmmo(_ammoOnBreak);

        if (PhotonNetwork.IsConnected && GetComponent<PhotonView>().IsMine) PhotonNetwork.Destroy(gameObject);
        else if (!PhotonNetwork.IsConnected) Destroy(this.gameObject);
    }
}
