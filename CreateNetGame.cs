using System.Windows.Forms;

namespace Checkers
{
    public partial class CreateNetGame : Form
    {
        public CreateNetGame(Form form, Game game)
        {
            InitializeComponent();
            rbBlack.Checked = game.Player == Player.Black;
            rbWhite.Checked = game.Player == Player.White;
        }

        public Player GetPlayer()
        {
            return rbBlack.Checked ? Player.Black : Player.White;
        }
    }

}
