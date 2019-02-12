namespace Checkers
{
    partial class CheckersForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckersForm));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.tsmiGame = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewGame = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSaveGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTools = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSelectSide = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiWhiteSide = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBlackSide = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiApplicationMode = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGameMode = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCollocationMode = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTunings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRules = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mainStatus = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainTools = new System.Windows.Forms.ToolStrip();
            this.tsbNewGame = new System.Windows.Forms.ToolStripButton();
            this.tsbOpenGame = new System.Windows.Forms.ToolStripButton();
            this.tsbSaveGame = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lbBlackScore = new System.Windows.Forms.Label();
            this.lbWhiteScore = new System.Windows.Forms.Label();
            this.lvLog = new System.Windows.Forms.ListView();
            this.chStep = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWhite = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBlack = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelLog = new System.Windows.Forms.Panel();
            this.saveGameDialog = new System.Windows.Forms.SaveFileDialog();
            this.openGameDialog = new System.Windows.Forms.OpenFileDialog();
            this.tsmiNetGame = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.mainStatus.SuspendLayout();
            this.mainTools.SuspendLayout();
            this.panelLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiGame,
            this.tsmiTools,
            this.tsmiHelp});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.mainMenu.Size = new System.Drawing.Size(500, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // tsmiGame
            // 
            this.tsmiGame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNewGame,
            this.tsmiNetGame,
            this.tsmiOpenGame,
            this.toolStripSeparator,
            this.tsmiSaveGame,
            this.toolStripSeparator2,
            this.tsmiExit});
            this.tsmiGame.Name = "tsmiGame";
            this.tsmiGame.Size = new System.Drawing.Size(46, 20);
            this.tsmiGame.Text = "&Игра";
            // 
            // tsmiNewGame
            // 
            this.tsmiNewGame.Image = ((System.Drawing.Image)(resources.GetObject("tsmiNewGame.Image")));
            this.tsmiNewGame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiNewGame.Name = "tsmiNewGame";
            this.tsmiNewGame.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.tsmiNewGame.Size = new System.Drawing.Size(180, 22);
            this.tsmiNewGame.Text = "&Новая";
            this.tsmiNewGame.Click += new System.EventHandler(this.tsmiNewGame_Click);
            // 
            // tsmiOpenGame
            // 
            this.tsmiOpenGame.Image = ((System.Drawing.Image)(resources.GetObject("tsmiOpenGame.Image")));
            this.tsmiOpenGame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiOpenGame.Name = "tsmiOpenGame";
            this.tsmiOpenGame.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.tsmiOpenGame.Size = new System.Drawing.Size(180, 22);
            this.tsmiOpenGame.Text = "&Открыть";
            this.tsmiOpenGame.Click += new System.EventHandler(this.tsmiOpenGame_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(177, 6);
            // 
            // tsmiSaveGame
            // 
            this.tsmiSaveGame.Enabled = false;
            this.tsmiSaveGame.Image = ((System.Drawing.Image)(resources.GetObject("tsmiSaveGame.Image")));
            this.tsmiSaveGame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiSaveGame.Name = "tsmiSaveGame";
            this.tsmiSaveGame.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmiSaveGame.Size = new System.Drawing.Size(180, 22);
            this.tsmiSaveGame.Text = "&Сохранить";
            this.tsmiSaveGame.Click += new System.EventHandler(this.tsmiSaveGame_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(180, 22);
            this.tsmiExit.Text = "Вы&ход";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // tsmiTools
            // 
            this.tsmiTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSelectSide,
            this.toolStripMenuItem1,
            this.tsmiApplicationMode,
            this.tsmiTunings});
            this.tsmiTools.Name = "tsmiTools";
            this.tsmiTools.Size = new System.Drawing.Size(79, 20);
            this.tsmiTools.Text = "&Настройки";
            // 
            // tsmiSelectSide
            // 
            this.tsmiSelectSide.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiWhiteSide,
            this.tsmiBlackSide});
            this.tsmiSelectSide.Name = "tsmiSelectSide";
            this.tsmiSelectSide.Size = new System.Drawing.Size(162, 22);
            this.tsmiSelectSide.Text = "Выбор стороны";
            this.tsmiSelectSide.DropDownOpening += new System.EventHandler(this.tsmiSelectSide_DropDownOpening);
            // 
            // tsmiWhiteSide
            // 
            this.tsmiWhiteSide.Name = "tsmiWhiteSide";
            this.tsmiWhiteSide.Size = new System.Drawing.Size(117, 22);
            this.tsmiWhiteSide.Text = "Белые";
            this.tsmiWhiteSide.Click += new System.EventHandler(this.tsmiPlayerWhite_Click);
            // 
            // tsmiBlackSide
            // 
            this.tsmiBlackSide.Name = "tsmiBlackSide";
            this.tsmiBlackSide.Size = new System.Drawing.Size(117, 22);
            this.tsmiBlackSide.Text = "Чёрные";
            this.tsmiBlackSide.Click += new System.EventHandler(this.tsmiPlayerWhite_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(159, 6);
            // 
            // tsmiApplicationMode
            // 
            this.tsmiApplicationMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiGameMode,
            this.tsmiCollocationMode});
            this.tsmiApplicationMode.Name = "tsmiApplicationMode";
            this.tsmiApplicationMode.Size = new System.Drawing.Size(162, 22);
            this.tsmiApplicationMode.Text = "Режим работы";
            this.tsmiApplicationMode.DropDownOpening += new System.EventHandler(this.tsmiApplicationMode_DropDownOpening);
            // 
            // tsmiGameMode
            // 
            this.tsmiGameMode.Name = "tsmiGameMode";
            this.tsmiGameMode.Size = new System.Drawing.Size(142, 22);
            this.tsmiGameMode.Text = "Игра";
            this.tsmiGameMode.Click += new System.EventHandler(this.tsmiGameMode_Click);
            // 
            // tsmiCollocationMode
            // 
            this.tsmiCollocationMode.Name = "tsmiCollocationMode";
            this.tsmiCollocationMode.Size = new System.Drawing.Size(142, 22);
            this.tsmiCollocationMode.Text = "Расстановка";
            this.tsmiCollocationMode.Click += new System.EventHandler(this.tsmiCollocationMode_Click);
            // 
            // tsmiTunings
            // 
            this.tsmiTunings.Enabled = false;
            this.tsmiTunings.Name = "tsmiTunings";
            this.tsmiTunings.Size = new System.Drawing.Size(162, 22);
            this.tsmiTunings.Text = "&Параметры...";
            this.tsmiTunings.Click += new System.EventHandler(this.tsmiTunings_Click);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRules,
            this.toolStripSeparator5,
            this.tsmiAbout});
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(65, 20);
            this.tsmiHelp.Text = "Спра&вка";
            // 
            // tsmiRules
            // 
            this.tsmiRules.Enabled = false;
            this.tsmiRules.Name = "tsmiRules";
            this.tsmiRules.Size = new System.Drawing.Size(162, 22);
            this.tsmiRules.Text = "&Правила игры...";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(159, 6);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Enabled = false;
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(162, 22);
            this.tsmiAbout.Text = "&О программе...";
            // 
            // mainStatus
            // 
            this.mainStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status});
            this.mainStatus.Location = new System.Drawing.Point(0, 366);
            this.mainStatus.Name = "mainStatus";
            this.mainStatus.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.mainStatus.Size = new System.Drawing.Size(500, 22);
            this.mainStatus.TabIndex = 1;
            this.mainStatus.Text = "statusStrip1";
            // 
            // status
            // 
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(45, 17);
            this.status.Text = "Готово";
            // 
            // mainTools
            // 
            this.mainTools.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewGame,
            this.tsbOpenGame,
            this.tsbSaveGame,
            this.toolStripSeparator1});
            this.mainTools.Location = new System.Drawing.Point(0, 24);
            this.mainTools.Name = "mainTools";
            this.mainTools.Size = new System.Drawing.Size(500, 25);
            this.mainTools.TabIndex = 2;
            this.mainTools.Text = "toolStrip1";
            // 
            // tsbNewGame
            // 
            this.tsbNewGame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNewGame.Image = ((System.Drawing.Image)(resources.GetObject("tsbNewGame.Image")));
            this.tsbNewGame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewGame.Name = "tsbNewGame";
            this.tsbNewGame.Size = new System.Drawing.Size(23, 22);
            this.tsbNewGame.Text = "&Новая игра";
            this.tsbNewGame.Click += new System.EventHandler(this.tsmiNewGame_Click);
            // 
            // tsbOpenGame
            // 
            this.tsbOpenGame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpenGame.Image = ((System.Drawing.Image)(resources.GetObject("tsbOpenGame.Image")));
            this.tsbOpenGame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpenGame.Name = "tsbOpenGame";
            this.tsbOpenGame.Size = new System.Drawing.Size(23, 22);
            this.tsbOpenGame.Text = "&Открыть";
            this.tsbOpenGame.Click += new System.EventHandler(this.tsmiOpenGame_Click);
            // 
            // tsbSaveGame
            // 
            this.tsbSaveGame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSaveGame.Enabled = false;
            this.tsbSaveGame.Image = ((System.Drawing.Image)(resources.GetObject("tsbSaveGame.Image")));
            this.tsbSaveGame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSaveGame.Name = "tsbSaveGame";
            this.tsbSaveGame.Size = new System.Drawing.Size(23, 22);
            this.tsbSaveGame.Text = "&Сохранить";
            this.tsbSaveGame.Click += new System.EventHandler(this.tsmiSaveGame_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lbBlackScore
            // 
            this.lbBlackScore.BackColor = System.Drawing.Color.Black;
            this.lbBlackScore.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbBlackScore.ForeColor = System.Drawing.Color.White;
            this.lbBlackScore.Location = new System.Drawing.Point(121, 25);
            this.lbBlackScore.Name = "lbBlackScore";
            this.lbBlackScore.Size = new System.Drawing.Size(89, 25);
            this.lbBlackScore.TabIndex = 2;
            this.lbBlackScore.Text = "Чёрные: 0";
            this.lbBlackScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbWhiteScore
            // 
            this.lbWhiteScore.BackColor = System.Drawing.Color.White;
            this.lbWhiteScore.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbWhiteScore.Location = new System.Drawing.Point(31, 25);
            this.lbWhiteScore.Name = "lbWhiteScore";
            this.lbWhiteScore.Size = new System.Drawing.Size(89, 25);
            this.lbWhiteScore.TabIndex = 2;
            this.lbWhiteScore.Text = "Белые: 0";
            this.lbWhiteScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lvLog
            // 
            this.lvLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chStep,
            this.chWhite,
            this.chBlack});
            this.lvLog.FullRowSelect = true;
            this.lvLog.GridLines = true;
            this.lvLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvLog.HideSelection = false;
            this.lvLog.Location = new System.Drawing.Point(8, 74);
            this.lvLog.MultiSelect = false;
            this.lvLog.Name = "lvLog";
            this.lvLog.ShowGroups = false;
            this.lvLog.ShowItemToolTips = true;
            this.lvLog.Size = new System.Drawing.Size(221, 235);
            this.lvLog.TabIndex = 1;
            this.lvLog.UseCompatibleStateImageBehavior = false;
            this.lvLog.View = System.Windows.Forms.View.Details;
            this.lvLog.VirtualMode = true;
            this.lvLog.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.lvLog_RetrieveVirtualItem);
            this.lvLog.SelectedIndexChanged += new System.EventHandler(this.lvLog_SelectedIndexChanged);
            // 
            // chStep
            // 
            this.chStep.Text = "Ход";
            this.chStep.Width = 40;
            // 
            // chWhite
            // 
            this.chWhite.Text = "Белые";
            this.chWhite.Width = 80;
            // 
            // chBlack
            // 
            this.chBlack.Text = "Чёрные";
            this.chBlack.Width = 80;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Счёт:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Партия:";
            // 
            // panelLog
            // 
            this.panelLog.BackColor = System.Drawing.Color.DarkGray;
            this.panelLog.Controls.Add(this.lvLog);
            this.panelLog.Controls.Add(this.label2);
            this.panelLog.Controls.Add(this.lbBlackScore);
            this.panelLog.Controls.Add(this.lbWhiteScore);
            this.panelLog.Controls.Add(this.label1);
            this.panelLog.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelLog.Location = new System.Drawing.Point(263, 49);
            this.panelLog.Name = "panelLog";
            this.panelLog.Size = new System.Drawing.Size(237, 317);
            this.panelLog.TabIndex = 5;
            // 
            // saveGameDialog
            // 
            this.saveGameDialog.DefaultExt = "che";
            this.saveGameDialog.Filter = "*.che|*.che";
            this.saveGameDialog.Title = "Сохранить игру";
            // 
            // openGameDialog
            // 
            this.openGameDialog.DefaultExt = "che";
            this.openGameDialog.Filter = "*.che|*.che";
            this.openGameDialog.Title = "Загрузить игру";
            // 
            // tsmiNetGame
            // 
            this.tsmiNetGame.Name = "tsmiNetGame";
            this.tsmiNetGame.Size = new System.Drawing.Size(180, 22);
            this.tsmiNetGame.Text = "Игра по сети...";
            this.tsmiNetGame.Click += new System.EventHandler(this.tsmiNetGame_Click);
            // 
            // CheckersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 388);
            this.Controls.Add(this.panelLog);
            this.Controls.Add(this.mainTools);
            this.Controls.Add(this.mainStatus);
            this.Controls.Add(this.mainMenu);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.mainMenu;
            this.MaximizeBox = false;
            this.Name = "CheckersForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Шашки";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CheckersForm_FormClosing);
            this.Load += new System.EventHandler(this.CheckersForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CheckersForm_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CheckersForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CheckersForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CheckersForm_MouseUp);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.mainStatus.ResumeLayout(false);
            this.mainStatus.PerformLayout();
            this.mainTools.ResumeLayout(false);
            this.mainTools.PerformLayout();
            this.panelLog.ResumeLayout(false);
            this.panelLog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiGame;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewGame;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenGame;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveGame;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolStripMenuItem tsmiTools;
        private System.Windows.Forms.ToolStripMenuItem tsmiTunings;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiRules;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.StatusStrip mainStatus;
        private System.Windows.Forms.ToolStripStatusLabel status;
        private System.Windows.Forms.ToolStrip mainTools;
        private System.Windows.Forms.ToolStripButton tsbNewGame;
        private System.Windows.Forms.ToolStripButton tsbOpenGame;
        private System.Windows.Forms.ToolStripButton tsbSaveGame;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvLog;
        private System.Windows.Forms.ColumnHeader chStep;
        private System.Windows.Forms.ColumnHeader chWhite;
        private System.Windows.Forms.ColumnHeader chBlack;
        private System.Windows.Forms.Label lbBlackScore;
        private System.Windows.Forms.Label lbWhiteScore;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem tsmiSelectSide;
        private System.Windows.Forms.ToolStripMenuItem tsmiWhiteSide;
        private System.Windows.Forms.ToolStripMenuItem tsmiBlackSide;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmiApplicationMode;
        private System.Windows.Forms.ToolStripMenuItem tsmiGameMode;
        private System.Windows.Forms.ToolStripMenuItem tsmiCollocationMode;
        private System.Windows.Forms.Panel panelLog;
        private System.Windows.Forms.SaveFileDialog saveGameDialog;
        private System.Windows.Forms.OpenFileDialog openGameDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiNetGame;
    }
}

