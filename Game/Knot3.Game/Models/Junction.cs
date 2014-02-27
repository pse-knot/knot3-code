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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.Xna.Framework;

using Knot3.Framework.Math;
using Knot3.Framework.Models;
using Knot3.Framework.Platform;
using Knot3.Framework.Storage;
using Knot3.Framework.Utilities;

using Knot3.Game.Data;

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

namespace Knot3.Game.Models
{
    /// <summary>
    /// Enthält Informationen über ein 3D-Modell, das einen Kantenübergang darstellt.
    /// </summary>
    [ExcludeFromCodeCoverageAttribute]
    public sealed class Junction : GameModelInfo, IJunction
    {
        /// <summary>
        /// Die Kante vor dem Übergang.
        /// </summary>
        public Edge EdgeFrom { get; set; }

        /// <summary>
        /// Die Kante nach dem Übergang.
        /// </summary>
        public Edge EdgeTo { get; set; }

        public Node Node { get; set; }

        private INodeMap NodeMap;

        public int Index { get; private set; }

        public List<IJunction> JunctionsAtNode
        {
            get {
                return (from j in NodeMap.JunctionsAtNode (NodeMap.NodeAfterEdge (EdgeFrom)) orderby j.Index ascending select j).ToList ();
            }
        }

        public int JunctionsAtNodeIndex
        {
            get {
                int i = 0;
                foreach (IJunction junction in JunctionsAtNode) {
                    if (junction == this as IJunction) {
                        break;
                    }
                    ++i;
                }
                return i;
            }
        }

        public List<IJunction> OtherJunctionsAtNode
        {
            get {
                return JunctionsAtNode.Where (x => x != this as IJunction).ToList ();
            }
        }

        public JunctionType Type
        {
            get {
                return EdgeFrom.Direction == EdgeTo.Direction ? JunctionType.Straight : JunctionType.Angled;
            }
        }

        public string NodeConfigKey
        {
            get {
                IEnumerable<string> _directions = JunctionsAtNode.Select (junction => junction.EdgeFrom.Direction + String.Empty + junction.EdgeTo.Direction);
                return "Node" + JunctionsAtNode.Count + ":" + string.Join (",", _directions);
            }
        }

