using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public RectTransform powerupBar;
    public RectTransform healthBar;
    public Text gunAmmo;
    public Text flareAmmo;
    public GameObject player;

    private PlayerMovement _playerController;
    private Shoot _playerAmmoController;

    // Use this for initialization
    void Start()
    {
        _playerController = player.GetComponent<PlayerMovement>();
        _playerAmmoController = player.GetComponentInChildren<Shoot>();
    }

    // Update is called once per frame
    void Update()
    {
        updatePowerupBar();
        updateHealthBar();
        updateAmmo();
    }

    void updatePowerupBar()
    {
        if (_playerController.remainingPowerupTime < 0 || _playerController.currentPowerup.Value == 0) return;
        powerupBar.localScale =
            new Vector3(1, _playerController.remainingPowerupTime / _playerController.currentPowerup.Value, 1);
    }

    void updateHealthBar()
    {
        if (_playerController.health < 0) return;
        healthBar.localScale = new Vector3(1, _playerController.health / 100, 1);
    }

    void updateAmmo()
    {
        if (_playerAmmoController.gunAmmo == -1 || _playerAmmoController.flareAmmo == -1)
        {
            gunAmmo.text = "inf";
            flareAmmo.text = "inf";
        }
        else
        {
            gunAmmo.text = _playerAmmoController.gunAmmo + "";
            flareAmmo.text = _playerAmmoController.flareAmmo + "";
        }
    }
}