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
using KnotData;

namespace Widgets
{
    /// <summary>
    /// Ein Dialog, der eine Texteingabe des Spielers entgegennimmt.
    /// </summary>
    public class TextInputDialog : ConfirmDialog
    {

        #region Properties

        /// <summary>
        /// Der Text, der durch den Spieler eingegeben wurde.
        /// </summary>
        public String InputText { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Erzeugt eine neue Instanz eines TextInputDialog-Objekts und ordnet dieser einen GameScreen zu.
        /// Zudem ist die Angabe der Zeichenreihenfolge, einer Zeichenkette für den Titel, einer Zeichenfolge
        /// für den eingeblendeten Text und eine Zeichenkette für voreingestellten Text (welche leer sein darf) Pflicht.
        /// </summary>
        public void TextInputDialog (GameScreen screen, DisplayLayer drawOrder, String title, String text, String inputText)
        {
            throw new System.NotImplementedException();
        }

        #endregion

    }
}

