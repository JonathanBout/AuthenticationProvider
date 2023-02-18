# AuthenticationProvider
A simple Authentication Provider for ASP.NET Core.
To initialize, add the following to your `Main` function:
```cs
...
builder.AddKeyAuthentication();
...
var app = builder.Build();
...
app.UseKeyAuthentication();
...
```
On startup an key is written to the console. Use this key to authenticate with the POST endpoint at `/Authentication/Login`. An example of such POST request body would be:
```json
{
  "key": "your key goes here",
  "userId": 1
}
```
Once logged in, you can send a request to `/Authentication/Key` to generate another key.

The `builder.AddKeyAuthentication()` function has an optional `configure` action, which lets you specify some stuff like the database location or the base path for the requests (e.g. to change `/Authenticate` to `/api/auth`)

To mark an function or controller to need authentication you can add the `KeyAutenticated` attribute. In a class with it you can disable it for a single request by adding the `SkipAuthentication` attribute. See [the test project](https://github.com/JonathanBout/AuthenticationProvider/tree/master/AuthenticationProviderTests) for an example.
