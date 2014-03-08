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
using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Knot3.Framework.Core;
using Knot3.Framework.Models;
using Knot3.Framework.Platform;
using Knot3.Framework.Storage;
using Knot3.Framework.Utilities;

using Knot3.Game.Data;

namespace Knot3.Game.Models
{
    /// <summary>
    /// Eine frei in der Spielwelt liegende Textur, die auf ein Rechteck gezeichnet wird.
    /// </summary>
    [ExcludeFromCodeCoverageAttribute]
    public sealed class TexturedRectangle : IGameObject, IDisposable, IEquatable<TexturedRectangle>
    {
        private IScreen Screen;

        GameObjectInfo IGameObject.Info { get { return Info; } }

        public TexturedRectangleInfo Info { get; private set; }

        public World World { get; set; }

        private Vector3 UpperLeft;
        private Vector3 LowerLeft;
        private Vector3 UpperRight;
        private Vector3 LowerRight;
        private Vector3 Normal;
        private VertexPositionNormalTexture[] Vertices;
        private short[] Indexes;
        private BasicEffect basicEffect;
        private Texture2D texture;

        public TexturedRectangle (IScreen screen, TexturedRectangleInfo info)
        {
            Screen = screen;
            Info = info;
            SetPosition (Info.Position);

            basicEffect = new BasicEffect (screen.GraphicsDevice);
            if (info.Texture != null) {
                texture = info.Texture;
            }
            else {
                texture = screen.LoadTexture (info.Texturename);
            }
            if (texture != null) {
                FillVertices ();
            }
        }

        [ExcludeFromCodeCoverageAttribute]
        public void Update (GameTime time)
        {
        }

        [ExcludeFromCodeCoverageAttribute]
        public void Draw (GameTime time)
        {
            if (Info.IsVisible) {
                // Setze den Viewport auf den der aktuellen Spielwelt
                Viewport original = Screen.Viewport;
                Screen.Viewport = World.Viewport;

                //Log.Debug ("basicEffect=", World);
                basicEffect.World = World.Camera.WorldMatrix;
                basicEffect.View = World.Camera.ViewMatrix;
                basicEffect.Projection = World.Camera.ProjectionMatrix;

                basicEffect.AmbientLightColor = new Vector3 (0.8f, 0.8f, 0.8f);
                //effect.LightingEnabled = true;
                basicEffect.TextureEnabled = true;
                basicEffect.VertexColorEnabled = false;
                basicEffect.Texture = texture;

                basicEffect.LightingEnabled = false;
                string effectName = Config.Default ["video", "knot-shader", "default"];
                if (Screen.InputManager.KeyHeldDown (Keys.Tab) || effectName == "celshader") {
                    basicEffect.EnableDefaultLighting ();  // Beleuchtung aktivieren
                }

                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes) {
                    pass.Apply ();

                    Screen.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture> (
                        PrimitiveType.TriangleList, Vertices, 0, Vertices.Length, Indexes, 0, Indexes.Length / 3
                    );
                }

                // Setze den Viewport wieder auf den ganzen Screen
                Screen.Viewport = original;
            }
        }

        private void FillVertices ()
        {
            // Fill in texture coordinates to display full texture
            // on quad
            Vector2 textureUpperLeft = new Vector2 (0.0f, 0.0f);
            Vector2 textureUpperRight = new Vector2 (1.0f, 0.0f);
            Vector2 textureLowerLeft = new Vector2 (0.0f, 1.0f);
            Vector2 textureLowerRight = new Vector2 (1.0f, 1.0f);

            Vertices = new VertexPositionNormalTexture [4];
            // Provide a normal for each vertex
            for (int i = 0; i < Vertices.Length; i++) {
                Vertices [i].Normal = Normal;
            }
            // Set the position and texture coordinate for each
            // vertex
            Vertices [0].Position = LowerLeft;
            Vertices [0].TextureCoordinate = textureLowerLeft;
            Vertices [1].Position = UpperLeft;
            Vertices [1].TextureCoordinate = textureUpperLeft;
            Vertices [2].Position = LowerRight;
            Vertices [2].TextureCoordinate = textureLowerRight;
            Vertices [3].Position = UpperRight;
            Vertices [3].TextureCoordinate = textureUpperRight;

            // Set the index buffer for each vertex, using
            // clockwise winding
            Indexes = new short [12];
            Indexes [0] = 0;
            Indexes [1] = 1;
            Indexes [2] = 2;
            Indexes [3] = 2;
            Indexes [4] = 1;
            Indexes [5] = 3;

            Indexes [6] = 2;
            Indexes [7] = 1;
            Indexes [8] = 0;
            Indexes [9] = 3;
            Indexes [10] = 1;
            Indexes [11] = 2;
        }

        private void SetPosition (Vector3 position)
        {
            Info.Position = position;
            // Calculate the quad corners
            Normal = Vector3.Cross (Info.Left, Info.Up);
            Vector3 uppercenter = (Info.Up * Info.Height / 2) + position;
            UpperLeft = uppercenter + (Info.Left * Info.Width / 2);
            UpperRight = uppercenter - (Info.Left * Info.Width / 2);
            LowerLeft = UpperLeft - (Info.Up * Info.Height);
            LowerRight = UpperRight - (Info.Up * Info.Height);
            FillVertices ();
        }

        private Vector3 Length ()
        {
            return Info.Left * Info.Width + Info.Up * Info.Height;
        }

        public BoundingBox[] Bounds ()
        {
            return new BoundingBox[] {
                LowerLeft.Bounds (UpperRight - LowerLeft), LowerRight.Bounds (UpperLeft - LowerRight),
                UpperRight.Bounds (LowerLeft - UpperRight), UpperLeft.Bounds (LowerRight - UpperLeft)
            };
        }

        public GameObjectDistance Intersects (Ray ray)
        {
            foreach (BoundingBox bounds in Bounds ()) {
                Nullable<float> distance = ray.Intersects (bounds);
                if (distance != null) {
                    GameObjectDistance intersection = new GameObjectDistance () {
                        Object=this, Distance=distance.Value
                    };
                    return intersection;
                }
            }
            return null;
        }

        public Vector3 Center ()
        {
            return LowerLeft + (UpperRight - LowerLeft) / 2;
        }

        [ExcludeFromCodeCoverageAttribute]
        public override string ToString ()
        {
            return "TexturedRectangle (" + UpperLeft + "," + UpperRight + "," + LowerRight + "," + LowerLeft + ")";
        }

        public void Dispose ()
        {
            if (texture != null) {
                texture.Dispose ();
                texture = null;
            }
        }

        public static bool operator == (TexturedRectangle a, TexturedRectangle b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals (a, b)) {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null)) {
                return false;
            }

            // Return true if the fields match:
            return a.Info.Position == b.Info.Position;
        }

        public static bool operator != (TexturedRectangle a, TexturedRectangle b)
        {
            return !(a == b);
        }

        public bool Equals (TexturedRectangle other)
        {
            return this.Info.Position == other.Info.Position;
        }

        public override bool Equals (object obj)
        {
            if (obj is Node) {
                return Equals ((Node)obj);
            }
            else {
                return false;
            }
        }

        [ExcludeFromCodeCoverageAttribute]
        public override int GetHashCode ()
        {
            return Info.Position.GetHashCode ();
        }
    }
}
