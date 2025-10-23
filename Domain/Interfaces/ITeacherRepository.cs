namespace Domain.Interfaces;

using Entities;

public interface ITeacherRepository
{
    Task Add(Teacher teacher);
}