using System;
using System.IO;

namespace fcb
{
    class Program
    {
        public static Coder coder = new Coder("mmd", 1);
        public static FileCoder filecoder = new FileCoder();

        public static string StartUp = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string StartUpFile = System.Reflection.Assembly.GetExecutingAssembly().Location;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(StartUp);
            while (true)
            {
                Console.WriteLine("Зашифровать всё начиная с этой папки: 1");
                Console.WriteLine("Расшифровать всё начиная с этой папки: 2");
                string command = Console.ReadLine();
                Console.Clear();

                if (command.Trim() == "1")
                {
                    CoderAllFileInAllFolders(StartUp, CoderStatus.EncryptFile);

                }
                else if (command.Trim() == "2")
                {
                    CoderAllFileInAllFolders(StartUp, CoderStatus.DecryptFile);
                }
            }
        }

        enum CoderStatus
        {
            EncryptFile = 0,
            DecryptFile = 1
        }

        static void CoderAllFileInAllFolders(string path, CoderStatus CoderFile)
        {
            string[] folders = Directory.GetDirectories(path);

            for(int i = 0; i < folders.Length; i++)
            {
                Console.WriteLine(folders[i]);
                CoderAllFileInAllFolders(folders[i], CoderFile);
            }

            string[] files = Directory.GetFiles(path);

            string[] donotcoder = {
                "fcb.deps.json",
                "fcb.dll",
                "fcb.exe",
                "fcb.pdb",
                "fcb.runtimeconfig.dev.json",
                "fcb.runtimeconfig.json",
            };

            foreach(string file in files)
            {
                if (!Array.Exists(donotcoder, x => x == Path.GetFileName(file)))
                {
                    if (CoderFile == CoderStatus.EncryptFile)
                        filecoder.EncryptFile(file);
                    else if (CoderFile == CoderStatus.DecryptFile)
                        filecoder.DecryptFile(file);
                }
            }
        }
    }
}
