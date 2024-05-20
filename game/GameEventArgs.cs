using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class GameEventArgs : EventArgs
    {
        public string result { get; set; }
        public List<Player> playersList { get; set; }
        public Player player { get; set; }
        public IRollResult rollResult { get; set; }
        public Bug bug { get; set; }

        public GameEventArgs() { }
        
        public GameEventArgs(Player player, IRollResult rollResult = null, Bug bug = null)
        {
            this.player = player;
            this.rollResult = rollResult;
            this.bug = bug;
        }
        public GameEventArgs(List<Player> playersList, string result)
        {
            this.playersList = playersList;
            this.result = result;
        }
        public GameEventArgs(string result)
        {
            this.result = result;
        }
    }
}
