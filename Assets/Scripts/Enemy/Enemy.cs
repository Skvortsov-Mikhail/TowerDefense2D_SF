using UnityEngine;
using UnityEngine.UI;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TowerDefense
{
    [RequireComponent(typeof(Destructible))]
    [RequireComponent(typeof(TDPatrolController))]
    public class Enemy : MonoBehaviour
    {
        private static Func<int, Projectile.DamageType, int, int>[] ArmorDamageFunctions =
        {
            (int power, Projectile.DamageType damageType, int armor) =>
            { // Enemy with Archer Armor
                switch(damageType)
                {
                    case Projectile.DamageType.Archer:
                        return Mathf.Max(power - armor, 1);
                    case Projectile.DamageType.Mage:
                        return Mathf.Max(power - (armor / 2), 1);
                    case Projectile.DamageType.Trap:
                        return power;
                    case Projectile.DamageType.Default:
                        return power;

                    default:
                    {
                        Debug.LogWarning("Invalid Projectile.DamageType");
                        return 0;
                    }
                }
            },
            (int power, Projectile.DamageType damageType, int armor) =>
            { // Enemy with Mage Armor
                switch(damageType)
                {
                    case Projectile.DamageType.Archer:
                        return power;
                    case Projectile.DamageType.Mage:
                        return Mathf.Max(power - armor, 1);
                    case Projectile.DamageType.Trap:
                        return Mathf.Max(power - (armor / 2), 1);
                    case Projectile.DamageType.Default:
                        return power;

                    default:
                    {
                        Debug.LogWarning("Invalid Projectile.DamageType");
                        return 0;
                    }
                }
            },
            (int power, Projectile.DamageType damageType, int armor) =>
            { // Enemy with Trap Armor
                switch(damageType)
                {
                    case Projectile.DamageType.Archer:
                        return Mathf.Max(power - (armor / 2), 1);
                    case Projectile.DamageType.Mage:
                        return power;
                    case Projectile.DamageType.Trap:
                        return Mathf.Max(power - armor, 1);
                    case Projectile.DamageType.Default:
                        return power;
                    default:
                    {
                        Debug.LogWarning("Invalid Projectile.DamageType");
                        return 0;
                    }
                }
            },
        };

        [SerializeField] private int m_Damage = 1;

        [SerializeField] private int m_Armor = 1;

        [SerializeField] private int m_Gold = 1;

        [SerializeField] private int m_Mana = 1;

        [SerializeField] private GameObject m_HealthBar;

        private Image m_Fill;

        private Destructible m_Destructible;

        private EnemyAsset.ArmorType m_ArmorType;

        public event Action OnEnd;

        private void OnDestroy()
        {
            OnEnd?.Invoke();

            OnEnd = null;
        }

        private void Start()
        {
            m_Destructible = GetComponent<Destructible>();
            m_HealthBar.GetComponentInChildren<Canvas>().worldCamera = FindObjectOfType<Camera>();
            m_Fill = m_HealthBar.transform.Find("Canvas").transform.Find("Fill").GetComponent<Image>();
        }

        private void Update()
        {
            m_HealthBar.transform.up = Vector2.up;
            m_Fill.fillAmount = (float)m_Destructible.CurrentHitPoints / (float)m_Destructible.MaxHitPoints;
        }

        public void Use(EnemyAsset asset)
        {
            var sr = GetComponentInChildren<SpriteRenderer>();
            sr.color = asset.color;
            sr.transform.localScale = new Vector3(asset.spriteScale.x, asset.spriteScale.y, 1);
            m_HealthBar.transform.localScale = new Vector3(3, 4, 1);

            var anim = GetComponentInChildren<Animator>();
            anim.runtimeAnimatorController = asset.animation;

            GetComponent<SpaceShip>().Use(asset);

            var col = GetComponentInChildren<CircleCollider2D>();
            col.radius = asset.colliderRadius;

            m_Damage = asset.damage;
            m_Armor = asset.armor;
            m_ArmorType = asset.armorType;
            m_Gold = asset.gold;
            m_Mana = asset.mana;
        }

        public void DamagePlayer()
        {
            TDPlayer.Instance.ReduceLife(m_Damage);
        }

        public void GivePlayerGoldAndMana()
        {
            TDPlayer.Instance.ChangeGold(m_Gold);
            TDPlayer.Instance.ChangeMana(m_Mana);
        }

        public void TakeDamage(int damage, Projectile.DamageType damageType)
        {
            m_Destructible.ApplyDamage(ArmorDamageFunctions[(int)m_ArmorType](damage, damageType, m_Armor));
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(Enemy))]
    public class EnemyInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EnemyAsset a = EditorGUILayout.ObjectField(null, typeof(EnemyAsset), false) as EnemyAsset;

            if (a != null)
            {
                (target as Enemy).Use(a);
            }
        }
    }

#endif
}