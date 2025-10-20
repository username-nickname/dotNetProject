namespace Application.DTO.Contracts;

public record ApiResponse<T>(
    bool Success,
    string Message,
    T? Data = default,
    List<ApiError>? Errors = null
);