using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Api.AuthRequirement
{
    /// <summary>
    /// https://docs.microsoft.com/pt-br/aspnet/core/security/authorization/policies?view=aspnetcore-5.0
    /// Um requisito de autorização é uma coleção de parâmetros de dados 
    /// que uma política pode usar para avaliar a entidade de usuário atual
    /// </summary>
    public class JwtRequirement : IAuthorizationRequirement { }


    /// <summary>
    /// Manipuladores de autorização
    /// Um manipulador de autorização é responsável pela avaliação das propriedades de um requisito. 
    /// O manipulador de autorização avalia os requisitos em relação a um AuthorizationHandlerContext 
    /// fornecido para determinar se o acesso é permitido.
    /// </summary>
    public class JwtRequirementHandler : AuthorizationHandler<JwtRequirement>
    {
        private readonly HttpClient _client;
        private readonly HttpContext _httpContext;

        public JwtRequirementHandler(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClientFactory.CreateClient();
            _httpContext = httpContextAccessor.HttpContext;
        }


        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            JwtRequirement requirement)
        {
            if (_httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var accessToken = authHeader.ToString().Split(' ')[1];

                // Nossa WEB API valia o acesso no servidor de autorização
                var response = await _client.GetAsync($"https://localhost:44340/oauth/validate?access_token={accessToken}");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    context.Succeed(requirement);
                }

            }
        }
    }


}
