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
using Knot3.KnotData;

namespace Knot3.Widgets
{
	public class ErrorDialog : ConfirmDialog
	{
		private TextBox textBox;

		public ErrorDialog (IGameScreen screen, DisplayLayer drawOrder, string message)
		: base (screen: screen, drawOrder: drawOrder, title: "Error")
		{
			textBox = new TextBox (screen: Screen, drawOrder: Index + DisplayLayer.MenuItem, text: message);
			textBox.Bounds = ContentBounds;
		}

		public override IEnumerable<IGameScreenComponent> SubComponents (GameTime time)
		{
			foreach (DrawableGameScreenComponent component in base.SubComponents (time)) {
				yield return component;
			}
			yield return textBox;
		}
	}
}
