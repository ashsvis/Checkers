using Checkers.Net;
using System;
using System.Windows.Forms;

namespace Checkers
{
    public partial class SelectNetEnemy : Form
    {
        private NetGame _net;

        public PeerEntry Selected
        {
            get
            {
                return lbPeerList.SelectedIndex >= 0 ? lbPeerList.SelectedItem as PeerEntry : null;
            }
        }

        public SelectNetEnemy(NetGame net)
        {
            InitializeComponent();
            _net = net;
            _net.ResolveProgressChanged += _net_ResolveProgressChanged;
            _net.ResolveCompleted += _net_ResolveCompleted;
            _net.Start(Properties.Settings.Default.P2PPort,
                       Properties.Settings.Default.P2PUserName,
                       Environment.MachineName);
        }

        private void _net_ResolveCompleted()
        {
            UpdatePeerList();
            // Повторно включаем кнопку "обновить"
            btnRefreshPeers.Enabled = _net.CanRefreshPeers;
        }

        private void _net_ResolveProgressChanged()
        {
            UpdatePeerList();
        }

        private void UpdatePeerList()
        {
            lbPeerList.Items.Clear();
            foreach (var item in _net.PeerList)
            {
                if (item.State != PeerState.Unknown)
                    lbPeerList.Items.Add(item);
            }
            lbPeerList.Invalidate();
        }

        private void LoadPeers()
        {
            if (_net.Started)
            {
                lbPeerList.Items.Clear();
                _net.RefreshPeers();
                btnRefreshPeers.Enabled = false;
            }
        }

        private void SelectNetEnemy_Load(object sender, EventArgs e)
        {
            LoadPeers();
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
    }
}
