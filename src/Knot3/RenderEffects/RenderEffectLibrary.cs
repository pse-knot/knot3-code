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
using Knot3.KnotData;
using Knot3.Widgets;

namespace Knot3.RenderEffects
{
	public class RenderEffectLibrary
	{
		private static EffectFactory[] EffectLibrary
		= new EffectFactory[] {
			new EffectFactory (
			    name: "default",
			    displayName: "Default",
			    createInstance: (screen) => new StandardEffect (screen)
			),
			new EffectFactory (
			    name: "celshader",
			    displayName: "Cel Shading",
			    createInstance: (screen) => new CelShadingEffect (screen)
			),
			new EffectFactory (
			    name: "opaque",
			    displayName: "opaque",
			    createInstance: (screen) => new OpaqueEffect (screen)
			),
			new EffectFactory (
			    name: "z-nebula",
			    displayName: "Z-Nebula",
			    createInstance: (screen) => new Z_Nebula (screen)
			),
		};

		public static Action<string, GameTime> RenderEffectChanged = (e, t) => {};

		public static IEnumerable<string> Names
		{
			get {
				foreach (EffectFactory factory in EffectLibrary) {
					yield return factory.Name;
				}
			}
		}

		public static string DisplayName (string name)
		{
			return Factory (name).DisplayName;
		}

		public static IRenderEffect CreateEffect (IGameScreen screen, string name)
		{
			return Factory (name).CreateInstance (screen);
		}

		private static EffectFactory Factory (string name)
		{
			foreach (EffectFactory factory in EffectLibrary) {
				if (factory.Name == name) {
					return factory;
				}
			}
			return EffectLibrary [0];
		}

		class EffectFactory
		{
			public string Name { get; private set; }

			public string DisplayName { get; private set; }

			public Func<IGameScreen, IRenderEffect> CreateInstance { get; private set; }

			public EffectFactory (string name, string displayName, Func<IGameScreen, IRenderEffect> createInstance)
			{
				Name = name;
				DisplayName = displayName;
				CreateInstance = createInstance;
			}
		}
	}
}
