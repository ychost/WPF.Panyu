using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfHuaWei.Utils
{
    public class FileNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
        {
            string source = (string)value;
            if(string.IsNullOrEmpty(source))
            {
                return new ValidationResult(false, "文件名不能为空!");
            }
            else
            {
                string[] invalidChars = new string[] { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };
                foreach(string invalidChar in invalidChars)
                {
                    if(source.Contains(invalidChar))
                    {
                        return new ValidationResult(false, "文件名不能包含 / : \\ * \" < ? > |");
                    }
                }
                return new ValidationResult(true, null);
            }
        }
    }
}
