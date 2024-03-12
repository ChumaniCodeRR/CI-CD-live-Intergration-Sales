
using Integration_Sales_Order_Test.Model.Accounts;

namespace Integration_Sales_Order_Test.Repository.ServicesEmail
{
    public interface IAccountService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);

        void RevokeToken(string token, string ipAddress);
        void Register(RegisterRequest model, string origin);
        
        void UserLogin (LoginRequest request);
        void VerifyEmail(string token);
        void ForgotPassword(ForgotPasswordRequest model, string origin);
        void ValidateResetToken(ValidateResetTokenRequest model);
        void ResetPassword(ResetPasswordRequest model);

        IEnumerable<AccountResponse> GetAll();
        AccountResponse GetById(int id);
        AccountResponse Create(CreateRequest model);
        AccountResponse Update(int id, UpdateRequest model);
        void Delete(int id);
    }

}
