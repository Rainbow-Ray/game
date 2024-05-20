using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class RollResultPart : IRollResult
    {
        public BugPart result { get; private set; }

        public RollResultPart(BugPart result)
        {
            this.result = result;
        }

        public BugParts getResult()
        {
            return result.name;
        }

        public string getResultName()
        {
            return result.getName();

        }

    }
}
