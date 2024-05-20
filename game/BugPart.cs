using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class BugPart
    {
        public BugParts name { get; private set; }
        public int diceNumber { get; private set; }
        public int rollOrder { get; private set; }

        public BugPart(BugParts name, int diceNumber, int rollOrder)
        {
            this.name = name;
            this.diceNumber = diceNumber;
            this.rollOrder = rollOrder;
        }

        public string getName()
        {
            switch (name)
            {
                case BugParts.Body:
                    return "Туловище";
                case BugParts.Head:
                    return "Голова";
                case BugParts.Eyes:
                    return "Глаза";                    
                case BugParts.Antennaes:
                    return "Усики";
                case BugParts.Legs:
                    return "Ножки";
                case BugParts.Tail:
                    return "Хвост";
                default:
                    return "";
            }
        }

    }
}
