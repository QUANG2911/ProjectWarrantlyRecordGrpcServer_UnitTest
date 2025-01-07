
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
            var expectedCustomers = new List<Customer>
            {
                new Customer
                {
                    IdCustomer = 1,
                    CustomerAddress = "Tp.Thủ Đức, Tp.Hồ Chí Minh",
                    CustomerEmail = "anv@gmail.com",
                    CustomerName = "Nguyễn Văn A",
                    CustomerPhone = "098xx890xx"
                }
            };

            // Cấu hình mock cho ICustomerService
            A.CallTo(() => _mockCustomerService.GetListCustomer()).Returns(expectedCustomers);

            var request = new GetListCustomerManagementRequest();

            // Act
            var response = await _customerGrpcService.ListCustomerManagement(request, null);

            // Assert
            Assert.NotNull(response);
            Assert.Single(response.ToCustomerList);

            var firstCustomer = response.ToCustomerList.First();
            Assert.Equal(1, firstCustomer.IdCusomer);
            Assert.Equal("Nguyễn Văn A", firstCustomer.CustomerName);
            Assert.Equal("098xx890xx", firstCustomer.CustomerPhone);
            Assert.Equal("anv@gmail.com", firstCustomer.CustomerEmail);
            Assert.Equal("Tp.Thủ Đức, Tp.Hồ Chí Minh", firstCustomer.CustomerAdrress);

            // Kiểm tra tương tác
            A.CallTo(() => _mockCustomerService.GetListCustomer()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ReadCustomerManagement_ReturnOk()
        {
            // Arrange
            var expectedCustomers = new List<DetailCustomerDto>
            {
                new DetailCustomerDto
                {
                    IdCusomer = 1,
                    CustomerAdrress = "Tp.Thủ Đức, Tp.Hồ Chí Minh",
                    CustomerEmail = "anv@gmail.com",
                    CustomerName = "Nguyễn Văn A",
                    CustomerPhone = "098xx890xx",
                    CustomerDevice = "Máy tính",
                    DateOfWarrant = DateOnly.Parse("12/01/2024"),
                    IdWarrantReport = 1,
                    TimeEnd = DateOnly.Parse("12/01/2025")
                }
            };

            // Cấu hình mock cho ICustomerService
            var request = new ReadCustomerRequest { IdCusomer = 1 };
            A.CallTo(() => _mockCustomerService.GetDetailCustomer(request.IdCusomer)).Returns(expectedCustomers);
            

            //Act
            var response = await _customerGrpcService.ReadCustomerManagement(request, null);

            // Assert
            Assert.NotNull(response);
            Assert.Single(response.ToDeviceList);

            var firstCustomer = response.ToDeviceList.First();
            Assert.Equal(1, firstCustomer.IdCusomer);
            Assert.Equal("Nguyễn Văn A", firstCustomer.CustomerName);
            Assert.Equal("098xx890xx", firstCustomer.CustomerPhone);
            Assert.Equal("anv@gmail.com", firstCustomer.CustomerEmail);
            Assert.Equal("Tp.Thủ Đức, Tp.Hồ Chí Minh", firstCustomer.CustomerAdrress);
            Assert.Equal("Máy tính", firstCustomer.CustomerDevice);
            Assert.Equal("12/1/2024", firstCustomer.DateOfWarrant);
            Assert.Equal("12/1/2025", firstCustomer.TimeEnd);
            // Kiểm tra tương tác
            A.CallTo(() => _mockCustomerService.GetDetailCustomer(request.IdCusomer)).MustHaveHappenedOnceExactly();
        }
    }
}
