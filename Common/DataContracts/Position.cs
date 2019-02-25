using System.Runtime.Serialization;

namespace Common.DataContracts
{
    [DataContract]
    public class Position
    {
        [DataMember]
        public int x;

        [DataMember]
        public int y;
    }
}
