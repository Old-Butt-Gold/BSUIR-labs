using System.Security.Cryptography;
using SharedComponents.AdapterPluginFunctionality;

namespace AdapterCryptorForm;

public partial class CryptField : Form, ISerializerOption
{
    public readonly Aes Aes = Aes.Create();

    public object PreprocessorSave(object list) => list;

    public object PreprocessorLoad(object list) => list;

    public Stream PostprocessorLoad(Stream stream)
    {
        if (cbCrypt.Checked)
        {
            try
            {
                return new CryptoStream(stream, Aes.CreateDecryptor(), CryptoStreamMode.Read);
            }
            catch
            {
                MessageBox.Show("Decryption doesn't work");
            }
        }

        return stream;
    }

    public Stream PostprocessorSave(Stream stream)
    {
        if (cbCrypt.Checked)
        {
            try
            {
                return new CryptoStream(stream, Aes.CreateEncryptor(), CryptoStreamMode.Read);
            }
            catch
            {
                MessageBox.Show("Encryption doesn't work");
            }
        }

        return stream;
    }

    public CryptField()
    {
        InitializeComponent();
        cbCrypt.Checked = true;
        tbKey.Text = BitConverter.ToString(Aes.Key).Replace("-", "");
        tbIV.Text = BitConverter.ToString(Aes.IV).Replace("-", "");
    }

    void bClose_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Yes;
        Close();
    }

    void tbKey_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Aes.Key = Convert.FromHexString(tbKey.Text);
        }
        catch { }
    }

    void tbIV_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Aes.IV = Convert.FromHexString(tbIV.Text);
        }
        catch { }
    }
}