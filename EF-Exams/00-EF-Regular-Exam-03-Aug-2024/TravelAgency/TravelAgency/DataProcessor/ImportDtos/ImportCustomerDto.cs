using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static TravelAgency.Data.Validations.GlobalConstants;

namespace TravelAgency.DataProcessor.ImportDtos;

[XmlType("Customer")]
public class ImportCustomerDto
{
	[XmlAttribute("phoneNumber")]
	[Required]
	[RegularExpression(CustomerPhoneNumberRegex)]
	public string PhoneNumber { get; set; } = null!;

	[XmlElement("FullName")]
	[Required]
	[StringLength(CustomerFullNameMaxLength, MinimumLength = CustomerFullNameMinLength)]
	public string FullName { get; set; } = null!;

	[XmlElement("Email")]
	[Required]
	[StringLength(CustomerEmailMaxLength, MinimumLength = CustomerEmailMinLength)]
	[EmailAddress]
	public string Email { get; set; } = null!;
}
