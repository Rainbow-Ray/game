using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace game
{
    public class GameProcess
    {
        Game gameProperties;
        Dice dice;

        public event EventHandler<GameEventArgs> WonGame;
        public event EventHandler<GameEventArgs> NeedNewRound;
        public event EventHandler<GameEventArgs> NeedNewTurn;

        public event EventHandler<GameEventArgs> LabelChange;
        public event EventHandler<GameEventArgs> LabelUpdate;
        public event EventHandler<GameEventArgs> DrawBug;
        public event EventHandler<GameEventArgs> Wait;

        public GameProcess(Game gameProperties, Dice dice) {
            this.gameProperties = gameProperties;
            this.dice = dice;
        }

        internal void startGame()
        {
            startRound();
            startTurn(gameProperties.playerList[0]);
        }

        public void RaiseEvent(EventHandler<GameEventArgs> Event, GameEventArgs eventArgs) {
            EventHandler<GameEventArgs> eventHandler = Event;
            if (Event!= null)
            {
                Event(this, eventArgs);
            }
        }
        public void nextPlayer(Player player)
        {
            var firstPlayer = gameProperties.playerList[0];
            if (gameProperties.IsLastPlayer(player))
            {
                var roundWinners = gameProperties.checkRoundWinners();
                //Два игрока могут выиграть, например при игре до 5 очков
                //var roundWinners = gameProperties.playerList;
                //gameProperties.playerList[0].score = 5;
                //gameProperties.playerList[1].score = 5;

                if (roundWinners.Count > 0)
                {
                    var gameWinners = gameProperties.getWinners();
                    if (gameWinners.Count > 0)
                    {
                        var eventsArgs = new GameEventArgs(gameWinners, gameProperties.formatPlayersScore());
                        RaiseEvent(WonGame, eventsArgs);
                    }
                    else
                    {
                        startRound();
                        startTurn(firstPlayer);
                    }
                }
                else
                {
                    startTurn(firstPlayer);
                }
            }
            else
            {
                var nextPlayer = gameProperties.playerList[player.id + 1];
                startTurn(nextPlayer);
            }
        }

        public void startRound()
        {
            gameProperties.newRound();

            var eventsArgs = new GameEventArgs(gameProperties.playerList[0]);
            RaiseEvent(NeedNewRound, eventsArgs);
        }

        public void startTurn(Player player)
        {
            var round = gameProperties.getLastRound();
            var turn = gameProperties.newTurn(player, round);
            var eventsArgs = new GameEventArgs(player);
            RaiseEvent(NeedNewTurn, eventsArgs);
        }

        public void throwDice()
        {
            var round = gameProperties.getLastRound();
            var turn = round.getLastTurn();
            var result = turn.getRollResult(dice);
            var isPartAdded = turn.addPart();

            RaiseEvent(LabelChange, new GameEventArgs(result.getResultName()));
            RaiseEvent(LabelUpdate, null);

            if (isPartAdded)
            {
                RaiseEvent(DrawBug, new GameEventArgs(turn.currentPlayer, result));
            }

            RaiseEvent(Wait, null);

            if (!gameProperties.isTurnContinuous || (!isPartAdded && gameProperties.isTurnContinuous))
            {
                nextPlayer(turn.currentPlayer);
            }
            else
            {
                startTurn(turn.currentPlayer);
            }
        }
    }
}
