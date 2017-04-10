namespace BaristaLabs.Skrapr.ChromeDevTools.DOMDebugger
{
    using Newtonsoft.Json;

    /// <summary>
    /// Returns event listeners of the given object.
    /// </summary>
    public sealed class GetEventListenersCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "DOMDebugger.getEventListeners";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

    
        
        /// <summary>
        /// Identifier of the object to return listeners for.
        /// </summary>
        
        [JsonProperty("objectId")]
        public string ObjectId
        {
            get;
            set;
        }
    
    }

    public sealed class GetEventListenersCommandResponse : ICommandResponse<GetEventListenersCommand>
    {
    
        
        /// <summary>
        /// Array of relevant listeners.
        ///</summary>
        
        [JsonProperty("listeners")]
        public EventListener[] Listeners
        {
            get;
            set;
        }
    
    }
}