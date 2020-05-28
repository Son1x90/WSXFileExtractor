using System;
using System.IO;
using System.Runtime.InteropServices;

namespace WSXFileMiner
{

    public class Logger
    {
        public static void Log(string str)
        {
            using (StreamWriter streamWriter = new StreamWriter(Environment.CurrentDirectory + "Log.txt", true))
            {
                Console.WriteLine(str);
                streamWriter.WriteLine(str);
            }

        }
    }


    class Program
    {

        static void Main(string[] args)
        {
            //Import File Recognition

            Logger.Log("Welcome to the WSX File Miner");
            Logger.Log("Drag and drop the file(s) onto the exe.");
            Logger.Log("--------------------------------------------------------------");
            Logger.Log(DateTime.Now.ToString());

            if (!DEBUGMODE)
            {
                if (args.Length > 0)
                {
                    FileManager fileManager = new FileManager();

                    for (int i = 0; i < args.Length; i++)
                    {
                        fileManager.AnalizeFile(args[i], AnalizeMode.Deep);
                        //fileManager.ExtractFiles(args[i]);
                    }
                }
                Logger.Log("Done! Press Enter to exit...");
                Console.ReadLine();
            }
            else
            {
                //D:\Games\LandsOfLoreDataMining\LOL3DVD\MUSIC1.WSX
                string filePath;
                filePath = Console.ReadLine();
                FileManager fileManager = new FileManager();
                fileManager.AnalizeFile(filePath, AnalizeMode.Deep);
                //fileManager.ExtractFiles(filePath);
            }
        }
        static bool DEBUGMODE = true;
    }
}