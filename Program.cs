using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;


namespace Varuautomat
{
    /*----------------------
         Product Class
     ----------------------*/
    public class Product
    {
        // Constructor create for deserialization
        public Product()
        {
        }
        // Return product name
        public string name { get; set; }
        public int price { get; set; }

    }

    /*----------------------
         ProductList Class
     ----------------------*/
    class ProductList : Product
    {
        // class fildes
        private string filePath;
        private List<Product> proList;

        // Constructor
        public ProductList(string fileName)
        {
            filePath = fileName;
            proList = new List<Product>();
        }

        // Set product from file to list
        public bool SetProList()
        {
            string s = "";
            if (!File.Exists(filePath))
            {
                return false;
            }
            else
            {
                // Read the file
                string jsonString = File.ReadAllText(filePath);

                // set the file element in the list
                proList = JsonConvert.DeserializeObject<List<Product>>(jsonString);

                return true;
            }
        }

        // Get product list
        public List<Product> GetProList()
        {
            return proList;
        }

        // Set new product/object in the List & file
        public void SetNewPro(string name, int price)
        {

            proList.Add(new Product
            {
                name = name,
                price = price
            });
            // update file
            File.WriteAllText(filePath, JsonConvert.SerializeObject(proList, Formatting.Indented));

        }


        // Set new product/object in the List & file
        public void UpdatePrice(int indexToUpdate, int pticeToUpdate)
        {

            proList[indexToUpdate].price = pticeToUpdate;
            // update file
            File.WriteAllText(filePath, JsonConvert.SerializeObject(proList, Formatting.Indented));

        }
        // find an element by index and delete it
        public bool DeletePro(int id)
        {
            int before = proList.Count();

            // controll idand list length
            if (id <= before)
            {
                proList.RemoveAt(id);
                int after = proList.Count;

                // controllera listan
                if (before > after && id <= proList.Count())
                {
                    // Update the file
                    File.WriteAllText(filePath, JsonConvert.SerializeObject(proList, Formatting.Indented));
                    return true;
                }
            }
            return false;
        }
    }


    /*----------------------
            Cart Class
    ----------------------*/
    class Cart : ProductList
    {
        // array to set number of product
        public int[] cartArr;

        // constructor
        public Cart(string fileName) : base(fileName)
        {
            base.SetProList();

            // cart lenght as long as product lenght
            cartArr = new int[base.GetProList().Count];

            // set default value to zero
            for (int i = 0; i < cartArr.Length; i++)
            {
                cartArr[i] = 0;
            }
        }

