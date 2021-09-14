using UnityEditor;
using UnityEngine;

namespace StvDEV.Galcon.UnityEditor
{
    [CustomEditor(typeof(Game.Planet))]
    public class EditorPlanet : Editor
    {
        Game.Planet planet;

        private void OnEnable()
        {
            planet = (Game.Planet)target;
        }

        public override void OnInspectorGUI()
        {
            if (planet.GetComponent<SpriteRenderer>() == null)
            {
                EditorGUILayout.HelpBox("Missing Sprite Renderer component! Add Sprite Renderer component to avoid errors.", MessageType.Error);
            }
            if (planet.GetComponent<CircleCollider2D>() == null)
            {
                EditorGUILayout.HelpBox("Missing Circle Collider 2D component! Add Circle Collider 2D component to avoid errors.", MessageType.Error);
            }
            if (planet.GetComponent<Rigidbody2D>() == null)
            {
                EditorGUILayout.HelpBox("Missing Rigidbody 2D component! Add Rigidbody 2D component to avoid errors.", MessageType.Warning);
            }

            this.serializedObject.Update();
            EditorGUILayout.BeginVertical();

            planet.EditorShowParameters = EditorGUILayout.Foldout(planet.EditorShowParameters, new GUIContent("Parameters"), EditorStyles.foldoutHeader);

            if (planet.EditorShowParameters)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("ShipsCreationCount"), new GUIContent("Ships Creation Count", "Number of ships that create planets per unit of time"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("ShipsCreationDelay"), new GUIContent("Ships Creation Delay", "Delay between ship creation (sec)"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("SpawnRate"), new GUIContent("Spawn Rate", "Percentage of ships that the planet will send to capture others"));
                EditorGUILayout.EndVertical();
            }

            planet.EditorShowUI = EditorGUILayout.Foldout(planet.EditorShowUI, new GUIContent("UI"), EditorStyles.foldoutHeader);

            if (planet.EditorShowUI)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("ShipsDisplay"), new GUIContent("Ships Display", "Text to display the number of ships on the planet"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Sprites"), new GUIContent("Sprites", "List of variations of planet images"));
                EditorGUILayout.EndVertical();
            }

            planet.EditorShowEvents = EditorGUILayout.Foldout(planet.EditorShowEvents, new GUIContent("Events"), EditorStyles.foldoutHeader);

            if (planet.EditorShowEvents)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OnOwnerChanged"), new GUIContent("On Owner Changed", "Events triggered when owner changed"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OnSwallowShip"), new GUIContent("On Swallow Ship", "Events triggered when planet swallow ship"));
                EditorGUILayout.EndVertical();
            }

            if (Application.isPlaying)
            {
                planet.EditorShowDebugInfo = EditorGUILayout.Foldout(planet.EditorShowDebugInfo, new GUIContent("Debug Information"), EditorStyles.foldoutHeader);

                if (planet.EditorShowDebugInfo)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Owner"), new GUIContent("Owner", "Planet Owner"));
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Game"), new GUIContent("Game", "A game in which the planet participates"));
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Score"), new GUIContent("Score", "Number of ships on the planet"));
                    GUI.enabled = true;
                    EditorGUILayout.EndVertical();
                }
            }

            EditorGUILayout.EndVertical();
            this.serializedObject.ApplyModifiedProperties();
        }
    }
}
