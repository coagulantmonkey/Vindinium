namespace Common.Messaging.Messages
{
    public class NewGameStartedMessage : InternalMessage
    {
        public string ViewUrl { get; set; }
    }
}
