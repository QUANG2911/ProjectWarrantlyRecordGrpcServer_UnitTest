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
    public class StaffTaskServiceTest
    {
        private readonly IStaffTaskService _mockTaskService;
        private readonly ILogger<StaffTaskGrpcService> _mockLogger;
        private readonly StaffTaskGrpcService _taskGrpcService;
        private readonly ITokenService _mockTokenService;
        private readonly ServerCallContext _mockContext;
        public static IEnumerable<object[]> CreateRepairManagementData =>
        new List<object[]>
        {
            new object[]
            {
                new CreateRepairManagementRequest
                {
                    ReasonBringFix = "Bị hỏng màn",
                    CustomerEmail = "anv@gmail.com",
                    CustomerName = "Nguyễn Văn A",
                    CustomerPhone = "098xx890xx",
                    DeviceName = "Máy Tính",
                    IdWarrantRecord = 1
                },
                1 // expected result
            },
            new object[]
            {
                new CreateRepairManagementRequest
                {
                    ReasonBringFix = "Bị hỏng màn",
                    CustomerEmail = "anv@gmail.com",
                    CustomerName = "Nguyễn Văn A",
                    CustomerPhone = "098xx890xx",
                    DeviceName = "Máy Tính",
                    IdWarrantRecord = 2
                },
                1 // expected result
            }


        };

        public StaffTaskServiceTest()
        {
            // Tạo mock cho ICustomerService và ILogger
            _mockTaskService = A.Fake<IStaffTaskService>();
            _mockLogger = A.Fake<ILogger<StaffTaskGrpcService>>();
            _mockTokenService = A.Fake<ITokenService>();
            _mockContext = A.Fake<ServerCallContext>();
            // Truyền mock vào lớp CustomerGrpcService
            _taskGrpcService = new StaffTaskGrpcService(_mockTaskService, _mockLogger,_mockTokenService);
        }

        [Theory]
        [MemberData(nameof(CreateRepairManagementData))]
        public async Task CreateRepairManagement_ReturnOk(CreateRepairManagementRequest request, int expect)
        {
            // Arrange
            int expectedTask = 1;

            A.CallTo(() => _mockTaskService.CreateNewStaffTask(request)).Returns(expectedTask);
            

            // Act
            var response = await _taskGrpcService.CreateRepairManagement(request, _mockContext);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(expect, response.IdTask);

            A.CallTo(() => _mockTaskService.CreateNewStaffTask(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ListRepairManagement_ReturnOk()
        {
            //Arrange
            var request = new GetListRepairManagementRequest
            {
                IdStaff = 1
            };

            var expectedTask = new ReadItemRepairManagementResponse
            {
                IdTask = 1,
                CustomerName = "Nguyễn Văn A",
                CustomerPhone = "098xx890xx",
                DateOfTask = "12/01/2025",
                DateOfWarranty = "12/01/2024",
                IdWarrantRecord = 1,
                StatusTask = 1
            };
            var expectedTaskList = new GetListRepairManagementResponse();
            expectedTaskList.ToList.Add(expectedTask);


            // Cấu hình mock cho
            A.CallTo(() => _mockTaskService.GetListStaffTask(request.IdStaff)).Returns(expectedTaskList);
            A.CallTo(() => _mockTokenService.CheckTokenIdStaff(request.IdStaff, _mockContext)).Returns("done");
            //Act
            var response = await _taskGrpcService.ListRepairManagement(request, _mockContext);

            // Assert
            Assert.NotNull(response);

            Assert.Equal(expectedTaskList, response);

            // Kiểm tra tương tác
            A.CallTo(() => _mockTaskService.GetListStaffTask(request.IdStaff)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mockTokenService.CheckTokenIdStaff(request.IdStaff, _mockContext)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetStaffTask_ReturnOk()
        {
            //Arrange
            var request = new ReadToRequest
            {
                IdTask = 1
            };

            var expectedTask = new GetItemRepaitPartInWarrantyReponce
            {
                Amount = 1,
                Price = 1000,
                RepairPartName ="Card màn hình",
            };
            var expectedTaskList = new ReadRepairManagementResponse();
            expectedTaskList.ToRepairPartList.Add(expectedTask);

            // Cấu hình mock cho ICustomerService
            A.CallTo(() => _mockTaskService.GetStaffTaskDone(request.IdTask)).Returns(expectedTaskList);
            //Act
            var response = await _taskGrpcService.ReadRepairDone(request, _mockContext);

            // Assert
            Assert.NotNull(response);

            Assert.Equal(expectedTaskList, response);

            // Kiểm tra tương tác
            A.CallTo(() => _mockTaskService.GetStaffTaskDone(request.IdTask)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateRepairManagement_ReturnOk()
        {
            //Arrange
            var request = new UpdateRepairManagementRequest { IdStaff = 1, IdTask = 1, StatusTask = 1 };

            var expectedTask = request.IdTask;

            //Cấu hình mock
            A.CallTo(() => _mockTaskService.UpdateStaffTask(request)).Returns(expectedTask);
            A.CallTo(() => _mockTokenService.CheckTokenIdStaff(request.IdStaff, _mockContext)).Returns("done");

            //Act
            var response = await _taskGrpcService.UpdateRepairManagement(request, _mockContext);

            //Assert
            Assert.NotNull(response);

            Assert.Equal(expectedTask, response.IdTask);

            // Kiểm tra tương tác
            A.CallTo(() => _mockTaskService.UpdateStaffTask(request)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mockTokenService.CheckTokenIdStaff(request.IdStaff, _mockContext)).MustHaveHappenedOnceExactly();
        }
    }
}
