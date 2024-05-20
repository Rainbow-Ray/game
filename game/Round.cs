using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class Round
    {
        private List<Turn> turnsOfPlayer { get; set; } = new List<Turn>();

        public Round() {}
        
        internal void addTurn(Turn turn)
        {
            turnsOfPlayer.Add(turn);
        }

        internal Turn getLastTurn() {
            return turnsOfPlayer.Last();
        }

    }
}
