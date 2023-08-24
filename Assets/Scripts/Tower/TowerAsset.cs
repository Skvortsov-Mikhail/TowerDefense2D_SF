using UnityEngine;

namespace TowerDefense
{
    [CreateAssetMenu]
    public class TowerAsset : ScriptableObject
    {
        public enum TowerType
        {
            Arrows,
            Magic,
            Trap
        }

        public int m_GoldCost = 15;

        public Sprite GUISprite;

        public Sprite sprite;

        public TowerType towerType;

        public TurretProperties turretProps;

        public float towerRadius;

        public float lead;

        public UpgradeAsset speedUpgradeAsset;

        public UpgradeAsset damageUpgradeAsset;
    }
}