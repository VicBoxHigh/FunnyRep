using DumyReportes.Data;
using DumyReportes.Flags;
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
            dummyToken.Append(";");
            dummyToken.Append(10 * (int)user.AccessLevel);

            dummyToken.Append(";");
            dummyToken.Append(user.Name);
            dummyToken.Append(";");
            dummyToken.Append(user.NumEmpleado);
            dummyToken.Append(";");

            string tokenb64 = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(dummyToken.ToString())
                );

            return tokenb64;
        }



        /*
         Si el token es valido retorna el objeto usuario
         */

        public static int MINUTES_VALID_TOKEN = 120;
        public static ErrorFlag ValidateToke(string token, out User user)
        {
            user = null;
            if (String.IsNullOrEmpty(token)) return ErrorFlag.ERROR_INVALID_TOKEN;
            byte[] tokenBytesDecrypted = null;
            try
            {
                tokenBytesDecrypted = Convert.FromBase64String(token);

            }
            catch (Exception ex)
            {
                return ErrorFlag.ERROR_PARSE;
            }

            if (tokenBytesDecrypted == null || tokenBytesDecrypted.Length == 0) return ErrorFlag.ERROR_INVALID_TOKEN;

            string tokenStr = Encoding.UTF8.GetString(tokenBytesDecrypted);

            ErrorFlag resultUFAT = UserFromArrayToken(tokenStr, out User userCredentialsLogin);
            if (resultUFAT != ErrorFlag.ERROR_OK_RESULT) return resultUFAT;//

            UserDataContext userDataCtx = new UserDataContext();

            IReportObject userRepObj = null;//basado en el 
            ErrorFlag resultGetUser = userDataCtx.Get(userCredentialsLogin.IdUser, out userRepObj, out string error);

            if (resultGetUser != ErrorFlag.ERROR_OK_RESULT) return resultGetUser;




            if (!UserCredentialAgainstDB(userCredentialsLogin, userRepObj))
            {
                return ErrorFlag.ERROR_INVALID_TOKEN;
            }
            user = userRepObj as User;
            user.Pass = "";//se elimina el pass, por cualquier cosa 
            user.CurrentToke = token;
            //TODO
            //if(tokenProperties == user) return ok else return null



            return ErrorFlag.ERROR_OK_RESULT;
        }

        private static bool UserCredentialAgainstDB(User userCredentialsLogin, IReportObject repoObj)
        {

            if (userCredentialsLogin == null) return false;
            if (repoObj == null) return false;


            User userDb = repoObj as User;

            return userDb.IsEnabled
                && userDb.IdUser == userCredentialsLogin.IdUser
                && userCredentialsLogin.UserName.Equals(userDb.UserName)
                && userCredentialsLogin.Pass.Equals(userDb.Pass);



        }

        private static ErrorFlag UserFromArrayToken(string token, out User user)
        {

            user = null;
            string[] tokenProperties = token.Split(';');



            if (tokenProperties == null || tokenProperties.Length == 0 || tokenProperties.Length != 5) return ErrorFlag.ERROR_INVALID_TOKEN;
            DateTime dateTime = DateTime.Parse(tokenProperties[3]);

            DateTime tokenExpiration = dateTime.AddMinutes(120);

            int resultCompare = DateTime.Compare(DateTime.Now, tokenExpiration);

            //if(resultCompare <=0) //DateNow es igual o menor que la fecha de expiration del token.
            if (resultCompare > 0)//DateNow sobrepasa el expiration date del token
            {
                return ErrorFlag.ERROR_EXPIRED_TOKEN;
            }

            /*if (Math.Abs(dateTime.Subtract(DateTime.Now).TotalMinutes) > MINUTES_VALID_TOKEN)
            {
                return ErrorFlag.ERROR_EXPIRED_TOKEN;
            }
*/

  
            user = new User()
            {
                IdUser = int.Parse(tokenProperties[0]),
                UserName = tokenProperties[1],
                AccessLevel = (Flags.AccessLevel)int.Parse(tokenProperties[4]),
                Name = tokenProperties[5],
                NumEmpleado = tokenProperties[6]


            };



            return ErrorFlag.ERROR_OK_RESULT;

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