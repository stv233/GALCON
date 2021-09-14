using UnityEditor;
using UnityEngine;

namespace StvDEV.Galcon.UnityEditor
{
    [CustomEditor(typeof(Game.Game))]
    public class EditorGame : Editor
    {
        Game.Game game;

        private void OnEnable()
        {
            game = (Game.Game)target;
        }

        public override void OnInspectorGUI()
        {
            if (game.Player == null)
            {
                EditorGUILayout.HelpBox("Missing instance of settings for the player!", MessageType.Error);
            }
            if (game.AI == null)
            {
                EditorGUILayout.HelpBox("Missing instance of settings for the AI!", MessageType.Error);
            }
            if (game.PlanetTemplate == null)
            {
                EditorGUILayout.HelpBox("Missing Planet Template!", MessageType.Error);
            }

            this.serializedObject.Update();
            EditorGUILayout.BeginVertical();

            game.EditorShowPlayersParameters = EditorGUILayout.Foldout(game.EditorShowPlayersParameters, new GUIContent("Players"), EditorStyles.foldoutHeader);

            if (game.EditorShowPlayersParameters)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Player"), new GUIContent("Player", "Instance of settings for the player"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("AI"), new GUIContent("AI", "Instance of settings for the AI"));
                if ((game.AI != null) && (!game.AI.IsAI))
                {
                    EditorGUILayout.HelpBox("The instance of settings for AI is not marked as AI. Check the IsAI field in the AI settings instance.", MessageType.Warning);
                }
                EditorGUILayout.EndVertical();
            }

            game.EditorShowPlanetsParameters = EditorGUILayout.Foldout(game.EditorShowPlanetsParameters, new GUIContent("Planets"), EditorStyles.foldoutHeader);

            if (game.EditorShowPlanetsParameters)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("PlanetsCount"), new GUIContent("Planets Count", "Number of planets in the game"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("PlanetTemplate"), new GUIContent("Planet Template", "Planet spawn template"));
                EditorGUILayout.EndVertical();
            }

            game.EditorShowPlayingFieldParameters = EditorGUILayout.Foldout(game.EditorShowPlayingFieldParameters, new GUIContent("Playing Field"), EditorStyles.foldoutHeader);

            if (game.EditorShowPlayingFieldParameters)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("UpperRightCorner"), new GUIContent("Upper Right Cornert", "Upper right corner of the playing field"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("LowerLeftCorner"), new GUIContent("Lower Left Corner", "Lower left corner of the playing field"));
                EditorGUILayout.EndVertical();
            }

            game.EditorShowEvents = EditorGUILayout.Foldout(game.EditorShowEvents, new GUIContent("Events"), EditorStyles.foldoutHeader);

            if (game.EditorShowEvents)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OnPlayerWin"), new GUIContent("On Player Win", "Events triggered if a player wins"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OnAIWin"), new GUIContent("On AI Win", "Events triggered if a ai wins"));
                EditorGUILayout.EndVertical();
            }

            if (Application.isPlaying)
            {
                game.EditorShowDebugInfo = EditorGUILayout.Foldout(game.EditorShowDebugInfo, new GUIContent("Debug Information"), EditorStyles.foldoutHeader);

                if (game.EditorShowDebugInfo)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Planets"), new GUIContent("Planets", "List of planets participating in the game"));
                    GUI.enabled = true;
                    EditorGUILayout.EndVertical();
                }
            }

            EditorGUILayout.EndVertical();
            this.serializedObject.ApplyModifiedProperties();
        }
    }
}
