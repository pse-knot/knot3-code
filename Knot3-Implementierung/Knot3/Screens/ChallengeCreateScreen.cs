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
using Knot3.Utilities;

namespace Knot3.Screens
{
	/// <summary>
	///
	/// </summary>
	public class ChallengeCreateScreen : MenuScreen
	{
		#region Properties

		/// <summary>
		/// Das Menü, das die Spielstände enthält, die als Startknoten ausgewählt werden.
		/// </summary>
		private VerticalMenu startKnotMenu;
		/// <summary>
		/// Das Menü, das die Spielstände enthält, die als Zielknoten ausgewählt werden.
		/// </summary>
		private VerticalMenu targetKnotMenu;
		private TextItem title;
		private InputItem challengeName;
		private MenuButton createButton;
		private Border createButtonBorder;

		// Spielstand-Loader
		private SavegameLoader<Knot, KnotMetaData> loader;
		private Knot selectedStartKnot;
		private Knot selectedTargetKnot;
		private Challenge selectedChallenge;

		#endregion

		#region Constructors

		/// <summary>
		/// Erzeugt eine neue Instanz eines ChallengeCreateScreen-Objekts und initialisiert diese mit einem Knot3Game-Objekt game.
		/// </summary>
		public ChallengeCreateScreen (Knot3Game game)
		: base(game)
		{
			startKnotMenu = new VerticalMenu (this, DisplayLayer.Menu);
			startKnotMenu.RelativePosition = () => new Vector2 (0.100f, 0.180f);
			startKnotMenu.RelativeSize = () => new Vector2 (0.375f, 0.620f);
			
			targetKnotMenu = new VerticalMenu (this, DisplayLayer.Menu);
			targetKnotMenu.RelativePosition = () => new Vector2 (0.525f, 0.180f);
			targetKnotMenu.RelativeSize = () => new Vector2 (0.375f, 0.620f);

			challengeName = new InputItem (this, DisplayLayer.MenuItem, "Name:", "");
			challengeName.RelativePosition = () => new Vector2 (0.100f, 0.860f);
			challengeName.RelativeSize = () => new Vector2 (0.575f, 0.040f);
			challengeName.OnValueChanged += () => TryConstructChallenge ();
			challengeName.NameWidth = 0.3f;
			challengeName.ValueWidth = 0.7f;
			
			createButton = new MenuButton (
			    screen: this,
			    drawOrder: DisplayLayer.MenuItem,
			    name: "Create!",
			    onClick: OnCreateChallenge
			);
			createButton.RelativePosition = () => new Vector2 (0.725f, 0.840f);
			createButton.RelativeSize = () => new Vector2 (0.175f, 0.060f);
			createButton.ForegroundColor = () => base.MenuItemForegroundColor (createButton.ItemState);
			createButton.BackgroundColor = () => base.MenuItemBackgroundColor (createButton.ItemState);
			createButtonBorder = new Border (this, DisplayLayer.MenuItem, createButton, 4, 4);

			startKnotMenu.RelativePadding = targetKnotMenu.RelativePadding = () => new Vector2 (0.010f, 0.010f);
			startKnotMenu.ItemAlignX = targetKnotMenu.ItemAlignX = HorizontalAlignment.Left;
			startKnotMenu.ItemAlignY = targetKnotMenu.ItemAlignY = VerticalAlignment.Center;

			lines.AddPoints (0, 50, 30, 970, 970, 50, 1000);

			title = new TextItem (screen: this, drawOrder: DisplayLayer.MenuItem, name: "Create Challenge");
			title.RelativePosition = () => new Vector2 (0.100f, 0.050f);
			title.RelativeSize = () => new Vector2 (0.900f, 0.050f);
			title.ForegroundColor = () => Color.White;
			
			// Erstelle einen Parser für das Dateiformat
			KnotFileIO fileFormat = new KnotFileIO ();
			// Erstelle einen Spielstand-Loader
			loader = new SavegameLoader<Knot, KnotMetaData> (fileFormat, "index-knots");
		}

		#endregion

		#region Methods

		private void UpdateFiles ()
		{
			// Leere das Spielstand-Menü
			startKnotMenu.Clear ();
			targetKnotMenu.Clear ();

			// Suche nach Spielständen
			loader.FindSavegames (AddSavegameToList);
		}

