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

using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Knot3.Framework.Platform;

namespace Knot3.Framework.Audio
{
    /// <summary>
    /// Ein Wrapper um die SoundEffect-Klasse des XNA-Frameworks.
    /// </summary>
    [ExcludeFromCodeCoverageAttribute]
    public class SoundEffectFile : IAudioFile
    {
        /// <summary>
        /// Der Anzeigename des SoundEffects.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gibt an, ob die Wiedergabe läuft oder gestoppt bzw. pausiert ist.
        /// </summary>
        public SoundState State { get { return Instance.State; } }

        public SoundEffect SoundEffect { get; private set; }

        private SoundEffectInstance Instance;

        private Sound SoundType;
        private float volume;

        /// <summary>
        /// Erstellt eine neue SoundEffect-Datei mit dem angegebenen Anzeigenamen und des angegebenen SoundEffect-Objekts.
        /// </summary>
        public SoundEffectFile (string name, SoundEffect soundEffect, Sound soundType)
        {
            Name = name;
            SoundEffect = soundEffect;
            Instance = soundEffect.CreateInstance ();
            SoundType = soundType;
        }
        
        public void Play ()
        {
            Log.Debug ("Play: ", Name);
            Instance.Volume = volume = AudioManager.Volume (SoundType);
            Instance.Play ();
        }
        
        public void Stop ()
        {
            Log.Debug ("Stop: ", Name);
            Instance.Stop ();
        }

        [ExcludeFromCodeCoverageAttribute]
        public void Update (GameTime time)
        {
            if (volume != AudioManager.Volume (SoundType)) {
                Instance.Volume = volume = AudioManager.Volume (SoundType);
            }
        }
    }
}
