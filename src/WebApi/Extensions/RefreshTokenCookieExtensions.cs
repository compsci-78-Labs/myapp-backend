namespace WebApi.Extensions;

public static class RefreshTokenCookieExtensions
{
    public static void SetRefreshTokenCookie(
        this HttpResponse response,
        string refreshToken)
    {
        response.Cookies.Append(
            "refreshToken",
            refreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/auth",
                MaxAge = TimeSpan.FromDays(7)
            });
    }

    public static void ClearRefreshTokenCookie(
        this HttpResponse response)
    {
        response.Cookies.Delete(
            "refreshToken",
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/auth"
            });
    }
}