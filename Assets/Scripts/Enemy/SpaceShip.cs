using UnityEngine;


namespace TowerDefense
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        #region Parameters

        /// <summary>
        /// ����� ��� �������������� ��������� � ������.
        /// </summary>
        [Header("Space Ship")]
        [SerializeField] private float m_Mass;

        /// <summary>
        /// ��������� ������ ����.
        /// </summary>
        [SerializeField] private float m_Thrust;
        private float m_DefaultThrust;

        /// <summary>
        /// ��������� ����.
        /// </summary>
        [SerializeField] private float m_Mobility;
        private float m_DefaultMobility;

        /// <summary>
        /// ������������ �������� ��������.
        /// </summary>
        [SerializeField] private float m_MaxLinearVelocity;
        public float MaxLinearVelocity => m_MaxLinearVelocity;

        private float m_MaxVelocityBackup;

        /// <summary>
        /// ������������ �������� ��������. � ��������/���.
        /// </summary>
        [SerializeField] private float m_MaxAngularVelocity;
        public float MaxAngularVelocity => m_MaxAngularVelocity;

        /// <summary>
        /// ����������� ������ �� �����.
        /// </summary>
        private Rigidbody2D m_Rigid;

        public bool IsPlayerShip = false;

        [SerializeField] private Sprite m_PreviewImage;
        public Sprite PreviewImage => m_PreviewImage;


        #endregion

        #region Public API

        /// <summary>
        /// ���������� �������� �����. �� -1.0 �� +1.0
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// ���������� ������������ �����. �� -1.0 �� +1.0
        /// </summary>
        public float TorqueControl { get; set; }

        #endregion

        #region UnityEvents

        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertia = 1;

            // InitArguments();
        }

        private void FixedUpdate()
        {
            UpdateRigidBody();

            // UpdateEnergyRegen();
        }

        #endregion

        /// <summary>
        /// ����� ���������� ��� ������� ��� ��������
        /// </summary>
        private void UpdateRigidBody()
        {
            m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        /// <summary>
        /// TODO: �������� ��������� �����-��������
        /// ������������ ��������
        /// </summary>
        /// <param name="count"></param>
        /// <returns>true ���� ������� ���� �������</returns>
        public bool DrawEnergy(int count)
        {
            return true;
        }

        /// <summary>
        /// TODO: �������� ��������� �����-��������
        /// ������������ ��������
        /// </summary>
        /// <param name="count"></param>
        /// <returns>true ���� ������� ���� �������</returns>
        public bool DrawAmmo(int count)
        {
            return true;
        }

        /// <summary>
        /// TODO: �������� ��������� �����-��������
        /// ������������ ��
        /// </summary>
        public void Fire(TurretMode mode)
        {
            return;
        }


        public new void Use(EnemyAsset asset)
        {
            m_MaxLinearVelocity = asset.moveSpeed;

            base.Use(asset);
        }

        public void DecreaseMaxLinearVelocity(float koef)
        {
            m_MaxVelocityBackup = m_MaxLinearVelocity;
            m_MaxLinearVelocity *= (1 - koef);
        }

        public void RestoreMaxLinearVelocity()
        {
            if (m_MaxVelocityBackup != 0)
            {
                m_MaxLinearVelocity = m_MaxVelocityBackup;
            }
        }

        /*

        [SerializeField] private Turret[] m_Turrets;

        public void Fire(TurretMode mode)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                if (m_Turrets[i].Mode == mode)
                {
                    m_Turrets[i].Fire();
                }
            }
        }

        #region PowerUps

        [SerializeField] private int m_MaxEnergy;
        [SerializeField] private int m_MaxAmmo;
        [SerializeField] private int m_EnergyRegenPerSecond;

        private float m_PrimaryEnergy;
        private int m_SecondaryAmmo;

        public void AddEnergy(int energy)
        {
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + energy, 0, m_MaxEnergy);
        }

        public void AddAmmo(int ammo)
        {
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        }

        private void InitArguments()
        {
            m_PrimaryEnergy = m_MaxEnergy;
            m_SecondaryAmmo = m_MaxAmmo;

            m_DefaultThrust = m_Thrust;
            m_DefaultMobility = m_Mobility;
        }

        private void SetDefaultParameters()
        {
            m_Thrust = m_DefaultThrust;
            m_Mobility = m_DefaultMobility;
        }

        private void UpdateEnergyRegen()
        {
            m_PrimaryEnergy += (float)m_EnergyRegenPerSecond * Time.fixedDeltaTime;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
        }

        

        #endregion

        public void AssignWeapon(TurretProperties props)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                m_Turrets[i].AssignLoadout(props);
            }
        }

        */
    }
}
