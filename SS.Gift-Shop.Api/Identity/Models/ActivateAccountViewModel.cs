using System;
using System.ComponentModel.DataAnnotations;

namespace SS.GiftShop.Api.Identity.Models
{
    public class ActivateAccountViewModel
    {
        public Guid UserId { get; set; }

        [DataType(DataType.Password)]
        //[PasswordComplexityRegularExpression]
        public string Password { get; set; }

        public string Token { get; set; }
    }
}
