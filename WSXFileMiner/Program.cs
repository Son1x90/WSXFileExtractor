using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

namespace WSXFileMiner
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Log("Welcome to the WSX File Miner/Extractor v" + Settings.SSettings.GetVersion());
            Logger.Log("Drag and drop the file(s) onto the exe or a single file into the console");
            Logger.Log("--------------------------------------------------------------");
            Logger.Log(DateTime.Now.ToString());

            Settings.GetOrCreateSettings(); //Init Settings

            for (int i = 0; i < args.Length; i++)
            {
                FileManager.ProcessFile(args[i], InfoDisplayMode.PackageDataAndFileData, true);
            }
            
            string input = null;
            while(true)
            {
                Logger.Log("Provide the File Path or type exit/quit to exit");
                input = Console.ReadLine();

                if (input == "exit" || input == "quit")
                {
                    Logger.Log("Have a nice day!");
                    break;
                }

                FileManager.ProcessFile(input, InfoDisplayMode.PackageDataAndFileData, true);
            }

        }
    }
}