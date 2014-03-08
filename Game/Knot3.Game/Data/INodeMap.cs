/*
 * Copyright (c) 2013-2014 Tobias Schulz, Maximilian Reuter, Pascal Knodel,
 *                         Gerd Augsburg, Christina Erler, Daniel Warzel
 *
 * This source code file is part of Knot3. Copying, redistribution and
 * use of the source code in this file in source and binary forms,
 * with or without modification, are permitted provided that the conditions
 * of the MIT license are met:
 *
 *   Permission is hereby granted, free of charge, to any person obtaining a copy
 *   of this software and associated documentation files (the "Software"), to deal
 *   in the Software without restriction, including without limitation the rights
 *   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *   copies of the Software, and to permit persons to whom the Software is
 *   furnished to do so, subject to the following conditions:
 *
 *   The above copyright notice and this permission notice shall be included in all
 *   copies or substantial portions of the Software.
 *
 *   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *   SOFTWARE.
 *
 * See the LICENSE file for full license details of the Knot3 project.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;

namespace Knot3.Game.Data
{
    public interface INodeMap
    {
        /// <summary>
        /// Die Skalierung, die bei einer Konvertierung in einen Vector3 des XNA-Frameworks durch die ToVector ()-Methode der Node-Objekte verwendet wird.
        /// </summary>
        int Scale { get; set; }

        IEnumerable<Edge> Edges { get; set; }

        Vector3 Offset { get; set; }

        Action IndexRebuilt { get; set; }

        /// <summary>
        /// Gibt die Rasterposition des Übergangs am Anfang der Kante zurück.
        /// </summary>
        Node NodeBeforeEdge (Edge edge);

        /// <summary>
        /// Gibt die Rasterposition des Übergangs am Ende der Kante zurück.
        /// </summary>
        Node NodeAfterEdge (Edge edge);

        List<IJunction> JunctionsAtNode (Node node);

        List<IJunction> JunctionsBeforeEdge (Edge edge);

        List<IJunction> JunctionsAfterEdge (Edge edge);

        IEnumerable<Node> Nodes { get; }

        /// <summary>
        /// Aktualisiert die Zuordnung, wenn sich die Kanten geändert haben.
        /// </summary>
        void OnEdgesChanged ();
    }
}
