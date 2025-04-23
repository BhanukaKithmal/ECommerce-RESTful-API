namespace ECommerceRESTfulAPI.Helpers
{
    public static class Utility
    {
        public static string NormalizeWhitespace(string input) =>
        string.Join(" ", input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }
}
