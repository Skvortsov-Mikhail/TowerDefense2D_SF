using UnityEngine;

namespace TowerDefense
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private WizardInMagicTower m_WarriorPrefab;

        public float m_Radius;

        private Turret[] m_Turrets;

        public TowerAsset.TowerType m_TowerType;

        public UpgradeAsset m_SpeedUpgrade;
        public UpgradeAsset m_DamageUpgrade;

        private Destructible m_Target = null;
        public Destructible Target => m_Target;

        private float m_Lead;

        private void Update()
        {
            if (m_Target != null)
            {
                Vector2 targetVector = m_Target.transform.position - transform.position;

                if (targetVector.magnitude <= m_Radius)
                {
                    foreach (var turret in m_Turrets)
                    {
                        turret.transform.up = m_Target.transform.position - turret.transform.position + (Vector3)m_Target.GetComponent<Rigidbody2D>().velocity * m_Lead;

                        var projectile = turret.Fire();
                    }
                }

                else
                {
                    m_Target = null;
                }
            }

            else
            {
                var enter = Physics2D.OverlapCircle(transform.position, m_Radius);

                if (enter != null)
                {
                    m_Target = enter.transform.root.GetComponent<Destructible>();
                }
            }
        }

        public void Use(TowerAsset towerAsset)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = towerAsset.sprite;

            m_Turrets = GetComponentsInChildren<Turret>();

            foreach (var turret in m_Turrets)
            {
                turret.AssignLoadout(towerAsset.turretProps);
                turret.IncreaseFireRateByUpgrade(m_SpeedUpgrade);
            }

            m_Radius = towerAsset.towerRadius;
            m_Lead = towerAsset.lead;
            m_TowerType = towerAsset.towerType;
            m_SpeedUpgrade = towerAsset.speedUpgradeAsset;
            m_DamageUpgrade = towerAsset.damageUpgradeAsset;

            if (m_TowerType == TowerAsset.TowerType.Magic)
            {
                var wizard = Instantiate(m_WarriorPrefab, transform.position, Quaternion.identity);
                wizard.SetParentTower(this);
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, m_Radius);
        }
#endif
    }
}
