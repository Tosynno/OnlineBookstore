using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineBookstore.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OnlineBookstore.Application.Models;
using Newtonsoft.Json.Serialization;
using OnlineBookstore.Infrastructure.Context;
using OnlineBookstore.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace OnlineBookstore.Application.Utilies
{
    public class EncryptionActionFilter : IActionFilter
    {
        private ILogger<EncryptionActionFilter> _logger;
        private string _request;
        private string _encryptedrequest;
        private readonly BookdbContext _bookdbContext;
        private readonly AppSettings _appSettings;

        public EncryptionActionFilter(ILogger<EncryptionActionFilter> logger, IOptions<AppSettings> appSettings, BookdbContext bookdbContext)
        {
            _logger = logger;
            _request = "";
            _encryptedrequest = "";
            _appSettings = appSettings.Value;
            _bookdbContext = bookdbContext;
        }
        static byte[] GenerateRandomIV()
        {
            //using (var rng = new RNGCryptoServiceProvider())
            //{
            byte[] iv = new byte[16]; // IV size for AES is typically 16 bytes
                                      //rng.GetBytes(iv);
            return iv;
            //}
        }
        public async void OnActionExecuted(ActionExecutedContext context)
        {
            var data = context.HttpContext.Response.Body;
            var result = context.Result;
            string IV = string.Empty;
            string Key = string.Empty;
            // var headerValue = context.HttpContext.Request.Headers["ClientId"].FirstOrDefault();

            //IV = user.Result.IV;
            Key = _appSettings.ClientKey;

            if (result is JsonResult json)
            {
                var x = json.Value;
                var status = json.StatusCode;
                var encryptdata = EncryptString(Key, JsonConvert.SerializeObject(x));
                _logger.LogInformation(JsonConvert.SerializeObject(x));
            }
            if (result is ObjectResult view)
            {

                var status = view.StatusCode;
                var x = view.Value;
                //var content = JsonConvert.SerializeObject(x);
                var reslt = JsonConvert.SerializeObject(x, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() } });
                var encryptdata = EncryptString(Key, reslt);
                view.Value = encryptdata;

                _logger.LogInformation(JsonConvert.SerializeObject(x));
            }

        }
        

        public async Task ProcessEncrytedBody(ActionExecutingContext context)
        {
            HttpContext httpContext = context.HttpContext;

            var param = context.ActionArguments.SingleOrDefault(p => p.Value is EncryptClass);

            var requestBody = param.Value as EncryptClass;
            if (!string.IsNullOrWhiteSpace(requestBody.Data))
            {

                string IV = string.Empty;
                string Key = string.Empty;
                //var headerValue = httpContext.Request.Headers["ClientId"].FirstOrDefault();

                // IV = user.Result.IV;
                Key = _appSettings.ClientKey;
                var data = context.ActionArguments.FirstOrDefault();

                var decryptRequest = DecryptString(Key, requestBody.Data);

                _encryptedrequest = requestBody.Data;
                _request = decryptRequest;
                //var result = httpContext.Request.QueryString = new QueryString("?"+""+ decryptedString + "");
               
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                   
                string[] auth = context.HttpContext.Request.Headers["Authorization"].ToString().Split(':');

                //Validate session
                var res = await _bookdbContext.Admin_Users.FirstOrDefaultAsync(c => c.Password.Equals(auth[0].ToString()));
                if (res != null)
                {
                    Admin_Activity_log log = new Admin_Activity_log();
                    log.AdminId = res.Id;
                    log.Messagex = $"Action: {descriptor.ActionName} - {decryptRequest.Split('=')[^1]}";
                    log.CreatedDate = DateTime.Now;
                    _bookdbContext.Admin_Activity_logs.Add(log);
                    await _bookdbContext.SaveChangesAsync();
                }

                context.HttpContext.Items["data"] = decryptRequest;


            }
        }


        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            HttpRequestRewindExtensions.EnableBuffering(request);



            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            string requestBody = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            return $"{requestBody}";
        }
        public async void OnActionExecuting(ActionExecutingContext context)
        {
            HttpContext httpContext = context.HttpContext;
            var data = context.HttpContext.Response.Body;
            var result = context.Result;
            if (httpContext.Request.Method == "POST")
            {
                ProcessEncrytedBody(context).GetAwaiter().GetResult();
                return;

            }

            //context.HttpContext.Request.Body = DecryptStream(context.HttpContext.Request.Body);
            if (context.ActionArguments.Count > 0)
            {
                string IV = string.Empty;
                string Key = string.Empty;
                //var headerValue = httpContext.Request.Headers["ClientId"].FirstOrDefault();

                //IV = user.Result.IV;
                Key = _appSettings.ClientKey;
                var data1 = context.ActionArguments.LastOrDefault();
                //var ddd = "VONQtae21zTWRa73fskVhCJKeJzJxURPf4loUGieLG1Slyw/LPi0Cj++TTIkQGAIEGrLiK5AVGcOn+RSh91DXzu+wOnkwXDmkMCSk9J4qLA=";
                var splitResult = data1.Value.ToString();
                var split = string.Empty;

                var decryptRequest = DecryptString(Key, splitResult);
                string decryptedString = splitResult[0] + "=" + decryptRequest;

                _encryptedrequest = splitResult;
                _request = decryptRequest;
                context.HttpContext.Items["data"] = decryptedString;

                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

                string[] auth = context.HttpContext.Request.Headers["Authorization"].ToString().Split(':');

                //Validate session
                var res = await _bookdbContext.Admin_Users.FirstOrDefaultAsync(c => c.Password.Equals(auth[0].ToString()));
                if (res != null)
                {
                    Admin_Activity_log log = new Admin_Activity_log();
                    log.AdminId = res.Id;
                    log.Messagex = $"Action: {descriptor.ActionName} - {decryptRequest.Split('=')[^1]}";
                    log.CreatedDate = DateTime.Now;
                    _bookdbContext.Admin_Activity_logs.Add(log);
                    await _bookdbContext.SaveChangesAsync();
                }
            }
        }

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = GenerateRandomIV();

            //if (!cipherText.Contains('='))
            //{
            //    return "Test is not base 64 encodded";
            //}
            byte[] buffer;
            try
            {
                buffer = Convert.FromBase64String(cipherText);
            }
            catch (Exception)
            {
                return cipherText;
            }

            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 256; // Choose an appropriate key size

                aes.GenerateKey();
                //using (var decryptors = aes.CreateDecryptor(aes.Key, iv))
                //{
                //    byte[] decryptedBytes = decryptors.TransformFinalBlock(buffer, 0, buffer.Length);
                //   // return Encoding.UTF8.GetString(decryptedBytes);
                //}
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        static byte[] HexToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
              .Where(x => x % 2 == 0)
              .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
              .ToArray();
        }

        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = GenerateRandomIV();
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 256; // Choose an appropriate key size

                aes.GenerateKey();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
        //private async Task<tbl_AuthenicateKey> Fetch(int ClientId)
        //{
        //    var query = @"Select * From [dbo].[tbl_AuthenicateKey] where ClientId = @ClientId";
        //    var param = new { ClientId };

        //    var response = await _connection.QueryFirstOrDefaultAsync<tbl_AuthenicateKey>(query, param);
        //    return response;
        //}
    }
}
