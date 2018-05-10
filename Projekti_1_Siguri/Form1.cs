using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace Projekti_1_Siguri
{
    public partial class Form1 : Form
    {
        private byte[] byteCiphertext;

        public Form1()
        {
            InitializeComponent();
            txtCelesi.PasswordChar = '*';
            txtPass.PasswordChar = '*';
        }

        

        private void btnFsheh_Click(object sender, EventArgs e)
        {

            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {

                byte[] bytePlaintexti = Encoding.UTF8.GetBytes(txtPlaintext.Text);
                byte[] byteCelesi = Encoding.UTF8.GetBytes(txtCelesi.Text);

                DESCryptoServiceProvider objDes = new DESCryptoServiceProvider();
                objDes.Key = byteCelesi;
                objDes.Padding = PaddingMode.Zeros;
                objDes.IV = Encoding.UTF8.GetBytes("11223344");
                objDes.Mode = CipherMode.CBC;

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, objDes.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(bytePlaintexti, 0, bytePlaintexti.Length);
                cs.Close();

                byteCiphertext = ms.ToArray();

                txtCiphertext.Text = Convert.ToBase64String(byteCiphertext);
                int madhesia = txtCiphertext.Text.Length;

                pictureBox1.ImageLocation = open.FileName;

                Bitmap bmp = new Bitmap(pictureBox1.ImageLocation);
                MemoryStream ms1 = new MemoryStream();
                bmp.Save(ms1, System.Drawing.Imaging.ImageFormat.Bmp);

                byte[] byteImage = ms1.ToArray();
                String msg1 = txtCiphertext.Text;
                byte[] byteTeksti = Encoding.UTF8.GetBytes(msg1);

                int pozita = byteImage.Length - 8;
                for (int i = 0; i < madhesia; i++)
                {
                    byteImage[pozita] = byteTeksti[i];
                    pozita = pozita - 8;
                }

                MemoryStream ms2 = new MemoryStream(byteImage);
                Bitmap bmp1 = new Bitmap(ms2);

                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bmp1.Save(sfd.FileName);

                }


            }


        }

        private void btnLexo_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] byteCiphertexti;
                OpenFileDialog open = new OpenFileDialog();
                if (open.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bmp = new Bitmap(open.FileName);
                    MemoryStream ms = new MemoryStream();
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                    byte[] byteImage = ms.ToArray();
                    StringBuilder sbMesazhi = new StringBuilder();
                    pictureBox1.ImageLocation = open.FileName;
                    int pozita = byteImage.Length - 8;
                    for (int i = 0; i < 1000; i++)
                    {

                        char tempCh = (char)byteImage[pozita];
                        if (tempCh != 'ÿ')
                        {
                            sbMesazhi.Append(tempCh);
                            pozita = pozita - 8;
                        }
                    }



                    byte[] byteCelesi = Encoding.UTF8.GetBytes(txtPass.Text);
                    DESCryptoServiceProvider objDes = new DESCryptoServiceProvider();
                    objDes.Key = byteCelesi;
                    objDes.Padding = PaddingMode.Zeros;
                    objDes.IV = Encoding.UTF8.GetBytes("11223344");

                    objDes.Mode = CipherMode.CBC;

                    byteCiphertexti = Convert.FromBase64String(sbMesazhi.ToString());

                    MemoryStream msi = new MemoryStream(byteCiphertexti);
                    CryptoStream cs = new CryptoStream(msi, objDes.CreateDecryptor(), CryptoStreamMode.Read);

                    byte[] byteTextiDekriptuar = new byte[msi.Length];

                    cs.Read(byteTextiDekriptuar, 0, byteTextiDekriptuar.Length);
                    cs.Close();

                    txtMesazhi.Text = Encoding.UTF8.GetString(byteTextiDekriptuar);

                }
            }
            catch
            {
                MessageBox.Show("Ju lutem shtypni te dhenat ne forme valide!");
            }


        }
    }
}

