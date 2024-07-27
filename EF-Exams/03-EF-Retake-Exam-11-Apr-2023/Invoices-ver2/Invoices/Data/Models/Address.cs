using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Invoices.Common.GlobalConstants;
using static System.Net.Mime.MediaTypeNames;

namespace Invoices.Data.Models;

public class Address
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(AddressStreetNameMaxLength)]
	public string StreetName { get; set; } = null!;

	[Required]
	public int StreetNumber { get; set; }

	[Required]
	public string PostCode { get; set; } = null!;

	[Required]
	[MaxLength(AddressCityMaxLength)]
	public string City { get; set; } = null!;

	[Required]
	[MaxLength(AddressCountryMaxLength)]
	public string Country { get; set; } = null!;

	[Required]
	public int ClientId { get; set; }

	[ForeignKey(nameof(ClientId))]
	public Client Client { get; set; } = null!;
}

//•	Id – integer, Primary Key
//•	StreetName – text with length[10…20] (required)
//•	StreetNumber – integer(required)
//•	PostCode – text(required)
//•	City – text with length[5…15] (required)
//•	Country – text with length[5…15] (required)
//•	ClientId – integer, foreign key(required)
//•	Client – Client
