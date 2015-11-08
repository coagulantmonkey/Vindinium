using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class Log4netManager
    {
        private static ILog Logger;

        static Log4netManager()
        {
            try
            {
                string filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                filePath = Path.Combine(filePath + @"\log4net.config");

                bool exists = File.Exists(filePath);

                XmlConfigurator.Configure(new FileInfo(filePath));
                Logger = log4net.LogManager.GetLogger("Vindinium");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Could not get the logger. The program will now terminate." + ex.ToString());
                throw ex;
            }
        }

        #region Private Methods

        private static string FormatString(string message, Type callingClass, string callingMemberName)
        {
            return string.Format("{0} : {1} -> {2}", callingClass.Name, callingMemberName, message);
        }

        private static void Debug(object message)
        {
            Logger.Debug(message);
        }

        private static void Debug(object message, Exception exception)
        {
            Logger.Debug(message, exception);
        }

        private static void DebugFormat(string format, params object[] args)
        {
            Logger.DebugFormat(format, args);
        }

        private static void DebugFormat(string format, object arg0)
        {
            Logger.DebugFormat(format, arg0);
        }

        private static void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.DebugFormat(provider, format, args);
        }

        private static void DebugFormat(string format, object arg0, object arg1)
        {
            Logger.DebugFormat(format, arg0, arg1);
        }

        private static void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.DebugFormat(format, arg0, arg1, arg2);
        }

        private static void Error(object message)
        {
            Logger.Error(message);
        }

        private static void Error(object message, Exception exception)
        {
            Logger.Error(message, exception);
        }

        private static void ErrorFormat(string format, params object[] args)
        {
            Logger.ErrorFormat(format, args);
        }

        private static void ErrorFormat(string format, object arg0)
        {
            Logger.ErrorFormat(format, arg0);
        }

        private static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.ErrorFormat(provider, format, args);
        }

        private static void ErrorFormat(string format, object arg0, object arg1)
        {
            Logger.ErrorFormat(format, arg0, arg1);
        }

        private static void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.ErrorFormat(format, arg0, arg1, arg2);
        }

        private static void Fatal(object message)
        {
            Logger.Fatal(message);
        }

        private static void Fatal(object message, Exception exception)
        {
            Logger.Fatal(message, exception);
        }

        private static void FatalFormat(string format, params object[] args)
        {
            Logger.FatalFormat(format, args);
        }

        private static void FatalFormat(string format, object arg0)
        {
            Logger.FatalFormat(format, arg0);
        }

        private static void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.FatalFormat(provider, format, args);
        }

        private static void FatalFormat(string format, object arg0, object arg1)
        {
            Logger.FatalFormat(format, arg0, arg1);
        }

        private static void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.FatalFormat(format, arg0, arg1, arg2);
        }

        private static void Info(object message)
        {
            Logger.Info(message);
        }

        private static void Info(object message, Exception exception)
        {
            Logger.Info(message, exception);
        }

        private static void InfoFormat(string format, params object[] args)
        {
            Logger.InfoFormat(format, args);
        }

        private static void InfoFormat(string format, object arg0)
        {
            Logger.InfoFormat(format, arg0);
        }

        private static void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.InfoFormat(provider, format, args);
        }

        private static void InfoFormat(string format, object arg0, object arg1)
        {
            Logger.InfoFormat(format, arg0, arg1);
        }

        private static void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.InfoFormat(format, arg0, arg1, arg2);
        }

        private static void Warn(object message)
        {
            Logger.Warn(message);
        }

        private static void Warn(object message, Exception exception)
        {
            Logger.Warn(message, exception);
        }

        private static void WarnFormat(string format, params object[] args)
        {
            Logger.WarnFormat(format, args);
        }

        private static void WarnFormat(string format, object arg0)
        {
            Logger.WarnFormat(format, arg0);
        }

        private static void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.WarnFormat(provider, format, args);
        }

        private static void WarnFormat(string format, object arg0, object arg1)
        {
            Logger.WarnFormat(format, arg0, arg1);
        }

        private static void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.WarnFormat(format, arg0, arg1, arg2);
        }

        #endregion

        public static bool IsDebugEnabled
        {
            get { return Logger.IsDebugEnabled; }
        }

        public static bool IsErrorEnabled
        {
            get { return Logger.IsErrorEnabled; }
        }

        public static bool IsFatalEnabled
        {
            get { return Logger.IsFatalEnabled; }
        }

        public static bool IsInfoEnabled
        {
            get { return Logger.IsInfoEnabled; }
        }

        public static bool IsWarnEnabled
        {
            get { return Logger.IsWarnEnabled; }
        }

        public static void DebugFormat(string message, Type callingClass, [CallerMemberName]string methodName = "")
        {
            DebugFormat(FormatString(message, callingClass, methodName));
        }

        public static void ErrorFormat(string message, Type callingClass, [CallerMemberName]string methodName = "")
        {
            ErrorFormat(FormatString(message, callingClass, methodName));
        }

        public static void LogException(string Message, Exception ex, Type callingClass, [CallerMemberName]string methodName = "")
        {
            ErrorFormat(FormatString(string.Format("Exception caught and logged; \n{0}, \n{1}.", ex, Message), callingClass, methodName));
        }

        public static void WarnFormat(string message, Type callingClass, [CallerMemberName]string methodName = "")
        {
            WarnFormat(FormatString(message, callingClass, methodName));
        }
    }
}
