using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace new_project
{
    

    // PhoneStore interface
    interface IPhoneStore
    {
        void AddPhone(Phone phone);
        void SellPhone(int phoneId, Customer customer, Payment payment);
        void DisplayInventory();
        void GenerateSalesReport();
    }

    // Phone class
    class Phone
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    // Customer class
    class Customer
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }

    // SalesRecord class
    class SalesRecord
    {
        public Phone Phone { get; set; }
        public Customer Customer { get; set; }
        public DateTime SaleDate { get; set; }
    }

    // Payment class
    class Payment
    {
        public decimal AmountPaid { get; set; }
    }

    // FileManager class
    class FileManager
    {

        private const string InventoryFilePath = "inventory.txt";
        private const string SalesFilePath = "sales.txt";

        public void SaveInventory(List<Phone> phones)
        {
            using (StreamWriter writer = new StreamWriter(InventoryFilePath))
            {
                foreach (Phone phone in phones)
                {
                    writer.WriteLine($"{phone.Id},{phone.Brand},{phone.Model},{phone.Price},{phone.Quantity}");
                }
            }
        }
        
        public List<Phone> LoadInventory()             
        {
            List<Phone> phones = new List<Phone>();         

            if (File.Exists(InventoryFilePath))  
            {
                using (StreamReader reader = new StreamReader(InventoryFilePath)) 
                {


                    string line; 

                    while ((line = reader.ReadLine()) != null)
                    {

                        string[] data = line.Split(',');  
                        
                        Phone phone = new Phone 
                        {


                            Id = int.Parse(data[0]),
                            Brand = data[1],
                            Model = data[2],
                            Price = decimal.Parse(data[3]),
                            Quantity = int.Parse(data[4])
                        };


                        phones.Add(phone);         
                    }
                }
            }

            return phones;  
        }

        public void SaveSales(List<SalesRecord> salesRecords)
        {

            using (StreamWriter writer = new StreamWriter(SalesFilePath))
            {

                foreach (SalesRecord salesRecord in salesRecords)
                {

                    writer.WriteLine($"{salesRecord.Phone.Id},{salesRecord.Customer.Name},{salesRecord.Customer.PhoneNumber},{salesRecord.SaleDate},{salesRecord.Phone.Price}");
                }

            }
        }

        public List<SalesRecord> LoadSales()
        {
            List<SalesRecord> salesRecords = new List<SalesRecord>();

            if (File.Exists(SalesFilePath))
            {
                using (StreamReader reader = new StreamReader(SalesFilePath))
                {
                    string line;   

                    while ((line = reader.ReadLine()) != null)
                    {

                        string[] data = line.Split(',');
                        SalesRecord salesRecord = new SalesRecord
                        {
                            Phone = new Phone { Id = int.Parse(data[0]), Price = decimal.Parse(data[4]) },
                            Customer = new Customer { Name = data[1], PhoneNumber = data[2] },
                            SaleDate = DateTime.Parse(data[3])
                        };
                        salesRecords.Add(salesRecord);
                    }
                }
            }

            return salesRecords;
        }
    }

    // PhoneStore class
    class PhoneStore : IPhoneStore  //interface 
    {
        private List<Phone> inventory;
        
        private List<SalesRecord> salesRecords;
     
        private FileManager fileManager;
        
        public PhoneStore()
        
        {
            fileManager = new FileManager();           
            inventory = fileManager.LoadInventory();       
            salesRecords = fileManager.LoadSales();                       

        }

        public void AddPhone(Phone phone) 
        {
            inventory.Add(phone);
            fileManager.SaveInventory(inventory);
        }

        public void SellPhone(int phoneId, Customer customer, Payment payment)
         
        {
            Phone phone = inventory.Find(p => p.Id == phoneId);

            if (phone != null && phone.Quantity > 0)
            {
                phone.Quantity--;
                SalesRecord salesRecord = new SalesRecord
                {
                    Phone = phone,
                    Customer = customer,
                    SaleDate = DateTime.Now
                };
                salesRecords.Add(salesRecord);
                fileManager.SaveInventory(inventory);
                fileManager.SaveSales(salesRecords);
                Console.WriteLine("                    ---------------------------------------------------------------------------       ");
                Console.WriteLine( $"                    Phone {phone.Brand} {phone.Model} sold to {customer.Name}. Payment amount: {payment.AmountPaid}");
                Console.WriteLine("                    ---------------------------------------------------------------------------       ");

            }
            else
            {
                Console.WriteLine("     ------------------------------------------------------------------       ");
                Console.WriteLine("              Phone not available in inventory or out of stock.    ");
                Console.WriteLine("      ------------------------------------------------------------------       ");

            }
        }

        public void DisplayInventory()
        {



            Console.WriteLine("\n\n\t\t----------Inventory:--------\n");
            foreach (Phone phone in inventory)
            {


                Console.WriteLine($"\t{phone.Id}: {phone.Brand} {phone.Model} \t- Price: {phone.Price} \t- Quantity: {phone.Quantity}");
            
            }
        }

        public void GenerateSalesReport()
        {


            Console.WriteLine("\t\t----------------Sales Report:------------");
            foreach (SalesRecord salesRecord in salesRecords)
            {
                Console.WriteLine($"\tPhone: {salesRecord.Phone.Brand} {salesRecord.Phone.Model} \t- Customer: {salesRecord.Customer.Name} \t- Sale Date: {salesRecord.SaleDate}");
            }
        }
    }

    // Main program
    class Program
    {

        static void Main(string[] args)
        {

            PhoneStore phoneStore = new PhoneStore();
            Console.WriteLine("\n\t\t\t--------------------------------");
            Console.WriteLine("\t\t\t-  Welcome to the Phone Store  -");
            Console.WriteLine("\t\t\t--------------------------------");


            while (true)
            {




                Console.WriteLine("\t1. Enter 1 to Add Phone");
                Console.WriteLine("\t2. Enter 2 to Sell Phone ");
                Console.WriteLine("\t3. Enter 3 to Display Inventory");
                Console.WriteLine("\t4. Enter 4 to Generate Sales Report");
                Console.WriteLine("\t5. Enter 5 to Exit");

                Console.WriteLine("\n\n");


                Console.Write("\tEnter your choice: ");


                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {






                    case 1:
                        Console.WriteLine("\t\t Add Phone:   ");
                        Console.Write("\t\tEnter phone brand: ");
                        string brand = Console.ReadLine();

                        Console.Write("\t\tEnter phone model: ");
                        string model = Console.ReadLine();

                        Console.Write("\t\tEnter phone price: ");
                        decimal price = decimal.Parse(Console.ReadLine());

                        Console.Write("\t\tEnter phone quantity: ");
                        int quantity = int.Parse(Console.ReadLine());


                        Phone phone = new Phone
                        {
                            Brand = brand,
                            Model = model,
                            Price = price,
                            Quantity = quantity
                        };

                        phoneStore.AddPhone(phone);
                        Console.WriteLine("Phone added to inventory.");
                        break;






                    case 2:
                        Console.Write("\t\tEnter phone ID: ");
                        int phoneId = int.Parse(Console.ReadLine());
                        Console.Write("\t\tEnter customer name: ");
                        string customerName = Console.ReadLine();
                        Console.Write("\t\tEnter customer phone number: ");
                        string customerPhoneNumber = Console.ReadLine();
                        Console.Write("\t\tEnter payment amount: ");
                        decimal paymentAmount = decimal.Parse(Console.ReadLine());

                        Customer customer = new Customer
                        {
                            Name = customerName,
                            PhoneNumber = customerPhoneNumber
                        };






                        Payment payment = new Payment
                        {
                            AmountPaid = paymentAmount
                        };

                        phoneStore.SellPhone(phoneId, customer, payment);
                        break;
                    case 3:
                        phoneStore.DisplayInventory();
                        break;
                    case 4:
                        phoneStore.GenerateSalesReport();
                        break;
                    case 5:
                        Console.WriteLine("\t\tExiting program...");
                        return;
                    default:
                        Console.WriteLine("\t\tInvalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }

}
