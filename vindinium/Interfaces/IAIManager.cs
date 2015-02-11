using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vindinium.Interfaces
{
    public interface IAIManager
    {
        #region Members
        string ViewURL { get; }
        bool Finished { get; }
        event EventHandler<string> ViewUrlChanged;
        #endregion
        void Deserialise(string jsonResponse);
        void Run();
    }
}
