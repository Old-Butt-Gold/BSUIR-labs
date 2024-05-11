namespace TI_4
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveFile = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.InstructionStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.ProgrammerStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.LabelP = new System.Windows.Forms.Label();
            this.TextBoxP = new System.Windows.Forms.TextBox();
            this.TextBoxQ = new System.Windows.Forms.TextBox();
            this.LabelQ = new System.Windows.Forms.Label();
            this.TextBoxR = new System.Windows.Forms.TextBox();
            this.LabelR = new System.Windows.Forms.Label();
            this.ButtonR = new System.Windows.Forms.Button();
            this.LabelD = new System.Windows.Forms.Label();
            this.TextBoxD = new System.Windows.Forms.TextBox();
            this.EulerLabel = new System.Windows.Forms.Label();
            this.TextBoxEuler = new System.Windows.Forms.TextBox();
            this.RadioButtonCreate = new System.Windows.Forms.RadioButton();
            this.RadioButtonCheck = new System.Windows.Forms.RadioButton();
            this.TextBoxE = new System.Windows.Forms.TextBox();
            this.LabelE = new System.Windows.Forms.Label();
            this.PlainText = new System.Windows.Forms.TextBox();
            this.PlainLabel = new System.Windows.Forms.Label();
            this.ResultButton = new System.Windows.Forms.Button();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.TextBoxHash = new System.Windows.Forms.TextBox();
            this.TextBoxEDS = new System.Windows.Forms.TextBox();
            this.HashLabel = new System.Windows.Forms.Label();
            this.EDSLabel = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.файлToolStripMenuItem, this.ClearStrip, this.InstructionStrip, this.ProgrammerStrip });
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(876, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.OpenFile, this.SaveFile });
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // OpenFile
            // 
            this.OpenFile.Name = "OpenFile";
            this.OpenFile.Size = new System.Drawing.Size(203, 24);
            this.OpenFile.Text = "Открыть файл";
            this.OpenFile.Click += new System.EventHandler(this.OpenFilePlain_Click);
            // 
            // SaveFile
            // 
            this.SaveFile.Name = "SaveFile";
            this.SaveFile.Size = new System.Drawing.Size(203, 24);
            this.SaveFile.Text = "Сохранить в файл";
            this.SaveFile.Click += new System.EventHandler(this.SaveFilePlain_Click);
            // 
            // ClearStrip
            // 
            this.ClearStrip.Name = "ClearStrip";
            this.ClearStrip.Size = new System.Drawing.Size(123, 24);
            this.ClearStrip.Text = "Очистить поля";
            this.ClearStrip.Click += new System.EventHandler(this.ClearStrip_Click);
            // 
            // InstructionStrip
            // 
            this.InstructionStrip.Name = "InstructionStrip";
            this.InstructionStrip.Size = new System.Drawing.Size(103, 24);
            this.InstructionStrip.Text = "Инструкция";
            // 
            // ProgrammerStrip
            // 
            this.ProgrammerStrip.Name = "ProgrammerStrip";
            this.ProgrammerStrip.Size = new System.Drawing.Size(133, 24);
            this.ProgrammerStrip.Text = "О разработчике";
            // 
            // LabelP
            // 
            this.LabelP.Location = new System.Drawing.Point(12, 53);
            this.LabelP.Name = "LabelP";
            this.LabelP.Size = new System.Drawing.Size(100, 23);
            this.LabelP.TabIndex = 1;
            this.LabelP.Text = "P:";
            // 
            // TextBoxP
            // 
            this.TextBoxP.Location = new System.Drawing.Point(39, 50);
            this.TextBoxP.Name = "TextBoxP";
            this.TextBoxP.Size = new System.Drawing.Size(222, 27);
            this.TextBoxP.TabIndex = 2;
            this.TextBoxP.TextChanged += new System.EventHandler(this.ClearStrip_Click);
            // 
            // TextBoxQ
            // 
            this.TextBoxQ.Location = new System.Drawing.Point(39, 83);
            this.TextBoxQ.Name = "TextBoxQ";
            this.TextBoxQ.Size = new System.Drawing.Size(222, 27);
            this.TextBoxQ.TabIndex = 4;
            this.TextBoxQ.TextChanged += new System.EventHandler(this.ClearStrip_Click);
            // 
            // LabelQ
            // 
            this.LabelQ.Location = new System.Drawing.Point(12, 86);
            this.LabelQ.Name = "LabelQ";
            this.LabelQ.Size = new System.Drawing.Size(100, 23);
            this.LabelQ.TabIndex = 3;
            this.LabelQ.Text = "Q:";
            // 
            // TextBoxR
            // 
            this.TextBoxR.Location = new System.Drawing.Point(349, 31);
            this.TextBoxR.Multiline = true;
            this.TextBoxR.Name = "TextBoxR";
            this.TextBoxR.ReadOnly = true;
            this.TextBoxR.Size = new System.Drawing.Size(275, 61);
            this.TextBoxR.TabIndex = 5;
            // 
            // LabelR
            // 
            this.LabelR.Location = new System.Drawing.Point(306, 50);
            this.LabelR.Name = "LabelR";
            this.LabelR.Size = new System.Drawing.Size(37, 23);
            this.LabelR.TabIndex = 6;
            this.LabelR.Text = "R:";
            // 
            // ButtonR
            // 
            this.ButtonR.Location = new System.Drawing.Point(39, 126);
            this.ButtonR.Name = "ButtonR";
            this.ButtonR.Size = new System.Drawing.Size(222, 30);
            this.ButtonR.TabIndex = 7;
            this.ButtonR.Text = "Рассчитать параметры";
            this.ButtonR.UseVisualStyleBackColor = true;
            this.ButtonR.Click += new System.EventHandler(this.ButtonR_Click);
            // 
            // LabelD
            // 
            this.LabelD.Location = new System.Drawing.Point(12, 187);
            this.LabelD.Name = "LabelD";
            this.LabelD.Size = new System.Drawing.Size(173, 23);
            this.LabelD.TabIndex = 8;
            this.LabelD.Text = "Закрытая константа D:";
            // 
            // TextBoxD
            // 
            this.TextBoxD.Location = new System.Drawing.Point(12, 213);
            this.TextBoxD.Name = "TextBoxD";
            this.TextBoxD.Size = new System.Drawing.Size(249, 27);
            this.TextBoxD.TabIndex = 9;
            this.TextBoxD.TextChanged += new System.EventHandler(this.ClearStrip_Click);
            // 
            // EulerLabel
            // 
            this.EulerLabel.Location = new System.Drawing.Point(296, 126);
            this.EulerLabel.Name = "EulerLabel";
            this.EulerLabel.Size = new System.Drawing.Size(56, 23);
            this.EulerLabel.TabIndex = 10;
            this.EulerLabel.Text = "φ(R):";
            // 
            // TextBoxEuler
            // 
            this.TextBoxEuler.Location = new System.Drawing.Point(349, 108);
            this.TextBoxEuler.Multiline = true;
            this.TextBoxEuler.Name = "TextBoxEuler";
            this.TextBoxEuler.ReadOnly = true;
            this.TextBoxEuler.Size = new System.Drawing.Size(275, 61);
            this.TextBoxEuler.TabIndex = 11;
            // 
            // RadioButtonCreate
            // 
            this.RadioButtonCreate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RadioButtonCreate.Checked = true;
            this.RadioButtonCreate.Location = new System.Drawing.Point(12, 282);
            this.RadioButtonCreate.Name = "RadioButtonCreate";
            this.RadioButtonCreate.Size = new System.Drawing.Size(249, 24);
            this.RadioButtonCreate.TabIndex = 12;
            this.RadioButtonCreate.TabStop = true;
            this.RadioButtonCreate.Text = "Создание цифровой подписи";
            this.RadioButtonCreate.UseVisualStyleBackColor = true;
            this.RadioButtonCreate.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // RadioButtonCheck
            // 
            this.RadioButtonCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RadioButtonCheck.Location = new System.Drawing.Point(12, 322);
            this.RadioButtonCheck.Name = "RadioButtonCheck";
            this.RadioButtonCheck.Size = new System.Drawing.Size(249, 24);
            this.RadioButtonCheck.TabIndex = 13;
            this.RadioButtonCheck.Text = "Проверка на подлинность";
            this.RadioButtonCheck.UseVisualStyleBackColor = true;
            this.RadioButtonCheck.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // TextBoxE
            // 
            this.TextBoxE.Location = new System.Drawing.Point(349, 212);
            this.TextBoxE.Multiline = true;
            this.TextBoxE.Name = "TextBoxE";
            this.TextBoxE.ReadOnly = true;
            this.TextBoxE.Size = new System.Drawing.Size(275, 46);
            this.TextBoxE.TabIndex = 14;
            // 
            // LabelE
            // 
            this.LabelE.Location = new System.Drawing.Point(401, 178);
            this.LabelE.Name = "LabelE";
            this.LabelE.Size = new System.Drawing.Size(174, 25);
            this.LabelE.TabIndex = 15;
            this.LabelE.Text = "Открытая константа E:";
            // 
            // PlainText
            // 
            this.PlainText.Location = new System.Drawing.Point(349, 311);
            this.PlainText.Multiline = true;
            this.PlainText.Name = "PlainText";
            this.PlainText.ReadOnly = true;
            this.PlainText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.PlainText.Size = new System.Drawing.Size(502, 105);
            this.PlainText.TabIndex = 16;
            // 
            // PlainLabel
            // 
            this.PlainLabel.Location = new System.Drawing.Point(349, 285);
            this.PlainLabel.Name = "PlainLabel";
            this.PlainLabel.Size = new System.Drawing.Size(339, 23);
            this.PlainLabel.TabIndex = 17;
            this.PlainLabel.Text = "Исходное сообщение:";
            // 
            // ResultButton
            // 
            this.ResultButton.Enabled = false;
            this.ResultButton.Location = new System.Drawing.Point(12, 359);
            this.ResultButton.Name = "ResultButton";
            this.ResultButton.Size = new System.Drawing.Size(249, 57);
            this.ResultButton.TabIndex = 20;
            this.ResultButton.Text = "Рассчитать";
            this.ResultButton.UseVisualStyleBackColor = true;
            this.ResultButton.Click += new System.EventHandler(this.ResultButton_Click);
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.FileName = "openFileDialog1";
            this.OpenFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt";
            // 
            // SaveFileDialog
            // 
            this.SaveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt";
            // 
            // TextBoxHash
            // 
            this.TextBoxHash.Location = new System.Drawing.Point(296, 466);
            this.TextBoxHash.Multiline = true;
            this.TextBoxHash.Name = "TextBoxHash";
            this.TextBoxHash.ReadOnly = true;
            this.TextBoxHash.Size = new System.Drawing.Size(275, 61);
            this.TextBoxHash.TabIndex = 21;
            // 
            // TextBoxEDS
            // 
            this.TextBoxEDS.Location = new System.Drawing.Point(589, 466);
            this.TextBoxEDS.Multiline = true;
            this.TextBoxEDS.Name = "TextBoxEDS";
            this.TextBoxEDS.ReadOnly = true;
            this.TextBoxEDS.Size = new System.Drawing.Size(275, 61);
            this.TextBoxEDS.TabIndex = 22;
            // 
            // HashLabel
            // 
            this.HashLabel.Location = new System.Drawing.Point(296, 438);
            this.HashLabel.Name = "HashLabel";
            this.HashLabel.Size = new System.Drawing.Size(174, 25);
            this.HashLabel.TabIndex = 23;
            this.HashLabel.Text = "Хеш-образ:";
            // 
            // EDSLabel
            // 
            this.EDSLabel.Location = new System.Drawing.Point(589, 438);
            this.EDSLabel.Name = "EDSLabel";
            this.EDSLabel.Size = new System.Drawing.Size(174, 25);
            this.EDSLabel.TabIndex = 24;
            this.EDSLabel.Text = "ЭЦП:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(876, 634);
            this.Controls.Add(this.EDSLabel);
            this.Controls.Add(this.HashLabel);
            this.Controls.Add(this.TextBoxEDS);
            this.Controls.Add(this.TextBoxHash);
            this.Controls.Add(this.ResultButton);
            this.Controls.Add(this.PlainLabel);
            this.Controls.Add(this.PlainText);
            this.Controls.Add(this.LabelE);
            this.Controls.Add(this.TextBoxE);
            this.Controls.Add(this.RadioButtonCheck);
            this.Controls.Add(this.RadioButtonCreate);
            this.Controls.Add(this.TextBoxEuler);
            this.Controls.Add(this.EulerLabel);
            this.Controls.Add(this.TextBoxD);
            this.Controls.Add(this.LabelD);
            this.Controls.Add(this.ButtonR);
            this.Controls.Add(this.LabelR);
            this.Controls.Add(this.TextBoxR);
            this.Controls.Add(this.TextBoxQ);
            this.Controls.Add(this.LabelQ);
            this.Controls.Add(this.TextBoxP);
            this.Controls.Add(this.LabelP);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(15, 15);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Теория информации Лабораторная работа №4";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox TextBoxHash;
        private System.Windows.Forms.TextBox TextBoxEDS;
        private System.Windows.Forms.Label HashLabel;
        private System.Windows.Forms.Label EDSLabel;

        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;

        private System.Windows.Forms.Button ResultButton;

        private System.Windows.Forms.TextBox PlainText;

        private System.Windows.Forms.Label PlainLabel;

        private System.Windows.Forms.Label LabelD;

        private System.Windows.Forms.TextBox TextBoxD;

        private System.Windows.Forms.RadioButton RadioButtonCreate;
        private System.Windows.Forms.RadioButton RadioButtonCheck;

        private System.Windows.Forms.Label EulerLabel;

        private System.Windows.Forms.TextBox TextBoxE;

        private System.Windows.Forms.Label LabelE;
        private System.Windows.Forms.TextBox TextBoxEuler;

        private System.Windows.Forms.Button ButtonR;

        private System.Windows.Forms.ToolStripMenuItem InstructionStrip;
        private System.Windows.Forms.ToolStripMenuItem ProgrammerStrip;
        private System.Windows.Forms.ToolStripMenuItem SaveFile;
        private System.Windows.Forms.ToolStripMenuItem OpenFile;
        private System.Windows.Forms.Label LabelP;
        private System.Windows.Forms.TextBox TextBoxP;
        private System.Windows.Forms.TextBox TextBoxQ;
        private System.Windows.Forms.Label LabelQ;
        private System.Windows.Forms.TextBox TextBoxR;
        private System.Windows.Forms.Label LabelR;

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearStrip;

        #endregion
    }
}