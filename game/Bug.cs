using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class Bug
    {
        private BugState state { get; set; }
        private List<BugPart> parts { get; set; } = new List<BugPart>();

        public Bug()
        {
            this.state = BugState.no_body;
        }


        public BugState getState(){ return state; }

        public bool changeState(BugPart result)
        {
            if (result.rollOrder == (int)state)
            {
                if (state == BugState.no_accs)
                {
                    state = BugState.one_acc;
                }
                else if (state == BugState.no_legs)
                {
                    state = BugState.one_legs;
                }
                else
                {
                    state += 1;
                }
            }
            else if
                (state == BugState.one_acc
                && (result.name == BugParts.Eyes || result.name == BugParts.Antennaes)
                && !parts.Contains(result))
            {
                state = BugState.no_legs;
            }
            else if (state == BugState.one_legs
                    && result.name == BugParts.Legs)
            {
                state = BugState.no_tail;
            }
            else
            {
                return false;
            }
            parts.Add(result);
            return true;
        }
    }
}
