namespace KITAB.Products.CrossCutting.Utils
{
    public class Numeric
    {
        public static string OnlyNumbers(string value)
        {
            string onlyNumber = "";

            foreach (char s in value)
            {
                if (char.IsDigit(s)) onlyNumber += s;
            }

            return onlyNumber.Trim();
        }
    }
}
