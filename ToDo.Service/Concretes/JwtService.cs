﻿using Core.Tokens.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ToDo.Models.Dtos.Tokens.Responses;
using ToDo.Models.Entities;
using ToDo.Service.Abstracts;

namespace ToDo.Service.Concretes;

public sealed class JwtService : IJwtService
{
    private readonly TokenOption _tokenOption;
    private readonly UserManager<User> _userManager;

    public JwtService(IOptions<TokenOption> tokenOption, UserManager<User> userManager)
    {
        _tokenOption = tokenOption.Value;
        _userManager = userManager;
    }
    public async Task<TokenResponseDto> CreateJwtTokenAsync(User user)
    {
        var accessTokenExpirarion = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
        var secretKey = SecurityKeyHelper.GetSecurityKey(_tokenOption.SecurityKey);

        SigningCredentials sc = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
            (
                issuer: _tokenOption.Issuer,
                claims: await GetClaims(user, _tokenOption.Audience),
                expires: accessTokenExpirarion,
                signingCredentials: sc
            );

        var handler = new JwtSecurityTokenHandler();
        string token = handler.WriteToken(jwtSecurityToken);

        return new TokenResponseDto
        {
            AccessToken = token,
            AccessTokenExpiration = accessTokenExpirarion
        };
    }

    public async Task<List<Claim>> GetClaims(User user, List<string> audiences)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("email", user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("Uraz_kağan", "Güneş")
        };

        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Count > 0)
        {
            claims.AddRange(roles.Select(x => new Claim("roles", x)));
        }

        claims.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

        return claims;
    }
}
