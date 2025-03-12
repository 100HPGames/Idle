using System;

namespace CodeBase.Tools.SimpleMessenger
{
    internal class CallbackWithPrecondition<T>
    {
        public Action<T> Action { get; }
        public CallbackWithPrecondition(Action<T> action)
        {
            Action = action;
        }
    }
}