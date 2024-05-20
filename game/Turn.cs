using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace game
{
     public class Turn
    {
        public Player currentPlayer { get; private set; }

        public IRollResult rollResult { get; private set; }

        public Turn(Player player) { 
            currentPlayer = player;
        }

        public IRollResult getRollResult(Dice dice)
        {
            rollResult = dice.Roll();
            return rollResult;
        }

        public bool addPart()
        {
            return currentPlayer.addPart(rollResult.result);
        }

        internal void setRollResult(IRollResult roll)
        {
            rollResult = roll;
        }
    }
}
