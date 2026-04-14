using System;
using System.Web.Helpers;
using NESI.Common.Exceptions;
using NESI.Common.Extensions;

namespace NESI.Common.Password
{
    /// <summary>
    /// Handles hashing and verification of hashed passwords
    /// </summary>
	public  static class PasswordHasher
	{
		/// <summary>Hash the given password</summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public static string HashPassword(string password) =>
			Crypto.HashPassword(password);
		

		/// <summary>Verify that a password matches the hashedPassword</summary>
		/// <param name="hashedPassword"></param>
		/// <param name="providedPassword"></param>
		/// <returns></returns>
		public static PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
		{
		    if (hashedPassword == null) throw new ArgumentNullException(nameof(hashedPassword));
		    if (providedPassword == null) throw new ArgumentNullException(nameof(providedPassword));

            //
            // Throw and exception in cases where we are trying to compare passwords where there is no valid has
            //  The crypto library will throw an exception in any case.
            //
            if(!hashedPassword.IsValidBase64EncodedString()) throw new NesiException("Invalid password format.");

		    return Crypto.VerifyHashedPassword(hashedPassword, providedPassword)
	            ? PasswordVerificationResult.Success
	            : PasswordVerificationResult.Failed;
		}
	}
}