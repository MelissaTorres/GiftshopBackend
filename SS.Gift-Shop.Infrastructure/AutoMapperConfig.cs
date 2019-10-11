using System;
using AutoMapper;

namespace SS.GiftShop.Infrastructure
{
    public static class AutoMapperConfig
    {
        public static void Configure(IMapperConfigurationExpression cfg)
        {
            if (cfg is null)
            {
                throw new ArgumentNullException(nameof(cfg));
            }

            cfg.ForAllMaps(AutoIgnorePropertiesInternal);
        }

        private static void AutoIgnorePropertiesInternal(TypeMap map, IMappingExpression expression)
        {
            //if (typeof(IHaveDateCreated).IsAssignableFrom(map.DestinationType))
            //{
            //    expression.ForMember(nameof(IHaveDateCreated.DateCreated), e => e.Ignore());
            //}

            //if (typeof(IHaveDateUpdated).IsAssignableFrom(map.DestinationType))
            //{
            //    expression.ForMember(nameof(IHaveDateUpdated.DateUpdated), e => e.Ignore());
            //}
        }
    }
}
