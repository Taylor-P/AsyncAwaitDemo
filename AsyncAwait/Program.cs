using System;
using System.Threading.Tasks;
using AsyncAwaitDemo.Models;

namespace AsyncAwaitDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {

                Console.WriteLine("Run Breakfast asynchronously? Y/N"); // Prompt
                var line = Console.ReadLine() ?? "y"; // Get string from user

                DateTime start;

                if (line.ToLower() == "n") // Check string
                {
                    Console.WriteLine("Sync Cooking");
                    start = DateTime.UtcNow;
                    HotWater hotwater = BoilKettle();
                    Egg eggs = FryEgg(2);
                    Bacon bacon = FryBacon(3);
                    Toast toast = MakeToast(3);
                    ButterToast(toast);
                    Tea tea = MakeTea(hotwater);
                    Console.WriteLine("Breakfast is ready!");
                    TimeSpan difference = DateTime.UtcNow.Subtract(start);
                    Console.WriteLine($"Time taken {difference.TotalSeconds} (seconds)");
                }
                else
                {
                    Console.WriteLine("Async Cooking");
                    start = DateTime.UtcNow;
                    Task<HotWater> hotwaterAsync = BoilKettleAsync();
                    Task<Egg> eggsaAsync = FryEggAsync(2);
                    Task<Bacon> baconAsync = FryBaconAsync(3);
                    Task<Toast> toastAsync = MakeToastAsync(3);
                    toastAsync = ToastWithButter(toastAsync);
                    Task<Tea> teaAsync = TeaWithHotWater(hotwaterAsync);
                    await Task.WhenAll(hotwaterAsync, eggsaAsync, baconAsync, toastAsync, teaAsync);
                    Console.WriteLine("Breakfast is ready!");
                    TimeSpan difference = DateTime.UtcNow.Subtract(start);
                    Console.WriteLine($"Time taken {difference.TotalSeconds} (seconds)");
                }
            }
        }

        #region methods

        private static async Task<Tea> TeaWithHotWater(Task<HotWater> hotwater)
        {
            var hotwaterReady = await hotwater;
            return MakeTea(hotwaterReady);
            
        }

        private static async Task<Toast> ToastWithButter(Task<Toast> breadToasted)
        {
            var toast = await breadToasted;
            ButterToast(toast);
            return toast;
        }

        private static void ButterToast(Toast toast)
        {
            Console.WriteLine($"Buttering Toast");
            Console.WriteLine("Toast is ready");
        }

        private static Toast MakeToast(int slices)
        {
            Console.WriteLine($"Toasting {slices} slices of Toast in toaster");
            Task.Delay(20000).Wait();
            Console.WriteLine("Adding Toast to plate");
            return  new Toast();
        }

        private static async Task<Toast> MakeToastAsync(int slices)
        {
            Console.WriteLine($"Toasting {slices} slices of Toast in toaster");
            await Task.Delay(20000);
            Console.WriteLine("Adding Toast to plate");
            return new Toast();
        }

        private static Tea MakeTea(HotWater hotwater)
        {
            Console.WriteLine("Place Tea bag in Cup");
            Task.Delay(500).Wait();
            Console.WriteLine("Pour Water into Cup");
            Task.Delay(500).Wait();
            Console.WriteLine("Take out tea bag");
            Task.Delay(500).Wait();
            Console.WriteLine("add milk to brew");
            Console.WriteLine("Tea is ready");
            return new Tea();
        }

        private static HotWater BoilKettle()
        {
            Console.WriteLine("Fill Kettle With Water");
            Task.Delay(3000).Wait();
            Console.WriteLine("Boil Kettle");
            Task.Delay(40000).Wait();
            Console.WriteLine("Hot Water ready");
            return new HotWater();
        }

        private static async Task<HotWater> BoilKettleAsync()
        {
            Console.WriteLine("Fill Kettle With Water");
            Task.Delay(3000).Wait();
            Console.WriteLine("Boil Kettle");
            await Task.Delay(40000);
            Console.WriteLine("Hot Water ready");
            return new HotWater();
        }

        private static Egg FryEgg(int eggs)
        {
            Console.WriteLine($"cracking {eggs} eggs in the pan");
            Console.WriteLine("cooking the eggs");
            Task.Delay(45000).Wait();
            Console.WriteLine("Adding eggs to plate");
            Console.WriteLine("Eggs are ready");
            return new Egg();
        }

        private static async Task<Egg> FryEggAsync(int eggs)
        {
            Console.WriteLine($"cracking {eggs} eggs in the pan");
            Console.WriteLine("cooking the eggs");
            await Task.Delay(45000);
            Console.WriteLine("Adding eggs to plate");
            Console.WriteLine("Eggs are ready");
            return new Egg();
        }

        private static Bacon FryBacon(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking the bacon");
            Task.Delay(25000).Wait();
            for (var slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Fliping a slice of bacon");
            }
            Console.WriteLine("cooking the bacon");
            Task.Delay(25000).Wait();
            Console.WriteLine("putting the bacon on the plate");
            Console.WriteLine("Bacon is ready");
            return new Bacon();
        }

        private static async Task<Bacon> FryBaconAsync(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking the bacon");
            await Task.Delay(25000);
            for (var slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Fliping a slice of bacon");
            }
            Console.WriteLine("cooking the bacon");
            await Task.Delay(25000);
            Console.WriteLine("putting the bacon on the plate");
            Console.WriteLine("Bacon is ready");
            return new Bacon();
        }

        #endregion
    }
}
