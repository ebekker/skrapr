namespace BaristaLabs.Skrapr.ChromeDevTools.Target
{
    using Newtonsoft.Json;

    /// <summary>
    /// Enables target discovery for the specified locations, when &lt;code&gt;setDiscoverTargets&lt;/code&gt; was set to &lt;code&gt;true&lt;/code&gt;.
    /// </summary>
    public sealed class SetRemoteLocationsCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Target.setRemoteLocations";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

        /// <summary>
        /// List of remote locations.
        /// </summary>
        [JsonProperty("locations")]
        public RemoteLocation[] Locations
        {
            get;
            set;
        }
    }

    public sealed class SetRemoteLocationsCommandResponse : ICommandResponse<SetRemoteLocationsCommand>
    {
    }
}