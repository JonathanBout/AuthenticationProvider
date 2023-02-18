using Microsoft.AspNetCore.Mvc.Filters;

namespace JonathanBout.Authentication
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SkipAuthenticationAttribute : ActionFilterAttribute
    {
    }
}
