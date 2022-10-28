using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace WSXFileMiner
{
    public static class Settings
    {
        static Settings()
        {
            m_xmlSerializer = new XmlSerializer(typeof(SSettings));
            m_settings = new SSettings();
        }

        public static SSettings GetOrCreateSettings()
        {
            if (!File.Exists(m_fileName))
                CreateSettings();

            return DeserializeSettings();
        }

        public static SSettings GetSettings()
        {
            return m_settings;
        }

        private static SSettings DeserializeSettings()
        {
            m_textReader = new StreamReader(m_fileName);

            try
            {
                m_settings = (SSettings)m_xmlSerializer.Deserialize(m_textReader);
            }
            catch
            {
                Logger.Log("Settings Corrupt, Renaming..");
                m_textReader.Close();
                File.Copy(m_fileName, m_fileName + "_oldCorrupt_" + DateTime.Now.ToFileTime());
                File.Delete(m_fileName);
                return GetOrCreateSettings();
            }

            m_textReader.Close();

            return m_settings;
        }

        private static void CreateSettings()
        {
            m_textWriter = new StreamWriter(m_fileName);
            m_xmlSerializer.Serialize(m_textWriter, m_settings);
            Logger.Log(m_fileName + " Created and autoconfiggured.");
            m_textWriter.Close();
        }

        static private SSettings m_settings;
        static private XmlSerializer m_xmlSerializer;
        static private TextWriter m_textWriter;
        static private TextReader m_textReader;
        static private string m_fileName = "Settings.xml";

        [Serializable]
        public class SSettings
        {
            static public string GetVersion()
            {
                return ProgramVersion;
            }

            public bool AddFileExtension = true;
            public bool UseUnknownFileData1AsName = true;
            private static readonly int ProgramVersionMajor = 1;
            private static readonly int ProgramVersionMinor = 1;
            private static readonly string ProgramVersion = ProgramVersionMajor.ToString() + "." + ProgramVersionMinor.ToString();
        }
    }
}
