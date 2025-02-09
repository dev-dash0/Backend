using DevDash.DTO;
using DevDash.DTO.User;

namespace DevDash.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<TokenDTO> Login(LoginDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterDTO registerationRequestDTO);
        Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO);

        Task RevokeRefreshToken(TokenDTO tokenDTO);
    }
}
