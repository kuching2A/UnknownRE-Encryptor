using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Resources;
using System.Security.Cryptography;
using System.Reflection;
using Microsoft.Win32;
using System.Windows;
using System.Drawing;
using System.Media;
using System.Windows.Media;
using System;

namespace UnknownREEncryptor
{
    class Stub
    {
        private static ResourceManager _resourceManager;
        private static bool _loadFromMemory = false;

        // For native assemblies
        private const UInt32 MEM_COMMIT = 0x1000;
        private const UInt32 PAGE_EXECUTE_READWRITE = 0x40;
        private const UInt32 MEM_RELEASE = 0x8000;

        [DllImport("kernel32")]
        private static extern UInt32 VirtualAlloc(UInt32 lpStartAddr, UInt32 size, UInt32 flAllocationType, UInt32 flProtect);
        [DllImport("kernel32")]
        private static extern bool VirtualFree(IntPtr lpAddress, UInt32 dwSize, UInt32 dwFreeType);
        [DllImport("kernel32")]
        private static extern IntPtr CreateThread(
          UInt32 lpThreadAttributes,
          UInt32 dwStackSize,
          UInt32 lpStartAddress,
          IntPtr param,
          UInt32 dwCreationFlags,
          ref UInt32 lpThreadId
        );

        public static RijndaelManaged GetRijndaelManaged(String secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));

