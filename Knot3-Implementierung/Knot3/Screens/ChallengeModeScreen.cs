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
		private World ChallengeWorld { get; set; }

		/// <summary>
		/// Die Spielwelt in der die 3D-Modelle des dargestellten Spielerknotens enthalten sind.
		/// </summary>
		private World PlayerWorld { get; set; }

		/// <summary>
		/// Der Controller, der aus dem Referenzknoten die 3D-Modelle erstellt.
		/// </summary>
		private KnotRenderer ChallengeKnotRenderer { get; set; }

		/// <summary>
		/// Der Controller, der aus dem Spielerknoten die 3D-Modelle erstellt.
		/// </summary>
		private KnotRenderer PlayerKnotRenderer { get; set; }

		/// <summary>
		/// Der Inputhandler, der die Kantenverschiebungen des Spielerknotens durchführt.
		/// </summary>
		private EdgeMovement PlayerKnotMovement { get; set; }

		/// <summary>
		/// Der Undo-Stack.
		/// </summary>
		public Stack<Knot> Undo { get; set; }

		/// <summary>
		/// Der Redo-Stack.
		/// </summary>
		public Stack<Knot> Redo { get; set; }

		/// <summary>
		/// Der Referenzknoten.
		/// </summary>
		public Knot ChallengeKnot { get; set; }

		/// <summary>
		/// Der Spielerknoten, der durch die Transformation des Spielers aus dem Ausgangsknoten entsteht.
		/// </summary>
		public Knot PlayerKnot { get; set; }

        #endregion

        #region Constructors

		/// <summary>
		/// Erzeugt eine neue Instanz eines ChallengeModeScreen-Objekts und initialisiert diese mit einem Knot3Game-Objekt, einem Spielerknoten playerKnot und dem Knoten challengeKnot, den der Spieler nachbauen soll.
		/// </summary>
		public ChallengeModeScreen (Knot3Game game, Knot playerKnot, Knot challengeKnot)
			: base(game)
		{
			throw new System.NotImplementedException ();
		}

        #endregion

        #region Methods

		/// <summary>
		/// Wird für jeden Frame aufgerufen.
		/// </summary>
		public override void Update (GameTime time)
		{
			throw new System.NotImplementedException ();
		}

		/// <summary>
		/// Fügt die 3D-Welten und den Inputhandler in die Spielkomponentenliste ein.
		/// </summary>
		public override void Entered (GameScreen previousScreen, GameTime GameTime)
		{
			throw new System.NotImplementedException ();
		}

        #endregion

	}
}

