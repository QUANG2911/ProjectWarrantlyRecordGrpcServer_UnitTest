﻿using FakeItEasy;
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

            // Cấu hình mock cho 
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
            //Act
            var response = await _taskGrpcService.ListRepairManagement(request, null);

            // Assert
            Assert.NotNull(response);

            Assert.Equal(expectedTaskList, response);

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

            var expectedTask = new GetItemRepaitPartInWarrantyReponce
            {
                IdTask = 1,
                CustomerName = "Nguyễn Văn A",
                CustomerPhone = "098xx890xx",
                Amount = 1,
                Price = 1000,
                ReasonBringFix ="Hỏng màn",
                RepairPartName ="Card màn hình",
                StatusTask = 1
            };
            var expectedTaskList = new ReadRepairManagementResponse();
            expectedTaskList.ToRepairPartList.Add(expectedTask);

            // Cấu hình mock cho ICustomerService
            A.CallTo(() => _mockTaskService.GetStaffTaskDone(request.IdTask)).Returns(expectedTaskList);
            //Act
            var response = await _taskGrpcService.ReadRepairDone(request, null);

            // Assert
            Assert.NotNull(response);

            Assert.Equal(expectedTaskList, response);

            // Kiểm tra tương tác
            A.CallTo(() => _mockTaskService.GetStaffTaskDone(request.IdTask)).MustHaveHappenedOnceExactly();
        }
    }
}
