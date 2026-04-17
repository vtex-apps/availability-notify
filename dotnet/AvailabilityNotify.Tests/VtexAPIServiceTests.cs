using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AvailabilityNotify.Data;
using AvailabilityNotify.Models;
using AvailabilityNotify.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using Vtex.Api.Context;
using Vtex.Api.Services;
using Xunit;

namespace AvailabilityNotify.Tests
{
    public class VtexAPIServiceTests
    {
        private const string TestAccount = "testaccount";

        private static DefaultHttpContext CreateHttpContext()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME] = TestAccount;
            httpContext.Request.Headers[Constants.HEADER_VTEX_CREDENTIAL] = "test-credential";
            return httpContext;
        }

        private static VtexAPIService CreateService(
            DefaultHttpContext httpContext,
            Func<HttpRequestMessage, HttpResponseMessage> httpResponder,
            Action<Mock<IIOContext>> configureVtex = null)
        {
            var handler = new VtexApiTestHttpMessageHandler(httpResponder);
            var httpClient = new HttpClient(handler);

            var clientFactory = new Mock<IHttpClientFactory>();
            clientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var httpAccessor = new Mock<IHttpContextAccessor>();
            httpAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            var env = new Mock<IVtexEnvironmentVariableProvider>();
            env.Setup(e => e.ApplicationVendor).Returns("vtex");
            env.Setup(e => e.ApplicationName).Returns("availability-notify");

            var repository = new Mock<IAvailabilityRepository>();
            repository.Setup(r => r.VerifySchema()).ReturnsAsync(true);

            var logger = new Mock<IIOLogger>(MockBehavior.Loose);
            var vtex = new Mock<IIOContext>();
            vtex.SetupGet(v => v.Logger).Returns(logger.Object);
            configureVtex?.Invoke(vtex);

            var io = new Mock<IIOServiceContext>();
            io.SetupGet(c => c.Vtex).Returns(vtex.Object);

            return new VtexAPIService(
                io.Object,
                env.Object,
                httpAccessor.Object,
                clientFactory.Object,
                repository.Object);
        }

        private static string ValidatedUserJson(string audience = "admin") =>
            JsonConvert.SerializeObject(new ValidatedUser
            {
                AuthStatus = "Success",
                Id = "login-42",
                User = "user@test.com",
                Audience = audience
            });

        [Fact]
        public async Task IsValidAuthUser_WhenAdminTokenEmpty_ReturnsUnauthorized()
        {
            var httpContext = CreateHttpContext();
            var service = CreateService(
                httpContext,
                _ => VtexApiTestHttpMessageHandler.Ok());

            var result = await service.IsValidAuthUser();

            Assert.Equal(HttpStatusCode.Unauthorized, result);
        }

        [Fact]
        public async Task IsValidAuthUser_WhenValidateAndLicenseManagerGrantSucceed_ReturnsOk()
        {
            var httpContext = CreateHttpContext();
            var service = CreateService(
                httpContext,
                req =>
                {
                    var url = req.RequestUri?.ToString() ?? string.Empty;
                    if (url.Contains("credential/validate", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Ok(ValidatedUserJson());
                    }

                    if (url.Contains("/logins/", StringComparison.Ordinal) && url.Contains("/granted", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Ok("true");
                    }

                    if (url.Contains("template-render", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Ok();
                    }

                    return VtexApiTestHttpMessageHandler.Status(HttpStatusCode.NotFound);
                },
                vtex => vtex.SetupGet(v => v.AdminUserAuthToken).Returns("admin-cookie"));

            var result = await service.IsValidAuthUser();

            Assert.Equal(HttpStatusCode.OK, result);
        }

        [Fact]
        public async Task IsValidAuthUser_WhenLicenseManagerReturnsFalse_ReturnsForbidden()
        {
            var httpContext = CreateHttpContext();
            var service = CreateService(
                httpContext,
                req =>
                {
                    var url = req.RequestUri?.ToString() ?? string.Empty;
                    if (url.Contains("credential/validate", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Ok(ValidatedUserJson());
                    }

                    if (url.Contains("/granted", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Ok("false");
                    }

                    if (url.Contains("template-render", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Ok();
                    }

                    return VtexApiTestHttpMessageHandler.Status(HttpStatusCode.NotFound);
                },
                vtex => vtex.SetupGet(v => v.AdminUserAuthToken).Returns("admin-cookie"));

            var result = await service.IsValidAuthUser();

            Assert.Equal(HttpStatusCode.Forbidden, result);
        }

        [Fact]
        public async Task IsValidAuthUser_WhenAudienceIsNotAdmin_ReturnsForbidden()
        {
            var httpContext = CreateHttpContext();
            var service = CreateService(
                httpContext,
                req =>
                {
                    var url = req.RequestUri?.ToString() ?? string.Empty;
                    if (url.Contains("credential/validate", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Ok(ValidatedUserJson(audience: "store"));
                    }

                    if (url.Contains("/granted", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Ok("true");
                    }

                    if (url.Contains("template-render", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Ok();
                    }

                    return VtexApiTestHttpMessageHandler.Status(HttpStatusCode.NotFound);
                },
                vtex => vtex.SetupGet(v => v.AdminUserAuthToken).Returns("admin-cookie"));

            var result = await service.IsValidAuthUser();

            Assert.Equal(HttpStatusCode.Forbidden, result);
        }

        [Fact]
        public async Task ValidateUserToken_WhenLicenseManagerReturnsFalse_ReturnsNull()
        {
            var httpContext = CreateHttpContext();
            var service = CreateService(
                httpContext,
                req =>
                {
                    var url = req.RequestUri?.ToString() ?? string.Empty;
                    if (url.Contains("credential/validate", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Ok(ValidatedUserJson());
                    }

                    if (url.Contains("/granted", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Ok("false");
                    }

                    if (url.Contains("template-render", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Ok();
                    }

                    return VtexApiTestHttpMessageHandler.Status(HttpStatusCode.NotFound);
                });

            var user = await service.ValidateUserToken("cookie");

            Assert.Null(user);
        }

        [Fact]
        public async Task ValidateUserToken_WhenCredentialValidateFails_ReturnsNull()
        {
            var httpContext = CreateHttpContext();
            var service = CreateService(
                httpContext,
                req =>
                {
                    var url = req.RequestUri?.ToString() ?? string.Empty;
                    if (url.Contains("credential/validate", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Status(HttpStatusCode.Unauthorized);
                    }

                    if (url.Contains("template-render", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Ok();
                    }

                    return VtexApiTestHttpMessageHandler.Status(HttpStatusCode.NotFound);
                });

            var user = await service.ValidateUserToken("any-token");

            Assert.Null(user);
        }

        [Fact]
        public async Task ValidateUserToken_WhenGrantedReturnsTrueText_ReturnsUser()
        {
            var httpContext = CreateHttpContext();
            var service = CreateService(
                httpContext,
                req =>
                {
                    var url = req.RequestUri?.ToString() ?? string.Empty;
                    if (url.Contains("credential/validate", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Ok(ValidatedUserJson());
                    }

                    if (url.Contains("/granted", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Ok("true");
                    }

                    if (url.Contains("template-render", StringComparison.Ordinal))
                    {
                        return VtexApiTestHttpMessageHandler.Ok();
                    }

                    return VtexApiTestHttpMessageHandler.Status(HttpStatusCode.NotFound);
                });

            var user = await service.ValidateUserToken("cookie");

            Assert.NotNull(user);
            Assert.Equal("Success", user.AuthStatus);
            Assert.Equal("login-42", user.Id);
            Assert.Equal("admin", user.Audience);
        }

        [Fact]
        public async Task ListNotifyRequests_ReturnsRepositoryData()
        {
            var expected = new[]
            {
                new NotifyRequest { SkuId = "1" }
            };

            var handler = new VtexApiTestHttpMessageHandler(req =>
            {
                var url = req.RequestUri?.ToString() ?? string.Empty;
                if (url.Contains("template-render", StringComparison.Ordinal))
                {
                    return VtexApiTestHttpMessageHandler.Ok();
                }

                return VtexApiTestHttpMessageHandler.Status(HttpStatusCode.NotFound);
            });
            var httpClient = new HttpClient(handler);
            var clientFactory = new Mock<IHttpClientFactory>();
            clientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var httpAccessor = new Mock<IHttpContextAccessor>();
            httpAccessor.Setup(a => a.HttpContext).Returns(CreateHttpContext());

            var env = new Mock<IVtexEnvironmentVariableProvider>();
            env.Setup(e => e.ApplicationVendor).Returns("vtex");
            env.Setup(e => e.ApplicationName).Returns("availability-notify");

            var repository = new Mock<IAvailabilityRepository>();
            repository.Setup(r => r.VerifySchema()).ReturnsAsync(true);
            repository.Setup(r => r.ListNotifyRequests()).ReturnsAsync(expected);

            var logger = new Mock<IIOLogger>(MockBehavior.Loose);
            var vtex = new Mock<IIOContext>();
            vtex.SetupGet(v => v.Logger).Returns(logger.Object);

            var io = new Mock<IIOServiceContext>();
            io.SetupGet(c => c.Vtex).Returns(vtex.Object);

            var service = new VtexAPIService(
                io.Object,
                env.Object,
                httpAccessor.Object,
                clientFactory.Object,
                repository.Object);

            var result = await service.ListNotifyRequests();

            Assert.Same(expected, result);
        }
    }
}
