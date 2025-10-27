using Application.DTO.Auth.External;
using Application.Interfaces.External;
using Domain.Exceptions;
using System.Net.Http.Json;

namespace Infrastructure.Security.External;

public class ExternalAuthApi : IExternalAuthApi
{
    private readonly HttpClient _httpClient;
    
    public ExternalAuthApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<ExternalRegisterResponseDto> Register(ExternalRegisterDto dto)
    {
        try
        {
            // Fake Response
            // await Task.Delay(50);
            // var fakeResponse = new ExternalRegisterResponseDto
            // {
            //     Success = true,
            //     Message = "User registered successfully",
            //     Data = new ExternalUserDataDto
            //     {
            //         ExternalId = Guid.NewGuid().ToString(),
            //         Email = dto.Email,
            //         FullName = dto.FullName,
            //         Role = "Docent"
            //     }
            // };
            // return fakeResponse;

            var response = await _httpClient.PostAsJsonAsync("http://127.0.0.1:8000/api/register", dto);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new ExternalAuthException("Register failed", (int)response.StatusCode);
            }
            
            var data = await response.Content.ReadFromJsonAsync<ExternalRegisterResponseDto>();

            if (data == null)
            {
                throw new ExternalAuthException("Empty response from external API.", 500);
            } 
         
            return data;
        }
        catch (ExternalAuthException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ExternalAuthException($"Unexpected error while registering user., {ex.Message}", 500);
        }
    }

    public async Task<ExternalLoginResponseDto> Login(ExternalLoginDto dto)
    {
        try
        {
            // Fake Response
            // await Task.Delay(50);
            // return new ExternalLoginResponseDto
            // {
            //     Success = true,
            //     Message = "Login successfully",
            //     Data = new ExternalUserDataDto
            //     {
            //         // ExternalId = "d9cbfb1a-e8f5-4c3d-9e05-a0aff956a140",
            //         ExternalId = Guid.NewGuid().ToString(),
            //         Email = dto.Email,
            //         FullName = "FullName",
            //         Role = "Docent"
            //     }
            // };
            
            var response = await _httpClient.PostAsJsonAsync("http://127.0.0.1:8000/api/login", dto);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new ExternalAuthException("Login failed", (int)response.StatusCode);
            }
            
            var data = await response.Content.ReadFromJsonAsync<ExternalLoginResponseDto>();

            if (data == null)
            {
                throw new ExternalAuthException("Empty response from external login API.", 500);
            }

            return data;
            
        }
        catch (ExternalAuthException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new ExternalAuthException("Unexpected error while logging in user.", 500);
        }
    }
}