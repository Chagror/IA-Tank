using System;
using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class TankShooting : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify the different players.
        public Rigidbody m_Shell;                   // Prefab of the shell.
        public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
        public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
        public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
        public float m_LaunchForce = 30;   // How long the shell can charge for before it is fired at max force.
        public float m_ShootCooldown = 1;

        [SerializeField] Enums.ControlType currentControlType;
        private string m_FireButton;                // The input axis that is used for launching shells.
        [HideInInspector]
        public bool m_Fired;                       // Whether or not the shell has been launched with this button press.
        private float m_ShootCountdown;


        private void Start ()
        {
            // The fire axis is based on the player number.
            m_FireButton = "Fire" + m_PlayerNumber;
            
            m_ShootCountdown = m_ShootCooldown;
        }

        public void SetControlType(Enums.ControlType newControlType)
        {
            currentControlType = newControlType;
        }
        

        private void Update ()
        {
            HandleInputs();
            HandleShooting();
        }


        
        
        void HandleInputs()
        {
            if (currentControlType == Enums.ControlType.Manual)
            {
                m_Fired = Input.GetButtonDown(m_FireButton);
            }
        }

        void HandleShooting()
        {
            if (m_ShootCountdown < m_ShootCooldown)
            {
                m_ShootCountdown += Time.deltaTime;
            }
            
            if (m_Fired && m_ShootCountdown >= m_ShootCooldown)
            {
                Fire();
            }
        }
        
        private void Fire ()
        {
            m_ShootCountdown = 0;
            // Create an instance of the shell and store a reference to it's rigidbody.
            Rigidbody shellInstance =
                Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance.velocity = m_LaunchForce * m_FireTransform.forward;

            // Change the clip to the firing clip and play it.
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play ();
            
            m_Fired = false;
        }
    }
}