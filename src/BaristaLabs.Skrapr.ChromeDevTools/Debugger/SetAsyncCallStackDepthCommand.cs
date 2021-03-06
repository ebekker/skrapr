namespace BaristaLabs.Skrapr.ChromeDevTools.Debugger
{
    using Newtonsoft.Json;

    /// <summary>
    /// Enables or disables async call stacks tracking.
    /// </summary>
    public sealed class SetAsyncCallStackDepthCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Debugger.setAsyncCallStackDepth";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

        /// <summary>
        /// Maximum depth of async call stacks. Setting to &lt;code&gt;0&lt;/code&gt; will effectively disable collecting async call stacks (default).
        /// </summary>
        [JsonProperty("maxDepth")]
        public long MaxDepth
        {
            get;
            set;
        }
    }

    public sealed class SetAsyncCallStackDepthCommandResponse : ICommandResponse<SetAsyncCallStackDepthCommand>
    {
    }
}