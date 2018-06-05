using ConsoleGetOrder;
using System;
using System.Collections.Generic;

namespace GetOrderConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            GetFromZalo zalo = new GetFromZalo();
            zalo.Init();
            List<OrderAfterGet> list = zalo.GetOrder();
            List<OrderAfterGet> sortList = zalo.SortOrderByDay(list);
            if (sortList.Count == 0)
            {
                Console.Write("Hom nay khong co don hang");
            }
            else
            {
                foreach (var item in sortList)
                {
                    Console.WriteLine(item.price);
                    Console.WriteLine(item.createdTime);
                    Console.WriteLine(item.deliverCity);
                    Console.WriteLine(item.productName);
                    Console.WriteLine(item.productImage);
                    Console.WriteLine(item.deliverCity);
                    Console.WriteLine(item.deliverDistrict);
                    Console.WriteLine(item.numberItem);
                }
            }

            Console.ReadKey();
        }
    }
}