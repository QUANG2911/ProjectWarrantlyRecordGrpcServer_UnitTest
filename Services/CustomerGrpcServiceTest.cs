
using FakeItEasy;
using Microsoft.Extensions.Logging;
using ProjectWarrantlyRecordGrpcServer.DTO;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;
using ProjectWarrantlyRecordGrpcServer.Services.Grpc;


namespace ProjectWarrantyRecordGrpcServer.Tests.Services
{
    public class CustomerGrpcServiceTest
    {
        private readonly ICustomerService _mockCustomerService;
        private readonly ILogger<CustomerGrpcService> _mockLogger;
        private readonly CustomerGrpcService _customerGrpcService;

        public CustomerGrpcServiceTest()
        {
            // Tạo mock cho ICustomerService và ILogger
            _mockCustomerService = A.Fake<ICustomerService>();
            _mockLogger = A.Fake<ILogger<CustomerGrpcService>>();

            // Truyền mock vào lớp CustomerGrpcService
            _customerGrpcService = new CustomerGrpcService(_mockCustomerService, _mockLogger);
        }

        [Fact]
        public async Task ListCustomerManagement_ReturnOk()
        {
            // Arrange
            var expectedCustomers = new GetItemInListCustomerResponse
            {
                IdCusomer = 1,
                CustomerAdrress = "Tp.Thủ Đức, Tp.Hồ Chí Minh",
                CustomerEmail = "anv@gmail.com",
                CustomerName = "Nguyễn Văn A",
                CustomerPhone = "098xx890xx"
           
            };

            var expectedList = new GetListCustomerManagementResponse();
            expectedList.ToCustomerList.Add(expectedCustomers);
            // Cấu hình mock cho ICustomerService
            A.CallTo(() => _mockCustomerService.GetListCustomer()).Returns(expectedList);

            var request = new GetListCustomerManagementRequest();

            // Act
            var response = await _customerGrpcService.ListCustomerManagement(request, null);

            // Assert
            Assert.NotNull(response);
            
            Assert.Equal(expectedList, response);

            // Kiểm tra tương tác
            A.CallTo(() => _mockCustomerService.GetListCustomer()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ReadCustomerManagement_ReturnOk()
        {
            // Arrange
            var expectedCustomers = new ReadItemDeviceCustomerManagementResponse
            {
                IdCusomer = 1,
                CustomerAdrress = "Tp.Thủ Đức, Tp.Hồ Chí Minh",
                CustomerEmail = "anv@gmail.com",
                CustomerName = "Nguyễn Văn A",
                CustomerPhone = "098xx890xx",
                CustomerDevice = "Máy tính",
                DateOfWarrant = "12/01/2024",
                IdWarrantReport = 1,
                TimeEnd = "12/01/2025"
                
            };
            var expectedList = new ReadCustomerManagementResponse();
            expectedList.ToDeviceList.Add(expectedCustomers); 
            // Cấu hình mock cho ICustomerService
            var request = new ReadCustomerRequest { IdCusomer = 1 };
            A.CallTo(() => _mockCustomerService.GetDetailCustomer(request.IdCusomer)).Returns(expectedList);
            

            //Act
            var response = await _customerGrpcService.ReadCustomerManagement(request, null);

            // Assert
            Assert.NotNull(response);

            Assert.Equal(expectedList, response);

            // Kiểm tra tương tác
            A.CallTo(() => _mockCustomerService.GetDetailCustomer(request.IdCusomer)).MustHaveHappenedOnceExactly();
        }
    }
}
