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
    public class RepairPartGrpcServiceTest
    {
        private readonly IRepairPart _mockRepairPartService;
        private readonly ILogger<RepairPartGrpcService> _mockLogger;
        private readonly RepairPartGrpcService _repairPartGrpcService;

        public RepairPartGrpcServiceTest()
        {
            // Tạo mock cho ICustomerService và ILogger
            _mockRepairPartService = A.Fake<IRepairPart>();
            _mockLogger = A.Fake<ILogger<RepairPartGrpcService>>();

            // Truyền mock vào lớp CustomerGrpcService
            _repairPartGrpcService = new RepairPartGrpcService(_mockRepairPartService, _mockLogger);
        }

        [Fact]
        public async Task ListRepairPartManagement_ReturnOk()
        {
            //Arrange
            var request = new GetListRepairPartRequest();

            var expextData = new GetRepairPartResponse { 
                IdRepairPart = 1,
                Price = 100,
                RepairPartName = "Màn hình"
            };
            var expextList = new GetListRepairPartResponse();
            expextList.ToListRepairPast.Add(expextData);

            A.CallTo(() => _mockRepairPartService.GetListRepairPart()).Returns(expextList);
            //Act
            var response = await _repairPartGrpcService.ListRepairPartManagement(request, null);

            // Assert
            Assert.NotNull(response);

            Assert.Equal(expextList, response);

            // Kiểm tra tương tác
            A.CallTo(() => _mockRepairPartService.GetListRepairPart()).MustHaveHappenedOnceExactly();
        }
    }
}
