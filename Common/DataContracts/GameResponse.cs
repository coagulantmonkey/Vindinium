using System.Runtime.Serialization;

namespace Common.DataContracts
{
    [DataContract]
    public class GameResponse
    {
        [DataMember]
        public Game game;

        [DataMember]
        public Hero hero;

        [DataMember]
        public string token;

        [DataMember]
        public string viewUrl;

        [DataMember]
        public string playUrl;
    }
}
