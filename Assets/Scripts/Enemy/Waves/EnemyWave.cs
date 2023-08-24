using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class EnemyWave : MonoBehaviour
    {
        public static Action<float> OnWavePrepare;

        [Serializable]
        private class Squad
        {
            public EnemyAsset asset;
            public int count;
        }

        [Serializable]
        private class PathGroup
        {
            public Squad[] squads;
        }

        [SerializeField] private PathGroup[] groups;

        [SerializeField] private float m_PrepareTime = 10.0f;
        public float GetRemainingTime() => m_PrepareTime - Time.time;

        private void Awake()
        {
            enabled = false;
        }

        private event Action OnWaveReady;

        private void Update()
        {
            if (Time.time >= m_PrepareTime)
            {
                OnWaveReady?.Invoke();

                enabled = false;
            }
        }

        public void Prepare(Action spawnEnemies)
        {
            enabled = true;

            m_PrepareTime += Time.time;

            OnWavePrepare?.Invoke(GetRemainingTime());

            OnWaveReady += spawnEnemies;
        }

        public IEnumerable<(EnemyAsset asset, int count, int pathIndex)> EnumerateSquads()
        {
            for (int i = 0; i < groups.Length; i++)
            {
                foreach(var squad in groups[i].squads)
                {
                    yield return (squad.asset, squad.count, i);
                }
            }            
        }

        [SerializeField] private EnemyWave m_NextWave;

        public EnemyWave PrepareNext(Action spawnEnemies)
        {
            OnWaveReady -= spawnEnemies;

            if(m_NextWave != null)
            {
                m_NextWave.Prepare(spawnEnemies);
            }

            return m_NextWave;      
        }
    }
}