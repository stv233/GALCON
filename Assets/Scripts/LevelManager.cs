using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StvDEV.Galcon.Unity
{

    public class LevelManager : MonoBehaviour
    {
        /// <summary>
        /// Load level by name.
        /// </summary>
        /// <param name="levelName">Level name</param>
        public void LoadLevel(string levelName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
        }

        /// <summary>
        /// Quit from application.
        /// </summary>
        public void Quit()
        {
            Application.Quit();
        }
    }
}
