using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [SerializeField] private TextMeshProUGUI upgradeText;

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private RegBullet regBullet;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private BulletCollisonHandler bulletHandler;
    [SerializeField] private EnemyDrops enemyDrops;

    public static int BulletPenetration = 0;
    public static Vector3 BulletSize = new Vector3(0.1f, 0.15f, 0.22f);

    private bool hasCritXPUpgrade = false;
    private bool hasHealthPackDropUpgrade = false;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this; // no DontDestroyOnLoad

        hasCritXPUpgrade = false;
        hasHealthPackDropUpgrade = false;

        BulletSize = new Vector3(0.1f, 0.15f, 0.22f);
        BulletPenetration = 0;
    }

    public void GrantUpgrade(int level, GameObject player) {
        int tier = 1;

        if (level >= 5 && level < 10) {
            tier = 2;
        }
        else if (level >= 10) {
            tier = 3;
        }

        List<Action> upgrades = new List<Action>();

        // Tier 1 Upgrades
        if (tier == 1) {
            upgrades.Add(() => {
                playerHealth.UpgradeMaxHealth(25);
                upgradeText.text = "UPGRADE: +25 MAX HEALTH";
                upgradeText.gameObject.SetActive(true);
            });

            if (!hasCritXPUpgrade) {
                upgrades.Add(() => {
                    bulletHandler.EnableCritXPUpgrade();
                    hasCritXPUpgrade = true;
                    upgradeText.text = "UPGRADE: +10% CHANCE OF CRIT";
                    upgradeText.gameObject.SetActive(true);
                });
            }

            upgrades.Add(() => {
                playerMovement.UpgradeSlide(1.5f);
                upgradeText.text = "UPGRADE: +50% DASH SPEED/DISTANCE";
                upgradeText.gameObject.SetActive(true);
            });
            
        }
        else if (tier == 2) {
            if (!hasCritXPUpgrade) {
                upgrades.Add(() => {
                    bulletHandler.EnableCritXPUpgrade();
                    hasCritXPUpgrade = true;
                    upgradeText.text = "UPGRADE: +10% CHANCE OF CRIT";
                    upgradeText.gameObject.SetActive(true);
                });
            }

            upgrades.Add(() => {
                regBullet.UpgradeBulletForce(1.3f);
                upgradeText.text = "UPGRADE: +30% BULLET SPEED";
                upgradeText.gameObject.SetActive(true);
            });
            
            
            upgrades.Add(() => {
                UpgradeManager.BulletSize += new Vector3(0.1f, 0.1f, 0.1f);
                upgradeText.text = "UPGRADE: +100% BULLET SIZE";
                upgradeText.gameObject.SetActive(true);
            });

            if (!hasHealthPackDropUpgrade) {
                upgrades.Add(() => {
                    enemyDrops.EnableHealthPackDrop();
                    hasHealthPackDropUpgrade = true;
                    upgradeText.text = "UPGRADE: +20% CHANCE TO DROP HEALTH PACKS";
                    upgradeText.gameObject.SetActive(true);
                });
            }
        }
        else if (tier == 3) {
            upgrades.Add(() => {
                UpgradeManager.BulletPenetration++;
                upgradeText.text = "UPGRADE: +1 ENEMY PIERCE";
                upgradeText.gameObject.SetActive(true);
            });
            upgrades.Add(() => {
                regBullet.UpgradeBulletForce(1.5f);
                upgradeText.text = "UPGRADE: +50% BULLET SPEED";
                upgradeText.gameObject.SetActive(true);
            });
            upgrades.Add(() => { 
                UpgradeManager.BulletSize += new Vector3(0.2f, 0.2f, 0.2f); 
                upgradeText.text = "UPGRADE: +200% BULLET SIZE";
                upgradeText.gameObject.SetActive(true);
            });
            if (!hasHealthPackDropUpgrade) {
                upgrades.Add(() => {
                    enemyDrops.EnableHealthPackDrop();
                    hasHealthPackDropUpgrade = true;
                    upgradeText.text = "UPGRADE: +20% CHANCE TO DROP HEALTH PACKS";
                    upgradeText.gameObject.SetActive(true);
                });
            }
            if (!hasCritXPUpgrade) {
                upgrades.Add(() => {
                    bulletHandler.EnableCritXPUpgrade();
                    hasCritXPUpgrade = true;
                    upgradeText.text = "UPGRADE: +10% CHANCE OF CRIT";
                    upgradeText.gameObject.SetActive(true);
                });
            }
        }

        if (upgrades.Count > 0) {
            int i = UnityEngine.Random.Range(0, upgrades.Count);
            upgrades[i].Invoke();
        }
    }
}
