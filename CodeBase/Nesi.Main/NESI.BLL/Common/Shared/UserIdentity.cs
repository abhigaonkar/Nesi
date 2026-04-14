using NESI.BLL.Core.User;
using NESI.Common.Password;

namespace NESI.BLL.Common.Shared
{
    /// <inheritdoc />
    /// <summary>
    /// Internal wrapper for identity
    /// </summary>
    public class UserIdentity : IIdentityProvider
    {
        private readonly User _user;

        public UserIdentity(User user)
        {
            _user = user;
        }

        /// <inheritdoc />
        /// <summary>
        /// How to explicitly implement the interface
        /// </summary>
        string IIdentityProvider.Id => _user.Id.ToString();
    }
}