using System.Runtime.Serialization;

namespace Common.DataContracts
{
    [DataContract]
    public class Board
    {
        [DataMember]
        public int size;

        [DataMember]
        public string tiles;
    }
}
