using FakeItEasy;
using Microsoft.Extensions.Logging;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Protos;
using ProjectWarrantlyRecordGrpcServer.Services.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWarrantyRecordGrpcServer.Tests.Services
{
    public class LoginGrpcServiceTest
    {
        private readonly ILoginService _mockLoginService;
        private readonly ILogger<LoginGrpcService> _mockLogger;
        private readonly LoginGrpcService _loginGrpcService;

        public LoginGrpcServiceTest()
        {
            // Tạo mock cho ICustomerService và ILogger
            _mockLoginService = A.Fake<ILoginService>();
            _mockLogger = A.Fake<ILogger<LoginGrpcService>>();

            // Truyền mock vào lớp CustomerGrpcService
            _loginGrpcService = new LoginGrpcService(_mockLoginService, _mockLogger);
        }

        [Fact]
        public async Task GetLogin_ReturnOk()
        {
            //Arrange
            var request = new GetLoginRequest { IdStaff = 1, Pass = "anv" };

            var expextData =  "Kĩ thuật viên" ;
            var expextToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE3MzY4NDAxMTAsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwNTkiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MDU5In0.4PxHbNHkvYlmLCqIzZl8cTedTokmsCec4N5n9ZwImno";

            A.CallTo(() => _mockLoginService.GetLogin(request.IdStaff,request.Pass)).Returns(expextData);
            A.CallTo(() => _mockLoginService.GetToken(request.IdStaff)).Returns(expextToken);
            //Act
            var response = await _loginGrpcService.GetLogin(request, null);

            // Assert
            Assert.NotNull(response);

            Assert.Equal(expextData, response.StaffPosition);
            Assert.Equal(expextToken, response.Token);

            // Kiểm tra tương tác
            A.CallTo(() => _mockLoginService.GetLogin(request.IdStaff, request.Pass)).MustHaveHappenedOnceExactly();
        }
    }
}
