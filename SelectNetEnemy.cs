using Checkers.Net;
using System;
using System.Windows.Forms;

namespace Checkers
{
    public partial class SelectNetEnemy : Form
    {
        private Form _form;
        private Game _game;
        private NetGame _net;

        public PeerEntry Selected
        {
            get
            {
                return lbPeerList.SelectedIndex >= 0 ? lbPeerList.SelectedItem as PeerEntry : null;
            }
        }

        public SelectNetEnemy(Form form, NetGame net, Game game)
        {
            InitializeComponent();
            _form = form;
            _game = game;
            _net = net;
            _net.ResolveProgressChanged += _net_ResolveProgressChanged;
            _net.ResolveCompleted += _net_ResolveCompleted;
        }

        private async void NetStart()
        {
            await _net.StartAsync(Properties.Settings.Default.P2PPort,
                       Properties.Settings.Default.P2PUserName, _game.Player,
                       Environment.MachineName);
            LoadPeers();
        }

        private void _net_ResolveCompleted()
        {
            UpdatePeerList();
            // Повторно включаем кнопку "обновить"
            var method = new MethodInvoker(() =>
            {
                btnRefreshPeers.Enabled = _net.CanRefreshPeers;
            });
            if (InvokeRequired)
                BeginInvoke(method);
            else
                method();
        }

        private void _net_ResolveProgressChanged()
        {
            UpdatePeerList();
        }

        private void UpdatePeerList()
        {
            var method = new MethodInvoker(() =>
            {
                lbPeerList.Items.Clear();
                foreach (var item in _net.PeerList.ToArray())
                {
                    if (item.State == PeerState.User)
                        lbPeerList.Items.Add(item);
                }
                lbPeerList.Invalidate();
            });
            if (InvokeRequired)
                BeginInvoke(method);
            else
                method();
        }

        private async void LoadPeers()
        {
            if (_net.Started)
            {
                btnRefreshPeers.Enabled = false;
                lbPeerList.Items.Clear();
                await _net.RefreshPeersAsync();
            }
        }

        private void SelectNetEnemy_Load(object sender, EventArgs e)
        {
            NetStart();
        }

        private void btnRefreshPeers_Click(object sender, EventArgs e)
        {
            LoadPeers();
        }

        private void SelectNetEnemy_FormClosing(object sender, FormClosingEventArgs e)
        {
            _net.ResolveProgressChanged -= _net_ResolveProgressChanged;
            _net.ResolveCompleted -= _net_ResolveCompleted;
        }

        private void lbPeerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSelect.Enabled = _net.Started && Selected != null && Selected.State == PeerState.User;
        }

        private void btnCreateNetGame_Click(object sender, EventArgs e)
        {
            var frm = new CreateNetGame(this, _game);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                _game.Player = frm.GetPlayer();
                _form.Invalidate();
                DialogResult = DialogResult.Yes;
                Close();
            }
        }
    }
}
