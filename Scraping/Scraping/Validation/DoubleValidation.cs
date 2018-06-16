using System.ComponentModel.DataAnnotations;

namespace Scraping.Validation
{
    /// <summary>
    /// 入力がDoubleかどうか判定します
    /// </summary>
    class DoubleValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return double.TryParse(value.ToString(), out double d);
        }
    }
}
