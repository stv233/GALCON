using UnityEditor;
using UnityEngine;

namespace StvDEV.Galcon.UnityEditor
{
    [CustomEditor(typeof(Game.Ship))]
    public class EditorShip : Editor
    {
        Game.Ship ship;

        private void OnEnable()
        {
            ship = (Game.Ship)target;
        }

        public override void OnInspectorGUI()
        {
            if (ship.GetComponent<SpriteRenderer>() == null)
            {
                EditorGUILayout.HelpBox("Missing Sprite Renderer component! Add Sprite Renderer component to avoid errors.", MessageType.Error);
            }
            if (ship.GetComponent<CircleCollider2D>() == null)
            {
                EditorGUILayout.HelpBox("Missing Circle Collider 2D component! Add Circle Collider 2D component to avoid errors.", MessageType.Error);
            }
            if (ship.GetComponent<Rigidbody2D>() == null)
            {
                EditorGUILayout.HelpBox("Missing Rigidbody 2D component! Add Rigidbody 2D component to avoid errors.", MessageType.Warning);
            }

            this.serializedObject.Update();
            EditorGUILayout.BeginVertical();

            ship.EditorShowParameters = EditorGUILayout.Foldout(ship.EditorShowParameters, new GUIContent("Parameters"), EditorStyles.foldoutHeader);

            if (ship.EditorShowParameters)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Speed"), new GUIContent("Speed", "Ship movement speed"));
                EditorGUILayout.EndVertical();
            }

            ship.EditorShowEvents = EditorGUILayout.Foldout(ship.EditorShowEvents, new GUIContent("Events"), EditorStyles.foldoutHeader);

            if (ship.EditorShowEvents)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OnCreated"), new GUIContent("On Created", "Events triggered when ship created"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OnDestroyed"), new GUIContent("On Destroyed", "Events triggered when ship destroyed"));
                EditorGUILayout.EndVertical();
            }

            if (Application.isPlaying)
            {
                ship.EditorShowDebugInfo = EditorGUILayout.Foldout(ship.EditorShowDebugInfo, new GUIContent("Debug Information"), EditorStyles.foldoutHeader);

                if (ship.EditorShowDebugInfo)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Owner"), new GUIContent("Owner", "Ship owner"));
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("Target"), new GUIContent("Target", "Target"));
                    GUI.enabled = true;
                    EditorGUILayout.EndVertical();
                }
            }

            EditorGUILayout.EndVertical();
            this.serializedObject.ApplyModifiedProperties();
        }
    }
}
