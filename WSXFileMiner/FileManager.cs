﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WSXFileMiner
{
    enum AnalizeMode
    {
        Normal,
        Deep
    }


    class FileManager
    {
        public FileManager()
        {
            streamWriter = File.AppendText("log.txt");
        }

        public void AnalizeFile(string wsxFile, AnalizeMode analizeMode)
        {
            m_filePath = wsxFile;
            m_fileName = Path.GetFileName(wsxFile);
            Logger.Log("Analizing: " + m_filePath);
            switch (analizeMode)
            {
                case AnalizeMode.Normal:
                    using (BinaryReader binaryReader = new BinaryReader(File.Open(m_filePath, FileMode.Open)))
                    {
                        m_numberOfFiles = binaryReader.ReadInt16();
                    }
                    PrintFileStats();
                    break;
                case AnalizeMode.Deep:
                    WSXPackage wsxPackage = new WSXPackage(m_filePath);
                    wsxPackage.PrintPackageInformation();
                    wsxPackage.PrintFileInformation();
                    break;
                default:
                    break;
            }
        }

        public void ExtractFiles(string wsxFile)
        {
            m_filePath = wsxFile;
            m_fileName = Path.GetFileName(wsxFile);
            m_filePathFolder = Path.GetDirectoryName(wsxFile);
            Logger.Log("Extracting: " + m_filePath);
            WSXPackage wsxPackage = new WSXPackage(m_filePath);

            string fileFolder = m_filePathFolder;
            string newDirectory = fileFolder + "\\" + m_fileName.Substring(0, m_fileName.Length - 4);
            Directory.CreateDirectory(newDirectory);
            for (int i = 0; i < wsxPackage.m_files.Count; i++)
            {
                string fileName = "ExtractedFile" + i;
                using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(newDirectory + "\\" + fileName + wsxPackage.m_files[i].fileExtension, FileMode.Create)))
                {
                    Logger.Log("Extracting File" + i);
                    binaryWriter.Write(wsxPackage.m_files[i].fileData,0, (int)wsxPackage.m_files[i].fileSize);
                    //binaryWriter.BaseStream.Position = 0;
                }
            }
            Logger.Log("Done!");
        }

        public void PrintFileStats()
        {
            Logger.Log("FileName: " + m_fileName);
            Logger.Log("Files inside WSX: " + m_numberOfFiles);
        }

        public void Log(string line)
        {
            streamWriter.WriteLine(line);
        }

        // Single File
        string m_filePath;
        string m_fileName;
        string m_filePathFolder;
        StreamWriter streamWriter;
        Int16 m_numberOfFiles;
    }
}