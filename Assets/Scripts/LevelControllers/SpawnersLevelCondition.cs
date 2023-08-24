using UnityEngine;

namespace TowerDefense
{
    public interface ISpawnerCondition
    {
        bool isCompleted { get; }
    }

    public class SpawnersLevelCondition : MonoBehaviour, ILevelCondition
    {
        [SerializeField] private Spawner m_Spawner;

        private ISpawnerCondition[] m_SpawnerConditions;

        private bool m_IsSpawnersFinished;
        private bool m_IsSpawnersAndEnemiesFinished; // TODO need comments

        public bool isCompleted => m_IsSpawnersAndEnemiesFinished; // TODO need comments

        // public bool isCompleted => m_IsSpawnersFinished;  // TODO delete comment

        private void Start()
        {
            m_SpawnerConditions = m_Spawner.GetComponentsInChildren<ISpawnerCondition>();
        }

        private void Update()
        {
            if (m_IsSpawnersFinished == false)
            {
                CheckSpawnersConditions();
            }

            // TODO need comments to all "else" block
            else
            {
                if(FindObjectsOfType<Enemy>().Length == 0)
                {
                    m_IsSpawnersAndEnemiesFinished = true;
                }
            }
        }

        private void CheckSpawnersConditions()
        {
            if (m_SpawnerConditions == null || m_SpawnerConditions.Length == 0) return;

            int numCompleted = 0;

            foreach (var v in m_SpawnerConditions)
            {
                if (v.isCompleted == true)
                {
                    numCompleted++;
                }

                if (numCompleted == m_SpawnerConditions.Length)
                {
                    m_IsSpawnersFinished = true;
                    print("All spawners completed");
                }
            }
        }
    }
}
