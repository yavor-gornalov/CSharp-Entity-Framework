using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos;
using TravelAgency.Helpers;

namespace TravelAgency.DataProcessor
{
	public class Deserializer
	{
		private const string ErrorMessage = "Invalid data format!";
		private const string DuplicationDataMessage = "Error! Data duplicated.";
		private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
		private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

		public static string ImportCustomers(TravelAgencyContext context, string xmlString)
		{
			var sb = new StringBuilder();

			var customersInfo = XmlSerializationHelper.Deserialize<ImportCustomerDto[]>(xmlString, "Customers");

			var customers = context.Customers.ToList();

			foreach (var customerDto in customersInfo)
			{
				if (!IsValid(customerDto))
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var isNameUnique = !customers.Any(c => c.FullName == customerDto.FullName);
				var isPhoneUnique = !customers.Any(c => c.PhoneNumber == customerDto.PhoneNumber);
				var isEmailUnique = !customers.Any(c => c.Email == customerDto.Email);

				if (!isNameUnique || !isPhoneUnique || !isEmailUnique)
				{
					sb.AppendLine(DuplicationDataMessage);
					continue;
				}

				var customer = new Customer
				{
					FullName = customerDto.FullName,
					PhoneNumber = customerDto.PhoneNumber,
					Email = customerDto.Email,
				};

				customers.Add(customer);
				sb.AppendLine(string.Format(SuccessfullyImportedCustomer, customer.FullName));
			}

			context.Customers.AddRange(customers);
			context.SaveChanges();

			return sb.ToString().Trim();
		}

		public static string ImportBookings(TravelAgencyContext context, string jsonString)
		{
			var sb = new StringBuilder();

			var bookingsInfo = JsonConvert.DeserializeObject<ImportBookingDto[]>(jsonString);

			var bookings = new List<Booking>();

			foreach (var bookingDto in bookingsInfo)
			{
				var isBookingDateValid = DateTime.TryParseExact(
					bookingDto.BookingDate,
					"yyyy-MM-dd",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None,
					out DateTime bookingDate);

				if (!IsValid(bookingDto) || !isBookingDateValid)
				{
					sb.AppendLine(ErrorMessage);
					continue;
				}

				var booking = new Booking
				{
					BookingDate = bookingDate,
					Customer = context.Customers.First(c => c.FullName == bookingDto.CustomerName),
					TourPackage = context.TourPackages.First(tp => tp.PackageName == bookingDto.TourPackageName)
				};

				bookings.Add(booking);
				sb.AppendLine(string.Format(SuccessfullyImportedBooking, booking.TourPackage.PackageName, bookingDto.BookingDate));
			}

			context.Bookings.AddRange(bookings);
			context.SaveChanges();

			return sb.ToString().Trim();

		}

		public static bool IsValid(object dto)
		{
			var validateContext = new ValidationContext(dto);
			var validationResults = new List<ValidationResult>();

			bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

			foreach (var validationResult in validationResults)
			{
				string currValidationMessage = validationResult.ErrorMessage;
			}

			return isValid;
		}
	}
}