        private static Dictionary<Tuple<Direction, Direction>, JunctionDirection> angledJunctionDirectionMap
            = new Dictionary<Tuple<Direction, Direction>, JunctionDirection> ()
        {
            { Tuple.Create (Direction.Up, Direction.Up), 			JunctionDirection.UpUp },
            { Tuple.Create (Direction.Up, Direction.Left), 			JunctionDirection.UpLeft },
            { Tuple.Create (Direction.Up, Direction.Right), 			JunctionDirection.UpRight },
            { Tuple.Create (Direction.Up, Direction.Forward), 		JunctionDirection.UpForward },
            { Tuple.Create (Direction.Up, Direction.Backward), 		JunctionDirection.UpBackward },

            { Tuple.Create (Direction.Down, Direction.Down), 		JunctionDirection.UpUp },
            { Tuple.Create (Direction.Down, Direction.Left), 		JunctionDirection.DownLeft },
            { Tuple.Create (Direction.Down, Direction.Right), 		JunctionDirection.DownRight },
            { Tuple.Create (Direction.Down, Direction.Forward), 		JunctionDirection.DownForward },
            { Tuple.Create (Direction.Down, Direction.Backward), 	JunctionDirection.DownBackward },

            { Tuple.Create (Direction.Left, Direction.Left), 		JunctionDirection.RightRight },
            { Tuple.Create (Direction.Left, Direction.Up), 			JunctionDirection.DownRight },
            { Tuple.Create (Direction.Left, Direction.Down), 		JunctionDirection.UpRight },
            { Tuple.Create (Direction.Left, Direction.Forward), 		JunctionDirection.LeftForward },
            { Tuple.Create (Direction.Left, Direction.Backward), 	JunctionDirection.LeftBackward },

            { Tuple.Create (Direction.Right, Direction.Right), 		JunctionDirection.RightRight },
            { Tuple.Create (Direction.Right, Direction.Up), 			JunctionDirection.DownLeft },
            { Tuple.Create (Direction.Right, Direction.Down), 		JunctionDirection.UpLeft },
            { Tuple.Create (Direction.Right, Direction.Forward), 	JunctionDirection.RightForward },
            { Tuple.Create (Direction.Right, Direction.Backward), 	JunctionDirection.RightBackward },

            { Tuple.Create (Direction.Forward, Direction.Forward), 	JunctionDirection.BackwardBackward },
            { Tuple.Create (Direction.Forward, Direction.Left), 		JunctionDirection.RightBackward },
            { Tuple.Create (Direction.Forward, Direction.Right), 	JunctionDirection.LeftBackward },
            { Tuple.Create (Direction.Forward, Direction.Up), 		JunctionDirection.DownBackward },
            { Tuple.Create (Direction.Forward, Direction.Down), 		JunctionDirection.UpBackward },

            { Tuple.Create (Direction.Backward, Direction.Backward), JunctionDirection.BackwardBackward },
            { Tuple.Create (Direction.Backward, Direction.Left), 	JunctionDirection.RightForward },
            { Tuple.Create (Direction.Backward, Direction.Right), 	JunctionDirection.LeftForward },
            { Tuple.Create (Direction.Backward, Direction.Up), 		JunctionDirection.DownForward },
            { Tuple.Create (Direction.Backward, Direction.Down), 	JunctionDirection.UpForward },
        };
        private static Dictionary<JunctionDirection, Angles3> angledJunctionRotationMap
            = new Dictionary<JunctionDirection, Angles3> ()
        {
            { JunctionDirection.UpForward, 			Angles3.FromDegrees (0, 0, 0) },
            { JunctionDirection.UpBackward, 		Angles3.FromDegrees (0, 180, 0) },
            { JunctionDirection.UpLeft, 			Angles3.FromDegrees (0, 90, 0) },
            { JunctionDirection.UpRight, 			Angles3.FromDegrees (0, 270, 0) },
            { JunctionDirection.DownForward, 		Angles3.FromDegrees (180, 180, 0) },
            { JunctionDirection.DownBackward, 		Angles3.FromDegrees (180, 0, 0) },
            { JunctionDirection.DownLeft, 			Angles3.FromDegrees (180, 270, 0) },
            { JunctionDirection.DownRight, 			Angles3.FromDegrees (180, 90, 0) },
            { JunctionDirection.RightForward, 		Angles3.FromDegrees (0, 0, 270) },
            { JunctionDirection.RightBackward, 		Angles3.FromDegrees (0, 90, 270) },
            { JunctionDirection.LeftForward, 		Angles3.FromDegrees (0, 270, 270) },
            { JunctionDirection.LeftBackward, 		Angles3.FromDegrees (0, 180, 270) },
            { JunctionDirection.UpUp, 				Angles3.FromDegrees (90, 0, 0) },
            { JunctionDirection.RightRight, 		Angles3.FromDegrees (0, 90, 0) },
            { JunctionDirection.BackwardBackward, 	Angles3.FromDegrees (0, 0, 0) },
        };
        private static Dictionary<Direction, Angles3> straightJunctionRotationMap
            = new Dictionary<Direction, Angles3> ()
        {
            { Direction.Up,			Angles3.FromDegrees (90, 0, 0) },
            { Direction.Down,		Angles3.FromDegrees (270, 0, 0) },
            { Direction.Left,		Angles3.FromDegrees (0, 90, 0) },
            { Direction.Right,		Angles3.FromDegrees (0, 270, 0) },
            { Direction.Forward,	Angles3.FromDegrees (0, 0, 0) },
            { Direction.Backward,	Angles3.FromDegrees (0, 0, 180) },
        };
        private static Dictionary<Tuple<Direction, Direction>, Tuple<float, float>> curvedJunctionBumpRotationMap
            = new Dictionary<Tuple<Direction, Direction>, Tuple<float, float>> ()
        {
            { Tuple.Create (Direction.Up, Direction.Left), 			Tuple.Create (90f, 0f) }, // works
            { Tuple.Create (Direction.Up, Direction.Right), 			Tuple.Create (-90f, 0f) }, // works
            { Tuple.Create (Direction.Up, Direction.Forward), 		Tuple.Create (0f, 180f) }, // works
            { Tuple.Create (Direction.Up, Direction.Backward), 		Tuple.Create (0f, 0f) }, // works

            { Tuple.Create (Direction.Down, Direction.Left), 		Tuple.Create (-90f, 0f) }, // works
            { Tuple.Create (Direction.Down, Direction.Right), 		Tuple.Create (90f, 0f) }, // works
            { Tuple.Create (Direction.Down, Direction.Forward), 		Tuple.Create (0f, 180f) }, // works
            { Tuple.Create (Direction.Down, Direction.Backward), 	Tuple.Create (0f, 0f) }, // works

            { Tuple.Create (Direction.Left, Direction.Up), 			Tuple.Create (0f, 90f) }, // works
            { Tuple.Create (Direction.Left, Direction.Down), 		Tuple.Create (0f, -90f) },
            { Tuple.Create (Direction.Left, Direction.Forward), 		Tuple.Create (-90f, 90f) }, // works
            { Tuple.Create (Direction.Left, Direction.Backward), 	Tuple.Create (-90f, -90f) }, // works

            { Tuple.Create (Direction.Right, Direction.Up), 			Tuple.Create (0f, -90f) }, // works
            { Tuple.Create (Direction.Right, Direction.Down), 		Tuple.Create (0f, 90f) }, // works
            { Tuple.Create (Direction.Right, Direction.Forward), 	Tuple.Create (90f, -90f) }, // works
            { Tuple.Create (Direction.Right, Direction.Backward), 	Tuple.Create (-90f, -90f) }, // works

            { Tuple.Create (Direction.Forward, Direction.Left), 		Tuple.Create (90f, -90f) }, // works
            { Tuple.Create (Direction.Forward, Direction.Right), 	Tuple.Create (90f, -90f) }, // works
            { Tuple.Create (Direction.Forward, Direction.Up), 		Tuple.Create (180f, 0f) }, // works
            { Tuple.Create (Direction.Forward, Direction.Down), 		Tuple.Create (180f, 0f) }, // works

            { Tuple.Create (Direction.Backward, Direction.Left), 	Tuple.Create (90f, 90f) }, // works
            { Tuple.Create (Direction.Backward, Direction.Right), 	Tuple.Create (-90f, -90f) }, // works
            { Tuple.Create (Direction.Backward, Direction.Up), 		Tuple.Create (0f, 0f) }, // works
            { Tuple.Create (Direction.Backward, Direction.Down), 	Tuple.Create (0f, 0f) }, // works
        };

