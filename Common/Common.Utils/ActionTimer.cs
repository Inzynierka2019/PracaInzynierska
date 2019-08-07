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

        /// <summary>
        /// Time in milliseconds before first execution.
        /// </summary>
        private const int dueTime = 0;

        public DateTime TimerStarted { get; }

        public ActionTimer(Action action, TimeSpan interval)
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
