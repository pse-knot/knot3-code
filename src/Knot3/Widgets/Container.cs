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
using Knot3.Input;
using Knot3.GameObjects;
using Knot3.Screens;
using Knot3.RenderEffects;
using Knot3.Data;

namespace Knot3.Widgets
{
	/// <summary>
	/// Ein Menü enthält Bedienelemente zur Benutzerinteraktion. Diese Klasse bietet Standardwerte für
	/// Positionen, Größen, Farben und Ausrichtungen der Menüeinträge. Sie werden gesetzt, wenn die Werte
	/// der Menüeinträge \glqq null\grqq~sind.
	/// </summary>
	public class Container : Widget, IEnumerable<Widget>
	{
		#region Properties

		/// <summary>
		/// Die vom Zustand des Menüeintrags abhängige Vordergrundfarbe des Menüeintrags.
		/// </summary>
		public Func<State, Color> ItemForegroundColor { get; set; }

		/// <summary>
		/// Die vom Zustand des Menüeintrags abhängige Hintergrundfarbe des Menüeintrags.
		/// </summary>
		public Func<State, Color> ItemBackgroundColor { get; set; }

		/// <summary>
		/// Die horizontale Ausrichtung der Menüeinträge.
		/// </summary>
		public HorizontalAlignment ItemAlignX { get; set; }

		/// <summary>
		/// Die vertikale Ausrichtung der Menüeinträge.
		/// </summary>
		public VerticalAlignment ItemAlignY { get; set; }

		protected List<Widget> items;
		private bool isVisible;

		public override bool IsVisible
		{
			get {
				return isVisible;
			}
			set {
				isVisible = value;
				if (items != null) {
					foreach (Widget item in items) {
						item.IsVisible = value;
					}
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Erzeugt ein neues Menu-Objekt und initialisiert dieses mit dem zugehörigen IGameScreen-Objekt.
		/// Zudem ist die Angabe der Zeichenreihenfolge Pflicht.
		/// </summary>
		public Container (IGameScreen screen, DisplayLayer drawOrder)
		: base (screen, drawOrder)
		{
			items = new List<Widget> ();
			ItemAlignX = HorizontalAlignment.Left;
			ItemAlignY = VerticalAlignment.Center;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Fügt einen Eintrag in das Menü ein. Falls der Menüeintrag \glqq null\grqq~oder leere Werte für
		/// Position, Größe, Farbe oder Ausrichtung hat, werden die Werte mit denen des Menüs überschrieben.
		/// </summary>
		public virtual void Add (MenuItem item)
		{
			item.ItemOrder = items.Count;
			assignMenuItemInformation (item);
			items.Add (item);
			item.Menu = this;
		}

		public void Add (Widget item)
		{
			assignMenuItemInformation (item);
			items.Add (item);
		}

		/// <summary>
		/// Entfernt einen Eintrag aus dem Menü.
		/// </summary>
		public virtual void Delete (Widget item)
		{
			if (items.Contains (item)) {
				items.Remove (item);
				for (int i = 0; i < items.Count; ++i) {
					if (items[i] is MenuItem) {
						(items[i] as MenuItem).ItemOrder = i;
					}
				}
			}
		}

		/// <summary>
		/// Gibt einen Eintrag des Menüs zurück.
		/// </summary>
		public Widget GetItem (int i)
		{
			while (i < 0) {
				i += items.Count;
			}
			return items [i % items.Count];
		}

		public Widget this [int i]
		{
			get {
				return GetItem (i);
			}
		}

		/// <summary>
		/// Gibt die Anzahl der Einträge des Menüs zurück.
		/// </summary>
		public int Size ()
		{
			return Count;
		}

		public int Count { get { return items.Count; } }

		public void Clear ()
		{
			items.Clear ();
		}

		/// <summary>
		/// Gibt einen Enumerator über die Einträge des Menüs zurück.
		/// [returntype=IEnumerator<MenuItem>]
		/// </summary>
		public virtual IEnumerator<Widget> GetEnumerator ()
		{
			return items.GetEnumerator ();
		}

		// Explicit interface implementation for nongeneric interface
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator (); // Just return the generic version
		}

		public override IEnumerable<IGameScreenComponent> SubComponents (GameTime time)
		{
			foreach (DrawableGameScreenComponent component in base.SubComponents (time)) {
				yield return component;
			}
			foreach (DrawableGameScreenComponent item in items) {
				yield return item;
			}
		}

		/// <summary>
		/// Die Reaktion auf eine Bewegung des Mausrads.
		/// </summary>
		public virtual void OnScroll (int scrollValue)
		{
		}

		protected virtual void assignMenuItemInformation (MenuItem item)
		{
			if (ItemForegroundColor != null) {
				item.ForegroundColorFunc = (s) => ItemForegroundColor (s);
			}
			if (ItemBackgroundColor != null) {
				item.BackgroundColorFunc = (s) => ItemBackgroundColor (s);
			}
			assignMenuItemInformation (item as Widget);
		}

		protected virtual void assignMenuItemInformation (Widget item)
		{
			item.AlignX = ItemAlignX;
			item.AlignY = ItemAlignY;
			item.IsVisible = isVisible;
		}

		public void Collapse ()
		{
			foreach (MenuItem item in items) {
				item.Collapse ();
			}
		}

		#endregion
	}
}
