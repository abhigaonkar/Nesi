using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace NESI.BLL.Common.Shared
{
	public static class Encryption
	{
		public static string EncryptString(string message)
		{
			return EncryptString(message, Configuration.MobileAuthKey);
		}

		public static string DecryptString(string message)
		{
			return DecryptString(message, Configuration.MobileAuthKey);
		}

		public static string EncryptPassword(string message)
		{
			return EncryptString(message, Configuration.EncryptionPass);
		}

		public static string DecryptPassword(string message)
		{
			return DecryptString(message, Configuration.EncryptionPass);
		}

		public static string DecryptString(string message, string passphrase)
		{
			byte[] results;
			var utf8 = new UTF8Encoding();
			var hashProvider = new MD5CryptoServiceProvider();
			var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(passphrase));
			var tdesAlgorithm = new TripleDESCryptoServiceProvider
			{
				Key = tdesKey,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7
			};
		    var dataToDecrypt = Encoding.ASCII.GetBytes(message); ;

            try
		    {
		        dataToDecrypt = Convert.FromBase64String(message);
		    }
		    catch
		    {
		        return message;
		    }

		    try
			{
				var decryptor = tdesAlgorithm.CreateDecryptor();
				results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
			}
			
			finally
			{
				tdesAlgorithm.Clear();
				hashProvider.Clear();
			}
			return results != null ? utf8.GetString(results) : "";
		}
		/// <summary>
		/// Encrypt string using Passphrase
		/// normally using encrypt password.
		/// </summary>
		/// <param name="message">the string to be encrypted</param>
		/// <param name="passphrase">pass seed, from web.config</param>
		/// <returns></returns>
		public static string EncryptString(string message, string passphrase)
		{
			byte[] results;
			var utf8 = new UTF8Encoding();
			var hashProvider = new MD5CryptoServiceProvider();
			var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(passphrase));
			var tdesAlgorithm = new TripleDESCryptoServiceProvider
			{
				Key = tdesKey,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7
			};
			var dataToEncrypt = utf8.GetBytes(message);
			try
			{
				var encryptor = tdesAlgorithm.CreateEncryptor();
				results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
			}
			finally
			{
				tdesAlgorithm.Clear();
				hashProvider.Clear();
			}
			return Convert.ToBase64String(results);
		}

		public static string RandomString(int size)
		{
			var builder = new StringBuilder();
			var random = new Random();
			for (var i = 0; i < size; i++)
			{
				var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
				builder.Append(ch);
			}
			return builder.ToString();
		}
	
	
		public static string get_md5(int size)
		{
			var str = RandomString(size);
			var enc = Encoding.Unicode.GetEncoder();
			var unicodeText = new byte[str.Length * 2];
			enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);
			MD5 md5 = new MD5CryptoServiceProvider();
			var result = md5.ComputeHash(unicodeText);
			var sb = new StringBuilder();
			foreach (byte t in result)
			{
				sb.Append(t.ToString("X2"));
			}
			return sb.ToString();
		}


		public static bool is_encrypted_password(string pass)
		{
		if (string.IsNullOrEmpty(pass))
			{
			return false;
			}
		else
			{
			var reBase64Chk = new Regex("^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$");
			return reBase64Chk.Match(pass).Success;
			}
		}
	}
}