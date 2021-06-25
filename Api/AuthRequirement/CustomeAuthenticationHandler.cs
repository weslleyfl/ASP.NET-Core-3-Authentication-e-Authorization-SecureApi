using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;


namespace Api.AuthRequirement
{
    /// <summary>
    /// https://referbruv.com/blog/posts/implementing-custom-authentication-scheme-and-handler-in-aspnet-core-3x
    /// </summary>
    //public class ValidateHashAuthenticationSchemeOptions : AuthenticationSchemeOptions { }

    public class CustomeAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        public CustomeAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // validation comes in here
            return Task.FromResult(AuthenticateResult.Fail("Autenticação Falhou!!!"));
        }


        //    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        //    {
        //        TokenModel model;

        //        // validation comes in here
        //        if (!Request.Headers.ContainsKey("X-Base-Token"))
        //        {
        //            return Task.FromResult(AuthenticateResult.Fail("Header Not Found."));
        //        }

        //        var token = Request.Headers["X-Base-Token"].ToString();

        //        try
        //        {
        //            // convert the input string into byte stream
        //            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(token)))
        //            {
        //                // deserialize stream into token model object
        //                model = Serializer.Deserialize<TokenModel>(stream);
        //            }
        //        }
        //        catch (System.Exception ex)
        //        {
        //            Console.WriteLine("Exception Occured while Deserializing: " + ex);
        //            return Task.FromResult(AuthenticateResult.Fail("TokenParseException"));
        //        }

        //        if (model != null)
        //        {
        //            // success case AuthenticationTicket generation
        //            // happens from here

        //            // create claims array from the model
        //            var claims = new[] {
        //                new Claim(ClaimTypes.NameIdentifier, model.UserId.ToString()),
        //                new Claim(ClaimTypes.Email, model.EmailAddress),
        //                new Claim(ClaimTypes.Name, model.Name) };

        //            // generate claimsIdentity on the name of the class
        //            var claimsIdentity = new ClaimsIdentity(claims,
        //                        nameof(ValidateHashAuthenticationHandler));

        //            // generate AuthenticationTicket from the Identity
        //            // and current authentication scheme
        //            var ticket = new AuthenticationTicket(
        //                new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);

        //            // pass on the ticket to the middleware
        //            return Task.FromResult(AuthenticateResult.Success(ticket));
        //        }

        //        return Task.FromResult(AuthenticateResult.Fail("Model is Empty"));
        //    }
        //}

        //public class TokenModel
        //{
        //    public int UserId { get; set; }
        //    public string Name { get; set; }
        //    public string EmailAddress { get; set; }
        //}
    }
}
