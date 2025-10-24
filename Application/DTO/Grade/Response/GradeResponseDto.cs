namespace Application.DTO.Grade.Response;

public class GradeResponseDto
{
    public int NumericValue { get; set; }
    public string LetterValue { get; set; }
    public DateTime Date { get; set; }

    public GradeResponseDto(int numericValue, string letterValue, DateTime date)
    {
        NumericValue = numericValue;
        LetterValue = letterValue;
        Date = date;
    }
}