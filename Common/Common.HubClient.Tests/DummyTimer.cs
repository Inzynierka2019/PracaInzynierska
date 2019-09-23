using System;
using System.Collections.Generic;
using System.Threading;

namespace Common.HubClient.Tests
{
    public class DummyTimer
    {
        private Timer _timer;
        private AutoResetEvent _autoResetEvent;
        private Action _action;

        public DateTime TimerStarted { get; }

        public DummyTimer(Action action)
        {
            _action = action;
            _autoResetEvent = new AutoResetEvent(false);
            _timer = new Timer(Execute, _autoResetEvent, 1000, 1500);
            TimerStarted = DateTime.Now;
        }

        public void Execute(object stateInfo)
        {
            _action();

            if ((DateTime.Now - TimerStarted).Minutes > 2)
            {
                _timer.Dispose();
            }
        }
    }
}
