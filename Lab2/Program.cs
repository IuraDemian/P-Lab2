using System;
using System.Collections.Generic;
using System.Linq;

class Composlugs
{
    public class Composluga
    {
        public string Surname { get; set; }
        public string Adress { get; set; }
        public DateTime Data { get; set; }
        public string TypeService { get; set; }
        public decimal Sum { get; set; }
    }
    public class Payment
    {
        public string Surname { get; set; }
        public string TypeService { get; set; }
        public decimal PaidSum { get; set; }
        public DateTime PaymentDate { get; set; }
    }

    public static class СomPoslugs
    {

        public static List<Composluga> composlugs = new List<Composluga>
        {
            new Composluga {Surname = "Iван", Adress = "Мукачево, вул. Стефаника 9", Data = new DateTime(2024, 8, 6), TypeService = "Електроенергiя", Sum = 350 },
            new Composluga {Surname = "Петро", Adress = "Київ, вул. Хрещатик 3", Data = new DateTime(2024, 9, 12), TypeService = "Водопостачання", Sum = 120 },
            new Composluga {Surname = "Юрiй", Adress = "Житомир, вул. Схiдна 56", Data = new DateTime(2024, 8, 21), TypeService = "Електроенергiя", Sum = 475 },
            new Composluga {Surname = "София", Adress = "Ужгород, вул. Капушанська 4", Data = new DateTime(2024, 6, 9), TypeService = "Електроенергiя", Sum = 395 },
            new Composluga {Surname = "Маша", Adress = "Тернопiль, вул. Галицька 47", Data = new DateTime(2024, 7, 28), TypeService = "Водопостачання", Sum = 90 },
        };

        public static List<Payment> payments = new List<Payment>
        {
            new Payment { Surname = "Iван", TypeService = "Електроенергiя", PaidSum = 275, PaymentDate = new DateTime(2024, 8, 17) },
            new Payment { Surname = "Петро", TypeService = "Водопостачання", PaidSum = 120, PaymentDate = new DateTime(2024, 9, 21) },
            new Payment { Surname = "Юрiй", TypeService = "Електроенергiя", PaidSum = 455, PaymentDate = new DateTime(2024, 9, 5) },
            new Payment { Surname = "София", TypeService = "Електроенергiя", PaidSum = 395, PaymentDate = new DateTime(2024, 7, 24) },
            new Payment { Surname = "Маша", TypeService = "Водопостачання", PaidSum = 85, PaymentDate = new DateTime(2024, 8, 2) },
        };

        public static void TaskA(string misto, string service, DateTime month)
        {
            var averagePayments = payments
                .Where(o => composlugs.Any(k => k.Surname == o.Surname && k.Adress.Contains(misto) && o.TypeService == service && o.PaymentDate.Month == month.Month && o.PaymentDate.Year == month.Year))
                .Average(o => o.PaidSum);

            Console.WriteLine($"Середнi сплати за послугу '{service}' у {month.ToString("MMMM yyyy")}: {averagePayments}");
        }

        public static void TaskB(DateTime quarter)
        {
            var servicesQuarter = composlugs
                .Where(k => k.Data >= quarter.AddMonths(-2) && k.Data <= quarter)
                .GroupBy(k => k.TypeService)
                .Select(g => new { TypeService = g.Key, TotalSum = g.Sum(k => k.Sum) })
                .OrderByDescending(g => g.TotalSum)
                .FirstOrDefault();

            if (servicesQuarter != null)
            {
                Console.WriteLine($"Послуга з найбiльшою нарахованою сумою за останнiй квартал: {servicesQuarter.TypeService}, сума: {servicesQuarter.TotalSum}");
            }
        }

        public static void TaskC()
        {
            var debtors = composlugs
                .GroupJoin(payments,
                    k => new { k.Surname, k.TypeService },
                    o => new { o.Surname, o.TypeService },
                    (k, o) => new { k.Surname, k.TypeService, accruedSum = k.Sum, PaidSum = o.Sum(x => x.PaidSum) })
                .Select(x => new { x.Surname, Arrears = x.accruedSum - x.PaidSum })
                .Where(x => x.Arrears > 0)
                .OrderByDescending(x => x.Arrears)
                .ToList();

            Console.WriteLine("Список боржникiв:");
            foreach (var debtor in debtors)
            {
                Console.WriteLine($"Прiзвище: {debtor.Surname}, Заборгованiсть: {debtor.Arrears}");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            СomPoslugs.TaskA("Київ", "Водопостачання", DateTime.Now);

            СomPoslugs.TaskB(new DateTime(2024, 9, 25));

            СomPoslugs.TaskC();
        }
    }

}