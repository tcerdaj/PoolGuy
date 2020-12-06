using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace PoolGuy.Mobile.Data.Models.SampleData
{

    public class CustomerListSample
    {
        CustomerSample[] _customers;
        public CustomerListSample()
        {
            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(CustomerListSample)).Assembly;
                var stream = assembly.GetManifestResourceStream("PoolGuy.Mobile.Data.Models.SampleData.Customers.json");
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    var rootobject = JsonConvert.DeserializeObject<CustomerSample[]>(json);
                    
                }

                _customers = JsonConvert.DeserializeObject<CustomerSample[]>(File.ReadAllText(@"Customers.json"));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);        
            }
        }

        public CustomerSample[] Customers { get { return _customers; } }
    }

    public class CustomerSample
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AdditionalInformation { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public int Surface { get; set; }
        public int Capacity { get; set; }
    }
}