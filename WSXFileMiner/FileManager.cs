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
        public static void ProcessFile(string wsxFile, InfoDisplayMode infoDisplayMode, bool extract)
        {
            if (!File.Exists(wsxFile))
            {
                Logger.Log("Error: File not found");
                return;
            }

            WSXPackage wsxPackage = new WSXPackage(wsxFile);
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
                case InfoDisplayMode.None:
                    break;
                default:
                    break;
            }

            if (extract)
            {
                Logger.Log("Extracting File " + wsxPackage.m_fileName);
                string newDirectory = wsxPackage.m_filePathFolder + "\\" + wsxPackage.m_fileName.Substring(0, wsxPackage.m_fileName.Length - 4);
                Directory.CreateDirectory(newDirectory);

                string fileName;
                for (int i = 0; i < wsxPackage.m_files.Count; i++)
                {
                    if (Settings.GetSettings().UseUnknownFileData1AsName)
                        fileName = "ExtractedFile" + wsxPackage.m_files[i].unknownFileData1; // TODO: Find out the real File name. (probably UnknownFileData is either the ID pointing to a location where the stringName is)
                    else
                        fileName = "ExtractedFile" + (i + 1);

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
