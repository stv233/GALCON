using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StvDEV.Galcon.Game
{
    /// <summary>
    /// Planet class.
    /// </summary>
    public class Planet : MonoBehaviour
    {
        /// <summary>
        /// Number of ships that create planets per unit of time.
        /// </summary>
        [SerializeField]
        public int ShipsCreationCount = 5;

        /// <summary>
        /// Delay between ship creation (sec).
        /// </summary>
        [SerializeField]
        public float ShipsCreationDelay = 1f;

        /// <summary>
        /// Percentage of ships that the planet will send to capture others.
        /// </summary>
        [SerializeField][Range(0.01f, 1)]
        public float SpawnRate = 0.5f;

        /// <summary>
        /// Text to display the number of ships on the planet.
        /// </summary>
        [SerializeField]
        public UnityEngine.UI.Text ShipsDisplay;

        /// <summary>
        /// List of variations of planet images.
        /// </summary>
        [SerializeField]
        public List<Sprite> Sprites;

        /// <summary>
        /// Events triggered when owner changed.
        /// </summary>
        [SerializeField]
        public UnityEngine.Events.UnityEvent OnOwnerChanged;

        /// <summary>
        /// Events triggered when planet swallow ship.
        /// </summary>
        [SerializeField]
        public UnityEngine.Events.UnityEvent OnSwallowShip;

        /// <summary>
        /// Planet Owner.
        /// </summary>
        [SerializeField]
        public Player Owner;

        /// <summary>
        /// A game in which the planet participates.
        /// </summary>
        [SerializeField]
        public Game Game;

        /// <summary>
        /// Number of ships on the planet.
        /// </summary>
        [SerializeField]
        public int Score = 0;

#if UNITY_EDITOR
        public bool EditorShowParameters = true;
        public bool EditorShowUI = true;
        public bool EditorShowEvents = false;
        public bool EditorShowDebugInfo = false;
#endif

        private bool _selected;
        private Color _defaultColor;
        private bool _spawnInProgress;


        void Start()
        {
            _defaultColor = GetComponent<SpriteRenderer>().color;
            StartCoroutine(GenerateShips(ShipsCreationDelay));
        }

        void Update()
        {
            if (ShipsDisplay != null)
            {
                ShipsDisplay.text = Score.ToString();
            }

        }

        /// <summary>
        /// Swallows the ship.
        /// </summary>
        /// <param name="ship">Ship</param>
        public void SwallowShip(Ship ship)
        {
            if (ship.Owner == Owner)
            {
                Score++;
            }
            else
            {
                if (Score == 0)
                {
                    ChangeOwner(ship.Owner);
                    Score++;
                }
                else
                {
                    Score--;
                }
            }

            OnSwallowShip.Invoke();
            Destroy(ship.gameObject);

        }

        /// <summary>
        /// Changes the owner of the planet.
        /// </summary>
        /// <param name="owner">New owner</param>
        public void ChangeOwner(Player owner)
        {
            Owner = owner;
            this.GetComponent<SpriteRenderer>().color = Owner.MainColor;
            OnOwnerChanged.Invoke();
        }

        /// <summary>
        /// Sets if this planet is currently selected.
        /// </summary>
        /// <param name="selection">Selected</param>
        public void SetSelection(bool selection)
        {

            _selected = selection;
            if (_selected)
            {
                GetComponent<SpriteRenderer>().color = Owner.HighlightingColor;
                Game.Player.SelectedPlanets.Add(this);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Owner.MainColor;
                Game.Player.SelectedPlanets.Remove(this);
            }
        }

        /// <summary>
        /// Spawn ships and send them to the target.
        /// </summary>
        /// <param name="target">Target</param>
        public IEnumerator SpawnShips(Planet target)
        {
            if (_spawnInProgress) { yield break; }
            _spawnInProgress = true;
            int count = (int)(Score * SpawnRate);
            float side = (transform.position.x - target.transform.position.x > 0) ? -1.5f : 1.5f;

            for (var i = 0; i < count; i++)
            {
                float offset = UnityEngine.Random.Range(0, GetComponent<CircleCollider2D>().radius);
                GameObject ship = (GameObject)Instantiate(Owner.ShipTemplate.gameObject, new Vector3(transform.position.x + (((GetComponent<CircleCollider2D>().radius * transform.localScale.x) + (Owner.ShipTemplate.GetComponent<CircleCollider2D>().radius * Owner.ShipTemplate.transform.localScale.x) - offset) * side), transform.position.y + (((Owner.ShipTemplate.GetComponent<CircleCollider2D>().radius * Owner.ShipTemplate.transform.localScale.x) + offset) * side)), transform.rotation);
                ship.GetComponent<Ship>().Owner = this.Owner;
                ship.GetComponent<Ship>().Target = target;
                ship.GetComponent<SpriteRenderer>().color = this.Owner.MainColor;
                ship.SetActive(true);
                Score--;
                yield return null;
            }
            _spawnInProgress = false;
        }

        /// <summary>
        /// Produces ships when a planet has an owner.
        /// </summary>
        /// <param name="delay">Delay</param>
        public IEnumerator GenerateShips(float delay)
        {
            while (true)
            {
                if (Owner != null)
                {
                    Score += ShipsCreationCount;
                }
                yield return new WaitForSeconds(delay);
            }
        }

        public void SetRandomSprite()
        {
            if (Sprites.Count == 0) { return; }

            GetComponent<SpriteRenderer>().sprite = Sprites[UnityEngine.Random.Range(0, Sprites.Count)];
        }

        private void OnMouseEnter()
        {
            if (!_selected)
            {
                GetComponent<SpriteRenderer>().color = Color.Lerp(Game.Player.HighlightingColor, GetComponent<SpriteRenderer>().color, 0.5f);
            }
        }

        private void OnMouseExit()
        {
            if (!_selected)
            {
                if (Owner == null)
                {
                    GetComponent<SpriteRenderer>().color = _defaultColor;
                }
                else
                {
                    GetComponent<SpriteRenderer>().color = Owner.MainColor;
                }
                
            }
        }

        private void OnMouseUp()
        {
            if ((Owner == null) || (Owner != Game.Player))
            {
                while (Game.Player.SelectedPlanets.Count > 0)
                {
                    Game.Player.SelectedPlanets[0].StartCoroutine(Game.Player.SelectedPlanets[0].SpawnShips(this));
                    Game.Player.SelectedPlanets[0].SetSelection(false);
                }
                return;
            }

            if (Owner != null)
            {
                SetSelection(!_selected);
            }

        }

        

    }
}
