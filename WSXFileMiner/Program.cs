using System;
using System.IO;
using System.Numerics;
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
            Logger.Log("Welcome to the WSX File Miner/Extractor");
            Logger.Log("Drag and drop the file(s) onto the exe.");
            Logger.Log("--------------------------------------------------------------");
            Logger.Log(DateTime.Now.ToString());


            if (args.Length > 0)
            {
                //FileManager fileManager = new FileManager();

                for (int i = 0; i < args.Length; i++)
                {
                    FileManager.AnalizeFile(args[i], AnalizeMode.Deep);
                    FileManager.ExtractFiles(args[i]);
                }
            }

            string filePath = null;
            do
            {
                Logger.Log("Provide the File Path or type exit/quit to exit");
                //D:\Games\LandsOfLoreDataMining\LOL3DVD\MUSIC1.WSX
                filePath = Console.ReadLine();
                //FileManager fileManager = new FileManager();
                FileManager.AnalizeFile(filePath, AnalizeMode.Deep);
                FileManager.ExtractFiles(filePath);
            } while (filePath != "exit" && filePath != "quit");
        }
    }
}