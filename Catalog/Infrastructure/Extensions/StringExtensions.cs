namespace Catalog.Infrastructure.Extensions;
public static class StringExtensions
{
    public static string ToKebabCase(this string input)
    {
        //string sanitizedInput = Regex.Replace(input, @"[^a-zA-Z0-9\s-]", "");

        //string kebab = Regex.Replace(sanitizedInput.Trim(), @"\s+", "-");

        //return kebab.ToLower();




        //string pattern = @"[^\u0600-\u06FF0-9\s-]";
        //string result = Regex.Replace(input, pattern, "");

        //// 2. تبدیل فاصله‌ها به خط تیره
        //result = Regex.Replace(result, @"\s+", "-");

        //// 3. حذف خط تیره‌های اضافی
        //result = Regex.Replace(result, @"-+", "-").Trim('-');

        //return result;



        // 1. حذف کاراکترهای غیرمجاز (فقط حروف فارسی، انگلیسی، اعداد، فاصله‌ها و خط تیره‌ها مجاز هستند)
        string pattern = @"[^\u0600-\u06FFa-zA-Z0-9\s-]";
        string result = Regex.Replace(input, pattern, "");

        // 2. تبدیل فاصله‌ها به خط تیره
        result = Regex.Replace(result, @"\s+", "-");

        // 3. حذف خط تیره‌های اضافی
        result = Regex.Replace(result, @"-+", "-").Trim('-');

        return result.ToLower(); // برگرداندن به حروف کوچک (در صورت نیاز)
    }
}
