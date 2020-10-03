using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace WSXFileMiner
{

    public class Settings
    {
        static public SSettings GetOrCreateSettings()
        {
            if (m_settings == null)
            {
                m_settings = new SSettings();
            }
            xmlSerializer = new XmlSerializer(typeof(SSettings));
            if (!File.Exists(m_fileName))
            {
                textWriter = new StreamWriter(m_fileName);
                xmlSerializer.Serialize(textWriter, m_settings);
                textWriter.Close();
                Logger.Log(m_fileName + " did not exist. Created and autoconfiggured.");
            }
            textReader = new StreamReader(m_fileName);
            m_settings = (SSettings)xmlSerializer.Deserialize(textReader);
            textReader.Close();
            return m_settings;
        }

        public SSettings GetSettings()
        {
            return m_settings;
        }

        static private SSettings m_settings;
        static private XmlSerializer xmlSerializer;
        static private TextWriter textWriter;
        static private TextReader textReader;
        static private string m_fileName = "Settings.xml";

        [Serializable]
        public class SSettings
        {

            static public string GetVersion()
            {
                return ProgramVersion;
            }

            public bool AddFileExtension = true;
            private static readonly int ProgramVersionMajor = 1;
            private static readonly int ProgramVersionMinor = 1;
            private static readonly string ProgramVersion = ProgramVersionMajor.ToString() + "." + ProgramVersionMinor.ToString();
        }
    }
}
