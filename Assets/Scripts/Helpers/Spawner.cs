using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TowerDefense
{
    public abstract class Spawner : MonoBehaviour, ISpawnerCondition
    {
        public enum SpawnMode
        {
            Start,
            Loop
        }

        public enum TimerMode
        {
            Constant,
            Temporary
        }

        protected abstract GameObject GenerateSpawnedEntity();

        [SerializeField] protected string m_SpawnerName;

        [SerializeField] protected CircleArea m_Area;

        [SerializeField] private SpawnMode m_SpawnMode;

        [SerializeField] protected int m_NumSpawns;

        [SerializeField] private float m_RespawnTime;

        [SerializeField] private float m_StartDelayTime;

        [SerializeField] private TimerMode m_TimerMode;

        [SerializeField] private float m_RunningTime;

        private float m_Timer;

        private bool m_IsFinishSpawn = false;

        bool ISpawnerCondition.isCompleted
        {
            get
            {
                return m_IsFinishSpawn;
            }
        }


        private void Start()
        {
            if (m_SpawnMode == SpawnMode.Start && m_StartDelayTime == 0)
            {
                SpawnEntities();
            }

            m_Timer = m_RespawnTime;
        }

        private void Update()
        {
            if(m_IsFinishSpawn == false)
            {
                if (m_RunningTime > 0)
                    m_RunningTime -= Time.deltaTime;

                if (m_TimerMode == TimerMode.Temporary && m_RunningTime <= 0)
                {
                    m_IsFinishSpawn = true;
                    enabled = false;
                }

                if (m_StartDelayTime > 0)
                {
                    m_StartDelayTime -= Time.deltaTime;
                }

                if (m_StartDelayTime <= 0)
                {
                    if (m_Timer > 0)
                        m_Timer -= Time.deltaTime;

                    if (m_SpawnMode == SpawnMode.Loop && m_Timer < 0)
                    {
                        SpawnEntities();

                        m_Timer = m_RespawnTime;
                    }

                    if (m_SpawnMode == SpawnMode.Start && m_StartDelayTime <= 0)
                    {
                        SpawnEntities();
                        m_IsFinishSpawn = true;
                        enabled = false;
                    }
                }
            }
        }

        protected virtual void SpawnEntities()
        {
            for (int i = 0; i < m_NumSpawns; i++)
            {
                var e = GenerateSpawnedEntity();

                e.transform.position = m_Area.GetRandomInsideZone();
            }
        }
    }
}
