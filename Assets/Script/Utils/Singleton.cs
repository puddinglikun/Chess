using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>
    /// 单例模式基类
    /// </summary>
    public class Singleton<T> where T : new()
    {
        private static T instance;
        public static T GetInstance()
        {
            if (instance == null)
                instance = new T();
            return instance;
        }

        public static T Ins
        {
            get
            {
                return GetInstance();
            }
        }
    }
}
