﻿namespace BaristaLabs.Skrapr
{
    using BaristaLabs.Skrapr.ChromeDevTools;
    using BaristaLabs.Skrapr.Definitions;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// Represents a worker that processes a skrapr definition.
    /// </summary>
    public interface ISkraprWorker : ITargetBlock<ISkraprTask>, IDataflowBlock
    {
        /// <summary>
        /// Gets the definition that the worker works on.
        /// </summary>
        SkraprDefinition Definition
        {
            get;
        }

        /// <summary>
        /// Gets the default cancellation token for the worker.
        /// </summary>
        CancellationToken CancellationToken
        {
            get;
        }

        /// <summary>
        /// Gets the Skrapr dev tools associated with the worker
        /// </summary>
        SkraprDevTools DevTools
        {
            get;
        }

        /// <summary>
        /// Gets a value that indicates if the worker is running in debug mode.
        /// </summary>
        /// <remarks>
        /// Tasks can implement behavior to perform additional functions, such as logging or skipping delays in debug mode.
        /// </remarks>
        bool IsDebugEnabled
        {
            get;
        }

        /// <summary>
        /// Gets a logger associated with the worker.
        /// </summary>
        ILogger Logger
        {
            get;
        }

        /// <summary>
        /// Gets the chrome session associated with the worker
        /// </summary>
        ChromeSession Session
        {
            get;
        }

        /// <summary>
        /// Adds a SkraprTask to the tasks that will be processed by the worker.
        /// </summary>
        /// <param name="target"></param>
        void Post(ISkraprTask task);

        /// <summary>
        /// Instructs the worker to cancel processing further work.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Immediately processes the specified task.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        Task ProcessSkraprTask(ISkraprTask task);
    }
}
