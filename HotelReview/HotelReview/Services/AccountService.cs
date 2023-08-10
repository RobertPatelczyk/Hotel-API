using HotelReview.Entities;
using HotelReview.Exceptions;
using HotelReview.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace HotelReview.Services
{
    public interface IAccountService
    {
        public void RegisterUser(RegisterUserDto dto);
        public string Login(LoginUserDto dto);
    }
    public class AccountService :IAccountService 
    {
        public AccountService(HotelDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
            this.authenticationSettings = authenticationSettings;
        }

        public HotelDbContext dbContext { get; }
        public IPasswordHasher<User> passwordHasher { get; }
        public AuthenticationSettings authenticationSettings { get; }

        public void RegisterUser(RegisterUserDto dto)
        {
            User newUser = new User()
            {
                DateOfBirth = dto.DateOfBirth,
                RoleId = dto.RoleId,
                Email = dto.Email
            };
            var hashedPassowrd = passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassowrd;
            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();
        }
        public string Login(LoginUserDto dto)
        {
            var user = dbContext.Users.Include(x => x.Role).FirstOrDefault(x => x.Email == dto.Email);
            if (user is null)
                throw new NotFoundException("Email or password was wrong!");

            var password = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (password == PasswordVerificationResult.Failed)
                throw new BadRequestException("Email or password was wrong!");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName}{user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}")
               // new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd"))
            };

            if(user.DateOfBirth.HasValue)
            {
                claims.Add(
                    new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd"))
                    );
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);
            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer,
                authenticationSettings.JwtIssuer,
                claims, expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
