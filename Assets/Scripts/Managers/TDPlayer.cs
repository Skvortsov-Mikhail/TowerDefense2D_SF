using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TowerDefense
{
    public class TDPlayer : Player
    {
        #region Instance for TDPlayer

        public static new TDPlayer Instance
        {
            get
            {
                return Player.Instance as TDPlayer;
            }
        }

        #endregion

        #region Static Events (Gold + Mana + Life)

        private static event Action<int> OnGoldUpdate;
        public static void GoldUpdateSubscribe(Action<int> act)
        {
            OnGoldUpdate += act;
            act(Instance.m_Gold);
        }

        public static void GoldUpdateUnSubscribe(Action<int> act)
        {
            OnGoldUpdate -= act;
        }

        private static event Action<int> OnManaUpdate;
        public static void ManaUpdateSubscribe(Action<int> act)
        {
            OnManaUpdate += act;
            act(Instance.m_Mana);
        }

        public static void ManaUpdateUnSubscribe(Action<int> act)
        {
            OnManaUpdate -= act;
        }

        public static event Action<int> OnLifeUpdate;
        public static void LifeUpdateSubscribe(Action<int> act)
        {
            OnLifeUpdate += act;
            act(Instance.NumLives);
        }

        public static void LifeUpdateUnSubscribe(Action<int> act)
        {
            OnLifeUpdate -= act;
        }

        #endregion

        [SerializeField] private int m_Gold = 0;
        public int GoldAmount => m_Gold;

        [SerializeField] private int m_Mana = 0;
        public int ManaAmount => m_Mana;

        [SerializeField] private Tower m_TowerPrefab;

        [SerializeField] private UpgradeAsset m_HealthUpgrade;

        private new void Awake()
        {
            base.Awake();

            AddLivesByUpgrades(m_HealthUpgrade);
        }

        public void ChangeGold(int change)
        {
            m_Gold += change;
            OnGoldUpdate(m_Gold);
        }
        
        public void ChangeMana(int change)
        {
            m_Mana += change;
            OnManaUpdate(m_Mana);
        }

        public void ReduceLife(int change)
        {
            TakeDamage(change);
            OnLifeUpdate(NumLives);
        }

        public void TryBuild(TowerAsset towerAsset, Transform buildSite)
        {
            if (m_Gold >= towerAsset.m_GoldCost)
            {
                ChangeGold(-towerAsset.m_GoldCost);

                var tower = Instantiate(m_TowerPrefab, buildSite.position, Quaternion.identity);

                tower.Use(towerAsset);

                Destroy(buildSite.gameObject);
            }
        }
    }
}
