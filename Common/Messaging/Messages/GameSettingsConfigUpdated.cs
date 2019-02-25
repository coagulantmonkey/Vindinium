namespace Common.Messaging.Messages
{
    public class GameSettingsConfigUpdated : InternalMessage
    {
        public int NumberOfTurns { get; set; }
        public string PrivateKey { get; set; }
        public string ServerURL { get; set; }
        public bool TrainingMode { get; set; }
    }
}
