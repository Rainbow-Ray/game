using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game
{
    public class Game
    {
        public List<Player> playerList { get; private set; } = new List<Player>();
        public List<Round> roundList { get; private set; } = new List<Round>();
        public bool isTurnContinuous { get; private set; }
        private bool isPointsToWin { get; set; }
        private int numberOfBugsToWin { get; set; }
        private int numberOfPointsToWin { get; set; }
        private List<Player> winners = new List<Player>();

        public Game(bool isTurnContinuous, bool isPointsToWin, int numberOfPointsToWin) {
            this.isTurnContinuous = isTurnContinuous;
            this.isPointsToWin = isPointsToWin;
            if (isPointsToWin)
            {
                this.numberOfPointsToWin = numberOfPointsToWin;
            }
            else
            {
                this.numberOfBugsToWin = numberOfBugsToWin;
            }
        }

        internal void addPlayers(ListBox.ObjectCollection items)
        {
            var playerList = new List<Player>();
            foreach (var player in items)
            {
                var name = player.ToString();
                var nPlayer = new Player(name, playerList.Count);
                playerList.Add(nPlayer);
            }
            this.playerList = playerList;
        }

        public Dice createDice(string type)
        {
            return new Dice(type);
        }

        internal List<Player> getWinners()
        {
            if (numberOfBugsToWin != 0 || numberOfPointsToWin != 0)
            {
                return checkWinners();
            }
            else
            {
                return checkRoundWinners();
            }
        }

        public List<Player> checkWinners()
        {
            foreach (var player in playerList)
            {
                if (isPointsToWin)
                {
                    if (player.score >= numberOfPointsToWin)
                    {
                        winners.Add(player);
                    }
                }
                else
                {
                    if (player.numberOfBugs >= numberOfBugsToWin)
                    {
                        winners.Add(player);
                    }
                }
            }
            return winners;
        }

        internal List<Player> checkRoundWinners()
        {
            var winners = new List<Player>();
            foreach (var player in playerList)
            {
                if (player.isBugComplete())
                {
                    winners.Add(player);
                }
            }
            return winners;
        }

        internal Round newRound()
        {
            var round = new Round();
            roundList.Add(round);

            foreach (var player in playerList)
            {
                player.createCurrentBug();
            }

            return round;
        }

        internal bool IsLastPlayer(Player player)
        {
            return player.id >= playerList.Count - 1;
        }

        internal string formatPlayersScore()
        {
            StringBuilder a = new StringBuilder();
            a.AppendLine($"{"Игрок",-30} {"Очки",5}");
            foreach (var player in playerList)
            {
                a.AppendLine($"{player.name,-30} {player.score,5}");
                a.AppendLine($"");
            }
            return a.ToString();
        }
        internal Turn newTurn(Player player, Round round)
        {
            var turn = new Turn(player);
            round.addTurn(turn);
            return turn;
        }

        internal Round getLastRound()
        {
            return roundList.Last();
        }




    }
}
