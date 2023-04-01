using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour
{
    public Button VentureForthButton,
        RestoreHpButton,
        UpgradeHpButton,
        UpgradeSpeedButton,
        UpgradeDamageButton,
        ReduceOverheatButton;
    GameManager GameManager;
    PlayerController playerController;

    ShootingController[] shootingController;

    Dictionary<UpgradeType, int> upgradeCosts;

    public enum UpgradeType
    {
        RestoreHP,
        UpgradeHP,
        UpgradeSpeed,
        UpgradeDamage,
        ReduceOverheat
    }

    void Awake()
    {
        GameManager = GameObject.FindFirstObjectByType<GameManager>();
        playerController = GameObject.FindFirstObjectByType<PlayerController>();
        shootingController = GameObject.FindObjectsOfType<ShootingController>();
        upgradeCosts = new Dictionary<UpgradeType, int>()
        {
            {
                UpgradeType.RestoreHP,
                4 * (playerController.maxHealth - playerController.currentHealth) + 100
            },
            { UpgradeType.UpgradeHP, (int)(3 * playerController.maxHealth) },
            { UpgradeType.UpgradeSpeed, (int)(20 * playerController.speed) },
            { UpgradeType.UpgradeDamage, 100 * shootingController[0].damageAmount },
            { UpgradeType.ReduceOverheat, (int)(20 * shootingController[0].overHeatThreshold) }
        };

        CheckButtons();
    }

    void Update()
    {
        CheckButtons();
    }

    private void CheckButtons()
    {
        RestoreHpButton.interactable =
            playerController.Money >= upgradeCosts[UpgradeType.RestoreHP]
            && playerController.currentHealth < playerController.maxHealth;
        UpgradeHpButton.interactable =
            playerController.Money >= upgradeCosts[UpgradeType.UpgradeHP];
        UpgradeSpeedButton.interactable =
            playerController.Money >= upgradeCosts[UpgradeType.UpgradeSpeed];
        UpgradeDamageButton.interactable =
            playerController.Money >= upgradeCosts[UpgradeType.UpgradeDamage];
        ReduceOverheatButton.interactable =
            playerController.Money >= upgradeCosts[UpgradeType.ReduceOverheat];
    }

    public void VentureForth()
    {
        GameManager.StartLevel();
    }

    public void RestoreHP()
    {
        playerController.UpdateShip(UpgradeType.RestoreHP, upgradeCosts[UpgradeType.RestoreHP], 1);
        upgradeCosts[UpgradeType.RestoreHP] =
            4 * (playerController.maxHealth - playerController.currentHealth);
    }

    public void UpgradeHP()
    {
        playerController.UpdateShip(
            UpgradeType.UpgradeHP,
            upgradeCosts[UpgradeType.UpgradeHP],
            1.1f
        );
        upgradeCosts[UpgradeType.UpgradeHP] = (int)(3 * playerController.maxHealth);
    }

    public void UpgradeSpeed()
    {
        playerController.UpdateShip(
            UpgradeType.UpgradeSpeed,
            upgradeCosts[UpgradeType.UpgradeSpeed],
            1.1f
        );
        upgradeCosts[UpgradeType.UpgradeSpeed] = (int)(20 * playerController.speed);
    }

    public void UpgradeDamage()
    {
        playerController.UpdateShip(
            UpgradeType.UpgradeDamage,
            upgradeCosts[UpgradeType.UpgradeDamage],
            1
        );
        shootingController[0].damageAmount += 1;
        shootingController[1].damageAmount += 1;
        upgradeCosts[UpgradeType.UpgradeDamage] = 100 * shootingController[0].damageAmount;
    }

    public void ReduceOverheat()
    {
        shootingController[0].overHeatThreshold = (int)(
            shootingController[0].overHeatThreshold * 1.1f
        );
        shootingController[1].overHeatThreshold = (int)(
            shootingController[1].overHeatThreshold * 1.1f
        );
        playerController.UpdateShip(
            UpgradeType.ReduceOverheat,
            upgradeCosts[UpgradeType.ReduceOverheat],
            shootingController[0].overHeatThreshold
        );
        upgradeCosts[UpgradeType.ReduceOverheat] = (int)(
            20 * shootingController[0].overHeatThreshold
        );
    }
}
