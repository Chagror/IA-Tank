using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Complete
{
    public class GameManager : MonoBehaviour
    {
        public int m_NumRoundsToWin = 5;            // The number of rounds a single player has to win to win the game.
        public float m_StartDelay = 5f;             // The delay between the start of RoundStarting and RoundPlaying phases.
        public float m_EndDelay = 3f;               // The delay between the end of RoundPlaying and RoundEnding phases.
        public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
        public Text m_MessageText;                  // Reference to the overlay Text to display winning text, etc.
        public GameObject m_TankPrefab;             // Reference to the prefab the players will control.
        public GameObject playerTankPrefab;
        public Team team1;
        public Team team2;
        public Grid gridManager;
        public float timeRemaining;
        public TankManager[] allTanks;


        [Header("UI")]
        public Text timerText;
        public GameObject menuUI;
        public GameObject gameUI;
        public GameObject endGameUI;
        public Text winnerText;

        [SerializeField]
        private int respawnDelay;
        [SerializeField]
        private float totalTimeForGame;
        [SerializeField]
        private GameEvent gameEndedEvent;
        private int m_RoundNumber;                  // Which round the game is currently on.
        private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
        private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
        private Team m_RoundWinner;          // Reference to the winner of the current round.  Used to make an announcement of who won.
        private TankManager m_GameWinner;           // Reference to the winner of the game.  Used to make an announcement of who won.


        const float k_MaxDepenetrationVelocity = float.PositiveInfinity;

        public static GameManager instance;

        void Awake()
        {
            if (instance != null && instance != this)
                Destroy(gameObject);    // Suppression d'une instance pr�c�dente (s�curit�...s�curit�...)

            instance = this;
            Application.targetFrameRate = 60;
        }


        private void Start()
        {

            // This line fixes a change to the physics engine.
            Physics.defaultMaxDepenetrationVelocity = k_MaxDepenetrationVelocity;

            m_StartWait = new WaitForSeconds (m_StartDelay);
            m_EndWait = new WaitForSeconds (m_EndDelay);

            LoadMenu();
        }

        private void LoadMenu()
        {
            menuUI.SetActive(true);
            gameUI.SetActive(false);
            endGameUI.SetActive(false);
        }

        public IEnumerator RespawnCoroutine(TankManager tank)
        {
            yield return new WaitForSeconds(respawnDelay);
            Debug.Log("Le tank a respawn");
            tank.m_Instance.SetActive(true);
            tank.m_Instance.transform.position = tank.m_SpawnPoint.position;
            tank.m_Instance.transform.rotation = tank.m_SpawnPoint.rotation;
        }

        private void LoadGameUI()
        {
            menuUI.SetActive(false);
            gameUI.SetActive(true);
            endGameUI.SetActive(false);
        }

        private void LoadEndGameUI()
        {
            menuUI.SetActive(false);
            gameUI.SetActive(false);
            endGameUI.SetActive(true);
        }


        public void StartNewGame(bool playersVersusAIs)
        {
            timeRemaining = totalTimeForGame;
            timerText.text = FormatTimer();

            SpawnAllTanks(playersVersusAIs);
            SetCameraTargets();

            LoadGameUI();

            // Once the tanks have been created and the camera is using them as targets, start the game.
            StartCoroutine (GameLoop());
        }

        private IEnumerator UpdateTimer()
        {
            while (timeRemaining > 0)
            {
                yield return new WaitForSecondsRealtime(1f);
                timeRemaining--;
                timerText.text = FormatTimer();
            }
        }

        private string FormatTimer()
        {
            string secondRemaining = timeRemaining % 60 < 10 ? "0" + (timeRemaining % 60).ToString() : (timeRemaining % 60).ToString();
            return $"{(int)(timeRemaining / 60)} : {secondRemaining}";
        }


        public void ReloadGame()
        {
            gameEndedEvent.Raise();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void SpawnAllTanks(bool firstTeamIsPlayers)
        {
            // For all the tanks...
            for (int i = 0; i < team1.Tanks.Length; i++)
            {
                // ... create them, set their player number and references needed for control.
                
                if (firstTeamIsPlayers)
                {
                    team1.Tanks[i].m_Instance =
                        Instantiate(playerTankPrefab, team1.Tanks[i].m_SpawnPoint.position,
                            team1.Tanks[i].m_SpawnPoint.rotation);
                    team1.Tanks[i].m_PlayerNumber = i + 1;
                    team1.Tanks[i].SetTeamInfos(team1);
                    team1.Tanks[i].Setup();
                    
                    team1.Tanks[i].GetMovement().SetControlType(Enums.ControlType.Manual);
                    team1.Tanks[i].GetShooting().SetControlType(Enums.ControlType.Manual);
                }
                else
                {
                    team1.Tanks[i].m_Instance =
                        Instantiate(m_TankPrefab, team1.Tanks[i].m_SpawnPoint.position,
                            team1.Tanks[i].m_SpawnPoint.rotation);
                    team1.Tanks[i].m_PlayerNumber = i + 1;
                    team1.Tanks[i].SetTeamInfos(team1);
                    team1.Tanks[i].Setup();
                    team1.Tanks[i].GetMovement().SetControlType(Enums.ControlType.UnityNavMesh);
                    team1.Tanks[i].GetShooting().SetControlType(Enums.ControlType.UnityNavMesh);
                }
            }

            team1.teamNameTextUI.text = team1.teamName;
            team1.teamNameTextUI.color = team1.teamColor;
            team1.teamPointsTextUI.text = 0.ToString();
            team1.teamPointsTextUI.color = team1.teamColor;

            for(int i = 0; i < team2.Tanks.Length; i++)
            {
                team2.Tanks[i].m_Instance =
                    Instantiate(m_TankPrefab, team2.Tanks[i].m_SpawnPoint.position, team2.Tanks[i].m_SpawnPoint.rotation);
                team2.Tanks[i].m_PlayerNumber = team1.Tanks.Length + i + 1;
                team2.Tanks[i].SetTeamInfos(team2);
                team2.Tanks[i].Setup();
                team2.Tanks[i].GetMovement().SetControlType(Enums.ControlType.UnityNavMesh);
                team2.Tanks[i].GetShooting().SetControlType(Enums.ControlType.UnityNavMesh);
            }

            team2.teamNameTextUI.text = team2.teamName;
            team2.teamNameTextUI.color = team2.teamColor;
            team2.teamPointsTextUI.text = 0.ToString();
            team2.teamPointsTextUI.color = team2.teamColor;

            allTanks = new TankManager[team1.Tanks.Length + team2.Tanks.Length];
            team1.Tanks.CopyTo(allTanks, 0);
            team2.Tanks.CopyTo(allTanks, team1.Tanks.Length);
        }


        private void SetCameraTargets()
        {
            // Create a collection of transforms the same size as the number of tanks.
            Transform[] targets = new Transform[allTanks.Length];

            // For each of these transforms...
            for (int i = 0; i < allTanks.Length; i++)
            {
                // ... set it to the appropriate tank transform.
                targets[i] = allTanks[i].m_Instance.transform;
            }

            // These are the targets the camera should follow.
            m_CameraControl.m_Targets = targets;
        }

        private IEnumerator GameLoop ()
        {
            // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
            yield return StartCoroutine (GameStarting ());

            StartCoroutine(UpdateTimer());

            // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
            yield return StartCoroutine (GamePlaying());

            // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
            yield return StartCoroutine (GameEnding());
        }


        private IEnumerator GameStarting ()
        {
            ResetAllTanks ();
            DisableTankControl ();

            // Snap the camera's zoom and position to something appropriate for the reset tanks.
            m_CameraControl.SetStartPositionAndSize ();

            timeRemaining = totalTimeForGame;

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return m_StartWait;
        }


        private IEnumerator GamePlaying ()
        {
            // As soon as the round begins playing let the players control the tanks.
            EnableTankControl ();

            // While there is not one tank left...
            while (timeRemaining > 0)
            {
                // ... return on the next frame.
                yield return null;
            }
        }



        private IEnumerator GameEnding ()
        {
            // Stop tanks from moving.
            DisableTankControl();

            // Clear the winner from the previous round.
            m_RoundWinner = null;

            // See if there is a winner now the round is over.
            m_RoundWinner = GetRoundWinner();

            if (m_RoundWinner != null)
            {
                winnerText.text = m_RoundWinner.teamName;
                winnerText.color = m_RoundWinner.teamColor;
            }
            else
            {
                winnerText.text = "DRAW";
            }

            LoadEndGameUI();

            yield return null;

        }

        // This function is to find out if there is a winner of the round.
        // This function is called with the assumption that 1 or fewer tanks are currently active.
        private Team GetRoundWinner()
        {
            if(team1.teamPoints > team2.teamPoints)
            {
                return team1;
            }
            else if(team1.teamPoints < team2.teamPoints)
            {
                return team2;
            }
            return null;
        }

        // This function is used to turn all the tanks back on and reset their positions and properties.
        private void ResetAllTanks()
        {
            for (int i = 0; i < allTanks.Length; i++)
            {
                allTanks[i].Reset();
            }
        }


        private void EnableTankControl()
        {
            for (int i = 0; i < allTanks.Length; i++)
            {
                allTanks[i].EnableControl();
            }
        }


        private void DisableTankControl()
        {
            for (int i = 0; i < allTanks.Length; i++)
            {
                allTanks[i].DisableControl();
            }
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }


    }
}
