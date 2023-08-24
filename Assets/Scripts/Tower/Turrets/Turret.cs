using UnityEngine;

namespace TowerDefense
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        private TurretProperties m_TurretProperties;

        private float m_DefaultRefireTime;

        private float m_RefireTimer;

        public bool CanFire => m_RefireTimer <= 0;

        private SpaceShip m_Ship;

        private Tower m_Tower;

        #region Unity Events

        private void Awake()
        {
            //m_Ship = transform.root.GetComponent<SpaceShip>();

            m_Tower = transform.root.GetComponent<Tower>();
        }

        private void Update()
        {
            if (m_RefireTimer > 0)
            {
                m_RefireTimer -= Time.deltaTime;
            }

            else if (m_Mode == TurretMode.Auto)
            {
                Fire();
            }
        }

        #endregion

        #region Public API

        public Projectile Fire()
        {
            if (m_TurretProperties == null) return null;

            if (m_RefireTimer > 0) return null;

            if (m_Ship != null)
            {
                if (m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false)
                    return null;

                if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false)
                    return null;
            }

            Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;

            projectile.SetParentTower(m_Tower);

            m_RefireTimer = m_DefaultRefireTime;

            // SFX if need?
            return projectile;
        }

        #endregion

        public void AssignLoadout(TurretProperties props)
        {
            m_RefireTimer = 0;
            m_TurretProperties = props;

            m_DefaultRefireTime = m_TurretProperties.RateOfFire;
        }

        public void IncreaseFireRateByUpgrade(UpgradeAsset asset)
        {
            if (asset == null) return;

            m_DefaultRefireTime *= Mathf.Pow(1 - asset.bonusPerLevel, Upgrades.GetUpgradeLevel(asset));

            if (m_DefaultRefireTime < 0.1f)
            {
                m_DefaultRefireTime = 0.1f;
            }
        }
    }
}
