using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        // 2020/9/16
        public string FileName
        {
            get
            {
                return _filename;
            }
        }

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

        // 调试信息。当打开文件时候，记载打开的过程
        StringBuilder _debugInfo = null;
        public string DebugInfo
        {
            get
            {
                return _debugInfo?.ToString();
            }
        }

        public ConfigSetting(string filename,
            bool auto_create,
            bool build_debugInfo = false)
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
            // 2021/9/9
            if (build_debugInfo)
                _debugInfo = new StringBuilder();

            SafeLoad(_dom,
    filename,
    auto_create);

            _filename = filename;
        }

        public static ConfigSetting Open(string filename,
            bool auto_create,
            bool build_debugInfo = false)
        {
            ConfigSetting config = new ConfigSetting(filename,
                auto_create,
                build_debugInfo);
            return config;
        }

        // 2022/1/24
        static string GetSection(string section)
        {
            if (string.IsNullOrEmpty(section) == true)
                throw new ArgumentException("section 参数值不应为空");
            if (char.IsDigit(section[0]) == true)
                return "n_" + section;
            return section;
        }

        // 写入一个字符串值
        public string Set(string section, string entry, string value)
        {
            // 2022/1/24
            section = GetSection(section);

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
            // 2022/1/24
            section = GetSection(section);

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

        // 保存瞬间临时备份的文件扩展名。
        // 如果保存中途出错，下次启动的时候会发现一个这样的文件，可以用于恢复
        // 如果保存最终成功，这个临时备份文件会被删除
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
                _debugInfo?.AppendLine($"发现文件 {filename} 的尺寸为零");
                var ret = RetryLoad(dom,
                    filename,
                    backupFileName,
                    this._debugInfo);
                if (ret == true)
                    return;
#if NO
                if (File.Exists(backupFileName))
                {
                    // 尝试从备份文件装载
                    try
                    {
                        dom.Load(backupFileName);
                        _changed = true;
                        return;
                    }
                    catch
                    {

                    }
                }

                // 2021/9/8
                var back_file = GetNewestBackupFilename(filename);
                if (string.IsNullOrEmpty(back_file) == false)
                {
                    // 尝试从备份文件装载
                    dom.Load(backupFileName);
                    _changed = true;
                    return;
                }

#endif

                // 2019/6/19
                // 当作新文件进行加载。增加系统抗毁坏性
                // TODO: 是否要把这种情况反馈给调主？
                _dom.LoadXml("<root />");
                _changed = true;
                _debugInfo?.AppendLine($"加载 <root />");
                return;
            }

            try
            {
                dom.Load(filename);
                _debugInfo?.AppendLine($"正常加载文件 {filename}");
            }
            catch (FileNotFoundException)
            {
                _debugInfo?.AppendLine($"文件 {filename} 不存在");
                var ret = RetryLoad(dom,
                    filename,
                    backupFileName,
                    this._debugInfo);
                if (ret == true)
                    return;
#if NO
                // 2021/3/2
                if (File.Exists(backupFileName))
                {
                    // 尝试从备份文件装载
                    try
                    {
                        dom.Load(backupFileName);
                        _changed = true;
                        return;
                    }
                    catch
                    {

                    }
                }

                // 2021/9/8
                var back_file = GetNewestBackupFilename(filename);
                if (string.IsNullOrEmpty(back_file) == false)
                {
                    // 尝试从备份文件装载
                    dom.Load(backupFileName);
                    _changed = true;
                    return;
                }

#endif

                if (auto_create)
                {
                    _dom.LoadXml("<root />");
                    _changed = true;
                    _debugInfo?.AppendLine($"因 auto_create 为 true，加载 <root />");
                }
                else
                    throw;
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _debugInfo?.AppendLine($"文件 {filename} 加载时出现异常: {ex.Message}");

                var ret = RetryLoad(dom,
                    filename,
                    backupFileName,
                    this._debugInfo);
                if (ret == true)
                    return;
                throw;
                /*
                // 尝试从备份文件装载
                if (File.Exists(backupFileName))
                {
                    dom.Load(backupFileName);
                    _changed = true;
                }
                else
                    throw;
                */
            }

            // 装载成功后，立刻备份一次
            var back_filename = SaveBackup(filename);
            _debugInfo?.AppendLine($"装载成功后，立即把内容备份到文件 {back_filename}");
        }

        bool RetryLoad(XmlDocument dom,
            string filename,
            string backupFileName,
            StringBuilder debugInfo)
        {
            if (File.Exists(backupFileName))
            {
                // 尝试从备份文件装载
                try
                {
                    dom.Load(backupFileName);
                    _changed = true;
                    debugInfo?.AppendLine($"尝试从临时文件 {backupFileName} 装载，成功");
                    return true;
                }
                catch (Exception ex)
                {
                    debugInfo?.AppendLine($"尝试从临时文件 {backupFileName} 装载，失败: {ex.Message}");
                }
            }

            // 2021/9/8
            var back_file = GetNewestBackupFilename(filename);
            if (string.IsNullOrEmpty(back_file) == false)
            {
                // 尝试从备份文件装载
                try
                {
                    dom.Load(back_file);
                    _changed = true;
                    debugInfo?.AppendLine($"尝试从备份文件 {back_file} 装载，成功");
                    return true;
                }
                catch (Exception ex)
                {
                    debugInfo?.AppendLine($"尝试从备份文件 {back_file} 装载，失败: {ex.Message}");
                    throw ex;
                }
            }
            return false;
        }

        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/11535a79-e565-4f6d-bb19-e47054508133/xmldocumentsave-0-length-files-half-written-files-etc
        public static void SafeSave(XmlDocument dom, string filename)
        {
            var directory = Path.GetDirectoryName(filename);
            var tempFileName = Path.Combine(directory, Path.GetFileName(filename) + BACKUP_EXTENSION);

            // 2021/3/2
            // 删除以前残留的临时文件
            if (File.Exists(tempFileName))
                File.Delete(tempFileName);

            // 先保存到临时文件
            // dom.Save(tempFileName);

            {
                var content = dom.DocumentElement.OuterXml;
                File.WriteAllText(tempFileName, content, Encoding.UTF8);
            }

            // 2021/9/6
            // 验证一下临时文件是否完整合法
            {
                XmlDocument domVerify = new XmlDocument();
                try
                {
                    domVerify.Load(tempFileName);
                }
                catch (Exception ex)
                {
                    // 这时留下临时文件和并未伤害的前次正式文件，等下次 SaveLoad() 时候处理
                    throw new Exception($"SafeSave() 在写入临时文件 '{tempFileName}' 后验证失败", ex);
                }
            }

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

        #region 备份

        // 备份文件的扩展名(后面还应该有数字部分)
        public const string BACKUP_EXT = ".bak";
        // 备份文件最大数
        public const int BACKUP_COUNT = 10;

        // 保存到备份文件
        public string SaveBackup(string filename)
        {
            var filenames = BuildBackupFileNames(filename);

            Debug.Assert(filenames.Count != 0);

            RemoveInvalidBackupFiles(filenames);

            // 寻找时间最靠后的一个事项
            int index = GetNewestIndex(filenames);

            // 取这个位置之后的一个位置
            if (index < filenames.Count - 1)
                index++;
            else
                index = 0;

            Debug.Assert(index != -1);

            var new_filename = filenames[index].FileName;
            _dom.Save(new_filename);
            return new_filename;
        }

        public static string GetNewestBackupFilename(string filename)
        {
            var filenames = BuildBackupFileNames(filename);

            RemoveInvalidBackupFiles(filenames);

            // 寻找时间最靠后的一个事项
            int index = GetNewestIndex(filenames);
            if (index == -1)
                return null;
            return filenames[index].FileName;
        }

        class FileItem
        {
            public string FileName { get; set; }
            public DateTime LastTime { get; set; }
        }

        // 删除内容不合法的 XML 备份文件
        static void RemoveInvalidBackupFiles(List<FileItem> filenames)
        {
            try
            {
                foreach (var item in filenames)
                {
                    if (File.Exists(item.FileName))
                    {
                        XmlDocument dom = new XmlDocument();
                        try
                        {
                            dom.Load(item.FileName);
                        }
                        catch
                        {
                            File.Delete(item.FileName);
                        }
                    }
                }
            }
            catch
            {

            }
        }

        static int GetNewestIndex(List<FileItem> filenames)
        {
            DateTime lastTime = DateTime.MinValue;
            int index = -1;
            // 寻找时间最靠后的一个事项
            for (int i = 0; i < filenames.Count; i++)
            {
                var item = filenames[i];
                if (File.Exists(item.FileName)
                    && item.LastTime > lastTime)
                {
                    lastTime = item.LastTime;
                    index = i;
                }
            }

            return index;
        }

        static List<FileItem> BuildBackupFileNames(string filename)
        {
            List<FileItem> results = new List<FileItem>();
            for (int i = 0; i < BACKUP_COUNT; i++)
            {
                FileItem item = new FileItem();
                item.FileName = filename + BACKUP_EXT + i.ToString();
                item.LastTime = File.GetLastWriteTime(item.FileName);
                results.Add(item);
            }

            return results;
        }

        #endregion

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

