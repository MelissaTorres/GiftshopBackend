namespace SS.GiftShop.Core
{
    [System.Serializable]
    public class SettingsException : System.Exception
    {
        public SettingsException() { }
        public SettingsException(string message) : base(message) { }
        public SettingsException(string message, System.Exception inner) : base(message, inner) { }
        protected SettingsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
