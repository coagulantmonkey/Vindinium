using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataContracts
{
    [DataContract]
    public class Hero
    {
        [DataMember]
        public int id;

        [DataMember]
        public string name;

        [DataMember]
        public int elo;

        [DataMember]
        public Position pos;

        [DataMember]
        public int life;

        [DataMember]
        public int gold;

        [DataMember]
        public int mineCount;

        [DataMember]
        public Position spawnPos;

        [DataMember]
        public bool crashed;
    }
}
