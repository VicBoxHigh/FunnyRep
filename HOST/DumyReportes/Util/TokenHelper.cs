﻿using DumyReportes.Data;
using DumyReportes.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace DumyReportes.Util
{
    public class TokenHelper
    {

        public static string GenerateToken(User user)
        {
            StringBuilder dummyToken = new StringBuilder();

            dummyToken.Append(user.IdUser);
            dummyToken.Append(";");
            dummyToken.Append(user.UserName);
            dummyToken.Append(";");
            dummyToken.Append(user.AccessLevel.ToString());
            dummyToken.Append(";");
            dummyToken.Append(DateTime.Now);

            string tokenb64 = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(dummyToken.ToString())
                );

            return tokenb64;
        }



        /*
         Si el token es valido retorna el objeto usuario
         */

        public static int MINUTES_VALID_TOKEN = 120;
        public static User ValidateToke(string token)
        {
            if (String.IsNullOrEmpty(token)) return null;

            byte[] tokenBytesDecrypted = Convert.FromBase64String(token);

            if (tokenBytesDecrypted == null || tokenBytesDecrypted.Length == 0) return null;

            string tokenStr = Encoding.UTF8.GetString(tokenBytesDecrypted);

            string[] tokenProperties = tokenStr.Split(';');

            if (tokenProperties == null || tokenProperties.Length == 0) return null;

            UserDataContext userDataCtx = new UserDataContext();

            DateTime dateTime = DateTime.Parse(tokenProperties[3]);
            int idUser = int.Parse(tokenProperties[0]);

            if (dateTime.Subtract(DateTime.Now).TotalMinutes > MINUTES_VALID_TOKEN)
            {
                return null;
            }

            IReportObject repoObj = null;//basado en el 
            userDataCtx.Get(idUser, out repoObj, out string error);

            User user = repoObj as User;
            user.CurrentToke = token;


            return user;
        }

        public static string GenerateToken(string userName)
        {
            var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
            var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
            var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
            var expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName) });

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                    audience: audienceToken,
                    issuer: issuerToken,
                    subject: claimsIdentity,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime)),
                    signingCredentials: signingCredentials);

            string jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;


        }

    }
}