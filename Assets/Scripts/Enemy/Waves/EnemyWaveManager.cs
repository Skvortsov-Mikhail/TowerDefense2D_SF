using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class EnemyWaveManager : MonoBehaviour
    {
        public static event Action<Enemy> OnEnemySpawn;

        [SerializeField] private Enemy m_EnemyPrefab;

        [SerializeField] private Path[] m_Paths;

        [SerializeField] private EnemyWave m_CurrentWave;

        [SerializeField] private float m_BonusForEverySecond;
        public float BonusForEverySecond => m_BonusForEverySecond;

        private bool m_IsAllWavesDead;
        public bool IsAllWavesDead => m_IsAllWavesDead;

        private bool m_IsLastWaveGone;
        public bool IsLastWaveGone => m_IsLastWaveGone;

        private int m_EnemiesCount;

        private void Start()
        {
            //m_IsLastWaveGone = false;
            m_CurrentWave.Prepare(SpawnEnemies);
        }

        public void ForceNextWave()
        {
            if (m_CurrentWave != null)
            {
                TDPlayer.Instance.ChangeGold((int)(m_CurrentWave.GetRemainingTime() * m_BonusForEverySecond));

                SpawnEnemies();
            }

            else
            {
                if (m_EnemiesCount == 0)
                {
                    m_IsAllWavesDead = true;
                }
            }
        }

        private void SpawnEnemies()
        {
            foreach ((EnemyAsset asset, int count, int pathIndex) in m_CurrentWave.EnumerateSquads())
            {
                if (pathIndex < m_Paths.Length)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var e = Instantiate(m_EnemyPrefab, m_Paths[pathIndex].StartArea.GetRandomInsideZone(), Quaternion.identity);

                        e.Use(asset);
                        e.GetComponent<TDPatrolController>().SetPath(m_Paths[pathIndex]);
                        e.OnEnd += RecordEnemyDead;

                        m_EnemiesCount++;

                        OnEnemySpawn?.Invoke(e);                        
                    }
                }

                else
                {
                    Debug.LogWarning($"Invalid pathIndex in {name}");
                }
            }

            m_CurrentWave = m_CurrentWave.PrepareNext(SpawnEnemies);
            
            if(m_CurrentWave == null)
            {
                m_IsLastWaveGone = true;
            }
        }

        private void RecordEnemyDead()
        {
            m_EnemiesCount--;

            if (m_EnemiesCount == 0)
            {
                ForceNextWave();
            }
        }
    }
}
