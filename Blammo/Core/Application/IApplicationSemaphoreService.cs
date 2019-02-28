using System;

namespace Agent.Core.Application
{
    public interface IApplicationSemaphoreService
    {
        void SynchronizeThread(Action synchronizedAction);
    }
}