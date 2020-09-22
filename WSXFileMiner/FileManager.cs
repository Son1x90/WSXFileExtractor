using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WSXFileMiner
{
    enum InfoDisplayMode
    {
        None,
        PackageData,
        PackageDataAndFileData
    }


    static class FileManager
    {

        /*
        public static void AnalizeFile(string wsxFile, AnalizeMode analizeMode)
        {
            if (!File.Exists(wsxFile))
            {
                Logger.Log("Error: File not found");
                return;
            }

            Logger.Log("Analizing: " + wsxFile);
            switch (analizeMode)
            {
                case AnalizeMode.Normal:
                    using (BinaryReader binaryReader = new BinaryReader(File.Open(wsxFile, FileMode.Open)))
                    {
                        m_numberOfFiles = binaryReader.ReadInt16();
                    }
                    PrintFileStats();
                    break;
                case AnalizeMode.Deep:
                    WSXPackage wsxPackage = new WSXPackage(wsxFile);
                    wsxPackage.PrintPackageInformation();
                    wsxPackage.PrintFileInformation();
                    break;
                default:
                    break;
            }
        }

        public static void ExtractFiles(string wsxFile)
        {
            if (!File.Exists(wsxFile))
            {
                Logger.Log("Error: File not found");
                return;
            }

            m_fileName = Path.GetFileName(wsxFile);
            m_filePathFolder = Path.GetDirectoryName(wsxFile);
            Logger.Log("Extracting: " + wsxFile);
            WSXPackage wsxPackage = new WSXPackage(wsxFile);

            string fileFolder = m_filePathFolder;
            string newDirectory = fileFolder + "\\" + m_fileName.Substring(0, m_fileName.Length - 4);
            Directory.CreateDirectory(newDirectory);
            for (int i = 0; i < wsxPackage.m_files.Count; i++)
            {
                string fileName = "ExtractedFile" + i;
                using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(newDirectory + "\\" + fileName + wsxPackage.m_files[i].fileExtension, FileMode.Create)))
                {
                    Logger.Log("Extracting File" + i);
                    binaryWriter.Write(wsxPackage.m_files[i].fileData, 0, (int)wsxPackage.m_files[i].fileSize);
                    //binaryWriter.BaseStream.Position = 0;
                }
            }
            Logger.Log("Done!");
        }
         */

        // Done TODO: Integrate ExtractFiles() into AnalizeFile() Passing a 3rd parameter in bool extract to reuse code and not to have things double.
        public static void ProcessFile(string wsxFile, InfoDisplayMode infoDisplayMode, bool extract)
        {
            if (!File.Exists(wsxFile))
            {
                Logger.Log("Error: File not found");
                return;
            }

            WSXPackage wsxPackage = new WSXPackage(wsxFile);

            if (infoDisplayMode != InfoDisplayMode.None)
            {
                Logger.Log("Analizing: " + wsxFile);
                switch (infoDisplayMode)
                {
                    case InfoDisplayMode.PackageData:
                        wsxPackage.PrintPackageInformation();
                        break;
                    case InfoDisplayMode.PackageDataAndFileData:
                        wsxPackage.PrintPackageInformation();
                        wsxPackage.PrintSubFileInformation();
                        break;
                    default:
                        break;
                }
            }

            if (extract)
            {
                Logger.Log("Extracting File " + wsxPackage.m_fileName);
                string newDirectory = wsxPackage.m_filePathFolder + "\\" + wsxPackage.m_fileName.Substring(0, wsxPackage.m_fileName.Length - 4);
                Directory.CreateDirectory(newDirectory);

                string fileName;
                for (int i = 0; i < wsxPackage.m_files.Count; i++)
                {
                    fileName = "ExtractedFile" + (i + 1); // so first file is File1
                    using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(newDirectory + "\\" + fileName + wsxPackage.m_files[i].fileExtension, FileMode.Create)))
                    {
                        Logger.Log("Extracting File" + i);
                        binaryWriter.Write(wsxPackage.m_files[i].fileData, 0, (int)wsxPackage.m_files[i].fileSize);
                    }
                }
            }
            Logger.Log("Done!");
        }
    }
}
