using UnityEngine;

namespace TowerDefense
{
    [CreateAssetMenu]
    public sealed class EnemyAsset : ScriptableObject
    {
        public enum ArmorType
        {
            Archer,
            Mage,
            Trap
        }

        [Header("VisualModel")]

        public Color color = Color.white;

        public Vector2 spriteScale = new Vector2(3, 3);

        public RuntimeAnimatorController animation;

        [Space]
        public float colliderRadius = 0.0f;

        [Header("Parameters")]

        public float moveSpeed = 1;

        public int maxHP = 1;

        public ArmorType armorType;

        public int armor = 1;

        public int scoreValue = 1;

        public int damage = 1;

        public int gold = 1;

        public int mana = 1;
    }
}
