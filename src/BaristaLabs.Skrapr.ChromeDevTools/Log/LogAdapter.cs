namespace BaristaLabs.Skrapr.ChromeDevTools.Log
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an adapter for the Log domain to simplify the command interface.
    /// </summary>
    public class LogAdapter
    {
        private readonly ChromeSession m_session;
        
        public LogAdapter(ChromeSession session)
        {
            m_session = session ?? throw new ArgumentNullException(nameof(session));
        }

        /// <summary>
        /// Gets the ChromeSession associated with the adapter.
        /// </summary>
        public ChromeSession Session
        {
            get { return m_session; }
        }

        /// <summary>
        /// Enables log domain, sends the entries collected so far to the client by means of the &lt;code&gt;entryAdded&lt;/code&gt; notification.
        /// </summary>
        public async Task<EnableCommandResponse> Enable(EnableCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<EnableCommand, EnableCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Disables log domain, prevents further log entries from being reported to the client.
        /// </summary>
        public async Task<DisableCommandResponse> Disable(DisableCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<DisableCommand, DisableCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Clears the log.
        /// </summary>
        public async Task<ClearCommandResponse> Clear(ClearCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<ClearCommand, ClearCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// start violation reporting.
        /// </summary>
        public async Task<StartViolationsReportCommandResponse> StartViolationsReport(StartViolationsReportCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<StartViolationsReportCommand, StartViolationsReportCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Stop violation reporting.
        /// </summary>
        public async Task<StopViolationsReportCommandResponse> StopViolationsReport(StopViolationsReportCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<StopViolationsReportCommand, StopViolationsReportCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }

        /// <summary>
        /// Issued when new message was logged.
        /// </summary>
        public void SubscribeToEntryAddedEvent(Action<EntryAddedEvent> eventCallback)
        {
            m_session.Subscribe(eventCallback);
        }
    }
}