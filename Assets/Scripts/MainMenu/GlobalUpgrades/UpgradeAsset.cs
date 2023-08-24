using UnityEngine;

namespace TowerDefense
{
    [CreateAssetMenu]
    public sealed class UpgradeAsset : ScriptableObject
    {
        public Sprite sprite;

        public int[] costByLevel = { 3 };

        public float bonusPerLevel;
    }
}
