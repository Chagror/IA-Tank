using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Complete
{
    public class TankMovement : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
        public float m_Speed = 12f;                 // How fast the tank moves forward and back.
        public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
        public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
        public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
        public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
		public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.

        
        public Team tankTeam;
        public NavMeshAgent m_NavMeshAgent;
        private NavMeshPath m_newPath;
        private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
        private string m_TurnAxisName;              // The name of the input axis for turning.
        private Rigidbody m_Rigidbody;              // Reference used to move the tank.
        public float m_MovementInputValue;         // The current value of the movement input.
        public float m_TurnInputValue;             // The current value of the turn input.
        private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.
        private ParticleSystem[] m_particleSystems; // References to all the particles systems used by the Tanks
        private Vector3 m_Destination;
        private int m_CurrentTargetPositionIndex;
        [SerializeField] private Enums.ControlType currentControlType;
        [SerializeField] private float rotationErrorMargin = 5f;
        [SerializeField]
        private float collisionDistance = 8f;
        [SerializeField]
        private List<PathMovement> pathMovements;

        private List<Vector3> m_CurrentPath = new List<Vector3>();
        
        

        private void Awake ()
        {
            m_Rigidbody = GetComponent<Rigidbody> ();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
        }


        private void OnEnable ()
        {

            //m_Rigidbody.isKinematic = false;
            // Also reset the input values.
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;

            m_Rigidbody.isKinematic = true;
            
            // We grab all the Particle systems child of that Tank to be able to Stop/Play them on Deactivate/Activate
            // It is needed because we move the Tank when spawning it, and if the Particle System is playing while we do that
            // it "think" it move from (0,0,0) to the spawn point, creating a huge trail of smoke
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Play();
            }
        }


        private void OnDisable ()
        {
            // When the tank is turned off, set it to kinematic so it stops moving.
            m_Rigidbody.isKinematic = true;
            
            // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
            for(int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Stop();
            }
        }


        private void Start ()
        {
            m_newPath = new NavMeshPath();
            // The axes names are based on player number.
            m_MovementAxisName = "Vertical" + m_PlayerNumber;
            m_TurnAxisName = "Horizontal" + m_PlayerNumber;

            // Store the original pitch of the audio source.
            m_OriginalPitch = m_MovementAudio.pitch;

            if (currentControlType != Enums.ControlType.Manual)
            {
                m_Destination = transform.position;
            }
            
        }


        private void Update ()
        {
            HandleInputs();

            NavigationSystem();

            DetectCollision();

            EngineAudio();
        }


        public void SetControlType(Enums.ControlType newControlType)
        {
            currentControlType = newControlType;
        }
        
        void NavigationSystem()
        {
            if (currentControlType == Enums.ControlType.Manual) return;
            
            float targetRotation;

            bool isNearCurrentCorner;

            if (m_CurrentPath != null && m_CurrentPath.Count > 1)
            {
                isNearCurrentCorner = Utils.CompareVectors(m_CurrentPath[m_CurrentTargetPositionIndex],
                    transform.position, m_NavMeshAgent.stoppingDistance);

                if (isNearCurrentCorner && m_CurrentPath.Count > m_CurrentTargetPositionIndex + 1)
                {
                    m_CurrentTargetPositionIndex++;
                }

                targetRotation = Vector3.SignedAngle(transform.forward, m_CurrentPath[m_CurrentTargetPositionIndex] - transform.position,
                    Vector3.up);


                if (Mathf.Abs(targetRotation) < rotationErrorMargin && !isNearCurrentCorner)
                {
                    m_MovementInputValue = 1;
                    m_TurnInputValue = 0;
                }
                else if (targetRotation <= 0 && !isNearCurrentCorner)
                {
                    m_TurnInputValue = -1;
                    m_MovementInputValue = 0;
                }
                else if (targetRotation >= 0 && !isNearCurrentCorner)
                {
                    m_TurnInputValue = 1;
                    m_MovementInputValue = 0;
                }
                else
                {
                    m_TurnInputValue = 0;
                    m_MovementInputValue = 0;
                }
            }
            else
            {
                m_MovementInputValue = 0;
            }
            
        }

        public void SetTeam(Team team)
        {
            tankTeam = team;
        }

        public Vector3 GetNearestWalkablePosition(Vector3 desiredPosition)
        {
            Vector3 newPosition = desiredPosition;
            
            if (NavMesh.FindClosestEdge(desiredPosition, out NavMeshHit hit, NavMesh.AllAreas))
            {
                newPosition = hit.position;
            }

            return newPosition;
        }
        
        private void OnDrawGizmos()
        {
            if (m_newPath == null) return;
            int i = 0;
            foreach (var corner in m_newPath.corners)
            {
                Gizmos.color = Color.green;
                if(i == m_CurrentTargetPositionIndex){Gizmos.color = Color.red;}
                
                Gizmos.DrawCube(corner, Vector3.one *.5f);
                i++;
            }
        }

        private void DetectCollision()
        {
            if (currentControlType == Enums.ControlType.Manual) return;
            
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, collisionDistance))
            {
                Node collidedObjectNode = Grid.instance.GetNodeFromWorldPos(hit.transform.position);
                PathMovement currentPathMovement = pathMovements.Find((x) => x.movementType == currentControlType);
                
                if (!Grid.instance.nodeBlockedDynamicly.ContainsKey(collidedObjectNode) && collidedObjectNode.walkable)
                {
                    Grid.instance.AddDynamiclyUnwalkableNode(collidedObjectNode,hit.transform);
                    m_CurrentPath = currentPathMovement.CalculatePath(transform.position, m_Destination);
                }
                else if (currentControlType == Enums.ControlType.UnityNavMesh)
                {
                    m_CurrentPath = currentPathMovement.CalculatePath(transform.position, m_Destination);
                }
                else
                {
                    return;
                }
                

                Debug.Log($"Tank {name} collided with {hit.transform.name} !");
            }
        }

        private void EngineAudio () //TODO : calculate angular velocity of the agent to play sounds and VFX
        {
            // If there is no input (the tank is stationary)...
            if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f)
            {
                // ... and if the audio source is currently playing the driving clip...
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    // ... change the clip to idling and play it.
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play ();
                }
            }
            else
            {
                // Otherwise if the tank is moving and if the idling clip is currently playing...
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    // ... change the clip to driving and play.
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }

        public void SetPath(List<Vector3> path)
        {
            m_CurrentPath = path;
        }

        public PathMovement GetCurrentPathMovement()
        {
            return pathMovements.Find((x) => x.movementType == currentControlType);
        }

        private void HandleInputs()
        {
            if (currentControlType == Enums.ControlType.Manual)
            {
                m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
                m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
            }
            else
            {
                if (Input.GetMouseButtonDown(1))
                {
                    RaycastHit hit;

                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit,
                            Mathf.Infinity))
                    {
                        m_Destination = hit.point;

                        m_CurrentTargetPositionIndex = 0;

                        PathMovement currentPathMovement = pathMovements.Find((x) => x.movementType == currentControlType);

                        if (!currentPathMovement)
                        {
                            m_Destination = transform.position;
                            return;
                        }

                        currentPathMovement.Init(this);

                        m_CurrentPath = currentPathMovement.CalculatePath(transform.position, m_Destination);

                    }
                }
            }
        }
        
        private void FixedUpdate ()
        {
            // Adjust the rigidbodies position and orientation in FixedUpdate.
            Move();
            Turn();
        }

        private void LateUpdate()
        {
            m_NavMeshAgent.nextPosition = m_Rigidbody.position;
        }

        private void Move ()
        {
            // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
            Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

            // Apply this movement to the rigidbody's position.
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }

        private void Turn ()
        {
            // Determine the number of degrees to be turned based on the input, speed and time between frames.
            float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

            // Make this into a rotation in the y axis.
            Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);

            // Apply this rotation to the rigidbody's rotation.
            m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
        }

        public float GetMovementInputValue()
        {
            return m_MovementInputValue;
        }
        
        
        
    }
}