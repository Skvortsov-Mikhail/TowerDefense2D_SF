using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class ProjectileTrap : Projectile
    {
        private TrapPoint[] m_TrapPoints;

        [SerializeField] private float m_RefireTimer;
        [SerializeField] private float m_AttackRadius;

        private float refireTimer;

        protected override void Start()
        {
            base.Start();

            m_TrapPoints = FindObjectsOfType<TrapPoint>();

            refireTimer = 0;

            transform.position = ChooseRandomPoint();
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }

        protected override void FixedUpdate()
        {
            m_Timer += Time.fixedDeltaTime;

            if (refireTimer > 0)
            {
                refireTimer -= Time.fixedDeltaTime;
            }

            if (m_Timer > m_Lifetime)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            Enemy enemy = collision.transform.root.GetComponent<Enemy>();

            if (enemy != null && refireTimer <= 0)
            {
                enemy.TakeDamage(m_Damage, m_DamageType);

                refireTimer = m_RefireTimer;
            }
        }

        private Vector3 ChooseRandomPoint()
        {
            List<TrapPoint> newTrapPoints = new List<TrapPoint>();

            for (int i = 0; i < m_TrapPoints.Length; i++)
            {
                if (Vector2.Distance(m_TrapPoints[i].transform.position, m_Parent.transform.position) <= m_AttackRadius)
                {
                    newTrapPoints.Add(m_TrapPoints[i]);
                }
            }

            Vector3 pos = newTrapPoints[Random.Range(0, newTrapPoints.Count)].transform.position;

            return pos;
        }
    }
}
