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

#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
using Knot3.Data;
using Knot3.Widgets;
using Knot3.Utilities;
using Knot3.Development;
using Knot3.Platform;

using OggSharp;

#endregion

namespace Knot3.Audio
{
	public class OggVorbisFile : IAudioFile
	{
		public string Name { get; private set; }

		public SoundState State { get { return internalFile.State; } }

		private SoundEffectFile internalFile;

		public OggVorbisFile (string name, string filepath, Sound soundType)
		{
			Name = name;
			string cachefile = SystemInfo.DecodedMusicCache
			                   + SystemInfo.Separator.ToString ()
			                   + soundType.ToString ()
			                   + "_"
			                   + name.GetHashCode ().ToString ()
			                   + ".wav";

			byte[] data;
			try {
				Log.Debug ("Read from cache: ", cachefile);
				data = File.ReadAllBytes (cachefile);
			}
			catch (Exception) {
				Log.Debug ("Decode: ", name);
				OggDecoder decoder = new OggDecoder ();
				decoder.Initialize (TitleContainer.OpenStream (filepath));
				data = decoder.SelectMany (chunk => chunk.Bytes.Take (chunk.Length)).ToArray ();
				using (MemoryStream stream = new MemoryStream ())
				using (BinaryWriter writer = new BinaryWriter (stream)) {
					WriteWave (writer, decoder.Stereo ? 2 : 1, decoder.SampleRate, data);
					stream.Position = 0;
					data = stream.ToArray ();
				}
				File.WriteAllBytes (cachefile, data);
			}

			using (MemoryStream stream = new MemoryStream (data)) {
				stream.Position = 0;
				SoundEffect soundEffect = SoundEffect.FromStream (stream);
				internalFile = new SoundEffectFile (name, soundEffect, soundType);
			}
		}

		public void Play ()
		{
			internalFile.Play ();
		}

		public void Stop ()
		{
			internalFile.Stop ();
		}

		public void Update (GameTime time)
		{
			internalFile.Update (time);
		}

		private static void WriteWave (BinaryWriter writer, int channels, int rate, byte[] data)
		{
			writer.Write (new char[4] { 'R', 'I', 'F', 'F' });
			writer.Write ((int)(36 + data.Length));
			writer.Write (new char[4] { 'W', 'A', 'V', 'E' });

			writer.Write (new char[4] { 'f', 'm', 't', ' ' });
			writer.Write ((int)16);
			writer.Write ((short)1);
			writer.Write ((short)channels);
			writer.Write ((int)rate);
			writer.Write ((int)(rate * ((16 * channels) / 8)));
			writer.Write ((short)((16 * channels) / 8));
			writer.Write ((short)16);

			writer.Write (new char[4] { 'd', 'a', 't', 'a' });
			writer.Write ((int)data.Length);
			writer.Write (data);
		}
	}
}
