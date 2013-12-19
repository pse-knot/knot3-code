using System;
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

using Core;
using GameObjects;
using Screens;
using RenderEffects;
using Widgets;

namespace KnotData
{
    /// <summary>
    /// Implementiert das Speicherformat für Challenges.
    /// </summary>
    public class ChallengeFileIO
    {

        #region Constructors

        /// <summary>
        /// Erstellt ein ChallengeFileIO-Objekt.
        /// </summary>
        public  ChallengeFileIO ()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Speichert eine Challenge in dem Dateinamen, der in dem Challenge-Objekt enthalten ist.
        /// </summary>
        public virtual void Save (Challenge challenge)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Lädt eine Challenge aus einer angegebenen Datei.
        /// </summary>
        public virtual Challenge Load (String filename)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Lädt die Metadaten einer Challenge aus einer angegebenen Datei.
        /// </summary>
        public virtual ChallengeMetaData LoadMetaData (String filename)
        {
            throw new System.NotImplementedException();
        }

        #endregion

    }
}

