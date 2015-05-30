using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
