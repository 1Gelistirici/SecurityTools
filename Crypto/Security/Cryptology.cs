using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Crypto.Security
{
    public class Cryptology
    {
        private static string Key { get; set; }
        private static string FolderPath { get; set; }
        public Cryptology(string key, string folderPath)
        {
            if (key == "")
            {
                Key = "f54aaW1wWA1D5Da1wWA1Da1hG2Da1hSrth";
            }
            else
            {
                Key = key;
            }

            FolderPath = folderPath;
        }

        public List<string> Encrypt()
        {
            return EncryptAllFilesInFolder("");
            //EncryptAllFilesInFolder("C:\\mails", "*.eml");
            //EncryptAllFilesInFolder("C:\\mails", "*.pdf");
            //EncryptAllFilesInFolder("C:\\mails", "*.docx");
            //EncryptAllFilesInFolder("C:\\mails", "*.xlsx");
            //EncryptAllFilesInFolder("C:\\mails", "*.zip");
            //EncryptAllFilesInFolder("C:\\mails", "*.exe");
            //EncryptAllFilesInFolder("C:\\mails", "*.txt");
        }

        public List<string> Decrypt()
        {
            return DecryptAllFilesInFolder("");
            //DecryptAllFilesInFolder("C:\\mails", "*.eml");
            //DecryptAllFilesInFolder("C:\\mails", "*.pdf");
            //DecryptAllFilesInFolder("C:\\mails", "*.docx");
            //DecryptAllFilesInFolder("C:\\mails", "*.xlsx");
            //DecryptAllFilesInFolder("C:\\mails", "*.zip");
            //DecryptAllFilesInFolder("C:\\mails", "*.exe");
            //DecryptAllFilesInFolder("C:\\mails", "*.txt");
        }

        /// <summary>
        /// Encrypt All or specific files in the folder
        /// </summary>
        /// <param name="fileType">Example: "*.eml" or all files =>""</param>
        public static List<string> EncryptAllFilesInFolder(string fileType)
        {
            List<string> errorList = new List<string>();

            try
            {
                DirectoryInfo d = new DirectoryInfo(FolderPath);
                FileInfo[] Files = d.GetFiles(fileType);
                string EncryptionKey = Key;

                foreach (FileInfo file in Files)
                {
                    try
                    {
                        string filePath = $"{FolderPath}\\{file.Name}";

                        FileAttributes attributes = File.GetAttributes(filePath);
                        //System, Offline, Readonly
                        if (!((attributes & FileAttributes.Offline) == FileAttributes.Offline))
                        {
                            ReadOnlySpan<byte> clearBytes = File.ReadAllBytes(filePath).AsSpan();
                            using (Aes encryptor = Aes.Create())
                            {
                                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                                encryptor.Key = pdb.GetBytes(32);
                                encryptor.IV = pdb.GetBytes(16);
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                                    {
                                        cs.Write(clearBytes.ToArray(), 0, clearBytes.Length);
                                        cs.FlushFinalBlock();
                                        cs.Close();
                                    }
                                    clearBytes = ms.ToArray();
                                }
                            }
                            File.WriteAllBytes(filePath, clearBytes.ToArray());
                            File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.Offline);
                        }

                    }
                    catch (Exception ex)
                    {
                        errorList.Add(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                errorList.Add(ex.Message);
            }

            return errorList;
        }
        /// <summary>
        /// Dencrypt All or specific files in the folder
        /// </summary>
        /// <param name="fileType">Example: "*.eml" or all files =>""</param>
        public static List<string> DecryptAllFilesInFolder(string fileType)
        {
            List<string> errorList = new List<string>();

            try
            {
                DirectoryInfo d = new DirectoryInfo(FolderPath);
                FileInfo[] Files = d.GetFiles(fileType);
                string EncryptionKey = Key;

                foreach (FileInfo file in Files)
                {
                    try
                    {
                        string filePath = $"{FolderPath}\\{file.Name}";
                        FileAttributes attributes = File.GetAttributes(filePath);
                        if (((attributes & FileAttributes.Offline) == FileAttributes.Offline))
                        {
                            attributes = attributes & ~FileAttributes.Offline;
                            File.SetAttributes(filePath, attributes);

                            ReadOnlySpan<byte> cipherBytes = File.ReadAllBytes(filePath).AsSpan();
                            using (Aes encryptor = Aes.Create())
                            {
                                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                                encryptor.Key = pdb.GetBytes(32);
                                encryptor.IV = pdb.GetBytes(16);
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                                    {
                                        cs.Write(cipherBytes.ToArray(), 0, cipherBytes.Length);
                                        cs.FlushFinalBlock();
                                        cs.Close();
                                    }
                                    cipherBytes = ms.ToArray();
                                }
                            }
                            File.WriteAllBytes(filePath, cipherBytes.ToArray());
                        }
                    }
                    catch (Exception ex)
                    {
                        errorList.Add(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                errorList.Add(ex.Message);
            }
            return errorList;
        }

        public static void EncryptFile(string filePath)
        {
            ReadOnlySpan<byte> clearBytes = File.ReadAllBytes(filePath).AsSpan();
            string EncryptionKey = "753ovb050y7hdd9ly2f7r4h3lq7jj2c89qbf";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes.ToArray(), 0, clearBytes.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    clearBytes = ms.ToArray();
                }
            }
            File.WriteAllBytes(filePath, clearBytes.ToArray());
        }

        public static void DecryptFile(string filePath)
        {
            ReadOnlySpan<byte> cipherBytes = File.ReadAllBytes(filePath).AsSpan();
            string EncryptionKey = "753ovb050y7hdd9ly2f7r4h3lq7jj2c89qbf";

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes.ToArray(), 0, cipherBytes.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    cipherBytes = ms.ToArray();
                }
            }
            File.WriteAllBytes(filePath, cipherBytes.ToArray());
        }

        public static bool MatchFiles(string sourceFilePath, string targetFilePath)
        {
            return File.ReadAllBytes(sourceFilePath).SequenceEqual(File.ReadAllBytes(targetFilePath));
        }

        public static List<string> MatchAllFilesInFolder(string sourcePath, string targetPath)
        {
            DirectoryInfo d = new DirectoryInfo(sourcePath);

            FileInfo[] Files = d.GetFiles();
            List<string> diffFilesList = new List<string>();
            foreach (FileInfo file in Files)
            {
                string sourceFilePath = $"{sourcePath}\\{file.Name}";
                string targetFilePath = $"{targetPath}\\{file.Name}";
                if (!MatchFiles(sourceFilePath, targetFilePath))
                {
                    diffFilesList.Add(file.Name);
                }
                else
                {
                    Console.WriteLine($"Şifrelenmemiş Dosya:{file.Name}");
                }
            }
            return diffFilesList;
        }

    }
}