        /// <summary>
        /// Erstellt ein neues Informationsobjekt für ein 3D-Modell, das einen Kantenübergang darstellt.
        /// [base="node1", Angles3.Zero, new Vector3 (1,1,1)]
        /// </summary>
        public Junction (INodeMap nodeMap, Edge from, Edge to, Node node, int index)
        : base ("pipe-straight", Angles3.Zero, Vector3.One * 25f)
        {
            EdgeFrom = from;
            EdgeTo = to;
            Node = node;
            NodeMap = nodeMap;
            Index = index;
            Position = nodeMap.NodeAfterEdge (EdgeFrom);

            // Kanten sind sichtbar, nicht auswählbar und nicht verschiebbar
            IsVisible = true;
            IsSelectable = false;
            IsMovable = false;

            // Wähle das Modell aus
            nodeMap.IndexRebuilt += () => {
                try {
                    chooseModel ();
                }
                catch (Exception ex) {
                    Log.Debug (ex);
                }
            };
        }

        private void chooseModel ()
        {
            if (JunctionsAtNode.Count == 1) {
                chooseModelOneJunction ();
            }
            else if (JunctionsAtNode.Count == 2) {
                chooseModelTwoJunctions ();
            }
            else if (JunctionsAtNode.Count == 3) {
                chooseModelThreeJunctions ();
            }
        }

