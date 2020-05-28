using System;
using System.Collections.Generic;
using System.IO;

namespace WSXFileMiner
{
    public class WSXPackage
    {
        public WSXPackage(string filePath)
        {
            m_files = new List<FileInfo>();

            using (BinaryReader binaryReader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                m_filesInsidePackage = binaryReader.ReadInt16();
                m_packageFileSize = binaryReader.ReadInt32();

                for (int i = 0; i < m_filesInsidePackage; i++)
                {
                    FileInfo fileInfo = new FileInfo();
                    fileInfo.unknownFileData1 = binaryReader.ReadUInt32();
                    fileInfo.fileAdressAfterPackageInfo = binaryReader.ReadUInt32();
                    fileInfo.fileSize = binaryReader.ReadUInt32();
                    fileInfo.fileData = new byte[fileInfo.fileSize];
                    fileInfo.fileExtension = "";
                    m_files.Add(fileInfo);
                }
                m_dataStart = binaryReader.BaseStream.Position;
                m_packageInfoSize = m_dataStart; //- 1; 
            }

            //Add Data to file
            for (int i = 0; i < m_filesInsidePackage; i++)
            {
                FileInfo fileInfo = m_files[i];
                //StreamReader streamReader = new StreamReader(filePath);
                BinaryReader binaryReader = new BinaryReader(File.Open(filePath, FileMode.Open));
                binaryReader.BaseStream.Position = m_dataStart + fileInfo.fileAdressAfterPackageInfo;
                binaryReader.BaseStream.Read(fileInfo.fileData, 0, (int)fileInfo.fileSize);// ReadBlock(fileInfo.fileData);
                binaryReader.Close();
            }
        }

        public void PrintFileInformation()
        {
            for (int i = 0; i < m_files.Count; i++)
            {
                PrintFileInformation(m_files[i]);
            }

        }

        public void PrintFileInformation(FileInfo fileInfo)
        {
            string UnknownFileData1Binary = Convert.ToString(fileInfo.unknownFileData1, 2);
            string UnknownFileData1Hex = Convert.ToString(fileInfo.unknownFileData1, 16);
            Logger.Log("UnknownFileData1: " + fileInfo.unknownFileData1);
            Logger.Log("UnknownFileData1Bin: " + UnknownFileData1Hex);
            Logger.Log("UnknownFileData1Hex: " + UnknownFileData1Binary);
            if (UnknownFileData1Binary.Substring(0, 1) == "0")
                Logger.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

            Logger.Log("FileAddressOffsetAfterPackageData: " + fileInfo.fileAdressAfterPackageInfo);
            Logger.Log("FileSize: " + fileInfo.fileSize);
        }

        public void PrintPackageInformation()
        {
            Logger.Log("Files Inside Package: " + m_filesInsidePackage);
            Logger.Log("Package File Size (without package info Size): " + m_packageFileSize + " bytes");
            Logger.Log("Data Start: " + m_dataStart);
            Logger.Log("PackageInfoSize: " + m_packageInfoSize);
        }

        public short m_filesInsidePackage;
        public int m_packageFileSize;
        public List<FileInfo> m_files;
        public long m_dataStart;
        public long m_packageInfoSize;
    }

    public struct FileInfo
    {
        public UInt32 unknownFileData1;
        public UInt32 fileAdressAfterPackageInfo; // first address always 00 00 00 00
        public UInt32 fileSize;
        public byte[] fileData;
        public string fileExtension;
    }
}
