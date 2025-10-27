﻿namespace Application.DTO.Student;

public class StudentDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public double AverageGrade { get; set; }
}