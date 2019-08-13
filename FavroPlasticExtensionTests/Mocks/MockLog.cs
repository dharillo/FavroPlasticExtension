//  Favro Plastic Extension
//  Copyright(C) 2019  David Harillo Sánchez
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published
//  by the Free Software Foundation, either version 3 of the License, or
//  any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU Lesser General Public License for more details in the project root.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program. If not, see<https://www.gnu.org/licenses/>

using System;
using System.Collections.Generic;
using log4net;
using log4net.Core;

namespace FavroPlasticExtensionTests.Mocks
{
    /// <summary>
    /// Mock for the log instances of the log4net library
    /// </summary>
    public class MockLog: ILog
    {
        public MockLog()
        {
            DebugMessages = new List<string>();
            InfoMessages = new List<string>();
            WarnMessages = new List<string>();
            ErrorMessages = new List<string>();
            FatalMessages = new List<string>();
            IsDebugEnabled = true;
            IsInfoEnabled = true;
            IsWarnEnabled = true;
            IsErrorEnabled = true;
            IsFatalEnabled = true;
        }

        public List<string> DebugMessages { get; }
        public List<string> ErrorMessages { get; }
        public List<string> WarnMessages { get; }
        public List<string> InfoMessages { get; }
        public List<string> FatalMessages { get; }

        public bool IsDebugEnabled { get; set; }

        public bool IsInfoEnabled { get; set; }

        public bool IsWarnEnabled { get; set; }

        public bool IsErrorEnabled { get; set; }

        public bool IsFatalEnabled { get; set; }

        public ILogger Logger { get; set; }

        public void Debug(object message)
        {
            AddDebug(message.ToString());
        }

        public void Debug(object message, Exception exception)
        {
            AddDebug(message + " Exception: " + exception.Message);
        }

        public void DebugFormat(string format, params object[] args)
        {
            AddDebug(string.Format(format, args));
        }

        public void DebugFormat(string format, object arg0)
        {
            AddDebug(string.Format(format, arg0));
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            AddDebug(string.Format(format, arg0, arg1));
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            AddDebug(string.Format(format, arg0, arg1, arg2));
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            AddDebug(string.Format(provider, format, args));
        }

        public void Error(object message)
        {
            AddError(message.ToString());
        }

        public void Error(object message, Exception exception)
        {
            AddError(message + " Exception: " + exception.Message);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            AddError(string.Format(format, args));
        }

        public void ErrorFormat(string format, object arg0)
        {
            AddError(string.Format(format, arg0));
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            AddError(string.Format(format, arg0, arg1));
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            AddError(string.Format(format, arg0, arg1, arg2));
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            AddError(string.Format(provider, format, args));
        }

        public void Fatal(object message)
        {
            AddFatal(message.ToString());
        }

        public void Fatal(object message, Exception exception)
        {
            AddFatal(message + " Exception: " + exception.Message);
        }

        public void FatalFormat(string format, params object[] args)
        {
            AddFatal(string.Format(format, args));
        }

        public void FatalFormat(string format, object arg0)
        {
            AddFatal(string.Format(format, arg0));
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            AddFatal(string.Format(format, arg0, arg1));
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            AddFatal(string.Format(format, arg0, arg1, arg2));
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            AddFatal(string.Format(provider, format, args));
        }

        public void Info(object message)
        {
            AddInfo(message.ToString());
        }

        public void Info(object message, Exception exception)
        {
            AddInfo(message + " Exception: " + exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            AddInfo(string.Format(format, args));
        }

        public void InfoFormat(string format, object arg0)
        {
            AddInfo(string.Format(format, arg0));
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            AddInfo(string.Format(format, arg0, arg1));
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            AddInfo(string.Format(format, arg0, arg1, arg2));
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            AddInfo(string.Format(provider, format, args));
        }

        public void Warn(object message)
        {
            AddWarn(message.ToString());
        }

        public void Warn(object message, Exception exception)
        {
            AddWarn(message + " Exception: " + exception.Message);
        }

        public void WarnFormat(string format, params object[] args)
        {
            AddWarn(string.Format(format, args));
        }

        public void WarnFormat(string format, object arg0)
        {
            AddWarn(string.Format(format, arg0));
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            AddWarn(string.Format(format, arg0, arg1));
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            AddWarn(string.Format(format, arg0, arg1, arg2));
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            AddWarn(string.Format(provider, format, args));
        }

        private void AddDebug(string message)
        {
            if (IsDebugEnabled)
            {
                Add(DebugMessages, "DEBUG: " + message);
            }
        }

        private void Add(List<string> storage, string message)
        {
            storage.Add(message);
        }

        private void AddError(string message)
        {
            if (IsErrorEnabled)
            {
                Add(ErrorMessages, "ERROR: " + message);
            }
        }

        private void AddFatal(string message)
        {
            if (IsFatalEnabled)
            {
                Add(FatalMessages, "FATAL: " + message);
            }
        }

        private void AddInfo(string message)
        {
            if (IsInfoEnabled)
            {
                Add(InfoMessages, "INFO: " + message);
            }
        }

        private void AddWarn(string message)
        {
            if (IsWarnEnabled)
            {
                Add(WarnMessages, "WARN: " + message);
            }
        }
    }
}
