using DigitalPlatform.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DigitalPlatform.Core
{
    // 注：代码从 chord 项目 DigitalPlatform.Common 复制过来
    /// <summary>
    /// 紧凑型日志。用于可能泛滥的日志场合
    /// </summary>
    public class CompactLog
    {
        internal ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        Dictionary<string, CompactEntry> _table = new Dictionary<string, CompactEntry>();

        public const int MaxEntryCount = 1000;

        // 添加一条日志
        public async Task<string> Add(string fmt, object[] args)
        {
            return await Task.Run(() => { return AddEntry(fmt, args); }).ConfigureAwait(false);
        }

        string AddEntry(string fmt, object[] args)
        {
            CompactEntry entry = null;
            _lock.EnterWriteLock();
            try
            {
                if (_table.Count >= MaxEntryCount)
                    return "Entry 数超过 " + MaxEntryCount + "，没有记入日志";

                DateTime now = DateTime.Now;
                if (_table.ContainsKey(fmt) == false)
                {
                    entry = new CompactEntry { Key = fmt, StartTime = now };
                    _table.Add(fmt, entry);
                }
                else
                {
                    entry = (CompactEntry)_table[fmt];
                    if (entry.StartTime == DateTime.MinValue)
                        entry.StartTime = now;
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }

            lock (entry)
            {
                entry.AddData(args);
            }

            return null;
        }

        // 把累积的条目一次性写入日志文件
        public int WriteToLog(delegate_writeLog func_writeLog,
            string style = "")
        {
            int count = 0;
            // List<string> keys = new List<string>();
            _lock.EnterReadLock();
            try
            {
                foreach (string key in _table.Keys)
                {
                    CompactEntry entry = _table[key];
                    lock (entry)
                    {
                        count += entry.WriteToLog(func_writeLog, style);
                    }
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }

#if NO
            _lock.EnterWriteLock();
            try
            {
                foreach (string key in keys)
                {
                    _table.Remove(key);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
#endif

            return count;
        }

        // 获得条目
        public List<CompactEntry> GetEntry(string fmt = null)
        {
            _lock.EnterReadLock();
            try
            {
                if (string.IsNullOrEmpty(fmt) == false)
                {
                    if (_table.ContainsKey(fmt))
                    {
                        return new List<CompactEntry>() { _table[fmt] };
                    }
                    return new List<CompactEntry>();
                }
                else
                {
                    var results = new List<CompactEntry>();
                    foreach (string key in _table.Keys)
                    {
                        results.Add(_table[key]);
                    }
                    return results;
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }

        }

        // 2021/3/28
        // 清除一个或全部条目
        // parameters:
        //      fmt 要清除的条目格式。如果为 null 或 "" 则表示清除所有累积的条目
        // return:
        //      清除的条目数
        public int RemoveEntry(string fmt = null)
        {
            _lock.EnterWriteLock();
            try
            {
                if (string.IsNullOrEmpty(fmt) == false)
                {
                    if (_table.ContainsKey(fmt))
                    {
                        _table.Remove(fmt);
                        return 1;
                    }
                    return 0;
                }
                else
                {
                    var count = _table.Count;
                    if (count > 0)
                        _table.Clear();
                    return count;
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        // 2021/3/28
        // 条目数
        public int EntryCount
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _table.Count;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }
    }

    // 条目
    public class CompactEntry
    {
        // 格式名
        public string Key { get; set; }
        // 第一笔数据的时间
        public DateTime StartTime { get; set; }
        // 数据总数
        public long TotalCount { get; set; }

        public List<CompactData> Datas { get; set; }

        public const int MaxDataCount = 10;

        public void AddData(object[] args)
        {
            if (Datas == null)
                Datas = new List<CompactData>();

            if (this.Datas.Count < MaxDataCount)
            {
                DateTime now = DateTime.Now;
                CompactData data = new CompactData
                {
                    Args = args,
                    Ticks = now.Ticks - StartTime.Ticks // 2021/3/23
                };
                Datas.Add(data);
            }
            TotalCount++;
        }

        public int WriteToLog(delegate_writeLog func_writeLog,
            string style = "")
        {
            if (this.Datas.Count == 0)
            {
                // 2021/3/28
                if (StringUtil.IsInList("reset_start_time", style))
                    this.StartTime = DateTime.MinValue;

                return 0;
            }

            string entry_time_format = StringUtil.GetParameterByPrefix(style, "entry_time_format");
            string data_format = StringUtil.GetParameterByPrefix(style, "data_format");

            if (string.IsNullOrEmpty(entry_time_format))
                entry_time_format = "HH:mm:ss";

            if (style == "display" || string.IsNullOrEmpty(data_format) || data_format == "display")
            {
                // 适合观看的格式
                StringBuilder text = new StringBuilder();
                text.Append("(" + this.TotalCount + " 项) 压缩日志 ");
                text.Append(this.Key + "\r\n");
                int i = 0;
                foreach (CompactData data in this.Datas)
                {
                    text.Append((i + 1).ToString() + ") " + data.GetTimeString(this.StartTime, entry_time_format) + ":");
                    text.Append(string.Format(this.Key, data.Args) + "\r\n");
                    i++;
                }
                if (i < this.TotalCount)
                    text.Append("... (余下 " + (this.TotalCount - i) + " 项被略去)");
                func_writeLog(text.ToString());
            }
            else
            {
                // 原始格式
                StringBuilder text = new StringBuilder();
                text.Append("(" + this.TotalCount + " 项)");
                text.Append(this.Key + "\r\n");
                int i = 0;
                foreach (CompactData data in this.Datas)
                {
                    // 逐项显示 data.Args
                    List<string> args = new List<string>();
                    foreach (var arg in data.Args)
                    {
                        args.Add(arg == null ? "(null)" : arg.ToString());
                    }

                    text.Append((i + 1).ToString() + ") " + data.GetString(this.StartTime, entry_time_format) + " " + StringUtil.MakePathList(args, ",") + "\r\n");
                    i++;
                }
                if (i < this.TotalCount)
                    text.Append("... (余下 " + (this.TotalCount - i) + " 项被略去)");
                func_writeLog(text.ToString());
            }

            int count = (int)this.TotalCount;
            this.Datas.Clear(); // 写入日志后，清除内存
            this.TotalCount = 0;

            // 2021/3/28
            if (StringUtil.IsInList("reset_start_time", style))
                this.StartTime = DateTime.MinValue;
            return count;
        }
    }

    // 数据
    public class CompactData
    {
        // 相对于开始时间的 Ticks
        public long Ticks { get; set; }

        // 参数值列表
        public object[] Args { get; set; }

        /*
        public string GetString(DateTime start, string time_fmt = "HH:mm:ss")
        {
            StringBuilder text = new StringBuilder();
            text.Append((new DateTime(start.Ticks + this.Ticks)).ToString(time_fmt) + ":");
            int i = 0;
            foreach (object o in Args)
            {
                if (i > 0)
                    text.Append(",");
                text.Append(o.ToString());
                i++;
            }

            return text.ToString();
        }
        */

        // 2021/9/4
        public string GetTimeString(DateTime start, string time_fmt = "HH:mm:ss")
        {
            return (new DateTime(start.Ticks + this.Ticks)).ToString(time_fmt);
        }

        public string GetString(DateTime start, string time_fmt = "HH:mm:ss")
        {
            StringBuilder text = new StringBuilder();
            text.Append((new DateTime(start.Ticks + this.Ticks)).ToString(time_fmt) + ":");
            if (Args == null) // 2021/8/20 巩固
            {
                text.Append("(Args is null)");
            }
            else
            {
                int i = 0;
                foreach (object o in Args)
                {
                    if (i > 0)
                        text.Append(",");
                    text.Append(o == null ? "(null)" : o.ToString());   // 2021/8/20 巩固
                    i++;
                }
            }

            return text.ToString();
        }

    }

    public delegate void delegate_writeLog(string text);
}

