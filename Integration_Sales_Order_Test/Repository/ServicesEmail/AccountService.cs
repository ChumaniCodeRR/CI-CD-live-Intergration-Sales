﻿using AutoMapper;
using Integration_Sales_Order_Test.Helpers;
using Microsoft.Extensions.Options;
using BC = BCrypt.Net.BCrypt;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Integration_Sales_Order_Test.Entities;
using Integration_Sales_Order_Test.Model.Accounts;
using Microsoft.AspNetCore.Identity;

namespace Integration_Sales_Order_Test.Repository.ServicesEmail
{
    public class AccountService : IAccountService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;

        public AccountService(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }
        /// <summary>
        /// Register, using authenication, and password functions 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var account = _context.Accounts.SingleOrDefault(c => c.Email == model.Email);

            if (account == null || !account.IsVerified || !BC.Verify(model.Password, account.PasswordHash))
            {
                throw new AppException("Email or password is not valid");
            }

            var jwtToken = generateJwtToken(account);
            var refreshToken = generateRefreshToken(ipAddress);
            account.RefreshTokens.Add(refreshToken);

            removeOldRefreshTokens(account);

            _context.Update(account);
            _context.SaveChanges();

            var response = _mapper.Map<AuthenticateResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = refreshToken.Token;
            return response;

        }

        public void UserLogin(LoginRequest request)
        {
            var user = _context.Accounts.SingleOrDefault(x => x.Email == request.Email);

            if (user == null)
            {
                throw new AppException("Email is not found !!!");
            }

            if (user == null || !user.IsVerified)
            {
                throw new AppException($"Unable to authenticate user {request.Email}");
            }

            var userinfo = _mapper.Map<UserLoginDetails>(request);

            userinfo.Email = user.Email;
            userinfo.LastName = user.LastName;
            userinfo.Date = DateTime.UtcNow;
            userinfo.SecurityStamp = randomTokenString();

           _context.UserLoginDetails.Add(userinfo);
            _context.SaveChanges();

        }

        public void Register(RegisterRequest model, string origin)
        {
            if (_context.Accounts.Any(x => x.Email == model.Email))
            {
                sendAlreadyRegisteredEmail(model.Email, origin);
                return;
            }

            var account = _mapper.Map<Account>(model);

            var isFirstAccount = _context.Accounts.Count() == 0;
            account.Role = isFirstAccount ? Role.Admin : Role.User;
            account.Created = DateTime.UtcNow;
            account.VerificationToken = randomTokenString();

            account.PasswordHash = BC.HashPassword(model.Password);

            _context.Accounts.Add(account);
            _context.SaveChanges();

            sendVerificationEmail(account, origin);
        }

        public void ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Email == model.Email);

            if (account == null) return;

            account.ResetToken = randomTokenString();
            account.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

            _context.Accounts.Update(account);
            _context.SaveChanges();

            sendPasswordResetEmail(account, origin);
        }

        public void ResetPassword(ResetPasswordRequest model)
        {
            var account = _context.Accounts.SingleOrDefault(x =>
                x.ResetToken == model.Token &&
                x.ResetTokenExpires > DateTime.UtcNow);

            if (account == null) throw new AppException("Invalid token");

            account.PasswordHash = BC.HashPassword(model.Password);
            account.PasswordReset = DateTime.UtcNow;
            account.ResetToken = null;
            account.ResetTokenExpires = null;

            _context.Accounts.Update(account);
            _context.SaveChanges();

        }

        /// <summary>
        /// CRUD for accounts functions 
        /// </summary>
        /// <returns></returns>
        // 
        public AccountResponse Create(CreateRequest model)
        {
            if (_context.Accounts.Any(x => x.Email == model.Email))
                throw new AppException($"Email '{model.Email}' is already registered");

            var account = _mapper.Map<Account>(model);
            account.Created = DateTime.UtcNow;
            account.Verified = DateTime.UtcNow;

            account.PasswordHash = BC.HashPassword(model.Password);

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return _mapper.Map<AccountResponse>(account);

        }

        public AccountResponse Update(int id, UpdateRequest model)
        {
            var account = getAccount(id);

            if (account.Email != model.Email && _context.Accounts.Any(x => x.Email == model.Email))
                throw new AppException($"Email '{model.Email}' is already taken");

            if (!string.IsNullOrEmpty(model.Password))
                account.PasswordHash = BC.HashPassword(model.Password);

            _mapper.Map(model, account);
            account.Updated = DateTime.UtcNow;
            _context.Accounts.Update(account);
            _context.SaveChanges();

            return _mapper.Map<AccountResponse>(account);
        }

        public IEnumerable<AccountResponse> GetAll()
        {
            var accounts = _context.Accounts;
            return _mapper.Map<IList<AccountResponse>>(accounts);

        }

        public AccountResponse GetById(int id)
        {
            var account = getAccount(id);
            return _mapper.Map<AccountResponse>(account);
        }

        public void Delete(int id)
        {
            var account = getAccount(id);
            _context.Accounts.Remove(account);
            _context.SaveChanges();
        }

        private Account getAccount(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null) throw new KeyNotFoundException("Account not found");
            return account;
        }

        /// <summary>
        /// token verification and token refresh functions 
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="AppException"></exception>
        // 
        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var (refreshToken, account) = getRefreshToken(token);

            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            account.RefreshTokens.Add(newRefreshToken);

            removeOldRefreshTokens(account);

            _context.Update(account);
            _context.SaveChanges();

            var jwtToken = generateJwtToken(account);
            var response = _mapper.Map<AuthenticateResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = newRefreshToken.Token;

            return response;

        }

        public void RevokeToken(string token, string ipAddress)
        {
            var (refreshToken, account) = getRefreshToken(token);

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _context.Update(account);
            _context.SaveChanges();
        }
        public void ValidateResetToken(ValidateResetTokenRequest model)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.ResetToken == model.Token &&
            x.ResetTokenExpires > DateTime.UtcNow);

            if (account == null) throw new AppException("Invalid token provided");
        }

        private (RefreshToken, Account) getRefreshToken(string token)
        {
            var account = _context.Accounts.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (account == null)
            {
                throw new AppException("Invalid token");
            }

            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive) throw new AppException("Invalid token");

            return (refreshToken, account);
        }

        private string generateJwtToken(Account account)
        {
            var tokenHandle = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("AccountId", account.AccountId.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandle.CreateToken(tokenDescriptor);

            return tokenHandle.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = randomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        private string randomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private void removeOldRefreshTokens(Account account)
        {
            account.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        /// <summary>
        /// emails these functions
        /// </summary>
        /// <param name="email"></param>
        /// <param name="origin"></param>
        /// 
        private void sendAlreadyRegisteredEmail(string email, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
                message = $@"<p>If you don't know your password please visit the <a href=""{origin}/account/forgot-password"">forgot password</a> page.</p>";
            else
                message = "<p>If you don't know your password you can reset it via the <code>/accounts/forgot-password</code> api route.</p>";

            _emailService.Send(
                to: email,
                subject: "Sign-up Verification API - Email Already Registered",
                html: $@"<h4>Email Already Registered</h4>
                         <p>Your email <strong>{email}</strong> is already registered.</p>
                         {message}"
            );
        }

        private void sendVerificationEmail(Account account, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/account/verify-email?token={account.VerificationToken}";
                message = $@"<p>Please click the below link to verify your email address:</p>
                             <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                             <p><code>{account.VerificationToken}</code></p>";
            }

            _emailService.Send(
                to: account.Email,
                subject: "Sign-up Verification API - Verify Email",
                html: $@"<h4>Verify Email</h4>
                         <p>Thanks for registering!</p>
                         {message}"
            );
        }

        public void VerifyEmail(string token)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.VerificationToken == token);

            if (account == null) throw new AppException("Verification failed");

            account.Verified = DateTime.UtcNow;
            account.VerificationToken = null;

            _context.Accounts.Update(account);
            _context.SaveChanges();
        }

        private void sendPasswordResetEmail(Account account, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/account/reset-password?token={account.ResetToken}";
                message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                             <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
                             <p><code>{account.ResetToken}</code></p>";
            }

            _emailService.Send(
                to: account.Email,
                subject: "Sign-up Verification API - Reset Password",
                html: $@"<h4>Reset Password Email</h4>
                         {message}"
            );
        }
    }
}
