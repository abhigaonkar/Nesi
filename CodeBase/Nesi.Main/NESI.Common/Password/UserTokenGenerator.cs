using System;
using System.IO;
using System.Reflection;
using log4net;
using Microsoft.Owin.Security.DataProtection;
using NESI.Common.Extensions;

namespace NESI.Common.Password
{
    /// <summary>
    /// This class will generate and validate a secure, time limited token for a
    /// given identity ID
    /// </summary>
    public class UserTokenGenerator
    {
        public static readonly string PurposeForgotPassword = "FORGOT_PASSWORD";
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default lifespan for any generated token
        /// </summary>
        private readonly TimeSpan _defaultLifespan = TimeSpan.FromDays(1.0);

        /// <summary>
        /// Construct the token generator using the provided data protector. 
        /// </summary>
        /// <param name="protector"></param>
        public UserTokenGenerator(IDataProtector protector)
        {
            this.Protector = protector ?? throw new ArgumentNullException(nameof(protector));
            this.TokenLifespan = _defaultLifespan;
        }

        /// <summary>
        /// Provided data protector
        /// </summary>
        public IDataProtector Protector { get; }

        /// <summary>
        /// Lifespan after which the token is considered expired
        /// </summary>
        public TimeSpan TokenLifespan { get; set; }

        /// <summary>
        /// Generate token
        /// </summary>
        /// <param name="purpose"></param>
        /// <param name="identityProvider"></param>
        /// <param name="expiredDate"></param>
        /// <returns></returns>
        public string Generate(string purpose, IIdentityProvider identityProvider, DateTime? expiredDate = null)
        {
            if (purpose == null) throw new ArgumentNullException(nameof(purpose));
            if (identityProvider == null) throw new ArgumentNullException(nameof(identityProvider));

            var ms = new MemoryStream();
            using (BinaryWriter writer = ms.CreateWriter())
            {
                writer.Write(expiredDate == null ? DateTimeOffset.UtcNow + TokenLifespan : (DateTimeOffset)expiredDate);
                writer.Write(identityProvider.Id);
                writer.Write(purpose);
            }
            byte[] protectedBytes = this.Protector.Protect(ms.ToArray());
            return Convert.ToBase64String(protectedBytes);
        }

        /// <summary>
        /// Validate the given token
        /// </summary>
        /// <param name="purpose"></param>
        /// <param name="token"></param>
        /// <param name="identityProvider"></param>
        /// <returns></returns>
        public TokenValidationResult Validate(string purpose, string token, IIdentityProvider identityProvider)
        {
            if (purpose == null) throw new ArgumentNullException(nameof(purpose));
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (identityProvider == null) throw new ArgumentNullException(nameof(identityProvider));

            try
            {
                byte[] unprotectedData = this.Protector.Unprotect(Convert.FromBase64String(token));
                MemoryStream ms = new MemoryStream(unprotectedData);
                using (BinaryReader reader = ms.CreateReader())
                {
                    DateTimeOffset expirationTime = reader.ReadDateTimeOffset();
                    if (expirationTime < DateTimeOffset.UtcNow) return TokenValidationResult.TokenValidationError(TokenValidationErrorReason.ExpiredToken);
                    
                    var tokenIdentityId = reader.ReadString();
                    if (!string.Equals(tokenIdentityId, identityProvider.Id,
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        return TokenValidationResult.TokenValidationError(TokenValidationErrorReason.InvalidIdentity);
                    }

                    var tokenPurpose = reader.ReadString();
                    return !string.Equals(tokenPurpose, purpose) ? 
                        TokenValidationResult.TokenValidationError(TokenValidationErrorReason.PurposeMismatch) : 
                        TokenValidationResult.TokenValidationSuccess();
                }
            }
            catch (Exception exception)
            {
                Logger.Error($"Failed to validate token, {purpose}", exception);
                return TokenValidationResult.TokenValidationError(TokenValidationErrorReason.ParseError);
            }
        }
    }
}