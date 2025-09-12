using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private RegBullet regBullet;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private BulletCollisonHandler bulletHandler;

    public static int BulletPenetration = 0;
    public static Vector3 BulletSize = new Vector3(0.15f, 0.25f, 0.35f);

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this; // no DontDestroyOnLoad
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
            upgrades.Add(() => playerHealth.UpgradeMaxHealth(25));
            upgrades.Add(() => regBullet.UpgradeBulletForce(1.3f));
            upgrades.Add(() => playerMovement.UpgradeSlide(1.5f));
        }
        else if (tier == 2) {
            upgrades.Add(() => UpgradeManager.BulletPenetration++);
            upgrades.Add(() => UpgradeManager.BulletSize += new Vector3(0.1f, 0.1f, 0.1f));
            //upgrades.Add(() =>
        }
        //else if (tier == 3) {
        //    upgrades.Add(() => 
        //    upgrades.Add(() => 
        //    upgrades.Add(() => 
        //}

        if (upgrades.Count > 0) {
            int i = UnityEngine.Random.Range(0, upgrades.Count);
            upgrades[i].Invoke();
        }
    }
}
