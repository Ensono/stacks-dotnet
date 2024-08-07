using System;
using System.Threading;

namespace Amido.Stacks.Core.Time
{
    /// <summary>
    /// Provides access to system time while allowing it to be set to a fixed <see cref="DateTime"/> value.
    /// </summary>
    /// <remarks>
    /// This class is thread safe.
    /// </remarks>
    public static class SystemClock
    {
        private static readonly AsyncLocal<DateTime?> fixedTime = new AsyncLocal<DateTime?>();

        /// <inheritdoc cref="DateTime.Now"/>
        public static DateTime Now
        {
            get
            {
                if (fixedTime.Value == null)
                    return DateTime.Now;
                else
                    return fixedTime.Value.Value;
            }
        }

        /// <inheritdoc cref="DateTime.Today"/>
        public static DateTime Today
        {
            get { return Now.Date; }
        }

        /// <inheritdoc cref="DateTime.UtcNow"/>
        public static DateTime UtcNow
        {
            get { return Now.ToUniversalTime(); }
        }

        /// <summary>
        /// Sets a fixed time for the current thread to return by <see cref="SystemClock.Now"/>. <br/>
        /// When used in <see langword="async"/> code, make sure it is called from top most method or from non-async methods, given async code does not propagate context backwards in the stack.
        /// </summary>
        public static void Set(DateTime time)
        {
            if (time.Kind != DateTimeKind.Local)
                time = time.ToLocalTime();

            fixedTime.Value = time;
        }

        /// <summary>
        /// Resets <see cref="SystemClock"/> to return the current <see cref="DateTime.Now"/>.
        /// </summary>
        public static void Reset()
        {
            fixedTime.Value = null;
        }
    }
}
