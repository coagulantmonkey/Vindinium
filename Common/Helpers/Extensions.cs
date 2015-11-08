using Common.DataContracts;
using Common.Helpers;
using Common.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Helpers
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

        public static int DistanceApart(this Vector2D source, Vector2D target)
        {
            Vector2D distance = new Vector2D((source.X - target.X), (source.Y - target.Y));
            return distance.Length();
        }

        public static Vector2D BoardPosition(this Hero hero)
        {
            return new Vector2D(hero.pos.x, hero.pos.y);
        }

        public static Vector2D Subtract(this Vector2D source, Vector2D target)
        {
            return new Vector2D((source.X - target.X), (source.Y - target.Y));
        }

        public static string DisplayString(this Vector2D source)
        {
            return string.Format("({0},{1})", source.X, source.Y);
        }

        public static string DisplayString(this StartNewGameMessage message)
        {
            return "Number of turns : " + message.NumberOfTurns +
                "Server Url : " + message.ServerURL +
                "Play mode : " + (message.TrainingMode ? "Training" : "Arena");
        }
    }
}
