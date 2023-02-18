namespace JonathanBout.Authentication
{
	internal class AuthenticationSession : IAuthenticationSession
	{
		private UserIdentifier? _identifier = null;
		public UserIdentifier Identifier
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