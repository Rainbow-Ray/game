using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public interface IRollResult
    {
        BugPart result { get; }
        BugParts getResult();
        
        string getResultName();
    }
}
