using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TowerDefense
{
    /// <summary>
    /// ������������ ������ �� �����. ��, ��� ����� ����� ���������.
    /// </summary>
    public class Destructible : Entity
    {
        #region Properties

        /// <summary>
        /// ������ ���������� �����������.
        /// </summary>
        [SerializeField] private bool m_Indestructible;

        /// <summary>
        /// ��������� ���������� ����������.
        /// </summary>
        [SerializeField]
        private int m_MaxHitPoints;
        public int MaxHitPoints => m_MaxHitPoints;

        /// <summary>
        /// ������� ���������� ����������.
        /// </summary>
        private int m_CurrentHitPoints;
        public int CurrentHitPoints => m_CurrentHitPoints;

        #endregion

        #region Unity Events

        protected virtual void Start()
        {
            m_CurrentHitPoints = m_MaxHitPoints;
        }

        #endregion

        #region Public API

        /// <summary>
        /// ���������� ������ � �������.
        /// </summary>
        /// <param name="damage"> ����, ��������� �������. </param>
        public void ApplyDamage(int damage)
        {
            if (m_Indestructible == true) return;

            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0)
                OnDeath();
        }

        #endregion

        /// <summary>
        /// ���������������� ������� ����������� �������.
        /// </summary>
        protected virtual void OnDeath()
        {
            m_EventOnDeath?.Invoke(gameObject);
            Destroy(gameObject);
        }

        #region DestructibleCollection

        private static HashSet<Destructible> m_AllDestructibles;

        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)
                m_AllDestructibles = new HashSet<Destructible>();

            m_AllDestructibles.Add(this);
        }

        protected virtual void OnDestroy()
        {
            m_AllDestructibles.Remove(this);
        }

        #endregion

        #region Teams

        public const int TeamIDNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;

        #endregion

        #region Score

        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;

        #endregion

        [SerializeField] private UnityEvent<GameObject> m_EventOnDeath;
        public UnityEvent<GameObject> EventOnDeath => m_EventOnDeath;

        protected void Use(EnemyAsset asset)
        {
            m_MaxHitPoints = asset.maxHP;
            m_ScoreValue = asset.scoreValue;
        }
    }
}
