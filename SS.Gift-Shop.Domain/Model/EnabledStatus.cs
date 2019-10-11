namespace SS.GiftShop.Domain.Model
{
    public enum EnabledStatus
    {
        Enabled,
        Disabled,
        Deleted
    }

    public static class EnabledStatusExtensions
    {
        public static bool ToBoolean(this EnabledStatus status)
        {
            return status == EnabledStatus.Enabled;
        }

        public static EnabledStatus ToEnabledStatus(this bool enabled)
        {
            return enabled ? EnabledStatus.Enabled : EnabledStatus.Disabled;
        }
    }
}