		/// <summary>
		/// Diese Methode wird für jede gefundene Spielstanddatei aufgerufen
		/// </summary>
		private void AddSavegameToList (string filename, KnotMetaData meta)
		{
			// Finde den Namen des Knotens
			string name = meta.Name.Length > 0 ? meta.Name : filename;

			// Erstelle die Lamdafunktionen, die beim Auswählen des Menüeintrags ausgeführt werden
			Action<GameTime> SelectStartKnot = (time) => {
				selectedStartKnot = loader.FileFormat.Load (filename);
				UpdateFiles ();
				TryConstructChallenge ();
			};
			
			// Erstelle die Lamdafunktionen, die beim Auswählen des Menüeintrags ausgeführt werden
			Action<GameTime> SelectTargetKnot = (time) => {
				selectedTargetKnot = loader.FileFormat.Load (filename);
				UpdateFiles ();
				TryConstructChallenge ();
			};

			// Erstelle die Menüeinträge
			MenuButton buttonStart = new MenuButton (
			    screen: this,
			    drawOrder: DisplayLayer.MenuItem,
			    name: name,
			    onClick: SelectStartKnot
			);
			MenuButton buttonTarget = new MenuButton (
			    screen: this,
			    drawOrder: DisplayLayer.MenuItem,
			    name: name,
			    onClick: SelectTargetKnot
			);

			Func<bool> matchesStartKnot = () => selectedStartKnot != null && meta == selectedStartKnot.MetaData;
			buttonStart.ForegroundColor = () => MenuItemForegroundColor (buttonStart.ItemState, matchesStartKnot);
			buttonStart.BackgroundColor = () => MenuItemBackgroundColor (buttonStart.ItemState, matchesStartKnot);

			Func<bool> matchesTargetKnot = () => selectedTargetKnot != null && meta == selectedTargetKnot.MetaData;
			buttonTarget.ForegroundColor = () => MenuItemForegroundColor (buttonTarget.ItemState, matchesTargetKnot);
			buttonTarget.BackgroundColor = () => MenuItemBackgroundColor (buttonTarget.ItemState, matchesTargetKnot);

			startKnotMenu.Add (buttonStart);
			targetKnotMenu.Add (buttonTarget);
		}

		public bool CanCreateChallenge
		{
			get {
				return selectedStartKnot != null && selectedTargetKnot != null &&
					selectedStartKnot.MetaData.Filename != selectedTargetKnot.MetaData.Filename
					&& challengeName.InputText.Length > 0;
			}
		}

		private bool TryConstructChallenge ()
		{
			bool can = createButton.IsEnabled = createButtonBorder.IsEnabled = CanCreateChallenge;

			if (can) {
				ChallengeMetaData challengeMeta = new ChallengeMetaData (
					name: challengeName.InputText,
					start: selectedStartKnot.MetaData,
					target: selectedTargetKnot.MetaData,
					filename: null,
					format: new ChallengeFileIO (),
					highscore: new List<KeyValuePair<string,int>> ()
				);
				selectedChallenge = new Challenge (
					meta: challengeMeta,
					start: selectedStartKnot,
					target: selectedTargetKnot
				);
			}
			else {
				selectedChallenge = null;
			}

			return can;
		}

		private void OnCreateChallenge (GameTime time)
		{
			if (TryConstructChallenge ()) {

			}
		}

		/// <summary>
		///
		/// </summary>
		public override void Entered (GameScreen previousScreen, GameTime time)
		{
			UpdateFiles ();
			base.Entered (previousScreen, time);
			AddGameComponents (time, startKnotMenu, targetKnotMenu, challengeName, createButton,
			                   createButtonBorder, title);
			TryConstructChallenge ();
		}

		protected Color MenuItemForegroundColor (ItemState itemState, Func<bool> matches)
		{
			if (matches ()) {
				return Color.Black;
			}
			else {
				return base.MenuItemForegroundColor (itemState);
			}
		}

		protected Color MenuItemBackgroundColor (ItemState itemState, Func<bool> matches)
		{
			if (matches ()) {
				return Lines.LineColor;
			}
			else {
				return base.MenuItemBackgroundColor (itemState);
			}
		}

		#endregion
	}
}

