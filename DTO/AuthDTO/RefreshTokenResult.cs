namespace CS_APIServerProject.DTO
{
    public class RefreshTokenResult
    {
        public string Token { get; set; }
        public DateTime ExpireAtUtc { get; set; }
    }
}
