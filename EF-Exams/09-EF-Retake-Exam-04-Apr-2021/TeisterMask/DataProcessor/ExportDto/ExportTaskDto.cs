﻿using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto;

[XmlType("Task")]
public class ExportTaskDto
{
	[XmlElement("Name")]
	public string Name { get; set; } = null!;

	[XmlElement("Label")]
	public string Label { get; set; } = null!;
}