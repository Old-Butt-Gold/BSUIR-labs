using System.ComponentModel;

namespace AdapterCryptorForm;

partial class CryptField
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
        bClose = new Button();
        cbCrypt = new CheckBox();
        tbKey = new TextBox();
        lKey = new Label();
        label1 = new Label();
        tbIV = new TextBox();
        SuspendLayout();
        // 
        // bClose
        // 
        bClose.Location = new Point(12, 146);
        bClose.Name = "bClose";
        bClose.Size = new Size(352, 31);
        bClose.TabIndex = 0;
        bClose.Text = "Close";
        bClose.UseVisualStyleBackColor = true;
        bClose.Click += bClose_Click;
        // 
        // cbCrypt
        // 
        cbCrypt.AutoSize = true;
        cbCrypt.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 204);
        cbCrypt.Location = new Point(12, 12);
        cbCrypt.Name = "cbCrypt";
        cbCrypt.Size = new Size(303, 35);
        cbCrypt.TabIndex = 1;
        cbCrypt.Text = "Encrypt and decrypt files";
        cbCrypt.UseVisualStyleBackColor = true;
        // 
        // tbKey
        // 
        tbKey.Location = new Point(68, 59);
        tbKey.Name = "tbKey";
        tbKey.Size = new Size(296, 27);
        tbKey.TabIndex = 2;
        tbKey.TextChanged += tbKey_TextChanged;
        // 
        // lKey
        // 
        lKey.AutoSize = true;
        lKey.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 204);
        lKey.Location = new Point(12, 53);
        lKey.Name = "lKey";
        lKey.Size = new Size(50, 31);
        lKey.TabIndex = 3;
        lKey.Text = "Key";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 204);
        label1.Location = new Point(12, 102);
        label1.Name = "label1";
        label1.Size = new Size(34, 31);
        label1.TabIndex = 4;
        label1.Text = "IV";
        // 
        // tbIV
        // 
        tbIV.Location = new Point(68, 102);
        tbIV.Name = "tbIV";
        tbIV.Size = new Size(296, 27);
        tbIV.TabIndex = 5;
        tbIV.TextChanged += tbIV_TextChanged;
        // 
        // CryptField
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(381, 189);
        Controls.Add(tbIV);
        Controls.Add(label1);
        Controls.Add(lKey);
        Controls.Add(tbKey);
        Controls.Add(cbCrypt);
        Controls.Add(bClose);
        //CryptName = "CryptField";
        Text = "CryptField";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button bClose;
    private CheckBox cbCrypt;
    private TextBox tbKey;
    private Label lKey;
    private Label label1;
    private TextBox tbIV;
}