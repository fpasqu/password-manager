using Microsoft.IdentityModel.Tokens;
using PasswordManagerAspNet.Models.Entities;
using PasswordManagerAspNet.Models.Repositories;
using System;
using Xunit;

namespace PasswordManagerAspNet.Test
{
    public class PasswordShould
    {
        [Fact]
        public void BeTheSameWhenEncryptedAndDecrypted()
        {
            Functions f = new Functions();
            string key = "this is a key";

            Password p = new Password();
            p.Id = new Guid().ToString();
            p.UserMail = "owner@text.com";
            p.AccountName = "Microsoft";
            p.AccountEmail = "email@test.com";
            p.PasswordValue = "this is a password";
            p.Notes = "My work account";
            
            f.EncryptDecryptPassword(p, key, true);
            f.EncryptDecryptPassword(p, key, false);
            Assert.Equal("this is a password", p.PasswordValue);
        }

        [Fact]
        public void BeEncryptedEvenWithMissingValues() 
        {
            Functions f = new Functions();
            string key = "this is a key";

            Password p = new Password();
            p.Id = new Guid().ToString();
            p.UserMail = "owner@text.com";
            p.AccountName = "Microsoft";
            p.AccountEmail = "email@test.com";
            p.PasswordValue = "this is a password";
            //notes is missing
            
            f.EncryptDecryptPassword(p, key, true);
            f.EncryptDecryptPassword(p, key, false);
            Assert.True(p.Notes.IsNullOrEmpty());
        }

        [Fact]
        public void EncryptDecryptWithStaticIV()
        {
            var functions = new Functions();
            var key = "YourSecretKey";
            var dataToEncrypt = "SensitiveData";
            var encryptedData = functions.Encrypt(dataToEncrypt, key);
            var decryptedData = functions.Decrypt(encryptedData, key);

            Assert.Equal(dataToEncrypt, decryptedData);
        }
    }
}