using game.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace game
{
    public partial class GameField : Form
    {
        Game gameProperties;
        Dice dice;
        List<Label> bugLabels = new List<Label>();
        List<Panel> panels = new List<Panel>();
        GameProcess game;

        public GameField()
        {
            InitializeComponent();
        }

        public GameField(Game newGame, Dice dice)
        {
            InitializeComponent();

            game = new GameProcess(newGame, dice);
            gameProperties = newGame;
            this.dice = dice;

            game.WonGame += Game_WonGame;
            game.NeedNewRound += Game_NeedNewRound;
            game.NeedNewTurn += Game_NeedNewTurn;
            game.LabelChange += Game_LabelChange;
            game.LabelUpdate += Game_LabelUpdate;
            game.DrawBug += Game_DrawBug;
            game.Wait += Game_Wait; ;

            game.startGame();
        }

        private void Game_Wait(object sender, GameEventArgs e)
        {
            button1.Enabled = false;
            Wait();
        }

        private void Game_DrawBug(object sender, GameEventArgs e)
        {
            drawBug(e.player, e.rollResult);
        }

        private void Game_LabelUpdate(object sender, GameEventArgs e)
        {
            label3.Update();
        }

        private void Game_LabelChange(object sender, GameEventArgs e)
        {
            var result = e.result;
            label3.Text = result;
        }

        private void Game_NeedNewTurn(object sender, GameEventArgs e)
        {
            startTurn(e.player.name);
        }

        private void Game_NeedNewRound(object sender, GameEventArgs e)
        {
            startRound();
        }

        private void Game_WonGame(object sender, GameEventArgs e)
        {
            declareWin(e.playersList, e.result);
        }


        public class Coords
        {
            public int x;
            public int y;

            public Coords(int X, int Y)
            {
                x = X; y = Y;
            }

        }


        private List<Coords> getCoords(int startX, int startY, int offsetX, int offsetY, int count, bool isHighRes)
        {
            var list = new List<Coords>();
            var newX = startX;
            if (isHighRes)
            {
                startY = (int)(startY/ 1.25);
            }

            for (int i = 0; i < count; i++)
            {
                if (i < 4)
                {
                    list.Add(new Coords(newX, startY));
                }
                else
                {
                    list.Add(new Coords(newX, startY + offsetY));
                }
                newX += offsetX;

                if (i == 3)
                {
                    newX = startX;
                }
            }
            return list;

        }


        private void GameField_Load(object sender, EventArgs e)
        {
            var count = 0;
            var x = 13;
            var isBigRes = true;
            var bigResYDiff = 1.25;
            var panelLabelcoords = getCoords(x, 270, 150, 200, gameProperties.playerList.Count, true);
            var panelncoords = getCoords(x, 295, 150, 200, gameProperties.playerList.Count, true);
            var panelBugncoords = getCoords(x, 285, 150, 200, gameProperties.playerList.Count, true);
            if (gameProperties.playerList.Count > 4)
            {
                this.Height += 200;
            }
         
            foreach (var player in gameProperties.playerList)
            {
                Label panelLabeln = new Label();
                this.Controls.Add(panelLabeln);
                panelLabeln.Text = player.name;
                panelLabeln.Location =  new Point(panelLabelcoords[player.id ].x, panelLabelcoords[player.id].y) ;
                panelLabeln.Name = $"panelLabel{count}";
                panelLabeln.Size = new Size(58, 17);

                Panel paneln = new Panel();
                Controls.Add(paneln);
                paneln.Location = new Point(panelncoords[player.id].x, panelncoords[player.id].y);
                paneln.Name = $"panel{count}";
                paneln.BackgroundImageLayout = ImageLayout.Zoom;
                paneln.Size = new Size((int)(130/1.25), (int)(130 /1.25));

                Label panelBugn = new Label();
                this.Controls.Add(panelBugn);
                panelBugn.Text = "";
                panelBugn.Location = new Point(panelBugncoords[player.id ].x, panelBugncoords[player.id].y);
                panelBugn.Name = $"panelBug{count}";
                panelBugn.Size = new Size(58, 17);
                panelBugn.AutoSize = true;

                panels.Add(paneln);
                bugLabels.Add(panelBugn);

                count += 1;
            }
        }

        private void startRound()
        {
            foreach (var label in bugLabels)
            {
                label.Text = "";
            }
            foreach (var panel in panels)
            {
                panel.Controls.Clear();
                panel.BackgroundImage = null;
            }
        }

        private void startTurn(string name)
        {
            label1.Text = $"{name}, ваш ход";
            label3.Text = "";
        }

        private void declareWin(List<Player> players, string playersScore)
        {
            var congrats = players.Count > 1 ? "Победители: " : "Победитель: ";
            foreach (var player in players)
            {
                congrats += player.name;
                congrats += "\n";
            }
            MessageBox.Show(congrats, "Есть победитель!", MessageBoxButtons.OK);
            MessageBox.Show(playersScore, "Счет игроков", MessageBoxButtons.OK);
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            game.throwDice();
        }

        private void Wait()
        {
            Thread.Sleep(500);
            button1.Enabled = true;
        }

        private void drawBug(Player player, String result)
        {
            var label = bugLabels[player.id];
            label.Text += result;
            label.Text += "\n";
        }

        private void drawBug(Player player, IRollResult result)
        {
            var label = bugLabels[player.id];
            var panel = panels[player.id];
            Bitmap image = getImage(player.currentBug, result);
            panel.BackgroundImage = image;

            //label.Text += result;
            //label.Text += "\n";

        }

        private Bitmap getImage(Bug bug, IRollResult result)
        {
            switch (bug.getState())
            {
                case BugState.no_head:
                    return Resources.Bugbody;
                case BugState.no_accs:
                    return Resources.BugHead;
                case BugState.one_acc:
                    if (result.getResult() == BugParts.Eyes)
                    {
                        return Resources.BugEyes;
                    }
                    return Resources.BugAntennaes;
                case BugState.no_legs:
                    return Resources.BugAntennaesAndEyes;
                case BugState.one_legs:
                    return Resources.BugLegsL;
                case BugState.no_tail:
                    return Resources.BugLegsR;
                case BugState.complete:
                    return Resources.BugTail;
                default:
                    return Resources.Bugbody;
            }
        }
    }
    }
