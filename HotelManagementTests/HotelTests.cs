using NUnit.Framework;
using HotelManagement.Models;
using HotelManagement.Data;
using HotelManagement.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using HotelManagement.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace HotelManagement.Test
{
    public class HotelTests
    {
        private HotelDbContext _context;     

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseInMemoryDatabase(databaseName: "HotelDatabase")
                .Options;
            _context = new HotelDbContext(options);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        //Test case for checking if hotels added are returned successfully
        [Test]
        public async Task TestIndex_ReturnsHotels()
        {
            // Arrange
            var hotelsController = new HotelsController(_context);
            _context.Hotels.Add(new Hotel { Name = "Hotel1", Address = "Address1", City = "City1", Country = "Country1", PhoneNumber = "123456", Website = "http://www.hotel1.com", Email = "hotel1@example.com" });
            _context.Hotels.Add(new Hotel { Name = "Hotel2", Address = "Address2", City = "City2", Country = "Country2", PhoneNumber = "789012", Website = "http://www.hotel2.com", Email = "hotel2@example.com" });
            await _context.SaveChangesAsync();

            // Act
            var result = await hotelsController.Index() as ViewResult;
            var hotels = result?.Model as List<Hotel>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(hotels);
            Assert.AreEqual(2, hotels.Count);
            Assert.AreEqual("Hotel1", hotels[0].Name);
            Assert.AreEqual("Hotel2", hotels[1].Name);
        }

        //Test case for checking if a hotel added is successfully returned with all details correctly.
        [Test]
        public async Task TestDetails_ReturnsHotel()
        {
            // Arrange
            var hotelsController = new HotelsController(_context);
            var hotel = new Hotel { Name = "Hotel1", Address = "Address1", City = "City1", Country = "Country1", PhoneNumber = "123456", Website = "http://www.hotel1.com", Email = "hotel1@example.com" };
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            // Act
            var result = await hotelsController.Details(hotel.Id) as ViewResult;
            var resultHotel = result?.Model as Hotel;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultHotel);
            Assert.AreEqual(hotel.Name, resultHotel.Name);
            Assert.AreEqual(hotel.Address, resultHotel.Address);
            Assert.AreEqual(hotel.City, resultHotel.City);
            Assert.AreEqual(hotel.Country, resultHotel.Country);
            Assert.AreEqual(hotel.PhoneNumber, resultHotel.PhoneNumber);
            Assert.AreEqual(hotel.Website, resultHotel.Website);
            Assert.AreEqual(hotel.Email, resultHotel.Email);
        }

        //Test case for checking if provided a wrong ID then error is returned
        [Test]
        public async Task TestDetails_ReturnsNotFound()
        {
            // Arrange
            var hotelsController = new HotelsController(_context);

            // Act
            var result = await hotelsController.Details(1) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        //Test case for checking if OnCreate Hotel returns a view.
        [Test]
        public void TestCreate_ReturnsView()
        {
            // Arrange
            var hotelsController = new HotelsController(_context);

            // Act
            var result = hotelsController.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        //Test case for checking if a hotel can be deleted successfully
        [Test]
        public async Task DeleteHotel_ValidInput_Success()
        {
            // Arrange
            var hotelsController = new HotelsController(_context);
            var hotel = new Hotel { Name = "Hotel1", Address = "Address1", City = "City1", Country = "Country1", PhoneNumber = "123456", Website = "http://www.hotel1.com", Email = "hotel1@example.com" };
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            // Act
            var result = await hotelsController.DeleteConfirmed(hotel.Id);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));            
        }

        //Test case for checking wrong PhoneNumber input
        [Test]
        public void Validate_ShouldReturnValidationResultWithErrorMessage_WhenInvalidPhoneNumberIsGiven()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "test hotel",
                Address = "test address",
                City = "test city",
                Country = "test country",
                PhoneNumber = "invalid phone number",
                Website = "https://test.com",
                Email = "test@test.com"            
            };

            // Act
            var validationResults = ValidateModel.IsValidPhoneNumber(hotel.PhoneNumber,new ValidationContext(hotel));

            // Assert           
            Assert.That(validationResults.ErrorMessage, Is.EqualTo("Phone numbers must be numeric but could start with “+”"));
        }

        //Test case for checking if adding Hotel and a room for it return the correct values.
        [Test]
        public void CreateHotelRoom_ValidRoom_ReturnsTrue()
        {
            // Arrange
            var hotel = new Hotel()
            {
                Name = "Test Hotel",
                Address = "Test Address",
                City = "Test City",
                Country = "Test Country",
                PhoneNumber = "1234567890",
                Website = "https://testhotel.com",
                Email = "test@test.com"
            };
            _context.Hotels.Add(hotel);
            _context.SaveChanges();

            var hotelRoom = new HotelRoom()
            {
                HotelId = hotel.Id,
                RoomType = "Test Room Type",
                Capacity = 2,
                PricePerNight = 100,
                Description = "Test Room Description"
            };

            // Act
            _context.HotelRooms.Add(hotelRoom);
            _context.SaveChanges();

            // Assert
            var hotelRooms = _context.HotelRooms.ToList();
            Assert.AreEqual(1, hotelRooms.Count);
            Assert.AreEqual(hotelRoom.HotelId, hotelRooms[0].HotelId);
            Assert.AreEqual(hotelRoom.RoomType, hotelRooms[0].RoomType);
            Assert.AreEqual(hotelRoom.Capacity, hotelRooms[0].Capacity);
            Assert.AreEqual(hotelRoom.PricePerNight, hotelRooms[0].PricePerNight);
            Assert.AreEqual(hotelRoom.Description, hotelRooms[0].Description);
        }

        //Test case for checking when Id is Null
        [Test]
        public async Task Details_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var hotelsController = new HotelsController(_context);
            // Arrange
            int? id = null;

            // Act
            var result = await hotelsController.Details(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        //Test case for checking when Email input is empty
        [Test]
        public void Email_ShouldNotBeEmpty()
        {
            // Arrange
            var hotel = new Hotel
            {                
                Address = "test address",
                City = "test city",
                Country = "test country",
                PhoneNumber = "+4478249320",
                Website = "https://test.com",
                Email = ""
            };
            var context = new ValidationContext(hotel, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(hotel, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.That(3, Is.EqualTo(results.Count));
            Assert.That(results[1].ErrorMessage, Is.EqualTo("The Email field is required."));
        }

        //Test case for checking when Website input is empty
        [Test]
        public void Website_ShouldNotBeEmpty()
        {
            // Arrange
            var hotel = new Hotel
            {
                Address = "test address",
                City = "test city",
                Country = "test country",
                PhoneNumber = "+4478249320",
                Website = "",
                Email = "info@test.com"
            };
            var context = new ValidationContext(hotel, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(hotel, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.That(3, Is.EqualTo(results.Count));
            Assert.That(results[1].ErrorMessage, Is.EqualTo("The Website field is required."));
        }

        //Test case for checking when Country input is empty
        [Test]
        public void Country_ShouldNotBeEmpty()
        {
            // Arrange
            var hotel = new Hotel
            {
                Address = "test address",
                City = "test city",
                Country = "",
                PhoneNumber = "+4478249320",
                Website = "https://test.com",
                Email = "info@test.com"
            };
            var context = new ValidationContext(hotel, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(hotel, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.That(3, Is.EqualTo(results.Count));
            Assert.That(results[1].ErrorMessage, Is.EqualTo("The Country field is required."));
        }

        //Test case for checking when City input is empty
        [Test]
        public void City_ShouldNotBeEmpty()
        {
            // Arrange
            var hotel = new Hotel
            {
                Address = "test address",
                City = "",
                Country = "UK",
                PhoneNumber = "+4478249320",
                Website = "https://test.com",
                Email = "info@test.com"
            };
            var context = new ValidationContext(hotel, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(hotel, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.That(3, Is.EqualTo(results.Count));
            Assert.That(results[1].ErrorMessage, Is.EqualTo("The City field is required."));
        }

        //Test case for checking when Address input is empty
        [Test]
        public void Address_ShouldNotBeEmpty()
        {
            // Arrange
            var hotel = new Hotel
            {
                Address = "",
                City = "test city",
                Country = "UK",
                PhoneNumber = "+4478249320",
                Website = "https://test.com",
                Email = "info@test.com"
            };
            var context = new ValidationContext(hotel, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(hotel, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.That(3, Is.EqualTo(results.Count));
            Assert.That(results[1].ErrorMessage, Is.EqualTo("The Address field is required."));
        }
    }
}