        // add to cart
        public bool addToCart(int proIndex)
        {
            // increase nummber of this product
            if (proIndex <= cartArr.Length)
            {
                try
                {
                    cartArr[proIndex]++;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
            return false;
        }

        // get the cart array
        public int[] getCart()
        {
            return cartArr;
        }

        // return number of products in cart times price of product
        public int getSum()
        {
            int sum = 0;
            int temp = 0;

            foreach (Product p in base.GetProList())
            {
                sum += cartArr[temp] * p.price;
                temp++;
            }
            return sum;
        }


        // reduce value of product in cart
        public bool deleteFromCart(int indexToDelete)
        {
            try
            {
                if (cartArr[indexToDelete] > 0)
                {
                    cartArr[indexToDelete]--;
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;

        }
    }

    /*----------------------
            Program Class
    ----------------------*/
    internal class Program
    {
        /*----------------------
            Static methods
        ----------------------*/
        // exit fromprogram
        public static void Exit()
        {
            Console.WriteLine("\tVälkommen åter!");
            System.Environment.Exit(0);
        }
        public static void Line()
        {
            Console.WriteLine("\n+-------------------------------+\n");
        }

        // print error message
        public static void Error()
        {
            Console.Clear();
            Console.WriteLine("\n\t\t>> FEL IMMATNING! <<\n Du måste välja från ett alternativ från meny<<");
        }
        // print proList
        public static void PrintProList(List<Product> proList)
        {
            int menuNr = 1;
            Line();
            foreach (Product p in proList)
            {

                Console.WriteLine("\t{0}- {1}: {2} kr/st", menuNr, p.name, p.price);
                menuNr++;
            }
            Line();
        }

        // print first menu
        public static void MainMenu()
        {
            Line();
            Console.WriteLine("\t->>VÄLJA FRÅN MENY!<<-");
            Console.WriteLine("\n\t1- Jag är personal:" +
                "\n\t2- Jag är kund:\n\t3- Avsluta: ");
            Line();

        }

        // print admins menu
        public static void AdminMenu()
        {
            Line();
            Console.WriteLine("\t->>VÄLJA FRÅN MENY!<<-");
            Console.WriteLine("\t1- Lägg till ny vara:" +
                "\n\t2- Radera en vara" +
                "\n\t3- Uppdatera pris" +
                "\n\t4- Logga ut: ");
            Line();

        }

        // print customers menu
        public static void CustomerMenu(int sum)
        {
            Line();
            Console.WriteLine("\t->>VÄLJA FRÅN MENY!<<-");
            Console.WriteLine("\t\n\t1- Köp en vara: " +
                "\n\t2- Radera en vara:" +
                "\n\t3- Kvitto:" +
                "\n\t4- Avsluta: " +
                 "\n\n   SUMMA: {0}:-", sum);
            Line();
        }

        /* controll submenu input datatype and value through menu lenght
         return -1 if its not valid number*/
        public static int SubMenuIntCheck(int currentNr)
        {
            int userInt = -1;

            try
            {
                userInt = Convert.ToInt32(Console.ReadLine());
                if (userInt > 0 && userInt <= currentNr)
                {
                    return userInt;
                }
            }
            catch (Exception)
            {
                return userInt = -1;
            }
            return -1;

        }

        /* Check user input if it is från the menu return it, else repeat it 
          For main- admin and customer menu print submenu
        Every time menunr is  3/CUSTOMER show sum as well*/
        public static int CheckInput(int menuNr, int sum)
        {
            int userInt = 0;
            bool getInput = true;
            int menuLenght = 0;

            // depend av menuNr print submenu
            while (getInput)
            {
                if (menuNr == 1)
                {
                    MainMenu();
                    menuLenght = 3;
                }
                else if (menuNr == 2)
                {
                    AdminMenu();
                    menuLenght = 5;
                }
                else if (menuNr == 3)
                {
                    CustomerMenu(sum);
                    menuLenght = 4;
                }

                // check dataype and value of input
                try
                {
                    userInt = Convert.ToInt32(Console.ReadLine());
                    if (userInt > 0 && userInt <= menuLenght)
                    {
                        getInput = false;
                        return userInt;
                    }
                    else
                    {
                        Error();
                    }
                }
                catch (Exception)
                {
                    Error();
                }
            }
            return userInt;
        }

        // get customer cart
        public static void getTheCart(int[] cartArr, List<Product> proList, int sum)
        {
            DateTime now = DateTime.Now;
            int temp = 0;

            if (sum > 1)     // controll cart if it is not empty
            {
                Console.Clear();
                Line();
                Console.WriteLine("\t->> KVITTO <<-\n");
                string createDate = now.ToString(" yyyy-MM-dd HH: mm");
                Console.WriteLine(createDate);
                Line();
                Console.WriteLine(" VARA\t\tANTAL\tSUMMA");

                foreach (Product product in proList)                // en combination av två lists
                {
                    if (cartArr[temp] > 0)
                    {
                        Console.WriteLine(" {0}\t\t{1}\t{2}", product.name, cartArr[temp], (cartArr[temp] * product.price));
                        temp++;
                    }
                }
                // total sum
                Console.WriteLine("\nTOTAL SUMMA:{0}:-", sum);
                Line();
            }
            else  // if cart is empty
            {
                Console.WriteLine("\n\t->>Korg är tmomt!<<-");
            }

        }
        /*----------------------
            MAIN METHOD
        ----------------------*/
        static void Main(string[] args)
        {
            // Create ariables
            List<Product> proList = new List<Product>();
            int[] cartArr;
            int mainMenuInput = 0;
            int adminInput = 0;
            int sum = 0;
            int costumerInput = 0;
            const string Pass = "12345";
            string userPass = "";
            bool getPass =true;

            // file path debug
            string dir = Directory.GetCurrentDirectory();

            // file path current folder
            string path
                = Directory.GetParent(dir).Parent.FullName.ToString();
            path = path + @"\list.json";

            // create objects
            ProductList listObj = new ProductList(path);
            Cart cart = new Cart(path);

            // declare caet array
            cartArr = cart.getCart();
            // set List från JSON file
            if (listObj.SetProList())
            {
                proList = listObj.GetProList();

            }

            /*----------------------
                    MAIN MENU
            ----------------------*/
            while (mainMenuInput != 3)
            {
                adminInput = -1;
                mainMenuInput = CheckInput(1, sum);

                // first menu 1-Amin/2-Customer, loop untill quit 
                while (mainMenuInput != 3 && adminInput != 4)
                {
                   
                    Console.Clear();
                    /*----------------------
                        Admin Menu
                    ----------------------*/
                    if (mainMenuInput == 1)
                    {

                        // check password
                        while (getPass)
                        {
                            Console.WriteLine("\n\tAnge ditt lösenord!:");
                            userPass = Console.ReadLine();
                            if (userPass.Equals(Pass))
                            {
                                getPass = false;
                                Console.Clear();
                                Console.WriteLine("\n\t>>Du är inloggad!<<");
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("\n\t>>Fel lösenord! Försök igen!<<");
                            }
                        }
      

                        adminInput = CheckInput(2, sum);
                        switch (adminInput)
                        {
                            // add new product
                            case 1:
                                bool getSt = true;
                                bool getInt = true;
                                string newName = "";
                                int newPrice = 0;

                                // check value and declare namne
                                while (getSt)
                                {
                                    Console.WriteLine("\tSkriv in varans namn:");
                                    newName = Console.ReadLine();
                                    if (newName.Length < 2)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\t>> Varans namn måste bestå av minst två karaktär <<\n");
                                    }
                                    else
                                    {
                                        getSt = false;
                                    }

                                }

                                // check datatype/value and declare price
                                while (getInt)
                                {
                                    Console.WriteLine("\tSkriv in varans pris:");
                                    newPrice = SubMenuIntCheck(1000);
                                    if (newPrice == -1)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n\t>>Varans namn måste bestå vara ett tal mindre mellan 1-1000!<<\n");
                                    }
                                    else
                                    {
                                        getInt = false;
                                    }
                                }
                                // add to the file 
                                listObj.SetNewPro(newName, newPrice);
                                Console.Clear();
                                Console.WriteLine("\n\t>> Varan har registrieras <<");
                                break;

                            // delete en product
                            case 2:
                                int indexToDelete = -1;
                                // update the list and print it
                                proList = listObj.GetProList();
                                Console.Clear();
                                PrintProList(proList);

                                getInt = true;
                                // check datatype/value and delete it
                                while (getInt)
                                {
                                    Console.WriteLine("\tSkriv in nummer av varan som ska raderas:");
                                    indexToDelete = SubMenuIntCheck(proList.Count);
                                    if (indexToDelete == -1)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n\t>> Välja en vara från listan! <<\n");
                                    }
                                    else
                                    {
                                        listObj.DeletePro(indexToDelete - 1);
                                        Console.Clear();
                                        Console.WriteLine("\n\t>> Varan har raderas <<");
                                        getInt = false;
                                    }
                                }
                                break;

                            // update price      
                            case 3:
                                int indexToUpdate = -1;
                                int updatePrice = -1;
                                bool getNewPeice = true;
                                // update the list and print it
                                proList = listObj.GetProList();
                                Console.Clear();
                                PrintProList(proList);

                                getInt = true;
                                // check datatype/value and delete it
                                while (getInt)
                                {
                                    Console.WriteLine("\tSkriv in nummer av varan som ska updateras:");
                                    indexToUpdate = SubMenuIntCheck(proList.Count);
                                    if (indexToUpdate == -1)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n\t>> Välja en vara från listan! <<\n");
                                    }
                                    else
                                    {
                                        // break first loop
                                        getInt = false;

                                        // repeat untill price foramt is ok
                                        while (getNewPeice)
                                        {
                                            Console.WriteLine("\tSkriv in nytt pris:");
                                            updatePrice = SubMenuIntCheck(1000);
                                            Console.WriteLine("--P", updatePrice, "--In", indexToUpdate - 1);
                                            if (updatePrice == -1)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("\t>> Fel format! <<\n");
                                            }
                                            else
                                            {
                                                // update list and file
                                                listObj.UpdatePrice(indexToUpdate - 1, updatePrice);
                                                Console.Clear();
                                                Console.WriteLine("\n\tVaran har updateras!");
                                                getNewPeice = false;
                                            }

                                        }

                                    }
                                }
                                break;
                            case 4:
                                Console.Clear();
                                Console.WriteLine("\n\t>> Du är utloggad <<");
                                getPass = true;
                                break;
                        }
                    }

                    /*----------------------
                        Customer Menu
                    ----------------------*/
                    if (mainMenuInput == 2)
                    {
                        Console.Clear();
                        costumerInput = CheckInput(3, sum);
                    }

                    switch (costumerInput)
                    {
                        // add new product
                        case 1:
                            Console.Clear();
                            bool getInt = true;

                            // check coustomer input to buy
                            while (getInt)
                            {
                                // print the productslist
                                PrintProList(proList);
                                Console.WriteLine("Skriv in varans nummer:");
                                costumerInput = SubMenuIntCheck(proList.Count) - 1;
                                if (costumerInput == -1)
                                {
                                    Console.Clear();
                                    Error();
                                }
                                else
                                {
                                    // if its done update the cart and sum
                                    if (cart.addToCart(costumerInput))
                                    {
                                        Console.Clear();
                                        getInt = false;
                                        Console.WriteLine("\n\t>> Ditt köp är registrierad! <<");

                                        sum = cart.getSum();
                                        cartArr = cart.getCart();
                                        Console.WriteLine("\n\t>> För menyn tryck på enter! <<");
                                        Console.ReadLine();
                                    }
                                    else
                                    {
                                        Error();
                                    }
                                }
                            }
                            break;

                        // delete en product
                        case 2:
                            int indexToDelete = 0;
                            costumerInput = -1;
                            getInt = true;
                            Console.Clear();
                            // check coustomer input to delete
                            while (getInt)
                            {
                                // print the productslist

                                PrintProList(proList);
                                Console.WriteLine("Skriv in varans nummer:");
                                indexToDelete = SubMenuIntCheck(proList.Count) - 1;
                                if (indexToDelete == -1)
                                {
                                    Error();
                                }
                                else
                                {
                                    // if its done update the cart and sum
                                    if (cart.deleteFromCart(indexToDelete))
                                    {
                                        getInt = false;
                                        Console.WriteLine("\n\t>> Varan har raderas från korg! <<");
                                        sum = cart.getSum();
                                        cartArr = cart.getCart();
                                        Console.WriteLine("\n>> För att ser menyn tryck på enter! <<");
                                        Console.ReadLine();
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n\tFel inmatning! Varan finns inte i korgen! <<");
                                    }
                                }
                            }
                            break;

                        // get the cart    
                        case 3:
                            sum = cart.getSum();
                            getTheCart(cartArr, proList, sum);
                            Console.WriteLine("\n>> För menyn tryck på enter! <<");
                            Console.ReadLine();

                            break;
                        case 4:
                            Console.Clear();
                            Exit();
                            break;
                    }
                }
                /*----------------------
                        Exit
                ----------------------*/
                if (mainMenuInput == 3)
                {
                    Console.WriteLine("\n\t>> Tack för ditt besök! <<");
                    Exit();
                }
            }
        }
    }
}