            return new RijndaelManaged
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.ISO10126,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }

        public static byte[] Decrypt(byte[] input, string password)
        {
            try
            {
                return GetRijndaelManaged(password).CreateDecryptor().TransformFinalBlock(input, 0, input.Length);
            }
            catch (CryptographicException)
            {
                write("Error: Invalid password specified!");
                write();
                return null;
            }
        }

        public static void write(String message = "")
        {
            Console.WriteLine(message);
        }

        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    _loadFromMemory = args[0] == "m";
                }

                _resourceManager = new ResourceManager("Encrypted", Assembly.GetExecutingAssembly());
                
                // Play the soundFile from memory 
                MediaPlayerFromResource soundPlayer = new MediaPlayerFromResource();
                soundPlayer.PlayBytes((byte[])_resourceManager.GetObject("UnknownREIntro", null), "UnknownREIntro.mp3");

                WriteWelcomeMessage();
            }
            catch (Exception exception)
            {
                write(exception.Message);
                write("This file is invalid!");
                write("Press any key to exit");
                Console.ReadKey();
            }
        }

        [STAThread]
        private static void WriteWelcomeMessage()
        {
            Console.Title = "This file has been protected by UnknownRE Encrypter";
            write("Protected by: ");
            write(@" _    _       _                              _____  ______ ");
            write(@"| |  | |     | |                            |  __ \|  ____|");
            write(@"| |  | |_ __ | | ___ __   _____      ___ __ | |__) | |__   ");
            write(@"| |  | | '_ \| |/ / '_ \ / _ \ \ /\ / / '_ \|  _  /|  __|  ");
            write(@"| |__| | | | |   <| | | | (_) \ V  V /| | | | | \ \| |____ ");
            write(@" \____/|_| |_|_|\_\_| |_|\___/ \_/\_/ |_| |_|_|  \_\______|");
            write("");
            write("");

            while (!successful()) ;
        }

        static bool successful()
        {
            try
            {
                write("Please fill in the password for the protected file:");

                string input = GetConsolePassword();
                string output = "";

                if (!_loadFromMemory)
                {
                    write("Type what you want to name the decrypted file (without extension):");
                    output = Console.ReadLine();
                }

                write("Attempting to decrypt...");

                string uriPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                string path = new Uri(uriPath).LocalPath + @"\" + output + (string)_resourceManager.GetObject("extension", null);

                byte[] decryptedBytes = Decrypt((byte[])_resourceManager.GetObject("file", null), input);

                if (decryptedBytes == null)
                {
                    return false;
                }

                if (!_loadFromMemory)
                {
                    File.WriteAllBytes(path, decryptedBytes);
                }
                else
                {
                    String myOwnLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    ExecuteInMemory.Run(decryptedBytes, myOwnLocation);

                    // Check if the assembly is native or not
                    if (true)
                    {
                        launchNETAssembly(decryptedBytes);
                    }
                    else 
                    {
                        launchNativeAssembly(decryptedBytes);
                    }
                }

                if (!_loadFromMemory)
                {
                    write("Successfully Decrypted!");
                    write("Your decrypted file is located at: " + path);
                    write("");
                    write("Press any key to exit");
                    Console.ReadKey();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        [DllImport("kernel32")] private static extern bool CloseHandle(IntPtr handle);
        [DllImport("kernel32")] private static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
        private static void launchNativeAssembly(byte[] decryptedBytes)
        {
            byte[] nativecode = decryptedBytes;

            UInt32 funcAddr = VirtualAlloc(0, (UInt32)nativecode.Length, MEM_COMMIT, PAGE_EXECUTE_READWRITE);
            Marshal.Copy(nativecode, 0, (IntPtr)(funcAddr), nativecode.Length);
            IntPtr hThread = IntPtr.Zero;
            UInt32 threadId = 0;

            hThread = CreateThread(0, 0, funcAddr, IntPtr.Zero, 0, ref threadId);
            WaitForSingleObject(hThread, 0xFFFFFFFF);

            CloseHandle(hThread);
            VirtualFree((IntPtr)funcAddr, 0, MEM_RELEASE);
        }

        private static void launchNETAssembly(byte[] decryptedBytes)
        {
            // Only works with .NET assemblies
            byte[] bin = decryptedBytes;
            Assembly assembly = Assembly.Load(bin);

            // Search for the entrypoint of the application
            MethodInfo method = assembly.EntryPoint;

            if (method != null)
            {
                // create an instance of the Startup form Main method
                object instance = assembly.CreateInstance(method.Name);

                // invoke the application starting point
                method.Invoke(instance, null);
            }
        }

        // Get the console password
        private static string GetConsolePassword()
        {
            StringBuilder stringBuilder = new StringBuilder();

            while (true)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                
                if (consoleKeyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }

                if (consoleKeyInfo.Key == ConsoleKey.Backspace)
                {
                    if (stringBuilder.Length > 0)
                    {
                        Console.Write("\b\0\b");
                        stringBuilder.Length--;
                    }
                    continue;
                }

                Console.Write('*');
                stringBuilder.Append(consoleKeyInfo.KeyChar);
            }
            return stringBuilder.ToString();
        }
    }

    // Memory Test
    public static unsafe class ExecuteInMemory
    {
        #region WinAPI
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, byte[] lpStartupInfo, int[] lpProcessInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern uint NtUnmapViewOfSection(IntPtr hProcess, IntPtr lpBaseAddress);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtWriteVirtualMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize, IntPtr lpNumberOfBytesWritten);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtGetContextThread(IntPtr hThread, IntPtr lpContext);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetContextThread(IntPtr hThread, IntPtr lpContext);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern uint NtResumeThread(IntPtr hThread, IntPtr SuspendCount);
        #endregion

        #region WinNT Definitions
        private const uint CONTEXT_FULL = 0x10007;
        private const int CREATE_SUSPENDED = 0x4;
        private const int MEM_COMMIT = 0x1000;
        private const int MEM_RESERVE = 0x2000;
        private const int PAGE_EXECUTE_READWRITE = 0x40;
        private const ushort IMAGE_DOS_SIGNATURE = 0x5A4D; // MZ
        private const uint IMAGE_NT_SIGNATURE = 0x00004550; // PE00
        #endregion

        // Run an application in memory
        public static bool Run(byte[] exeBuffer, string hostProcess, string optionalArguments = "")
        {
            var IMAGE_SECTION_HEADER = new byte[0x28]; // pish
            var IMAGE_NT_HEADERS = new byte[0xf8]; // pinh
            var IMAGE_DOS_HEADER = new byte[0x40]; // pidh
            var PROCESS_INFO = new int[0x4]; // pi
            var CONTEXT = new byte[0x2cc]; // ctx

            byte* pish;
            fixed (byte* p = &IMAGE_SECTION_HEADER[0])
                pish = p;

            byte* pinh;
            fixed (byte* p = &IMAGE_NT_HEADERS[0])
                pinh = p;

            byte* pidh;
            fixed (byte* p = &IMAGE_DOS_HEADER[0])
                pidh = p;

            byte* ctx;
            fixed (byte* p = &CONTEXT[0])
                ctx = p;

            // Set the flag.
            *(uint*)(ctx + 0x0 /* ContextFlags */) = CONTEXT_FULL;

            // Get the DOS header of the EXE.
            Buffer.BlockCopy(exeBuffer, 0, IMAGE_DOS_HEADER, 0, IMAGE_DOS_HEADER.Length);

            /* Sanity check:  See if we have MZ header. */
            if (*(ushort*)(pidh + 0x0 /* e_magic */) != IMAGE_DOS_SIGNATURE)
                return false;

            var e_lfanew = *(int*)(pidh + 0x3c);

            // Get the NT header of the EXE.
            Buffer.BlockCopy(exeBuffer, e_lfanew, IMAGE_NT_HEADERS, 0, IMAGE_NT_HEADERS.Length);

            /* Sanity check: See if we have PE00 header. */
            if (*(uint*)(pinh + 0x0 /* Signature */) != IMAGE_NT_SIGNATURE)
                return false;

            // Run with parameters if necessary.
            if (!string.IsNullOrEmpty(optionalArguments))
                hostProcess += " " + optionalArguments;

            if (!CreateProcess(null, hostProcess, IntPtr.Zero, IntPtr.Zero, false, CREATE_SUSPENDED, IntPtr.Zero, null, new byte[0x44], PROCESS_INFO))
                return false;

            var ImageBase = new IntPtr(*(int*)(pinh + 0x34));
            NtUnmapViewOfSection((IntPtr)PROCESS_INFO[0] /* pi.hProcess */, ImageBase);
            if (VirtualAllocEx((IntPtr)PROCESS_INFO[0] /* pi.hProcess */, ImageBase, *(uint*)(pinh + 0x50 /* SizeOfImage */), MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE) == IntPtr.Zero)
                Run(exeBuffer, hostProcess, optionalArguments); // Memory allocation failed; try again (this can happen in low memory situations)

            fixed (byte* p = &exeBuffer[0])
                NtWriteVirtualMemory((IntPtr)PROCESS_INFO[0] /* pi.hProcess */, ImageBase, (IntPtr)p, *(uint*)(pinh + 84 /* SizeOfHeaders */), IntPtr.Zero);

            for (ushort i = 0; i < *(ushort*)(pinh + 0x6 /* NumberOfSections */); i++)
            {
                Buffer.BlockCopy(exeBuffer, e_lfanew + IMAGE_NT_HEADERS.Length + (IMAGE_SECTION_HEADER.Length * i), IMAGE_SECTION_HEADER, 0, IMAGE_SECTION_HEADER.Length);
                fixed (byte* p = &exeBuffer[*(uint*)(pish + 0x14 /* PointerToRawData */)])
                    NtWriteVirtualMemory((IntPtr)PROCESS_INFO[0] /* pi.hProcess */, (IntPtr)((int)ImageBase + *(uint*)(pish + 0xc /* VirtualAddress */)), (IntPtr)p, *(uint*)(pish + 0x10 /* SizeOfRawData */), IntPtr.Zero);
            }

            NtGetContextThread((IntPtr)PROCESS_INFO[1] /* pi.hThread */, (IntPtr)ctx);
            NtWriteVirtualMemory((IntPtr)PROCESS_INFO[0] /* pi.hProcess */, (IntPtr)(*(uint*)(ctx + 0xAC /* ecx */)), ImageBase, 0x4, IntPtr.Zero);
            *(uint*)(ctx + 0xB0 /* eax */) = (uint)ImageBase + *(uint*)(pinh + 0x28 /* AddressOfEntryPoint */);
            NtSetContextThread((IntPtr)PROCESS_INFO[1] /* pi.hThread */, (IntPtr)ctx);
            NtResumeThread((IntPtr)PROCESS_INFO[1] /* pi.hThread */, IntPtr.Zero);

            return true;
        }
    }

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
        /// Plays an audio file that is built into the assembly as a resource.
        /// </summary>
        /// <param name="uri">The complete URI to the file. (Assembly;component/Folder/file.extension)</param>
        /// "/UnknownREEncrypter;component/Resources/soundFile.mp3"
        protected internal virtual void PlayBytes(byte[] bytes, string outputFileName)
        {
            Open(WriteByteArrayToTemp(bytes, outputFileName));
            Play();
        }

        /// <summary>
        /// The MediaPlayer cannot play sound files which are built in as a resource.
        /// This method writes a certain file to the users AppData temp
        /// </summary>
        /// <param name="uri">The complete URI to the file. (Assembly;component/Folder/file.extension)</param>
        /// <returns>A path to the file in the users temp directory.</returns>
        /// "/UnknownREEncrypter;component/Resources/soundFile.mp3"
        protected internal Uri WriteByteArrayToTemp(byte[] bytes, string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(TempDirectory))
                {
                    TempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                    Directory.CreateDirectory(TempDirectory);
                }

                var file = Path.Combine(TempDirectory, fileName);
                File.WriteAllBytes(file, bytes);
                return new Uri(file, UriKind.Absolute);
            }
            catch (Exception) { return null; }
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