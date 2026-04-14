namespace NESI.Common.Password
{
	/// <summary>
	/// We need to restrict tokens to a particular identity
	/// Implement this interface (explicitly) in your type to provide this semantics
	/// </summary>
	public interface IIdentityProvider
	{
		/// <summary>
		/// Identity value
		/// </summary>
		string Id { get; }
	}
}