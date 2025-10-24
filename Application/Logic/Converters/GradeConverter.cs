using Application.Interfaces;

namespace Application.Logic.Converters;

public class GradeConverter : IGradeConverter
{
    public string ConvertToLetter(int numericValue)
    {
        if (numericValue >= 90) return "A";
        if (numericValue >= 82) return "B";
        if (numericValue >= 75) return "C";
        if (numericValue >= 67) return "D";
        if (numericValue >= 60) return "E";
        if (numericValue >= 35) return "Fx";
        return "F";
    }
}