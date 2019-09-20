using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Utils
{
    public class ActionTimer : IDisposable
    {
        private Timer _timer;
        private AutoResetEvent _autoResetEvent;
        private Action _action;

        public DateTime TimerStarted { get; }

        public TimeSpan TimeElapsed { get => TimerStarted.Subtract(DateTime.Now); }

        /// <summary>
        /// Initializes a new instance of the ActionTimer class
        /// for a scheduled invocation of an action method.
        /// </summary>
        /// <param name="action">The method delegate.</param>
        /// <param name="interval">The interval of the callback.</param>
        /// <param name="dueTime">The time in milliseconds before first callback.</param>
        public ActionTimer(Action action, TimeSpan interval, int dueTime = 0)
        {
            _action = action;
            _autoResetEvent = new AutoResetEvent(false);
            _timer = new Timer(Execute, _autoResetEvent, dueTime, interval.Milliseconds);
            TimerStarted = DateTime.Now;
        }

        public void Execute(object stateInfo)
        {
            _action();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
