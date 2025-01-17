using FakeItEasy;
using Grpc.Core;
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
    public class WarrantyRecordGrpcServiceTest
    {
        private readonly IWarranyRecordService _mockWarranyRecordService;
        private readonly ILogger<WarrantyRecordGrpcService> _mockLogger;
        private readonly WarrantyRecordGrpcService _warrantyRecordGrpcService;
        private readonly ITokenService _mockTokenService;
        private readonly ServerCallContext _mockContext;
        public WarrantyRecordGrpcServiceTest()
        {
            // Tạo mock cho ICustomerService và ILogger
            _mockWarranyRecordService = A.Fake<IWarranyRecordService>();
            _mockLogger = A.Fake<ILogger<WarrantyRecordGrpcService>>();
            _mockTokenService = A.Fake<ITokenService>();
            _mockContext = A.Fake<ServerCallContext>();
            // Truyền mock vào lớp CustomerGrpcService
            _warrantyRecordGrpcService = new WarrantyRecordGrpcService(_mockWarranyRecordService, _mockLogger, _mockTokenService);
        }

        [Fact]
        public async Task GetListWarrantyRecordManagement_ReturnOk()
        {
            //Arrange
            var request = new GetWarrantyListRequest { IdStaff = 1};
            
            var expextData = new GetWarrantyResponse
            {
                CustomerName = "Nguyễn Văn A",
                DateOfResig = "12/1/2024",
                DeviceName = "Máy tính",
                IdCustomer = 1,
                IdWarrantyRecord = 1,
                TimeEnd = "12/1/2025"
            };
            var expextList = new GetWarrantyListResponse();
            expextList.ToWarrantyList.Add(expextData);

            A.CallTo(() => _mockWarranyRecordService.GetListWarrantyList()).Returns(expextList);
            A.CallTo(() => _mockTokenService.CheckTokenIdStaff(request.IdStaff, _mockContext)).Returns("done");
            //Act
            var response = await _warrantyRecordGrpcService.GetListWarrantyRecordManagement(request, _mockContext);

            // Assert
            Assert.NotNull(response);

            Assert.Equal(expextList, response);

            // Kiểm tra tương tác
            A.CallTo(() => _mockWarranyRecordService.GetListWarrantyList()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mockTokenService.CheckTokenIdStaff(request.IdStaff, _mockContext)).MustHaveHappenedOnceExactly();
        }
    }
}
