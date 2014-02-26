#region Copyright

/*
 * Copyright (c) 2013-2014 Tobias Schulz, Maximilian Reuter, Pascal Knodel,
 *                         Gerd Augsburg, Christina Erler, Daniel Warzel
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Knot3.Framework.Core;
using Knot3.Framework.Input;
using Knot3.Framework.Platform;
using Knot3.Framework.Utilities;

using Knot3.Game.Core;
using Knot3.Game.Effects;
using Knot3.Game.Input;
using Knot3.Game.Models;
using Knot3.Game.Screens;
using Knot3.Game.Widgets;

namespace Knot3.Game.Data
{
    /// <summary>
    /// Ein Exportformat für 3D-Drucker.
    /// </summary>
    [ExcludeFromCodeCoverageAttribute]
    public class PrinterIO : IKnotIO
    {

        /// <summary>
        /// Die gültigen Dateiendungen für das 3D-Drucker-Format.
        /// </summary>
        public IEnumerable<string> FileExtensions { get; set; }



        /// <summary>
        /// Erstellt ein neues PrinterIO-Objekt.
        /// </summary>
        public PrinterIO ()
        {
            throw new System.NotImplementedException ();
        }



        /// <summary>
        /// Exportiert den Knoten in einem gültigen 3D-Drucker-Format.
        /// </summary>
        public virtual void Save (Knot knot)
        {
            throw new System.NotImplementedException ();
        }

        /// <summary>
        /// Gibt eine IOException zurück.
        /// </summary>
        public virtual Knot Load (string filename)
        {
            throw new System.NotImplementedException ();
        }

        /// <summary>
        /// Gibt eine IOException zurück.
        /// </summary>
        public virtual KnotMetaData LoadMetaData (string filename)
        {
            throw new System.NotImplementedException ();
        }

    }
}
