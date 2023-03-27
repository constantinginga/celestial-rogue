using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour
{
    public Button VentureForthButton;
    GameManager GameManager;
    PlayerController playerController;

    void Awake()
    {
        GameManager = GameObject.FindFirstObjectByType<GameManager>();
        playerController = GameObject.FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        // check if player has enough credits to buy upgrades
        // check if player has enough credits to restore hp
        // check if player has enough credits to upgrade hp
        // check if player has enough credits to upgrade speed
        // check if player has enough credits to upgrade damage
        // check if player has enough credits to reduce overheat
    }

    public void VentureForth()
    {
        GameManager.StartLevel();
    }

    public void RestoreHP()
    {
        playerController.RestoreHP();
    }

    public void UpgradeHP()
    {
        playerController.UpgradeHP();
        // upgrade ship hp based on cost, and subtract cost from player's credits
    }

    public void UpgradeSpeed()
    {
        playerController.UpgradeSpeed();
        // if speed updgraded, notify player, otherwise notify player that they don't have enough credits
    }

    public void UpgradeDamage()
    {
        playerController.UpgradeDamage();
    }

    public void ReduceOverheat()
    {
        playerController.ReduceOverheat();
    }
}
