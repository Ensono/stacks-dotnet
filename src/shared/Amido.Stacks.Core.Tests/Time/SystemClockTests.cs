using System;
using System.Threading.Tasks;
using Amido.Stacks.Core.Time;
using Xunit;

namespace Amido.Stacks.Core.Tests.Time
{
    [Trait("TestType", "UnitTests")]
    public class SystemClockTests
    {
        [Fact]
        public void DateTimeAndSystemClockShouldHaveSameTime()
        {
            var system = DateTime.Now.TrimMilliseconds();
            var systemUtc = DateTime.UtcNow.TrimMilliseconds();

            var mock = SystemClock.Now.TrimMilliseconds();
            var mockUtc = SystemClock.UtcNow.TrimMilliseconds();

            Assert.Equal(system, mock);
            Assert.Equal(systemUtc, mockUtc);
        }

        [Fact]
        public void DateTimeAndSystemClockShouldHaveSameTimeAfterReset()
        {
            var system = DateTime.Now.TrimMilliseconds();
            var mock = SystemClock.Now.TrimMilliseconds();

            Assert.Equal(system, mock);

            var myDate = new DateTime(1987, 01, 23, 05, 40, 32);
            SystemClock.Set(myDate);

            mock = SystemClock.Now;

            Assert.Equal(myDate, mock);

            SystemClock.Reset();

            system = DateTime.Now.TrimMilliseconds();
            mock = SystemClock.Now.TrimMilliseconds();

            Assert.Equal(system, mock);
        }

        [Fact]
        public async Task ParallelRunShouldNotAffectOtherClock()
        {
            Task[] tasks = new Task[50];
            for (int i = 0; i < 50; i++)
            {
                tasks[i] = Task.Run(UseCustomTime);
            }

            await Task.WhenAll(tasks);
        }

        Random rnd = new Random();
        private async Task UseCustomTime()
        {
            var date = new DateTime(rnd.Next(1900, 2020), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 12), rnd.Next(0, 59), rnd.Next(0, 59));

            SystemClock.Set(date);

            await Task.Delay(1000); //wait for other threads to set their desired time

            var mock = SystemClock.UtcNow;

            Assert.Equal(date, mock);
        }
    }

    static class DateTimeTrimmer
    {
        public static DateTime TrimMilliseconds(this DateTime date)
        {
            return new DateTime(date.Ticks - date.Ticks % TimeSpan.TicksPerSecond);
        }
    }
}
