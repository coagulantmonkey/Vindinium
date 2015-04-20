using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace vindiniumWPF.Helpers
{
    public static class Extensions
    {
        public static void DoActionWithReadLock(this ReaderWriterLockSlim lockObject, Action action)
        {
            if (lockObject.TryEnterReadLock(1000))
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    Log4netManager.LogException("Exception caught under read lock.", ex, typeof(Extensions));
                    throw;
                }
                finally
                {
                    lockObject.ExitReadLock();
                }
            }
            else
            {
                Log4netManager.WarnFormat("Failed to acquire read lock.", typeof(Extensions));
                throw new Exception("Could not acquire read lock");
            }
        }

        public static void DoActionWithWriteLock(this ReaderWriterLockSlim lockObject, Action action)
        {
            if (lockObject.TryEnterWriteLock(1000))
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    Log4netManager.LogException("Exception caught under write lock.", ex, typeof(Extensions));
                    throw;
                }
                finally
                {
                    lockObject.ExitReadLock();
                }
            }
            else
            {
                Log4netManager.WarnFormat("Failed to acquire write lock.", typeof(Extensions));
                throw new Exception("Could not acquire write lock");
            }
        }
    }
}
