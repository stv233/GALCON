using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StvDEV.Galcon.Game
{
    /// <summary>
    /// Ship class.
    /// </summary>
    public class Ship : MonoBehaviour
    {
        /// <summary>
        /// Ship movement speed.
        /// </summary>
        [SerializeField]
        public float Speed = 0.05f;

        /// <summary>
        /// Events triggered when ship created.
        /// </summary>
        [SerializeField]
        public UnityEngine.Events.UnityEvent OnCreated;

        /// <summary>
        /// Events triggered when ship destroyed.
        /// </summary>
        [SerializeField]
        public UnityEngine.Events.UnityEvent OnDestroyed;

        /// <summary>
        /// Ship owner.
        /// </summary>
        [SerializeField]
        public Player Owner;

        /// <summary>
        /// Ship target.
        /// </summary>
        [SerializeField]
        public Planet Target;

#if UNITY_EDITOR
        public bool EditorShowParameters = true;
        public bool EditorShowEvents = false;
        public bool EditorShowDebugInfo = false;
#endif

        private Planet[] _planets;
        private Collider2D _collider;

        void Start()
        {
            if (Target == null)
            {
                Destroy(this);
            }

            _planets = Planet.FindObjectsOfType<Planet>();
            _collider = GetComponent<Collider2D>();
            RotateToTarget();
            OnCreated.Invoke();

        }
        
        void Update()
        {
            Move();
            CheckShipDestination();
        }

        private void OnDestroy()
        {
            OnDestroyed.Invoke();
        }

        /// <summary>
        /// Rotate ship to his target.
        /// </summary>
        public void RotateToTarget()
        {
            var vectorToTarget = (Vector2)Target.transform.position - (Vector2)transform.position;
            var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        /// <summary>
        /// Moves the ship forward.
        /// </summary>
        public void Move()
        {
            RotateToTarget();
            transform.position += transform.up * Speed;
        }

        /// <summary>
        /// Checks if the ship has reached its destination.
        /// </summary>
        private void CheckShipDestination()
        {
            foreach (Planet planet in _planets)
            {
                if (_collider.IsTouching(planet.GetComponent<Collider2D>()))
                {
                    if (planet == Target)
                    {
                        planet.SwallowShip(this);
                        return;
                    }
                }
            }
        }
    }
}
