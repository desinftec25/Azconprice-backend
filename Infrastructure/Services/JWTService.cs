﻿using Application.Models;
using Application.Services;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class JWTService : IJWTService
    {
        private readonly JWTConfig _config;

        public JWTService(JWTConfig config)
        {
            _config = config;
        }

        public string GenerateSecurityToken(string id, string email,string firstName,string lastName, IEnumerable<string> roles, IEnumerable<Claim> userClaims)
        {
            var claims = new[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email),
                new Claim("userId", id),
                new Claim("firstName", firstName),
                new Claim("lastName", lastName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, string.Join(",", roles))
            }.Concat(userClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                expires: DateTime.UtcNow.AddHours(_config.ExpireMunites),
                signingCredentials: signingCredentials,
                claims: claims
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return accessToken;
        }
    }
}
