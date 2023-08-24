using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TowerDefense
{
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            Patrol
        }

        [SerializeField] private AIBehaviour m_AIBehaviour;
        [SerializeField] private AIPointPatrol m_PatrolPoint;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;

        [SerializeField] private float m_RandomSelectMovePointTime;

        [SerializeField] private float m_FindNewTargetTime;

        [SerializeField] private float m_ShootDelay;

        [SerializeField] private float m_EvadeRayLength;

        private SpaceShip m_Ship;

        private Vector3 m_MovePosition;

        private Destructible m_SelectedTarget;

        private Timer m_RandomizeDirectionTimer;
        private Timer m_FireTimer;
        private Timer m_FindNewTargetTimer;

        private void Start()
        {
            m_Ship = GetComponent<SpaceShip>();

            InitTimers();
        }

        private void Update()
        {
            UpdateTimers();

            UpdateAI();
        }

        private void UpdateAI()
        {
            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                UpdateBehaviourPatrol();
            }
        }

        private void UpdateBehaviourPatrol()
        {
            ActionFindNewMovePosition();
            ActionControlShip();
            ActionFindNewAttackTarget();
            ActionFire();
            ActionEvadeCollision();
        }

        private void ActionFindNewMovePosition()
        {
            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                if (m_SelectedTarget != null)
                {
                    m_MovePosition = m_SelectedTarget.transform.position;
                }

                else
                {
                    if (m_PatrolPoint != null)
                    {
                        bool isInsidePatrolZone = (m_PatrolPoint.transform.position - transform.position).sqrMagnitude < m_PatrolPoint.Radius * m_PatrolPoint.Radius;

                        if (isInsidePatrolZone == true)
                        {
                            GetNewPoint();
                        }

                        else
                        {
                            m_MovePosition = m_PatrolPoint.transform.position;
                        }
                    }
                }
            }
        }

        protected virtual void GetNewPoint()
        {
            if (m_RandomizeDirectionTimer.IsFinished == true)
            {
                Vector2 newPoint = UnityEngine.Random.onUnitSphere * m_PatrolPoint.Radius + m_PatrolPoint.transform.position;

                m_MovePosition = newPoint;

                m_RandomizeDirectionTimer.RestartTimer();
            }
        }

        private void ActionEvadeCollision()
        {
            if (Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength) == true)
            {
                m_MovePosition = transform.position + transform.right * 1000.0f;
            }
        }

        private void ActionControlShip()
        {
            m_Ship.ThrustControl = m_NavigationLinear;

            m_Ship.TorqueControl = ComputeAliginTorqueNormalized(m_MovePosition, m_Ship.transform) * m_NavigationAngular;
        }

        private const float MAX_ANGLE = 45.0f;

        private static float ComputeAliginTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);

            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);

            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;

            return -angle;
        }

        private void ActionFindNewAttackTarget()
        {
            if (m_FindNewTargetTimer.IsFinished == true)
            {
                m_SelectedTarget = FindNearestDestructibleTarget();

                m_FindNewTargetTimer.RestartTimer();
            }
        }

        private void ActionFire()
        {
            bool canFire = IsCanFire();

            if (m_SelectedTarget != null && canFire == true)
            {
                if (m_FireTimer.IsFinished == true)
                {
                    m_Ship.Fire(TurretMode.Primary);

                    m_FireTimer.RestartTimer();
                }
            }
        }

        private bool IsCanFire()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, float.MaxValue);

            if (hit && hit.collider.transform.root.GetComponent<Destructible>() != null)
            {
                if (hit.collider.transform.root.GetComponent<Destructible>().TeamId != m_Ship.TeamId &&
                    hit.collider.transform.root.GetComponent<Destructible>().TeamId != Destructible.TeamIDNeutral)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }

            else
            {
                return false;
            }
        }

        private Destructible FindNearestDestructibleTarget()
        {
            float maxDist = float.MaxValue;

            Destructible potentialTarget = null;

            foreach (var v in Destructible.AllDestructibles)
            {
                if (v.GetComponent<SpaceShip>() == m_Ship) continue;
                if (v.TeamId == Destructible.TeamIDNeutral) continue;
                if (v.TeamId == m_Ship.TeamId) continue;

                float dist = Vector2.Distance(m_Ship.transform.position, v.transform.position);
                if (dist < maxDist)
                {
                    maxDist = dist;
                    potentialTarget = v;
                }
            }

            return potentialTarget;
        }

        #region Timers

        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
            m_FireTimer = new Timer(m_ShootDelay);
            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
        }

        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);
            m_FireTimer.RemoveTime(Time.deltaTime);
            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);
        }
        #endregion

        public void SetPatrolBehaviour(AIPointPatrol point)
        {
            m_AIBehaviour = AIBehaviour.Patrol;
            m_PatrolPoint = point;
        }
    }
}
