﻿namespace Checkers
{
    partial class SelectNetEnemy
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lbPeerList = new System.Windows.Forms.ListBox();
            this.btnRefreshPeers = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Возможные противники в локальной сети:";
            // 
            // lbPeerList
            // 
            this.lbPeerList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPeerList.FormattingEnabled = true;
            this.lbPeerList.ItemHeight = 15;
            this.lbPeerList.Location = new System.Drawing.Point(12, 27);
            this.lbPeerList.Name = "lbPeerList";
            this.lbPeerList.Size = new System.Drawing.Size(329, 154);
            this.lbPeerList.TabIndex = 1;
            this.lbPeerList.SelectedIndexChanged += new System.EventHandler(this.lbPeerList_SelectedIndexChanged);
            // 
            // btnRefreshPeers
            // 
            this.btnRefreshPeers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefreshPeers.Location = new System.Drawing.Point(15, 194);
            this.btnRefreshPeers.Name = "btnRefreshPeers";
            this.btnRefreshPeers.Size = new System.Drawing.Size(75, 24);
            this.btnRefreshPeers.TabIndex = 2;
            this.btnRefreshPeers.Text = "Обновить";
            this.btnRefreshPeers.UseVisualStyleBackColor = true;
            this.btnRefreshPeers.Click += new System.EventHandler(this.btnRefreshPeers_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSelect.Enabled = false;
            this.btnSelect.Location = new System.Drawing.Point(177, 194);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 24);
            this.btnSelect.TabIndex = 4;
            this.btnSelect.Text = "Выбрать";
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(258, 194);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // SelectNetEnemy
            // 
            this.AcceptButton = this.btnSelect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(353, 232);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.btnRefreshPeers);
            this.Controls.Add(this.lbPeerList);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectNetEnemy";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Выбор противника по сети";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectNetEnemy_FormClosing);
            this.Load += new System.EventHandler(this.SelectNetEnemy_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbPeerList;
        private System.Windows.Forms.Button btnRefreshPeers;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
    }
}