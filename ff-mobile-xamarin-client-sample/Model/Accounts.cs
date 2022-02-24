using System;
using System.Collections.Generic;

namespace ff_mobile_xamarin_client_sample.Model
{
    public class Accounts
    {

        public static List<String> Names
		{
			get
			{
				return new List<string> {
					"Aptiv",
					"Experian",
					"Fiserv",
					"Harness",
					"Palo Alto Networks"
				};
			}
		}

    }
}
