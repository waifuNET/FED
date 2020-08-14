using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace fcb
{
    class Coder
    {
        static int _key;
        static int _complexity;

        public string alph = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        List<char> alph_list = new List<char>();
        List<string> DoubleCode = new List<string>();

        Random rand = new Random();

        public Coder(string key, int complexity)
        {
            _key = SetKey(key);
            _complexity = complexity;

            rand = new Random(_key);
            foreach (char ch in alph)
            {
                alph_list.Add(ch);
            }

            string done = "";
            for (int j = 0; j < alph_list.Count; j++)
            {
                do
                {
                    done = "";
                    for (int i = 0; i < _complexity; i++)
                    {
                        done += alph_list[rand.Next(0, alph_list.Count)];
                    }
                } while (DoubleCode.Exists(x => x == done));
                DoubleCode.Add(done);
                done = "";
            }
        }

        public string Encrypt(string text)
        {
            rand = new Random(_key);

            string CriptText = "";
            for (int j = 0; j < text.Length; j++)
            {
                for (int i = 0; i < alph_list.Count; i++)
                {
                    if (text[j] == alph_list[i])
                        CriptText += DoubleCode[i];
                }
            }
            return CriptText;
        }
        public string Decrypt(string text)
        {
            rand = new Random(_key);

            string buffer = "";
            List<string> CriptCode = new List<string>();
            foreach (char i in text)
            {
                buffer = buffer += i;
                if (buffer.Length >= _complexity)
                {
                    CriptCode.Add(buffer);
                    buffer = "";
                }
            }

            string CriptText = "";
            for (int j = 0; j < CriptCode.Count; j++)
            {
                for (int i = 0; i < DoubleCode.Count; i++)
                {
                    if (CriptCode[j] == DoubleCode[i])
                        CriptText += alph_list[i];
                }
            }
            return CriptText;
        }

        public int SetKey(string key)
        {
            int newkey = 0;

            for (int i = 0; i < key.Length; i++)
            {
                newkey += key[i];
            }

            return newkey;
        }
    }
    class FileCoder
    {
        public void EncryptFile(string file_path)
        {
            try
            {
                byte[] byteArray = File.ReadAllBytes(file_path);
                int newline = 70;
                using (StreamWriter file = new StreamWriter(file_path))
                {
                    for (int i = 0; i < byteArray.Length; i++)
                    {
                        file.Write(Program.coder.Encrypt(byteArray[i].ToString()));
                        if (i < byteArray.Length - 1)
                        {
                            file.Write("|");
                            if (i == newline)
                            {
                                file.Write(Environment.NewLine);
                                newline += 70;
                            }
                        }
                    }
                }
                Console.WriteLine(file_path.Remove(0, Program.StartUp.Length - Path.GetFileName(Path.GetDirectoryName(Program.StartUpFile)).Length));
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Encrypt file error: " + file_path.Remove(0, Program.StartUp.Length - Path.GetFileName(Path.GetDirectoryName(Program.StartUpFile)).Length));
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void DecryptFile(string file_path)
        {
            try
            {
                const Int32 BufferSize = 128;
                List<string> arr = new List<string>();
                using (var fileStream = File.OpenRead(file_path))
                {
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        String line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            string[] splitter = line.Split('|');
                            foreach (string str in splitter)
                            {
                                if (str != "")
                                {
                                    arr.Add(Program.coder.Decrypt(str));
                                }
                            }
                        }
                    }
                }

                byte[] byte_arr = new byte[arr.Count];
                for (int i = 0; i < arr.Count; i++)
                {
                    byte_arr[i] = Byte.Parse(arr[i]);
                }

                using (FileStream fs = new FileStream(file_path, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byte_arr, 0, byte_arr.Length);
                    fs.Dispose();
                }
                Console.WriteLine(file_path.Remove(0, Program.StartUp.Length - Path.GetFileName(Path.GetDirectoryName(Program.StartUpFile)).Length));
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Decrypt file error: " + file_path.Remove(0, Program.StartUp.Length - Path.GetFileName(Path.GetDirectoryName(Program.StartUpFile)).Length));
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
