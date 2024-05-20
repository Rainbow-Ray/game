using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace game
{
    public partial class GameProperties : Form
    {
        public GameProperties()
        {
            InitializeComponent();
        }

        private void GameProperties_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length < 1)
            {
                MessageBox.Show("Введите имя","Игрок без имени", MessageBoxButtons.OK);
            }
            else
            {
                listBox1.Items.Add(textBox1.Text);
            }
            textBox1.Clear();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var item in groupBox2.Controls)
            {
                ((Control)item).Enabled = checkBox1.Checked ? true : false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (checkFields())
            {

                var isPointsToWin = radioButton4.Checked ? true : false;
                var ifTurnIsContinuous = checkBox2.Checked ? true : false;
                var numberToWin = int.Parse(numericUpDown1.Value.ToString());

                Game newGame = new Game(ifTurnIsContinuous, isPointsToWin, numberToWin);
                newGame.addPlayers(listBox1.Items);
                var type = radioButton1.Checked ? "numbers" : "parts";
                var dice = newGame.createDice(type);
                GameField game = new GameField(newGame, dice);
                game.ShowDialog();
                this.Close();
            }

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Value = 0;
        }

        private bool checkFields()
        {
            if (checkBox1.Checked)
            {
                if (numericUpDown1.Value == 0)
                {
                    MessageBox.Show("Введите, до какого количества очков или жучков вы хотите играть", "Не задано количество для выигрыша", MessageBoxButtons.OK);
                    return false;
                }
            }
            if (listBox1.Items.Count < 2)
            {
                MessageBox.Show("Введите больше игроков, минимальное количество: 2", "Слишком мало игроков", MessageBoxButtons.OK);
                return false;
            }
            if (listBox1.Items.Count > 8)
            {
                MessageBox.Show("Введите меньше игроков, максимальное количество: 8", "Слишком много игроков", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            {
                var player = listBox1.SelectedItems[0];
                listBox1.Items.Remove(player);
            }
        }




        //private void enterPointsToWin(int number)
        //{
        //   // newGame.numberOfBugsToWin = number;
        //}

        //private void enterNewPlayer(string name)
        //{
        //    //newGame.addPlayer(name);
        //}

        //private void chooseIfGameIsContinuous(bool choice)
        //{
        //   //newGame.isTurnContinuous = choice;
        //}

        //private void chooseDiceType(string type)
        //{
        //}

        //private void chooseBugsOrPointsToWin(bool result)
        //{
        //    //newGame.isPointsToWin = result;
        //}

        //private void radioButton3_CheckedChanged(object sender, EventArgs e)
        //{
        //    //numericUpDown1.Enabled = radioButton3.Checked ? true : false;
        //    //numericUpDown1.Value = 1;
        //}
    }
}
