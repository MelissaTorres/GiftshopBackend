namespace SS.GiftShop.Api
{
    public static class AuthConstants
    {
        public const string CookieName = ".GiftShop.Identity";
        public const string DefaultPolicy = "default";
        public const string BearerScheme = "bearer";
        public const string BearerPolicy = "bearer";

        public const string DefaultCorsPolicy = "default";

        public const int MinPasswordLength = 8;
        public const int MaxPasswordLength = 100;
    }
}