        private void chooseModelOneJunction ()
        {
            if (Type == JunctionType.Angled) {
                Modelname = Config.Models [NodeConfigKey, "modelname" + JunctionsAtNodeIndex, "pipe-angled"];
                Rotation = angledJunctionRotationMap [angledJunctionDirectionMap [Tuple.Create (EdgeFrom.Direction, EdgeTo.Direction)]];
            }
        }

        private void chooseModelTwoJunctions ()
        {
            if (Type == JunctionType.Angled) {
                Modelname = Config.Models[NodeConfigKey, "modelname" + JunctionsAtNodeIndex, "pipe-angled"];
                Rotation = angledJunctionRotationMap [angledJunctionDirectionMap [Tuple.Create (EdgeFrom.Direction, EdgeTo.Direction)]];
            }
            else if (Type == JunctionType.Straight) {
                if (OtherJunctionsAtNode [0].Type == JunctionType.Straight) {
                    // Drehung des Übergangs
                    Modelname = Config.Models[NodeConfigKey, "modelname" + JunctionsAtNodeIndex, "pipe-curved1"];
                    Rotation = straightJunctionRotationMap [EdgeFrom.Direction];

                    // Drehung der Delle
                    var directionTuple = Tuple.Create (JunctionsAtNode [0].EdgeFrom.Direction, JunctionsAtNode [1].EdgeFrom.Direction);
                    float defaultRotation = curvedJunctionBumpRotationMap [directionTuple].At (JunctionsAtNodeIndex);
                    float bumpRotationZ = Config.Models[NodeConfigKey, "bump" + JunctionsAtNodeIndex, defaultRotation];
                    Rotation += Angles3.FromDegrees (0, 0, bumpRotationZ);

                    // debug
                    /*
                    Log.Debug (
                        "Index="
                        + Index.ToString ()
                        + ", Directions="
                        + directionTuple
                        + ", Rotation="
                        + Rotation
                        + ", bumpRotationZ="
                        + bumpRotationZ.ToString ()
                        + ", ...="
                        + Angles3.FromDegrees (0, 0, bumpRotationZ)
                    );
                    */
                }
                else {
                    Modelname = Config.Models[NodeConfigKey, "modelname" + JunctionsAtNodeIndex, "pipe-straight"];
                    Rotation = straightJunctionRotationMap [EdgeFrom.Direction];
                }
            }
        }

        private void chooseModelThreeJunctions ()
        {
            if (Type == JunctionType.Angled) {
                Modelname = Config.Models[NodeConfigKey, "modelname" + JunctionsAtNodeIndex, "pipe-angled"];
                Rotation = angledJunctionRotationMap [angledJunctionDirectionMap [Tuple.Create (EdgeFrom.Direction, EdgeTo.Direction)]];
            }
            else if (Type == JunctionType.Straight) {
                // Drehung des Übergangs
                Modelname = Config.Models[NodeConfigKey, "modelname" + JunctionsAtNodeIndex, "pipe-curved1"];
                Rotation = Angles3.FromDegrees (0, 0, 0) + straightJunctionRotationMap [EdgeFrom.Direction];

                // Drehung der Delle
                float bumpRotationZ = Config.Models[NodeConfigKey, "bump" + JunctionsAtNodeIndex, 0];
                Rotation += Angles3.FromDegrees (0, 0, bumpRotationZ);
            }
        }

        public override bool Equals (GameObjectInfo other)
        {
            if (other == null) {
                return false;
            }

            if (other is Junction) {
                if (this.EdgeFrom == (other as Junction).EdgeFrom
                        && this.EdgeTo == (other as Junction).EdgeTo
                        && base.Equals (other)) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return base.Equals (other);
            }
        }
    }

    enum JunctionDirection {
        UpForward,
        UpBackward,
        UpLeft,
        UpRight,
        DownForward,
        DownBackward,
        DownLeft,
        DownRight,
        RightForward,
        RightBackward,
        LeftForward,
        LeftBackward,
        UpUp,
        RightRight,
        BackwardBackward,
    }
}
