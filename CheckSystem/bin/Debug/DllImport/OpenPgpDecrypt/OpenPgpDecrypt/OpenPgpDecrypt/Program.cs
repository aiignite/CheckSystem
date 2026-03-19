using Newtonsoft.Json;
using PgpCore;
using System;
using System.IO;

namespace OpenPgpDecrypt
{
    internal class Program
    {
        private static void Main()
        {
            var srcFilePath = Console.ReadLine();
            var privateKeyPath = Console.ReadLine();
            var password = Console.ReadLine();

            var errorMsg = string.Empty;
            if (string.IsNullOrEmpty(srcFilePath))
            {
                errorMsg += "Source file path is empty. ";
            }
            else if (string.IsNullOrEmpty(privateKeyPath))
            {
                errorMsg += "Private key path is empty. ";
            }
            else if (string.IsNullOrEmpty(password))
            {
                errorMsg += "Password is empty. ";
            }
            else
            {
                // Load keys
                var privateKey = new FileInfo(privateKeyPath);
                // Reference input files
                var inputFile = new FileInfo(srcFilePath);

                if (!privateKey.Exists)
                {
                    errorMsg += "Private key file does not exist. ";
                }
                else if (!inputFile.Exists)
                {
                    errorMsg += "Source file path does not exist. ";
                }
                else
                {
                    try
                    {
                        // Load keys
                        var encryptionKeys = new EncryptionKeys(privateKey, password);
                        var inputFileFolder = inputFile.Directory;
                        var inputFileName = inputFile.Name.TrimEnd(inputFile.Extension.ToCharArray());
                        // Reference input/output files
                        var decryptedFile = new FileInfo(string.Format(@"{0}\{1}", inputFileFolder, inputFileName));
                        if (decryptedFile.Exists)
                        {
                            errorMsg += "Decrypted file already exists. ";
                        }
                        else
                        {
                            using (var pgp = new PGP(encryptionKeys))
                                pgp.Decrypt(inputFile, decryptedFile);

                            var decryptOkResult = new DecryptResult
                            {
                                IsDecryptOk = true,
                                DecryptFilePath = decryptedFile.FullName,
                                ErrorMsg = string.Empty
                            };
                            Console.WriteLine(JsonConvert.SerializeObject(decryptOkResult));
                            Console.ReadLine();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMsg += ex.Message;
                    }
                }
            }

            var decryptErrorResult = new DecryptResult
            {
                IsDecryptOk = false,
                DecryptFilePath = string.Empty,
                ErrorMsg = errorMsg
            };
            Console.WriteLine(JsonConvert.SerializeObject(decryptErrorResult));
            Console.ReadLine();
        }

        private class DecryptResult
        {
            public bool IsDecryptOk { get; set; } = false;
            public string DecryptFilePath { get; set; } = string.Empty;
            public string ErrorMsg { get; set; } = string.Empty;
        }
    }
}
