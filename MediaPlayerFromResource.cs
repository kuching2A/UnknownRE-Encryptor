using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.IO;
using System.Windows;
using System.Threading;
using System.ComponentModel;
using System.Windows.Resources;

namespace UnknownREEncrypter
{
    public class MediaPlayerFromResource : MediaPlayer, IDisposable
    {
        protected internal string TempDirectory;
        public bool IsRepeating;

        public MediaPlayerFromResource()
        {
            MediaEnded += SoundPlayer_MediaEnded;
        }

        public void Dispose()
        {
            if (TempDirectory != null)
            {
                try
                {
                    Close();

                    //Delete all files first
                    foreach (var path in Directory.GetFiles(TempDirectory))
                    {
                        File.Delete(path);
                    }
                    Directory.Delete(TempDirectory, true);
                }
                catch (Exception)
                {
                    // do nothing
                }
            }
        }

        /// <summary>
        /// Plays an audio file that is built into the assembly as a resource.
        /// </summary>
        /// <param name="uri">The complete URI to the file. (Assembly;component/Folder/file.extension)</param>
        /// "/UnknownREEncrypter;component/Resources/soundFile.mp3"
        protected internal virtual void PlayResourceFile(Uri uri)
        {
            Open(WriteResourceFileToTemp(uri));
            Play();
        }

        /// <summary>
        /// The MediaPlayer cannot play sound files which are built in as a resource.
        /// This method writes a certain file to the users AppData temp
        /// </summary>
        /// <param name="uri">The complete URI to the file. (Assembly;component/Folder/file.extension)</param>
        /// <returns>A path to the file in the users temp directory.</returns>
        /// "/UnknownREEncrypter;component/Resources/soundFile.mp3"
        protected internal Uri WriteResourceFileToTemp(Uri uri)
        {
            try
            {
                if (string.IsNullOrEmpty(TempDirectory))
                {
                    TempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                    Directory.CreateDirectory(TempDirectory);
                }

                var file = Path.Combine(TempDirectory, Path.GetFileName(uri.ToString()));

                if (!File.Exists(file))
                {
                    var streamResourceInfo = Application.GetResourceStream(uri);

                    if (streamResourceInfo != null)
                    {
                        var stream = streamResourceInfo.Stream;
                        var fileStream = File.Create(file);
                        stream.CopyTo(fileStream);
                        fileStream.Close();
                    }
                }
                return new Uri(file, UriKind.Absolute);
            }
            catch (Exception) { return null; }
        }

        protected internal void WriteStreamToFile(Stream stream, string file)
        {
            var length = stream.Length;
            var data = new byte[length];
            stream.Read(data, 0, (int)length);

            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                fileStream.Write(data, 0, (int)length);
                fileStream.Flush();
                fileStream.Close();
            }
        }

        /// <summary>
        /// Occurs when the Media has been played to the end. Depending on IsRepeating, the player will start playing the file again.
        /// </summary>
        private void SoundPlayer_MediaEnded(object sender, EventArgs e)
        {
            if (IsRepeating)
            {
                Position = new TimeSpan(0);
            }
        }
    }
}
