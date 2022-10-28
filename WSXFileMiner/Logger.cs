using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

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

}
