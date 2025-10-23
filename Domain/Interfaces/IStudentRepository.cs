namespace Domain.Interfaces;

using Entities;

public interface IStudentRepository
{
    Task Add(Student student);
}