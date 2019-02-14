using Checkers.Net;
using System;
using System.Windows.Forms;

namespace Checkers
{
    public partial class CreateNetGame : Form
    {
        private NetGame _net;
        private Game _game;

        public CreateNetGame(Form form, NetGame net, Game game)
        {
            InitializeComponent();
            _net = net;
            _game = game;
            rbBlack.Checked = game.Player == Player.Black;
            rbWhite.Checked = game.Player == Player.White;
        }

        private void CreateNetGame_Load(object sender, EventArgs e)
        {
            NetStart();
        }

        private async void NetStart()
        {
            await _net.StartAsync(Properties.Settings.Default.P2PPort,
                       Properties.Settings.Default.P2PUserName, _game.Player,
                       Environment.MachineName);
        }

        public Player GetPlayer()
        {
            return rbBlack.Checked ? Player.Black : Player.White;
        }
    }

}
