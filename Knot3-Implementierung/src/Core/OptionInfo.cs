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

using GameObjects;
using Screens;
using RenderEffects;
using KnotData;
using Widgets;

namespace Core
{
    /// <summary>
    /// Enthält Informationen über einen Eintrag in einer Einstellungsdatei.
    /// </summary>
    public class OptionInfo
    {

        #region Properties

        /// <summary>
        /// Die Einstellungsdatei.
        /// </summary>
        private ConfigFile configFile { get; set; }

        /// <summary>
        /// Der Abschnitt der Einstellungsdatei.
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// Der Name der Option.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Der Standardwert der Option.
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Der Wert der Option.
        /// </summary>
        public string Value { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Erstellt ein neues OptionsInfo-Objekt aus den übergegebenen Werten.
        /// </summary>
        public OptionInfo (string section, string name, string defaultValue, ConfigFile configFile)
        {
            throw new System.NotImplementedException();
        }

        #endregion

    }
}

