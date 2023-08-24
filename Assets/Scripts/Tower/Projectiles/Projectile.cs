using UnityEngine;

namespace TowerDefense
{
    public class Projectile : Entity
    {
        public enum DamageType
        {
            Archer,
            Mage,
            Trap,
            Default
        }

        [SerializeField] protected float m_Velocity;

        [SerializeField] protected float m_Lifetime;

        [SerializeField] protected int m_Damage;

        [SerializeField] protected DamageType m_DamageType;

        [SerializeField] protected ImpactEffect m_ImpactEffectPrefab;

        [Header("Sounds")]
        [SerializeField] private Sound m_ShotSound;

        [SerializeField] private Sound m_HitSound;

        protected Tower m_Parent;

        protected float m_Timer;

        protected virtual void Start()
        {
            m_ShotSound.Play();

            var upgrade = m_Parent.m_DamageUpgrade;

            if (upgrade != null)
            {
                m_Damage += Upgrades.GetUpgradeLevel(upgrade) * (int)upgrade.bonusPerLevel;
            }
        }

        protected virtual void FixedUpdate()
        {
            float stepLength = Time.fixedDeltaTime * m_Velocity;
            Vector2 step = transform.up * stepLength;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength);

            if (hit)
            {
                Enemy enemy = hit.collider.transform.root.GetComponent<Enemy>();

                if(enemy != null)
                {
                    enemy.TakeDamage(m_Damage, m_DamageType);

                    // Player.Instance.AddScore(dest.ScoreValue);

                    /*
                    if (dest.GetComponent<SpaceShip>() != null && (dest.CurrentHitPoints - m_Damage) <= 0 && m_IsPlayerProjectile == true)
                    {
                        Player.Instance.AddKill();
                    }

                    if (m_IsPlayerProjectile == true)
                    {
                        Player.Instance.AddScore(dest.ScoreValue);
                    }
                    */
                }

                OnProjectileLifeEnd(hit.collider, hit.point);
            }

            m_Timer += Time.fixedDeltaTime;

            if(m_Timer > m_Lifetime)
            {
                Destroy(gameObject);
            }

            transform.position += new Vector3(step.x, step.y, 0);
        }

        protected virtual void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
        {
            if(col.isTrigger == false)
            {
                if (m_ImpactEffectPrefab != null)
                {
                    var destroyEffect = Instantiate(m_ImpactEffectPrefab);
                    destroyEffect.transform.position = pos;
                }

                m_HitSound.Play();

                Destroy(gameObject);
            }
        }

        public void SetParentTower(Tower parent)
        {
            m_Parent = parent;
        }
    }
}