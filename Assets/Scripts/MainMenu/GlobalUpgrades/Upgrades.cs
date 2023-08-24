using System;
using UnityEngine;

namespace TowerDefense
{
    public class Upgrades : SingletoneBase<Upgrades>
    {
        [Serializable]
        private class UpgradeSave
        {
            public UpgradeAsset asset;
            public int level = 0;
        }

        public const string m_UpgradesFilename = "upgrades.dat";

        [SerializeField] private UpgradeSave[] m_Saves;

        public static void BuyUpgrade(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.m_Saves)
            {
                if (upgrade.asset == asset)
                {
                    upgrade.level++;

                    Saver<UpgradeSave[]>.Save(m_UpgradesFilename, Instance.m_Saves);
                }
            }
        }

        public static int GetTotalCost()
        {
            int result = 0;

            foreach (var upgrade in Instance.m_Saves)
            {
                for (int i = 0; i < upgrade.level; i++)
                {
                    result += upgrade.asset.costByLevel[i];
                }
            }

            return result;
        }

        public static int GetUpgradeLevel(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.m_Saves)
            {
                if (upgrade.asset == asset)
                {
                    return upgrade.level;
                }
            }

            return 0;
        }

        private new void Awake()
        {
            base.Awake();

            Saver<UpgradeSave[]>.TryLoad(m_UpgradesFilename, ref m_Saves);
        }
    }
}