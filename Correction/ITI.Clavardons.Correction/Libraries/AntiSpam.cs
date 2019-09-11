using System;
using ITI.Clavardons.Providers;

namespace ITI.Clavardons.Libraries
{
    /// <summary>
    /// The AntiSpam class provides a way to check whether a user is attempting
    /// to spam the chat or not.
    ///
    /// It works by allowing a certain number of messages during a certain window.
    /// (e.g. 10 messages every 10 seconds)
    /// </summary>
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

        /// <summary>
        /// Check whether or not a user is spamming.
        /// </summary>
        /// <returns>true if the user is not spamming, false otherwise</returns>
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
