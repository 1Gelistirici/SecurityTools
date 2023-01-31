using Crypto.Security;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Crypto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckInputs();
        }

        private void btn_choose_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog file = new FolderBrowserDialog();

            if (file.ShowDialog() == DialogResult.OK)
            {
                text_folderPath.Text = file.SelectedPath;
                logList.Items.Add($"Dosya adresi seçildi. {file.SelectedPath}");
            }

            CheckInputs();
        }

        private void text_folderPath_TextChanged(object sender, EventArgs e)
        {
            CheckInputs();
        }

        private void btn_encrypt_Click(object sender, EventArgs e)
        {
            Cryptology cryptology = new Cryptology(text_key.Text, text_folderPath.Text);
            List<string> errorList = cryptology.Encrypt();

            writeRusult(errorList, "Dosyalar şifrelendi", "Dosyalar şifrelenirken bir hata oluştu.");
        }

        private void btn_decrypt_Click(object sender, EventArgs e)
        {
            Cryptology cryptology = new Cryptology(text_key.Text, text_folderPath.Text);
            List<string> errorList = cryptology.Decrypt();

            writeRusult(errorList, "Dosyalar çözüldü.", "Dosyalar çözülürken bir hata oluştu.");
        }


        void writeRusult(List<string> errorList, string successText, string failText)
        {
            if (errorList.Count > 0)
            {
                logList.Items.Add($"Başarısız. {failText}");
            }
            else
            {
                logList.Items.Add($"Başarılı. {successText}");
            }

            foreach (var errorText in errorList)
            {
                logList.Items.Add(errorText);
            }
        }

        public void CheckInputs()
        {
            bool isSuccess = true;

            if (text_folderPath.Text == "")
            {
                logList.Items.Add("Dosya adresi giriniz.");
                isSuccess = false;
            }


            if (isSuccess)
            {
                btn_decrypt.Enabled = true;
                btn_encrypt.Enabled = true;
            }
            else
            {
                btn_decrypt.Enabled = false;
                btn_encrypt.Enabled = false;
            }
        }


    }
}
