using log4net;
using log4net.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Common.Helpers
{
    public class Log4netManager
    {
        private ILog _logger;

        public Log4netManager()
        {
            try
            {
                string filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                filePath = Path.Combine(filePath + @"\log4net.config");

                bool exists = File.Exists(filePath);

                XmlConfigurator.Configure(new FileInfo(filePath));
                _logger = log4net.LogManager.GetLogger("Vindinium");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Could not get the logger. The program will now terminate." + ex.ToString());
                throw ex;
            }
        }

        private string FormatString(string message, Type callingClass, string callingMemberName)
        {
            return string.Format("{0} : {1} -> {2}", callingClass.Name, callingMemberName, message);
        }

        private void Debug(object message)
        {
            _logger.Debug(message);
        }

        private void Debug(object message, Exception exception)
        {
            _logger.Debug(message, exception);
        }

        private void DebugFormat(string format, params object[] args)
        {
            _logger.DebugFormat(format, args);
        }

        private void DebugFormat(string format, object arg0)
        {
            _logger.DebugFormat(format, arg0);
        }

        private void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.DebugFormat(provider, format, args);
        }

        private void DebugFormat(string format, object arg0, object arg1)
        {
            _logger.DebugFormat(format, arg0, arg1);
        }

        private void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            _logger.DebugFormat(format, arg0, arg1, arg2);
        }

        private void Error(object message)
        {
            _logger.Error(message);
        }

        private void Error(object message, Exception exception)
        {
            _logger.Error(message, exception);
        }

        private void ErrorFormat(string format, params object[] args)
        {
            _logger.ErrorFormat(format, args);
        }

        private void ErrorFormat(string format, object arg0)
        {
            _logger.ErrorFormat(format, arg0);
        }

        private void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.ErrorFormat(provider, format, args);
        }

        private void ErrorFormat(string format, object arg0, object arg1)
        {
            _logger.ErrorFormat(format, arg0, arg1);
        }

        private void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            _logger.ErrorFormat(format, arg0, arg1, arg2);
        }

        private void Fatal(object message)
        {
            _logger.Fatal(message);
        }

        private void Fatal(object message, Exception exception)
        {
            _logger.Fatal(message, exception);
        }

        private void FatalFormat(string format, params object[] args)
        {
            _logger.FatalFormat(format, args);
        }

        private void FatalFormat(string format, object arg0)
        {
            _logger.FatalFormat(format, arg0);
        }

        private void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.FatalFormat(provider, format, args);
        }

        private void FatalFormat(string format, object arg0, object arg1)
        {
            _logger.FatalFormat(format, arg0, arg1);
        }

        private void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            _logger.FatalFormat(format, arg0, arg1, arg2);
        }

        private void Info(object message)
        {
            _logger.Info(message);
        }

        private void Info(object message, Exception exception)
        {
            _logger.Info(message, exception);
        }

        private void InfoFormat(string format, params object[] args)
        {
            _logger.InfoFormat(format, args);
        }

        private void InfoFormat(string format, object arg0)
        {
            _logger.InfoFormat(format, arg0);
        }

        private void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.InfoFormat(provider, format, args);
        }

        private void InfoFormat(string format, object arg0, object arg1)
        {
            _logger.InfoFormat(format, arg0, arg1);
        }

        private void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            _logger.InfoFormat(format, arg0, arg1, arg2);
        }

        private void Warn(object message)
        {
            _logger.Warn(message);
        }

        private void Warn(object message, Exception exception)
        {
            _logger.Warn(message, exception);
        }

        private void WarnFormat(string format, params object[] args)
        {
            _logger.WarnFormat(format, args);
        }

        private void WarnFormat(string format, object arg0)
        {
            _logger.WarnFormat(format, arg0);
        }

        private void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.WarnFormat(provider, format, args);
        }

        private void WarnFormat(string format, object arg0, object arg1)
        {
            _logger.WarnFormat(format, arg0, arg1);
        }

        private void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            _logger.WarnFormat(format, arg0, arg1, arg2);
        }

        public bool IsDebugEnabled
        {
            get { return _logger.IsDebugEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return _logger.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return _logger.IsFatalEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return _logger.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return _logger.IsWarnEnabled; }
        }

        public void DebugFormat(string message, Type callingClass, [CallerMemberName]string methodName = "")
        {
            DebugFormat(FormatString(message, callingClass, methodName));
        }

        public void ErrorFormat(string message, Type callingClass, [CallerMemberName]string methodName = "")
        {
            ErrorFormat(FormatString(message, callingClass, methodName));
        }

        public void LogException(string Message, Exception ex, Type callingClass, [CallerMemberName]string methodName = "")
        {
            ErrorFormat(FormatString(string.Format("Exception caught and logged; \n{0}, \n{1}.", ex, Message), callingClass, methodName));
        }

        public void WarnFormat(string message, Type callingClass, [CallerMemberName]string methodName = "")
        {
            WarnFormat(FormatString(message, callingClass, methodName));
        }
    }
}
