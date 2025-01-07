using FakeItEasy;
using Microsoft.Extensions.Logging;
using ProjectWarrantlyRecordGrpcServer.DTO;
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

        public StaffTaskServiceTest()
        {
            // Tạo mock cho ICustomerService và ILogger
            _mockTaskService = A.Fake<IStaffTaskService>();
            _mockLogger = A.Fake<ILogger<StaffTaskGrpcService>>();

            // Truyền mock vào lớp CustomerGrpcService
            _taskGrpcService = new StaffTaskGrpcService(_mockTaskService, _mockLogger);
        }

        [Fact]
        public async Task CreateRepairManagement_ReturnOk()
        {
            //Arrange
            var request = new CreateRepairManagementRequest
            {
                ReasonBringFix = "Bị hỏng màn",
                CustomerEmail = "anv@gmail.com",
                CustomerName = "Nguyễn Văn A",
                CustomerPhone = "098xx890xx",
                DeviceName = "Máy Tính",
                IdWarrantRecord = 1
            };

            int expectedTask = 1; // id task sửa chữa mới

            // Cấu hình mock cho ICustomerService
            A.CallTo(() => _mockTaskService.CreateNewStaffTask(request)).Returns(expectedTask);
            //Act
            var response = await _taskGrpcService.CreateRepairManagement(request, null);

            // Assert
            Assert.NotNull(response);
            
            Assert.Equal(expectedTask, response.IdTask);

            // Kiểm tra tương tác
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

            var expectedTask = new List<ItemInListStaffTaskDto>
            {
                new ItemInListStaffTaskDto
                {                    
                    CustomerName = "Nguyễn Văn A",
                    CustomerPhone = "098xx890xx",
                    IdTask = 1,
                    DateOfTask = DateOnly.Parse("12/01/2025"),
                    DateOfWarranty = DateOnly.Parse("12/01/2024"),
                    IdWarrantyRecord = 1,
                    StatusTask = 1
                }
            };

            // Cấu hình mock cho ICustomerService
            A.CallTo(() => _mockTaskService.GetListStaffTask(request.IdStaff)).Returns(expectedTask);
            //Act
            var response = await _taskGrpcService.ListRepairManagement(request, null);

            // Assert
            Assert.NotNull(response);

            Assert.Equal(expectedTask.Count, response.ToList.Count);

            var firstCustomer = response.ToList.First();
            Assert.Equal(expectedTask.First().IdWarrantyRecord, firstCustomer.IdWarrantRecord);
            Assert.Equal(expectedTask.First().IdTask, firstCustomer.IdTask);
            Assert.Equal(expectedTask.First().StatusTask, firstCustomer.StatusTask);
            Assert.Equal("12/1/2024", firstCustomer.DateOfWarranty);
            Assert.Equal(expectedTask.First().CustomerName, firstCustomer.CustomerName);
            Assert.Equal(expectedTask.First().CustomerPhone, firstCustomer.CustomerPhone);
            Assert.Equal("12/1/2025", firstCustomer.DateOfTask);

            // Kiểm tra tương tác
            A.CallTo(() => _mockTaskService.GetListStaffTask(request.IdStaff)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetStaffTask_ReturnOk()
        {
            //Arrange
            var request = new ReadToRequest
            {
                IdTask = 1
            };

            var expectedTask = new List<ItemInListStaffTaskDto>
            {
                new ItemInListStaffTaskDto
                {
                    CustomerName = "Nguyễn Văn A",
                    CustomerPhone = "098xx890xx",
                    IdTask = 1,
                    DateOfTask = DateOnly.Parse("12/01/2025"),
                    DateOfWarranty = DateOnly.Parse("12/01/2024"),
                    IdWarrantyRecord = 1,
                    StatusTask = 1
                }
            };

            // Cấu hình mock cho ICustomerService
            A.CallTo(() => _mockTaskService.GetListStaffTask(request.IdStaff)).Returns(expectedTask);
            //Act
            var response = await _taskGrpcService.ListRepairManagement(request, null);

            // Assert
            Assert.NotNull(response);

            Assert.Equal(expectedTask.Count, response.ToList.Count);

            var firstCustomer = response.ToList.First();
            Assert.Equal(expectedTask.First().IdWarrantyRecord, firstCustomer.IdWarrantRecord);
            Assert.Equal(expectedTask.First().IdTask, firstCustomer.IdTask);
            Assert.Equal(expectedTask.First().StatusTask, firstCustomer.StatusTask);
            Assert.Equal("12/1/2024", firstCustomer.DateOfWarranty);
            Assert.Equal(expectedTask.First().CustomerName, firstCustomer.CustomerName);
            Assert.Equal(expectedTask.First().CustomerPhone, firstCustomer.CustomerPhone);
            Assert.Equal("12/1/2025", firstCustomer.DateOfTask);

            // Kiểm tra tương tác
            A.CallTo(() => _mockTaskService.GetListStaffTask(request.IdStaff)).MustHaveHappenedOnceExactly();
        }
    }
}
