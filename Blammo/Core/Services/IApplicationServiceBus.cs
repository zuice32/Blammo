using System;
using System.Collections.Generic;

namespace Agent.Core.Services
{
    public interface IApplicationServiceBus : IDisposable
    {
        event Action Initialized;
        event Action InitializedComplete;

        /// <summary>
        ///     Return a service that matches the input type.
        ///     If more that one service of the input type exits, the first one found is returned.
        /// </summary>
        /// <typeparam name="T">The type of the service to be returned.</typeparam>
        /// <returns>Throws ServiceNotFoundException if no matching service found.</returns>
        T GetService<T>() where T : class, IApplicationService;

        IList<T> GetServices<T>() where T : class, IApplicationService;

        void RemoveService (IApplicationService serviceToRemove);
        void AddService(IApplicationService service);
    }
}