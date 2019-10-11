using System;

namespace SS.GiftShop.Core
{
    public interface IUrlService
    {
        string GetUri(params string[] paths);
        //Uri GetUri(string path);
        string GetSurveyUri(Guid surveyId, Guid employeeId);
    }
}
