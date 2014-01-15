using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using Knot3.Core;
using Knot3.GameObjects;
using Knot3.RenderEffects;
using Knot3.KnotData;
using Knot3.Widgets;
using Knot3.Debug;

namespace Knot3.Screens
{
	/// <summary>
	/// Der Spielzustand, der während dem Spielen einer Challenge aktiv ist und für den Ausgangs- und Referenzknoten je eine 3D-Welt zeichnet.
	/// </summary>
	public class ChallengeModeScreen : GameScreen
	{

        #region Properties

		/// <summary>
		/// Die Spielwelt in der die 3D-Modelle des dargestellten Referenzknotens enthalten sind.
		/// </summary>
		private World ChallengeWorld;

		/// <summary>
		/// Die Spielwelt in der die 3D-Modelle des dargestellten Spielerknotens enthalten sind.
		/// </summary>
		private World PlayerWorld;

		/// <summary>
		/// Der Controller, der aus dem Referenzknoten die 3D-Modelle erstellt.
		/// </summary>
		private KnotRenderer ChallengeKnotRenderer;

		/// <summary>
		/// Der Controller, der aus dem Spielerknoten die 3D-Modelle erstellt.
		/// </summary>
		private KnotRenderer PlayerKnotRenderer;

		/// <summary>
		/// Der Inputhandler, der die Kantenverschiebungen des Spielerknotens durchführt.
		/// </summary>
		private EdgeMovement PlayerEdgeMovement;

		/// <summary>
		/// Der Undo-Stack.
		/// </summary>
		public Stack<Knot> Undo { get; set; }

		/// <summary>
		/// Der Redo-Stack.
		/// </summary>
		public Stack<Knot> Redo { get; set; }

		/// <summary>
		/// Die Challenge.
		/// </summary>
		public Challenge Challenge {
			get {
				return _challenge;
			}
			set {
				_challenge = value;
			}
		}

		private Challenge _challenge;

		/// <summary>
		/// Der Spielerknoten, der durch die Transformation des Spielers aus dem Ausgangsknoten entsteht.
		/// </summary>
		public Knot PlayerKnot {
			get {
				return _playerKnot;
			}
			set {
				_playerKnot = value;
				// Undo- und Redo-Stacks neu erstellen
				Redo = new Stack<Knot> ();
				Undo = new Stack<Knot> ();
				Undo.Push (_playerKnot);
				// den Knoten dem KnotRenderer zuweisen
				PlayerKnotRenderer.Knot = _playerKnot;
				// den Knoten dem Kantenverschieber zuweisen
				PlayerKnotRenderer.Knot = _playerKnot;
				// Event registrieren
				_playerKnot.EdgesChanged += OnEdgesChanged;
				// coloring.Knot = knot;
				knotModified = false;
			}
		}

		private Knot _playerKnot;
		private bool knotModified;
		private KnotInputHandler knotInput;
		private ModelMouseHandler modelMouseHandler;
		private MousePointer pointer;
		private Overlay overlay;
		private DebugBoundings debugBoundings;

        #endregion

        #region Constructors

		/// <summary>
		/// Erzeugt eine neue Instanz eines ChallengeModeScreen-Objekts und initialisiert diese mit einem Knot3Game-Objekt, einem Spielerknoten playerKnot und dem Knoten challengeKnot, den der Spieler nachbauen soll.
		/// </summary>
		public ChallengeModeScreen (Knot3Game game, Challenge challenge)
			: base(game)
		{
			// world
			PlayerWorld = new World (screen: this);
			ChallengeWorld = new World (
				screen: this,
				relativePosition: new Vector2 (0f, 0.6f),
				relativeSize: new Vector2 (0.4f, 0.4f)
			);
			// input
			knotInput = new KnotInputHandler (screen: this, world: PlayerWorld);
			// overlay
			overlay = new Overlay (screen: this, world: PlayerWorld);
			// pointer
			pointer = new MousePointer (screen: this);
			// model mouse handler
			modelMouseHandler = new ModelMouseHandler (screen: this, world: PlayerWorld);

			// knot renderer
			PlayerKnotRenderer = new KnotRenderer (screen: this, position: Vector3.Zero);
			PlayerWorld.Add (PlayerKnotRenderer);
			ChallengeKnotRenderer = new KnotRenderer (screen: this, position: Vector3.Zero);
			ChallengeWorld.Add (ChallengeKnotRenderer);

			// debug displays
			debugBoundings = new DebugBoundings (screen: this, position: Vector3.Zero);

			// edge movements
			PlayerEdgeMovement = new EdgeMovement (screen: this, world: PlayerWorld, position: Vector3.Zero);
			PlayerWorld.Add (PlayerEdgeMovement);

			// assign the specified knot
			PlayerKnot = challenge.Start.Clone () as Knot;
		}

        #endregion

        #region Methods

		private void OnEdgesChanged ()
		{
			knotModified = true;
			Undo.Push (_playerKnot);
			Redo.Clear ();
		}

		private void OnUndo ()
		{
			Knot current = Undo.Pop ();
			Knot previous = Undo.Peek ();
			Redo.Push (current);
			_playerKnot = previous;
		}

		private void OnRedo ()
		{
			Knot next = Redo.Pop ();
			Redo.Push (_playerKnot);
			Undo.Push (_playerKnot);
			_playerKnot = next;
		}

		/// <summary>
		/// Wird für jeden Frame aufgerufen.
		/// </summary>
		public override void Update (GameTime time)
		{
		}

		/// <summary>
		/// Fügt die 3D-Welten und den Inputhandler in die Spielkomponentenliste ein.
		/// </summary>
		public override void Entered (GameScreen previousScreen, GameTime time)
		{
			base.Entered (previousScreen, time);
			AddGameComponents (time, knotInput, overlay, pointer, PlayerWorld, ChallengeWorld, modelMouseHandler);

			// Einstellungen anwenden
			debugBoundings.Info.IsVisible = Options.Default ["debug", "show-boundings", false];
		}

        #endregion

	}
}

