namespace TI_2;

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
        this.ResultButton = new System.Windows.Forms.Button();
        this.RegisterTextBox = new System.Windows.Forms.TextBox();
        this.LabelRegister = new System.Windows.Forms.Label();
        this.PlainTextBox = new System.Windows.Forms.TextBox();
        this.PlainLabel = new System.Windows.Forms.Label();
        this.KeyTextBox = new System.Windows.Forms.TextBox();
        this.KeyLabel = new System.Windows.Forms.Label();
        this.CipherTextBox = new System.Windows.Forms.TextBox();
        this.LabelCipherText = new System.Windows.Forms.Label();
        this.LengthLabel = new System.Windows.Forms.Label();
        this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
        this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
        this.panel1 = new System.Windows.Forms.Panel();
        this.panel7 = new System.Windows.Forms.Panel();
        this.panel2 = new System.Windows.Forms.Panel();
        this.panel5 = new System.Windows.Forms.Panel();
        this.panel4 = new System.Windows.Forms.Panel();
        this.panel3 = new System.Windows.Forms.Panel();
        this.panel6 = new System.Windows.Forms.Panel();
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.FileItem = new System.Windows.Forms.ToolStripMenuItem();
        this.OpenFileItem = new System.Windows.Forms.ToolStripMenuItem();
        this.SaveFileItem = new System.Windows.Forms.ToolStripMenuItem();
        this.ClearItem = new System.Windows.Forms.ToolStripMenuItem();
        this.InstructionItem = new System.Windows.Forms.ToolStripMenuItem();
        this.ProgrammerItem = new System.Windows.Forms.ToolStripMenuItem();
        this.panel1.SuspendLayout();
        this.panel7.SuspendLayout();
        this.panel2.SuspendLayout();
        this.panel5.SuspendLayout();
        this.panel4.SuspendLayout();
        this.panel3.SuspendLayout();
        this.panel6.SuspendLayout();
        this.menuStrip1.SuspendLayout();
        this.SuspendLayout();
        // 
        // ResultButton
        // 
        this.ResultButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(150)))));
        this.ResultButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.ResultButton.ForeColor = System.Drawing.Color.White;
        this.ResultButton.Location = new System.Drawing.Point(535, 87);
        this.ResultButton.Name = "ResultButton";
        this.ResultButton.Size = new System.Drawing.Size(246, 41);
        this.ResultButton.TabIndex = 3;
        this.ResultButton.Text = "Зашифровать/Дешифровать";
        this.ResultButton.UseVisualStyleBackColor = false;
        this.ResultButton.Click += new System.EventHandler(this.ResultButton_Click);
        // 
        // RegisterTextBox
        // 
        this.RegisterTextBox.Location = new System.Drawing.Point(12, 39);
        this.RegisterTextBox.Multiline = true;
        this.RegisterTextBox.Name = "RegisterTextBox";
        this.RegisterTextBox.Size = new System.Drawing.Size(769, 42);
        this.RegisterTextBox.TabIndex = 4;
        this.RegisterTextBox.TextChanged += new System.EventHandler(this.RegisterTextBox_TextChanged);
        // 
        // LabelRegister
        // 
        this.LabelRegister.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
        this.LabelRegister.Location = new System.Drawing.Point(223, -3);
        this.LabelRegister.Name = "LabelRegister";
        this.LabelRegister.Size = new System.Drawing.Size(308, 29);
        this.LabelRegister.TabIndex = 5;
        this.LabelRegister.Text = "Состояние регистра (29 состояний):";
        this.LabelRegister.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // PlainTextBox
        // 
        this.PlainTextBox.Location = new System.Drawing.Point(11, 182);
        this.PlainTextBox.Multiline = true;
        this.PlainTextBox.Name = "PlainTextBox";
        this.PlainTextBox.ReadOnly = true;
        this.PlainTextBox.Size = new System.Drawing.Size(369, 95);
        this.PlainTextBox.TabIndex = 6;
        // 
        // PlainLabel
        // 
        this.PlainLabel.Location = new System.Drawing.Point(100, 0);
        this.PlainLabel.Name = "PlainLabel";
        this.PlainLabel.Size = new System.Drawing.Size(171, 29);
        this.PlainLabel.TabIndex = 7;
        this.PlainLabel.Text = "Исходный текст:";
        this.PlainLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // KeyTextBox
        // 
        this.KeyTextBox.Location = new System.Drawing.Point(12, 46);
        this.KeyTextBox.Multiline = true;
        this.KeyTextBox.Name = "KeyTextBox";
        this.KeyTextBox.ReadOnly = true;
        this.KeyTextBox.Size = new System.Drawing.Size(369, 95);
        this.KeyTextBox.TabIndex = 8;
        // 
        // KeyLabel
        // 
        this.KeyLabel.Location = new System.Drawing.Point(78, 0);
        this.KeyLabel.Name = "KeyLabel";
        this.KeyLabel.Size = new System.Drawing.Size(206, 29);
        this.KeyLabel.TabIndex = 9;
        this.KeyLabel.Text = "Сгенерированный ключ:";
        this.KeyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // CipherTextBox
        // 
        this.CipherTextBox.Location = new System.Drawing.Point(14, 43);
        this.CipherTextBox.Multiline = true;
        this.CipherTextBox.Name = "CipherTextBox";
        this.CipherTextBox.ReadOnly = true;
        this.CipherTextBox.Size = new System.Drawing.Size(354, 234);
        this.CipherTextBox.TabIndex = 10;
        // 
        // LabelCipherText
        // 
        this.LabelCipherText.Location = new System.Drawing.Point(89, 0);
        this.LabelCipherText.Name = "LabelCipherText";
        this.LabelCipherText.Size = new System.Drawing.Size(197, 29);
        this.LabelCipherText.TabIndex = 11;
        this.LabelCipherText.Text = "Зашифрованный текст:";
        this.LabelCipherText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // LengthLabel
        // 
        this.LengthLabel.ForeColor = System.Drawing.SystemColors.Desktop;
        this.LengthLabel.Location = new System.Drawing.Point(12, 84);
        this.LengthLabel.Name = "LengthLabel";
        this.LengthLabel.Size = new System.Drawing.Size(249, 41);
        this.LengthLabel.TabIndex = 12;
        // 
        // OpenFileDialog
        // 
        this.OpenFileDialog.FileName = "openFileDialog1";
        // 
        // panel1
        // 
        this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(189)))));
        this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.panel1.Controls.Add(this.panel7);
        this.panel1.Controls.Add(this.RegisterTextBox);
        this.panel1.Controls.Add(this.LengthLabel);
        this.panel1.Controls.Add(this.ResultButton);
        this.panel1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
        this.panel1.Location = new System.Drawing.Point(12, 27);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(804, 147);
        this.panel1.TabIndex = 13;
        // 
        // panel7
        // 
        this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(188)))), ((int)(((byte)(147)))));
        this.panel7.Controls.Add(this.LabelRegister);
        this.panel7.Location = new System.Drawing.Point(12, 3);
        this.panel7.Name = "panel7";
        this.panel7.Size = new System.Drawing.Size(769, 29);
        this.panel7.TabIndex = 13;
        // 
        // panel2
        // 
        this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(189)))));
        this.panel2.Controls.Add(this.panel5);
        this.panel2.Controls.Add(this.panel4);
        this.panel2.Controls.Add(this.KeyTextBox);
        this.panel2.Controls.Add(this.PlainTextBox);
        this.panel2.Location = new System.Drawing.Point(12, 193);
        this.panel2.Name = "panel2";
        this.panel2.Size = new System.Drawing.Size(395, 286);
        this.panel2.TabIndex = 14;
        // 
        // panel5
        // 
        this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(188)))), ((int)(((byte)(147)))));
        this.panel5.Controls.Add(this.PlainLabel);
        this.panel5.Location = new System.Drawing.Point(12, 147);
        this.panel5.Name = "panel5";
        this.panel5.Size = new System.Drawing.Size(368, 29);
        this.panel5.TabIndex = 11;
        // 
        // panel4
        // 
        this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(188)))), ((int)(((byte)(147)))));
        this.panel4.Controls.Add(this.KeyLabel);
        this.panel4.Location = new System.Drawing.Point(13, 11);
        this.panel4.Name = "panel4";
        this.panel4.Size = new System.Drawing.Size(368, 29);
        this.panel4.TabIndex = 10;
        // 
        // panel3
        // 
        this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(189)))));
        this.panel3.Controls.Add(this.panel6);
        this.panel3.Controls.Add(this.CipherTextBox);
        this.panel3.Location = new System.Drawing.Point(426, 193);
        this.panel3.Name = "panel3";
        this.panel3.Size = new System.Drawing.Size(390, 286);
        this.panel3.TabIndex = 15;
        // 
        // panel6
        // 
        this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(188)))), ((int)(((byte)(147)))));
        this.panel6.Controls.Add(this.LabelCipherText);
        this.panel6.Location = new System.Drawing.Point(14, 11);
        this.panel6.Name = "panel6";
        this.panel6.Size = new System.Drawing.Size(354, 29);
        this.panel6.TabIndex = 12;
        // 
        // menuStrip1
        // 
        this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(150)))));
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.FileItem, this.ClearItem, this.InstructionItem, this.ProgrammerItem });
        this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
        this.menuStrip1.Size = new System.Drawing.Size(828, 24);
        this.menuStrip1.TabIndex = 16;
        this.menuStrip1.Text = "menuStrip1";
        // 
        // FileItem
        // 
        this.FileItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(150)))));
        this.FileItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.OpenFileItem, this.SaveFileItem });
        this.FileItem.Name = "FileItem";
        this.FileItem.Size = new System.Drawing.Size(48, 20);
        this.FileItem.Text = "Файл";
        // 
        // OpenFileItem
        // 
        this.OpenFileItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(150)))));
        this.OpenFileItem.Name = "OpenFileItem";
        this.OpenFileItem.Size = new System.Drawing.Size(174, 22);
        this.OpenFileItem.Text = "Открыть файл";
        this.OpenFileItem.Click += new System.EventHandler(this.OpenFile_Click);
        // 
        // SaveFileItem
        // 
        this.SaveFileItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(150)))));
        this.SaveFileItem.Name = "SaveFileItem";
        this.SaveFileItem.Size = new System.Drawing.Size(174, 22);
        this.SaveFileItem.Text = "Сохранить в файл";
        this.SaveFileItem.Click += new System.EventHandler(this.SaveFile_Click);
        // 
        // ClearItem
        // 
        this.ClearItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(150)))));
        this.ClearItem.Name = "ClearItem";
        this.ClearItem.Size = new System.Drawing.Size(101, 20);
        this.ClearItem.Text = "Очистить поля";
        this.ClearItem.Click += new System.EventHandler(this.MenuClear_Click);
        // 
        // InstructionItem
        // 
        this.InstructionItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(150)))));
        this.InstructionItem.Name = "InstructionItem";
        this.InstructionItem.Size = new System.Drawing.Size(85, 20);
        this.InstructionItem.Text = "Инструкция";
        // 
        // ProgrammerItem
        // 
        this.ProgrammerItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(150)))));
        this.ProgrammerItem.Name = "ProgrammerItem";
        this.ProgrammerItem.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
        this.ProgrammerItem.Size = new System.Drawing.Size(89, 20);
        this.ProgrammerItem.Text = "Разработчик";
        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(102)))), ((int)(((byte)(128)))));
        this.ClientSize = new System.Drawing.Size(828, 491);
        this.Controls.Add(this.panel3);
        this.Controls.Add(this.panel2);
        this.Controls.Add(this.panel1);
        this.Controls.Add(this.menuStrip1);
        this.DoubleBuffered = true;
        this.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.HelpButton = true;
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Location = new System.Drawing.Point(15, 15);
        this.MainMenuStrip = this.menuStrip1;
        this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.Name = "MainForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Лабораторная работа №2";
        this.panel1.ResumeLayout(false);
        this.panel1.PerformLayout();
        this.panel7.ResumeLayout(false);
        this.panel2.ResumeLayout(false);
        this.panel2.PerformLayout();
        this.panel5.ResumeLayout(false);
        this.panel4.ResumeLayout(false);
        this.panel3.ResumeLayout(false);
        this.panel3.PerformLayout();
        this.panel6.ResumeLayout(false);
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem FileItem;
    private System.Windows.Forms.ToolStripMenuItem ClearItem;
    private System.Windows.Forms.ToolStripMenuItem InstructionItem;
    private System.Windows.Forms.ToolStripMenuItem ProgrammerItem;
    private System.Windows.Forms.ToolStripMenuItem OpenFileItem;
    private System.Windows.Forms.ToolStripMenuItem SaveFileItem;

    private System.Windows.Forms.Panel panel7;

    private System.Windows.Forms.Panel panel6;

    private System.Windows.Forms.Panel panel5;

    private System.Windows.Forms.Panel panel4;

    private System.Windows.Forms.Panel panel3;

    private System.Windows.Forms.Panel panel2;

    private System.Windows.Forms.Panel panel1;

    private System.Windows.Forms.SaveFileDialog SaveFileDialog;
    private System.Windows.Forms.OpenFileDialog OpenFileDialog;

    private System.Windows.Forms.Label LengthLabel;

    private System.Windows.Forms.Label LabelCipherText;

    private System.Windows.Forms.TextBox CipherTextBox;

    private System.Windows.Forms.Label KeyLabel;

    private System.Windows.Forms.TextBox KeyTextBox;

    private System.Windows.Forms.Label PlainLabel;

    private System.Windows.Forms.TextBox PlainTextBox;

    private System.Windows.Forms.Label LabelRegister;

    private System.Windows.Forms.TextBox RegisterTextBox;

    private System.Windows.Forms.Button ResultButton;

    #endregion
}