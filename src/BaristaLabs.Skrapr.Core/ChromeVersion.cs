﻿namespace BaristaLabs.Skrapr
{
    using Newtonsoft.Json;

    public class ChromeVersion
    {
        [JsonProperty(PropertyName = "Browser")]
        public string Browser
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "Protocol-Version")]
        public string ProtocolVersion
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "User-Agent")]
        public string UserAgent
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "V8-Version")]
        public string V8Version
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "WebKit-Version")]
        public string WebKitVersion
        {
            get;
            set;
        }
    }
}
