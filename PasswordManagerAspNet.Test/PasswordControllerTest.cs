using Microsoft.AspNetCore.Mvc;
using Moq;
using PasswordManagerAspNet.Controllers;
using PasswordManagerAspNet.Models.Entities;
using PasswordManagerAspNet.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace PasswordManagerAspNet.Test
{
    public class PasswordControllerTest
    {
        public readonly Mock<IPasswordRepository> mockPasswordRepository = new();
        public readonly Mock<IFunctions> mockFunctions = new();

        [Fact]
        public async Task ListTest()
        {
            mockFunctions.Setup(f => f.GetCurrentUserEmail(It.IsAny<ClaimsPrincipal>()))
                .Returns(It.IsAny<string>);
            mockFunctions.Setup(f => f.Encrypt(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<string>);
            mockPasswordRepository.Setup(repo => repo.GetPasswordsForUserAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Password>(){
                    new Password(),
                    new Password()
                });
            mockFunctions.Setup(f => f.EncryptDecryptPassword(It.IsAny<Password>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(It.IsAny<Password>());

            var controller = new PasswordController(mockPasswordRepository.Object, mockFunctions.Object);
            var result = await controller.List();
            Assert.IsType<ViewResult>(result);
        }

        /*
        [SkippableFact]
        public async Task DetailsTest()
        {
            mockFunctions.Setup(f => f.GetCurrentUserEmail(It.IsAny<ClaimsPrincipal>()))
                .Returns(It.IsAny<string>);
            mockPasswordRepository.Setup(repo => repo.GetPasswordByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<Password>);
            mockFunctions.Setup(f => f.EncryptDecryptPassword(It.IsAny<Password>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(It.IsAny<Password>());

            var controller = new PasswordController(mockPasswordRepository.Object, mockFunctions.Object);
            var result = await controller.Details(Guid.NewGuid());
            Assert.IsType<ViewResult>(result);
        }
        */

        [Fact]
        public async Task EditTest()
        {
            mockFunctions.Setup(f => f.GetCurrentUserEmail(It.IsAny<ClaimsPrincipal>()))
                .Returns(It.IsAny<string>);
            mockFunctions.Setup(f => f.EncryptDecryptPassword(It.IsAny<Password>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(It.IsAny<Password>());
            mockPasswordRepository.Setup(repo => repo.UpdatePasswordAsync(It.IsAny<Password>()))
                .ReturnsAsync(It.IsAny<Password>());

            var controller = new PasswordController(mockPasswordRepository.Object, mockFunctions.Object);
            var result = await controller.Edit(It.IsAny<Password>());
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task CreateTest()
        {
            mockFunctions.Setup(f => f.GetCurrentUserEmail(It.IsAny<ClaimsPrincipal>()))
                .Returns("prova@gmail.com");
            mockFunctions.Setup(f => f.EncryptDecryptPassword(It.IsAny<Password>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(It.IsAny<Password>());
            mockPasswordRepository.Setup(repo => repo.CreatePasswordAsync(It.IsAny<Password>()))
                .ReturnsAsync(It.IsAny<Password>());
            Password p = new Password { UserMail = "prova@gmail.com" };

            var controller = new PasswordController(mockPasswordRepository.Object, mockFunctions.Object);
            var result = await controller.Create(p);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task DeleteTest()
        {
            mockFunctions.Setup(f => f.GetCurrentUserEmail(It.IsAny<ClaimsPrincipal>()))
                .Returns(It.IsAny<string>);
            mockPasswordRepository.Setup(repo => repo.GetPasswordByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<Password>());
            mockFunctions.Setup(f => f.EncryptDecryptPassword(It.IsAny<Password>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(It.IsAny<Password>());
            
            var controller = new PasswordController(mockPasswordRepository.Object, mockFunctions.Object);
            var result = await controller.Delete(It.IsAny<Guid>());
            Assert.IsType<ViewResult>(result);
        }
    }
}
