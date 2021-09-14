using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StvDEV.Galcon.Game
{
    /// <summary>
    /// Galcon mechanics class.
    /// </summary>
    public class Game : MonoBehaviour
    {
        /// <summary>
        /// Player parameters.
        /// </summary>
        [SerializeField]
        public Player Player;

        /// <summary>
        /// AI paramenters
        /// </summary>
        [SerializeField]
        public Player AI;

        /// <summary>
        /// Number of planets in the game.
        /// </summary>
        [SerializeField][Range(5, 25)]
        public int PlanetsCount = 5;

        /// <summary>
        /// Planet spawn template.
        /// </summary>
        [SerializeField]
        public Planet PlanetTemplate;

        /// <summary>
        /// Upper right corner of the playing field.
        /// </summary>
        [SerializeField]
        public Vector2 UpperRightCorner;

        /// <summary>
        /// Lower left corner of the playing field.
        /// </summary>
        [SerializeField]
        public Vector2 LowerLeftCorner;

        /// <summary>
        /// Events triggered if a player wins.
        /// </summary>
        [SerializeField]
        public UnityEngine.Events.UnityEvent OnPlayerWin;

        /// <summary>
        /// Events triggered if a ai wins.
        /// </summary>
        [SerializeField]
        public UnityEngine.Events.UnityEvent OnAIWin;

        /// <summary>
        /// List of planets participating in the game.
        /// </summary>
        [SerializeField]
        public List<Planet> Planets = new List<Planet>();

#if UNITY_EDITOR
        public bool EditorShowPlayersParameters = true;
        public bool EditorShowPlanetsParameters = true;
        public bool EditorShowPlayingFieldParameters = true;
        public bool EditorShowEvents = false;
        public bool EditorShowDebugInfo = false;
#endif

        void Start()
        {
            if (Player == null)
            {
                Debug.LogError("Missing instance of settings for the player!");
                enabled = false;
            }
            if (AI == null)
            {
                Debug.LogError("Missing instance of settings for the AI!");
                enabled = false;
            }
            if (PlanetTemplate == null)
            {
                Debug.LogError("Missing Planet Template!");
                enabled = false;
            }

            PlanetTemplate.gameObject.SetActive(false);
            GeneratePlanets();
            StartCoroutine(AIContriol(AI.AIDelay));
        }

        private void Update()
        {
            CheckGameStatus();
        }

        /// <summary>
        /// Creates planets for the game.
        /// </summary>
        private void GeneratePlanets()
        {
            for (var i = 0; i < PlanetsCount; i++)
            {
                Planet planet = Instantiate(PlanetTemplate);
                planet.Game = this;
                planet.SetRandomSprite();
                planet.Score = UnityEngine.Random.Range(5, 101);
                float scaleModifer = UnityEngine.Random.Range(-planet.transform.localScale.x * 0.2f, planet.transform.localScale.x * 0.2f);
                planet.transform.localScale = new Vector3(planet.transform.localScale.x + scaleModifer, planet.transform.localScale.y + scaleModifer, planet.transform.localScale.z);
                bool satisfy = false;
                while (!satisfy)
                {
                    float randomX = UnityEngine.Random.Range(LowerLeftCorner.x, UpperRightCorner.x);
                    float randomY = UnityEngine.Random.Range(LowerLeftCorner.y, UpperRightCorner.y);
                    planet.transform.position = new Vector2(randomX, randomY);
                    satisfy = true;
                    foreach (Planet readyPlanet in Planets)
                    {
                        if (Vector2.Distance(planet.transform.position, readyPlanet.transform.position)
                            < (((planet.GetComponent<CircleCollider2D>().radius * planet.transform.localScale.x)
                            + ((readyPlanet.GetComponent<CircleCollider2D>().radius * readyPlanet.transform.localScale.x))) * 2))
                        {
                            satisfy = false;
                            break;
                        }
                    }
                }
                planet.gameObject.SetActive(true);
                Planets.Add(planet);
            }

            Planets[0].ChangeOwner(Player);
            Planets[0].Score = 50;
            Planets[1].ChangeOwner(AI);
            Planets[1].Score = 50;
        }

        /// <summary>
        /// Requests moves from the AI.
        /// </summary>
        /// <param name="delay">Delay between AI moves.</param>
        public IEnumerator AIContriol(float delay)
        {
            while (true)
            {
                Player.AI.RequestMove(AI, this);
                yield return new WaitForSeconds(delay);
            }
        }

        /// <summary>
        /// Checks the game for one of the sides to win.
        /// </summary>
        private void CheckGameStatus()
        {
            int playerCountr = 0;
            int aiCounter = 0;
            foreach (Planet planet in Planets)
            {
                if (planet.Owner == Player)
                {
                    playerCountr++;
                }
                else if (planet.Owner == AI)
                {
                    aiCounter++;
                }
            }

            if (playerCountr == 0)
            {
                OnAIWin.Invoke();
            }
            else if (aiCounter == 0)
            {
                OnPlayerWin.Invoke();
            }
        }
    }
}
