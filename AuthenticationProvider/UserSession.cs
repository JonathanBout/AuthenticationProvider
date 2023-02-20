namespace JonathanBout.Authentication
{
	internal class AuthenticationSession : IAuthenticationSession
	{
		private IUserIdentifier? _identifier = null;
		public IUserIdentifier Identifier
		{
			get
			{
				if (_identifier is null)
				{
					throw new InvalidOperationException();
				}
				return _identifier;
			}
			set
			{
				_identifier = value;
			}
		}

		public int CurrentUserId
		{
			get => _identifier?.UserId ?? -1;
		}
	}
}