using UnityEditor;
using UnityEngine;

namespace StvDEV.Galcon.UnityEditor
{
    [CustomEditor(typeof(Game.Player))]
    public class EditorPlayer : Editor
    {
        Game.Player player;

        private void OnEnable()
        {
            player = (Game.Player)target;
        }

        public override void OnInspectorGUI()
        {
            if (player.ShipTemplate == null)
            {
                EditorGUILayout.HelpBox("Missing Ship Template!", MessageType.Error);
            }

            this.serializedObject.Update();
            EditorGUILayout.BeginVertical();

            player.EditorShowParameters = EditorGUILayout.Foldout(player.EditorShowParameters, new GUIContent("Parameters"), EditorStyles.foldoutHeader);

            if (player.EditorShowParameters)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("MainColor"), new GUIContent("Main Color", "Player's main color"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("HighlightingColor"), new GUIContent("Highlighting Color", "Player's highlighting color"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("ShipTemplate"), new GUIContent("Ship Template", "Player ship template"));
                EditorGUILayout.EndVertical();
            }

            player.EditorShowAIParameters = EditorGUILayout.Foldout(player.EditorShowAIParameters, new GUIContent("AI"), EditorStyles.foldoutHeader);

            if (player.EditorShowAIParameters)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("IsAI"), new GUIContent("Is AI", "Is this instance of AI"));
                if (player.IsAI)
                {
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("AIDelay"), new GUIContent("AI Delay", "The delay between the moves of a given player as an AI (sec)"));
                }
                EditorGUILayout.EndVertical();
            }

            if (Application.isPlaying)
            {
                player.EditorShowDebugInfo = EditorGUILayout.Foldout(player.EditorShowDebugInfo, new GUIContent("Debug Information"), EditorStyles.foldoutHeader);

                if (player.EditorShowDebugInfo)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("SelectedPlanets"), new GUIContent("Selected Planets", "List of planets currently selected by the player"));
                    EditorGUILayout.EndVertical();
                }
            }

            EditorGUILayout.EndVertical();
            this.serializedObject.ApplyModifiedProperties();
        }
    }
}
