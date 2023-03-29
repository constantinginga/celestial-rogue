using System.Collections;
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
        upgradeCosts = new Dictionary<UpgradeType, int>()
        {
            {
                UpgradeType.RestoreHP,
                5 * (playerController.maxHealth - playerController.currentHealth)
            },
            { UpgradeType.UpgradeHP, (int)(3 * playerController.maxHealth) },
            { UpgradeType.UpgradeSpeed, (int)(200 * playerController.speed) },
            { UpgradeType.UpgradeDamage, 10 },
            { UpgradeType.ReduceOverheat, 10 }
        };
    }

    // void OnBecameVisible()
    // {
    //     upgradeCosts = new Dictionary<UpgradeType, int>()
    //     {
    //         {
    //             UpgradeType.RestoreHP,
    //             5 * (playerController.maxHealth - playerController.currentHealth)
    //         },
    //         { UpgradeType.UpgradeHP, (int)(3 * playerController.maxHealth) },
    //         { UpgradeType.UpgradeSpeed, 10 },
    //         { UpgradeType.UpgradeDamage, 10 },
    //         { UpgradeType.ReduceOverheat, 10 }
    //     };
    // }

    void Update()
    {
        CheckButtons();
    }

    private void CheckButtons()
    {
        //Debug.Log($"{playerController.currentHealth}, {playerController.maxHealth}");
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
        playerController.UpdateShip(UpgradeType.RestoreHP, upgradeCosts[UpgradeType.RestoreHP]);
        upgradeCosts[UpgradeType.RestoreHP] =
            5 * (playerController.maxHealth - playerController.currentHealth);
    }

    public void UpgradeHP()
    {
        playerController.UpdateShip(UpgradeType.UpgradeHP, upgradeCosts[UpgradeType.UpgradeHP]);
        upgradeCosts[UpgradeType.UpgradeHP] = (int)(3 * playerController.maxHealth);
    }

    public void UpgradeSpeed()
    {
        playerController.UpdateShip(
            UpgradeType.UpgradeSpeed,
            upgradeCosts[UpgradeType.UpgradeSpeed]
        );
        upgradeCosts[UpgradeType.UpgradeSpeed] = (int)(200 * playerController.speed);
    }

    public void UpgradeDamage()
    {
        playerController.UpdateShip(
            UpgradeType.UpgradeDamage,
            upgradeCosts[UpgradeType.UpgradeDamage]
        );
    }

    public void ReduceOverheat()
    {
        playerController.UpdateShip(
            UpgradeType.ReduceOverheat,
            upgradeCosts[UpgradeType.ReduceOverheat]
        );
    }
}
