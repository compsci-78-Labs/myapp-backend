using WebApi.Domain.Users;
using WebApi.Application.Users;
using WebApi.Presentation.Auth;

namespace WebApi.Application.Auth;

public interface IAuthService
{ 
     Task Signup(AuthSignupRequest request);
     Task <AuthSigninResponse> Signin(AuthSigninRequest request);
     Task<User?> Me(Guid id);
     Task Signout(string token);
     Task <AuthRefreshResponse> Refresh(string token);
}