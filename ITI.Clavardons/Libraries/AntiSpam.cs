using System;
using ITI.Clavardons.Providers;

namespace ITI.Clavardons.Libraries
{
    public class AntiSpam
    {
        private ITimeProvider _timeProvider;
        private TimeSpan _windowDuration;
        private int _messagesPerWindow;

        private DateTime? _windowStart;
        private int _messagesCountInWindow;

        public AntiSpam(TimeSpan windowDuration, int messagesPerWindow, ITimeProvider timeProvider = null)
        {
            _timeProvider = timeProvider == null ? new SystemTimeProvider() : timeProvider;
            _windowDuration = windowDuration;
            _messagesPerWindow = messagesPerWindow;
        }

        public bool Check()
        {
            var now = _timeProvider.GetNow();

            var newWindow = _windowStart == null || now - _windowStart >= _windowDuration;

            if ( newWindow )
            {
                _windowStart = now;
                _messagesCountInWindow = 0;
            }

            if ( _messagesCountInWindow == _messagesPerWindow )
            {
                return false;
            }

            _messagesCountInWindow++;

            return true;
        }
    }
}
