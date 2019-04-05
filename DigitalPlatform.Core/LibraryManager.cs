using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalPlatform.Core
{
    /// <summary>
    /// chord 项目内所有函数库的共同全局参数
    /// </summary>
    public static class LibraryManager
    {
        public static ILog Log { get; set; }
    }
}
