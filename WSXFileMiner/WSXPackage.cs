using System;
using System.Collections.Generic;
using System.IO;

namespace WSXFileMiner
{
    public class WSXPackage
    {
        public WSXPackage(string filePath)
        {
            m_files = new List<WsxSubFileInfo>();

            using (BinaryReader binaryReader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                Settings.SSettings settings = Settings.GetOrCreateSettings();
                
                m_fileName = Path.GetFileName(filePath);
                m_filePathFolder = Path.GetDirectoryName(filePath);

                m_filesInsidePackage = binaryReader.ReadInt16();
                m_packageFileSize = binaryReader.ReadInt32();

                for (int i = 0; i < m_filesInsidePackage; i++)
                {
                    WsxSubFileInfo wsxSubFileInfo; // = new FileInfo();
                    wsxSubFileInfo.unknownFileData1 = binaryReader.ReadUInt32();
                    wsxSubFileInfo.fileAdressAfterPackageInfo = binaryReader.ReadUInt32();
                    wsxSubFileInfo.fileSize = binaryReader.ReadUInt32();
                    wsxSubFileInfo.fileData = new byte[wsxSubFileInfo.fileSize];

                    if (settings.AddFileExtension)
                        wsxSubFileInfo.fileExtension = FileExtensionInspector.GetExtension(binaryReader.BaseStream, 6 /* 2bytes m_filesInsidePackage + 4 unknown */ + (m_filesInsidePackage * 12 /*one file info */) + wsxSubFileInfo.fileAdressAfterPackageInfo);
                    else
                        wsxSubFileInfo.fileExtension = "";

                    m_files.Add(wsxSubFileInfo);
                }

                m_dataStart = binaryReader.BaseStream.Position;
                m_packageInfoSize = m_dataStart;

                //Add Data to file
                for (int i = 0; i < m_filesInsidePackage; i++)
                {
                    WsxSubFileInfo wsxSubFileInfo = m_files[i];
                    binaryReader.BaseStream.Position = m_dataStart + wsxSubFileInfo.fileAdressAfterPackageInfo;
                    binaryReader.BaseStream.Read(wsxSubFileInfo.fileData, 0, (int)wsxSubFileInfo.fileSize); // ReadBlock(fileInfo.fileData);
                }
            }
        }



        public void PrintSubFileInformation()
        {
            for (int i = 0; i < m_files.Count; i++)
            {
                PrintSubFileInformation(m_files[i]);
            }
        }

        public void PrintSubFileInformation(WsxSubFileInfo wsxSubFileInfo)
        {
            string UnknownFileData1Binary = Convert.ToString(wsxSubFileInfo.unknownFileData1, 2);
            string UnknownFileData1Hex = Convert.ToString(wsxSubFileInfo.unknownFileData1, 16);
            Logger.Log("UnknownFileData1: " + wsxSubFileInfo.unknownFileData1);
            Logger.Log("UnknownFileData1Bin: " + UnknownFileData1Hex);
            Logger.Log("UnknownFileData1Hex: " + UnknownFileData1Binary);
            Logger.Log("FileAddressOffsetAfterPackageData: " + wsxSubFileInfo.fileAdressAfterPackageInfo);
            Logger.Log("FileSize: " + wsxSubFileInfo.fileSize);
            Logger.Log("FileExtension: " + wsxSubFileInfo.fileExtension);
        }

        public void PrintPackageInformation()
        {
            Logger.Log("Files Inside Package: " + m_filesInsidePackage);
            Logger.Log("Package File Size (without package info Size): " + m_packageFileSize + " bytes");
            Logger.Log("Data Start: " + m_dataStart);
            Logger.Log("PackageInfoSize: " + m_packageInfoSize);
        }

        public string m_fileName;
        public string m_filePathFolder;

        //File Metadata Start
        public short m_filesInsidePackage; 
        public int m_packageFileSize;
        //File Metadata End

        public List<WsxSubFileInfo> m_files;
        public long m_dataStart;
        public long m_packageInfoSize;
    }

    public struct WsxSubFileInfo
    {
        public UInt32 unknownFileData1;
        public UInt32 fileAdressAfterPackageInfo; // first address always 00 00 00 00
        public UInt32 fileSize;
        public byte[] fileData;
        public string fileExtension;
    }
}