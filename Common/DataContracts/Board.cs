using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
