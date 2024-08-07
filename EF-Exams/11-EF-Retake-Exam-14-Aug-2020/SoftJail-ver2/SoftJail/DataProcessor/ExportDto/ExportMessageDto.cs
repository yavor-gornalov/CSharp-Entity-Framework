﻿using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto;

[XmlType("Message")]
public class ExportMessageDto
{
	[XmlElement("Description")]
	public string Description { get; set; } = null!;
}