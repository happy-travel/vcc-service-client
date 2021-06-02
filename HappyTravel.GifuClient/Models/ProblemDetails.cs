namespace HappyTravel.GifuClient.Models
{
    internal readonly struct ProblemDetails
    {
        public ProblemDetails(string detail)
        {
            Detail = detail;
        }
        
        
        public string Detail { get; }
    }
}