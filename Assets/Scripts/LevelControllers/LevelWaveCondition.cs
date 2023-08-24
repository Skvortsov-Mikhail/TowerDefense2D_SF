using UnityEngine;

namespace TowerDefense
{
    public class LevelWaveCondition : MonoBehaviour, ILevelCondition
    {
        public bool isCompleted => m_EnemyWaveManager.IsAllWavesDead;

        [SerializeField] private EnemyWaveManager m_EnemyWaveManager;
    }
}