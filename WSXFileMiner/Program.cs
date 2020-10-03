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
            Logger.Log("Welcome to the WSX File Miner/Extractor v" + Settings.SSettings.GetVersion());
            Logger.Log("Drag and drop the file(s) onto the exe or a single file into the console");
            Logger.Log("--------------------------------------------------------------");
            Logger.Log(DateTime.Now.ToString());

            Settings.GetOrCreateSettings(); // Injitializes the Settings


            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    FileManager.ProcessFile(args[i], InfoDisplayMode.PackageDataAndFileData, true);
                }
            }

            string filePath = null;
            do
            {
                Logger.Log("Provide the File Path or type exit/quit to exit");
                filePath = Console.ReadLine();
                FileManager.ProcessFile(filePath, InfoDisplayMode.PackageDataAndFileData, true);
            } while (filePath != "exit" && filePath != "quit");
        }
    }
}