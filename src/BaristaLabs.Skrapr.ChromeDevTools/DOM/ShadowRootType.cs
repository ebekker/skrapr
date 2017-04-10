namespace BaristaLabs.Skrapr.ChromeDevTools.DOM
{
    /// <summary>
    /// Shadow root type.
    /// </summary>
    public sealed class ShadowRootType
    {
        private string m_value;

        private ShadowRootType(string value)
        {
            m_value = value;
        }

        public override string ToString()
        {
            return m_value;
        }

    
        public static ShadowRootType UserAgent = new ShadowRootType("user-agent");
    
        public static ShadowRootType Open = new ShadowRootType("open");
    
        public static ShadowRootType Closed = new ShadowRootType("closed");
    
    }
}