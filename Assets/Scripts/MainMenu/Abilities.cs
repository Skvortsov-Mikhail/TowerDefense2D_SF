using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class Abilities : SingletoneBase<Abilities>
    {
        [Serializable]
        public class FireAbility
        {
            [SerializeField] private UpgradeAsset m_FireAbilityUpgrade;
            public UpgradeAsset FireAbilityUpgrade => m_FireAbilityUpgrade;

            [SerializeField] private int m_Damage;

            [SerializeField] private float m_Radius;

            [SerializeField] private float m_FireCooldown;

            private bool isFireCooldown = false;
            public bool IsFireCooldown => isFireCooldown;
            // public bool IsFireCooldown => !Instance.m_FireButton.interactable;

            [SerializeField] private int m_FireCost;
            public int FireCost => m_FireCost;

            public void Use()
            {
                var target = Instantiate(Instance.m_FireTargetPrefab);
                target.transform.localScale = new Vector2(m_Radius * 2, m_Radius * 2);
                Cursor.visible = false;

                ClickProtection.Instance.Activate((Vector2 v) =>
                {
                    Vector3 position = v;
                    position.z = -Camera.main.transform.position.z;
                    position = Camera.main.ScreenToWorldPoint(position);

                    foreach (var collider in Physics2D.OverlapCircleAll(position, m_Radius))
                    {
                        if (collider.transform.parent.TryGetComponent<Enemy>(out var enemy))
                        {
                            enemy.TakeDamage((int)(m_Damage * Upgrades.GetUpgradeLevel(m_FireAbilityUpgrade) * m_FireAbilityUpgrade.bonusPerLevel), Projectile.DamageType.Default);
                        }
                    }

                    Destroy(target);
                    Cursor.visible = true;

                    Instance.StartCoroutine(DisableTimeAbility());
                });

                IEnumerator DisableTimeAbility()
                {
                    Instance.m_FireButton.interactable = false;
                    isFireCooldown = true;

                    yield return new WaitForSeconds(m_FireCooldown);

                    Instance.m_FireButton.interactable = true;
                    isFireCooldown = false;
                }
            }
        }

        [Serializable]
        public class FreezeAbility
        {
            [SerializeField] private UpgradeAsset m_FreezeAbilityUpgrade;
            public UpgradeAsset FreezeAbilityUpgrade => m_FreezeAbilityUpgrade;

            [SerializeField] private float m_Duration;

            [Range(0f, 0.2f)]
            [SerializeField] private float m_StartDecelerationFactor;

            [SerializeField] private float m_FreezeCooldown;

            private bool isFreezeCooldown = false;
            public bool IsFreezeCooldown => isFreezeCooldown;

            [SerializeField] private int m_FreezeCost;
            public int FreezeCost => m_FreezeCost;

            public void Use()
            {
                void Slow(Enemy enemy)
                {
                    enemy.GetComponent<SpaceShip>().DecreaseMaxLinearVelocity(m_StartDecelerationFactor * Upgrades.GetUpgradeLevel(m_FreezeAbilityUpgrade));
                }

                foreach (var ship in FindObjectsOfType<SpaceShip>())
                {
                    ship.DecreaseMaxLinearVelocity(m_StartDecelerationFactor * Upgrades.GetUpgradeLevel(m_FreezeAbilityUpgrade));
                }

                EnemyWaveManager.OnEnemySpawn += Slow;

                IEnumerator Restore()
                {
                    yield return new WaitForSeconds(m_Duration);

                    foreach (var ship in FindObjectsOfType<SpaceShip>())
                    {
                        ship.RestoreMaxLinearVelocity();
                    }

                    EnemyWaveManager.OnEnemySpawn -= Slow;
                }

                Instance.StartCoroutine(Restore());

                IEnumerator DisableTimeAbility()
                {
                    Instance.m_FreezeButton.interactable = false;
                    isFreezeCooldown = true;

                    yield return new WaitForSeconds(m_FreezeCooldown);

                    Instance.m_FreezeButton.interactable = true;
                    isFreezeCooldown = false;
                }

                Instance.StartCoroutine(DisableTimeAbility());
            }
        }

        [Header("Fire Ability")]
        [SerializeField] private FireAbility m_FireAbility;
        [SerializeField] private GameObject m_FireTargetPrefab;
        [SerializeField] private Button m_FireButton;
        [SerializeField] private Text m_FireCostText;

        private int fireUpgradeLevel;

        [Header("Freeze Ability")]
        [SerializeField] private FreezeAbility m_FreezeAbility;
        [SerializeField] private Button m_FreezeButton;
        [SerializeField] private Text m_FreezeCostText;

        private int freezeUpgradeLevel;

        private void Start()
        {
            m_FireCostText.text = m_FireAbility.FireCost.ToString();
            fireUpgradeLevel = Upgrades.GetUpgradeLevel(m_FireAbility.FireAbilityUpgrade);

            m_FreezeCostText.text = m_FreezeAbility.FreezeCost.ToString();
            freezeUpgradeLevel = Upgrades.GetUpgradeLevel(m_FreezeAbility.FreezeAbilityUpgrade);
        }

        private void Update()
        {
            m_FireButton.interactable = fireUpgradeLevel >= 1 && TDPlayer.Instance.ManaAmount >= m_FireAbility.FireCost && !m_FireAbility.IsFireCooldown;

            m_FreezeButton.interactable = freezeUpgradeLevel >= 1 && TDPlayer.Instance.ManaAmount >= m_FreezeAbility.FreezeCost && !m_FreezeAbility.IsFreezeCooldown;
        }

        public void UseFireAbility()
        {
            m_FireAbility.Use();
            TDPlayer.Instance.ChangeMana(-m_FireAbility.FireCost);
        }

        public void UseFreezeAbility()
        {
            m_FreezeAbility.Use();
            TDPlayer.Instance.ChangeMana(-m_FreezeAbility.FreezeCost);
        }
    }
}