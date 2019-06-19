using System;
using System.IO;
using System.Text;
using System.Xml;

namespace DigitalPlatform.Core
{
    /// <summary>
    /// 用 XML 文件存储配置信息
    /// </summary>
    public class ConfigSetting
    {
        XmlDocument _dom = new XmlDocument();
        string _filename = "";

        bool _changed = false;
        public bool Changed
        {
            get
            {
                return _changed;
            }
            set
            {
                _changed = value;
            }
        }

        public ConfigSetting(string filename, bool auto_create)
        {
#if NO
            try
            {
                _dom.Load(filename);
            }
            catch (FileNotFoundException)
            {
                if (auto_create)
                    _dom.LoadXml("<root />");
                else
                    throw;
            }
#endif
            SafeLoad(_dom,
    filename,
    auto_create);

            _filename = filename;
        }

        public static ConfigSetting Open(string filename, bool auto_create)
        {
            ConfigSetting config = new ConfigSetting(filename, auto_create);
            return config;
        }

        // 写入一个字符串值
        public string Set(string section, string entry, string value)
        {
            XmlElement element = _dom.DocumentElement.SelectSingleNode(section) as XmlElement;
            if (element == null)
            {
                element = _dom.CreateElement(section);
                _dom.DocumentElement.AppendChild(element);
                _changed = true;
            }

            string old_value = element.GetAttribute(entry);
            element.SetAttribute(entry, value);
            if (old_value != value) // 2019/6/19
                _changed = true;
            return old_value;
        }

        // 读取一个字符串值
        public string Get(string section, string entry, string default_value = null)
        {
            XmlElement element = _dom.DocumentElement.SelectSingleNode(section) as XmlElement;
            if (element == null)
                return default_value;

            if (element.GetAttributeNode(entry) == null)
                return default_value;

            return element.GetAttribute(entry);
        }

        // 获得一个整数值
        // return:
        //		所获得的整数值
        public int GetInt(string section,
            string entry,
            int default_value)
        {
            string value = Get(section, entry, default_value.ToString());
            return Convert.ToInt32(value);
        }

        public void SetInt(string section,
    string entry,
    int value)
        {
            Set(section, entry, value.ToString());
        }

        // 2019/6/19
        public bool GetBoolean(string section,
    string entry,
    bool default_value)
        {
            int value = GetInt(section, entry, default_value ? 1 : 0);
            return value == 0 ? false : true;
        }

        // 2019/6/19
        public void SetBoolean(string section,
string entry,
bool value)
        {
            SetInt(section, entry, value ? 1 : 0);
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(_filename) == false)
            {
                // _dom.Save(_filename);
                SafeSave(_dom, _filename);
                _changed = false;
            }
            else
            {
                // TODO: 是否抛出异常?
            }
        }

        public const string BACKUP_EXTENSION = ".~backup";

        public void SafeLoad(XmlDocument dom,
            string filename,
            bool auto_create)
        {
            var directory = Path.GetDirectoryName(filename);
            var backupFileName = Path.Combine(directory, Path.GetFileName(filename) + BACKUP_EXTENSION);

            // 2019/5/15
            FileInfo fi = new FileInfo(filename);
            if (File.Exists(filename) && fi.Length == 0)
            {
                if (File.Exists(backupFileName))
                {
                    // 尝试从备份文件装载
                    dom.Load(backupFileName);
                    _changed = true;
                    return;
                }

                // 2019/6/19
                // 当作新文件进行加载。增加系统抗毁坏性
                // TODO: 是否要把这种情况反馈给调主？
                _dom.LoadXml("<root />");
                _changed = true;
                return;
            }

            try
            {
                dom.Load(filename);
            }
            catch (FileNotFoundException)
            {
                if (auto_create)
                {
                    _dom.LoadXml("<root />");
                    _changed = true;
                }
                else
                    throw;
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }
            catch
            {
                // 尝试从备份文件装载
                if (File.Exists(backupFileName))
                {
                    dom.Load(backupFileName);
                    _changed = true;
                }
                else
                    throw;
            }
        }

        public static void SafeSave(XmlDocument dom, string filename)
        {
            var directory = Path.GetDirectoryName(filename);
            var tempFileName = Path.Combine(directory, Path.GetFileName(filename) + BACKUP_EXTENSION);

            // 先保存到临时文件
            dom.Save(tempFileName);
            // 删除正式文件
            File.Delete(filename);
            // 临时文件改名为正式文件
            File.Move(tempFileName, filename);
        }
#if NO
        public static void SafeSave(XmlDocument dom, string filename)
        {
            // 临时备份一个原来文件，避免保存中途出错造成 0 bytes 的文件
            var directory = Path.GetDirectoryName(filename);
            var backupFileName = Path.Combine(directory, Path.GetFileName(filename) + BACKUP_EXTENSION);
            if (File.Exists(filename))
                File.Copy(filename, backupFileName, true);
            else
                backupFileName = null;

            // 进行保存
            dom.Save(filename);

            // 如果保存成功，则删除临时备份文件
            if (string.IsNullOrEmpty(backupFileName) == false)
                File.Delete(backupFileName);
        }
#endif
        public string Dump()
        {
            StringBuilder text = new StringBuilder();
            text.Append("filename=" + _filename + "\r\n");
            XmlNodeList nodes = _dom.DocumentElement.SelectNodes("*");
            foreach (XmlElement section in nodes)
            {
                foreach (XmlAttribute attr in section.Attributes)
                {
                    text.Append("section=" + section.Name + ", name=" + attr.Name + ", value=" + attr.Value + "\r\n");
                }
            }

            return text.ToString();
        }
    }
}

