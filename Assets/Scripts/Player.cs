using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StvDEV.Galcon.Game
{

    /// <summary>
    /// Player settings class.
    /// </summary>
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// AI class.
        /// </summary>
        public static class AI
        {
            /// <summary>
            /// Asks the AI to make a move.
            /// </summary>
            /// <param name="player">AI player settings</param>
            /// <param name="game">AI game</param>
            public static void RequestMove(Player player, Game game)
            {
                List<Planet> playerPlanets = GetPlayerPlanets(player, game);
                List<Planet> notPlayerPlanets = GetNotPlayerPlanets(player, game);
                List<Planet> neutralPlanets = GetNeutralPlanets(game);

                foreach (Planet playerPlanet in playerPlanets)
                {
                    foreach (Planet neutralPlanet in neutralPlanets)
                    {
                        if ((playerPlanet.Score * playerPlanet.SpawnRate) > neutralPlanet.Score)
                        {
                            playerPlanet.StartCoroutine(playerPlanet.SpawnShips(neutralPlanet));
                            return;
                        }
                    }

                    foreach (Planet notPlayerPlanet in notPlayerPlanets)
                    {
                        if ((playerPlanet.Score * playerPlanet.SpawnRate) > notPlayerPlanet.Score)
                        {
                            playerPlanet.StartCoroutine(playerPlanet.SpawnShips(notPlayerPlanet));
                            return;
                        }
                    }
                }
            }

            /// <summary>
            /// Gets a list of the planets owned by the player with the specified settings.
            /// </summary>
            /// <param name="player">Player settings</param>
            /// <param name="game">Player game</param>
            /// <returns>List of the planets</returns>
            private static List<Planet> GetPlayerPlanets(Player player, Game game)
            {
                List<Planet> planets = new List<Planet>();
                foreach (Planet planet in game.Planets)
                {
                    if (planet.Owner == player)
                    {
                        planets.Add(planet);
                    }
                }
                return planets;
            }

            /// <summary>
            /// Gets a list of the planets not owned by the player with the specified settings.
            /// </summary>
            /// <param name="player">Player settings</param>
            /// <param name="game">Player game</param>
            /// <returns>List of the planets</returns>
            private static List<Planet> GetNotPlayerPlanets(Player player, Game game)
            {
                List<Planet> planets = new List<Planet>();
                foreach (Planet planet in game.Planets)
                {
                    if (planet.Owner != player)
                    {
                        planets.Add(planet);
                    }
                }
                return planets;
            }

            /// <summary>
            /// Gets a list of neutral planets.
            /// </summary>
            /// <param name="game">Game</param>
            /// <returns>List of the planets</returns>
            private static List<Planet> GetNeutralPlanets(Game game)
            {
                List<Planet> planets = new List<Planet>();
                foreach (Planet planet in game.Planets)
                {
                    if (planet.Owner == null)
                    {
                        planets.Add(planet);
                    }
                }
                return planets;
            }
        }

        /// <summary>
        /// Player's main color.
        /// </summary>
        [SerializeField]
        public Color MainColor;

        /// <summary>
        /// Player's highlighting color.
        /// </summary>
        [SerializeField]
        public Color HighlightingColor;

        /// <summary>
        /// Player ship template.
        /// </summary>
        [SerializeField]
        public Ship ShipTemplate;

        /// <summary>
        /// Is this instance of AI.
        /// </summary>
        [SerializeField]
        public bool IsAI;

        /// <summary>
        /// The delay between the moves of a given player as an AI (sec).
        /// </summary>
        [SerializeField]
        public float AIDelay=5f;

        /// <summary>
        /// List of planets currently selected by the player.
        /// </summary>
        [SerializeField]
        public List<Planet> SelectedPlanets;

#if UNITY_EDITOR
        public bool EditorShowParameters = true;
        public bool EditorShowAIParameters = false;
        public bool EditorShowDebugInfo = false;
#endif

        void Start()
        {
            if (ShipTemplate == null)
            {
                Debug.LogError("Missing Ship Template!");
                enabled = false;
            }
            ShipTemplate.gameObject.SetActive(false);
        }


    }
}
