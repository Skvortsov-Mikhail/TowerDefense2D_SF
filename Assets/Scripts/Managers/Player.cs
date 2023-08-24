using UnityEngine;
using System;

namespace TowerDefense
{
    public class Player : SingletoneBase<Player>
    {
        [SerializeField] private int m_NumLives;
        public int NumLives => m_NumLives;

        public event Action OnPlayerDeath;

        protected override void Awake()
        {
            base.Awake();
        }

        protected void TakeDamage(int damage)
        {
            m_NumLives -= damage;

            if (m_NumLives <= 0)
            {
                m_NumLives = 0;
                OnPlayerDeath?.Invoke();

                // LevelController.Instance.EndLevel();
                // LevelSequenceController.Instance.FinishCurrentLevel(false);
            }
        }

        protected void AddLivesByUpgrades(UpgradeAsset asset)
        {
            m_NumLives += (int)(asset.bonusPerLevel * Upgrades.GetUpgradeLevel(asset));
        }

        #region Score

        public int Score { get; private set; }

        public int NumKills { get; private set; }

        public void AddKill()
        {
            NumKills++;
        }

        public void AddScore(int num)
        {
            Score += num;
        }

        #endregion
    }
}
