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

                Console.WriteLine("Make Breakfast using differnt levels asynchronicity");
                Console.WriteLine("Enter 1 for sync");
                Console.WriteLine("Enter 2 for sync over async");
                Console.WriteLine("Enter 3 for async");

                var line = Console.ReadLine() ?? "3"; 

                DateTime start;
                TimeSpan difference;

                switch (line)
                {
                    case "1": 
                    
                        WriteMessage("sync selected");
                        start = DateTime.UtcNow;
                        Kettle hotwater = BoilKettle();
                        Egg eggs = FryEgg(2);
                        Bacon bacon = FryBacon(3);
                        Toast toast = MakeToast(3);
                        ButterToast(toast);
                        Tea tea = MakeTea(hotwater);
                        WriteMessage("Breakfast is ready!");
                        difference = DateTime.UtcNow.Subtract(start);
                        WriteMessage($"Time taken {difference.TotalSeconds} (seconds)");
                        break;

                    case "2":
                        WriteMessage("sync over async");
                        start = DateTime.UtcNow;
                        Task<Kettle> hotwaterAsync = BoilKettleAsync();
                        Task<Egg> eggsaAsync = FryEggAsync(2);
                        Task<Bacon> baconAsync = FryBaconAsync(3);
                        Task<Toast> toastAsync = MakeToastAsync(3);
                        /*
                        We are calling ButterToast synchronously
                        i.e we have to await the result of toastAsync 
                        to pass it ButterToast affectlying making the process
                        syncronously
                        */
                        WriteMessage("Awaiting Toast");
                        var toastready = await toastAsync;
                        ButterToast(toastready);
                        /*
                        We are calling MakeTea synchronously
                        i.e we have to await the result of hotwaterAsync 
                        to pass it MakeTea affectlying making the process
                        syncronously
                        */
                        WriteMessage("Awaiting Kettle");
                        var hotwaterready = await hotwaterAsync;
                        MakeTea(hotwaterready);

                        WriteMessage("Awaiting Breakfast");
                        await Task.WhenAll(hotwaterAsync, eggsaAsync, baconAsync, toastAsync);
                        WriteMessage("Breakfast is ready!");
                        difference = DateTime.UtcNow.Subtract(start);
                        WriteMessage($"Time taken {difference.TotalSeconds} (seconds)");
                        break;

                    case "3":
                    
                        WriteMessage("async");
                        start = DateTime.UtcNow;
                        Task<Kettle> hotwaterFullAsync = BoilKettleAsync();
                        Task<Egg> eggsFullAsync = FryEggAsync(2);
                        Task<Bacon> baconFullAsync = FryBaconAsync(3);
                        Task<Toast> toastFullAsync = MakeToastAsync(3);
                        /*
                        We pass the Task returned from MakeToastAsync to ToastWithButter so the thread can continue
                        processing. ToastWithButter awaits the Task.
                        */
                        toastFullAsync = ToastWithButter(toastFullAsync);
                        /*
                        We pass the Task returned from hotwaterFullAsync to TeaWithHotWater so the thread can continue
                        processing. TeaWithHotWater awaits the Task.
                        */
                        Task<Tea> teaFullAsync = TeaWithHotWater(hotwaterFullAsync);
                        WriteMessage("Awaiting Breakfast");
                        await Task.WhenAll(hotwaterFullAsync, eggsFullAsync, baconFullAsync, toastFullAsync, teaFullAsync);
                        WriteMessage("Breakfast is ready!");
                        difference = DateTime.UtcNow.Subtract(start);
                        WriteMessage($"Time taken {difference.TotalSeconds} (seconds)");
                        break;
                    
                }
            }
        }

        #region methods

        private static void WriteMessage(string message)
        {
            Console.WriteLine($"{DateTime.UtcNow} {message} ");
        }

        private static async Task<Tea> TeaWithHotWater(Task<Kettle> hotwater)
        {
            WriteMessage("Awaiting Kettle");
            var hotwaterReady = await hotwater;
            return MakeTea(hotwaterReady);
            
        }

        private static async Task<Toast> ToastWithButter(Task<Toast> breadToasted)
        {
            WriteMessage("Awaiting Toast");
            var toast = await breadToasted;
            ButterToast(toast);
            return toast;
        }

        private static void ButterToast(Toast toast)
        {
            WriteMessage($"Buttering Toast");
            WriteMessage("Toast is ready");
        }

        private static Toast MakeToast(int slices)
        {
            var toast = new Toast();
            WriteMessage($"Toasting {slices} slices of Toast in toaster");
            Task.Delay(toast.Toasttime).Wait();
            WriteMessage("Adding Toast to plate");
            return toast;
        }

        private static async Task<Toast> MakeToastAsync(int slices)
        {
            var toast = new Toast();
            WriteMessage($"Toasting {slices} slices of Toast in toaster");
            await Task.Delay(toast.Toasttime);
            WriteMessage("Adding Toast to plate");
            return toast;
        }

        private static Tea MakeTea(Kettle hotwater)
        {
            WriteMessage("Place Tea bag in Cup");
            Task.Delay(500).Wait();
            WriteMessage("Pour Water into Cup");
            Task.Delay(500).Wait();
            WriteMessage("Take out tea bag");
            Task.Delay(500).Wait();
            WriteMessage("add milk to brew");
            WriteMessage("Tea is ready");
            return new Tea();
        }

        private static Kettle BoilKettle()
        {
            var kettle = new Kettle();
            WriteMessage("Fill Kettle With Water");
            Task.Delay(kettle.Filltime).Wait();
            WriteMessage("Boil Kettle");
            Task.Delay(kettle.BoilTime).Wait();
            WriteMessage("Hot Water ready");
            return kettle;
        }

        private static async Task<Kettle> BoilKettleAsync()
        {
            var kettle = new Kettle();
            WriteMessage("Fill Kettle With Water");
            Task.Delay(kettle.Filltime).Wait();
            WriteMessage("Boil Kettle");
            await Task.Delay(kettle.BoilTime);
            WriteMessage("Hot Water ready");
            return kettle;
        }

        private static Egg FryEgg(int eggs)
        {
            var egg = new Egg();
            WriteMessage($"cracking {eggs} eggs in the pan");
            WriteMessage("cooking the eggs");
            Task.Delay(egg.CookTime).Wait();
            WriteMessage("Adding eggs to plate");
            WriteMessage("Eggs are ready");
            return egg;
        }

        private static async Task<Egg> FryEggAsync(int eggs)
        {
            var egg = new Egg();
            WriteMessage($"cracking {eggs} eggs in the pan");
            WriteMessage("cooking the eggs");
            await Task.Delay(egg.CookTime);
            WriteMessage("Adding eggs to plate");
            WriteMessage("Eggs are ready");
            return egg;
        }

        private static Bacon FryBacon(int slices)
        {
            var bacon = new Bacon();
            WriteMessage($"putting {slices} slices of bacon in the pan");
            WriteMessage("cooking the bacon");
            Task.Delay(bacon.Cooktime).Wait();
            for (var slice = 0; slice < slices; slice++)
            {
                WriteMessage("Fliping a slice of bacon");
            }
            WriteMessage("cooking the bacon");
            Task.Delay(bacon.Cooktime).Wait();
            WriteMessage("putting the bacon on the plate");
            WriteMessage("Bacon is ready");
            return bacon;
        }

        private static async Task<Bacon> FryBaconAsync(int slices)
        {
            var bacon = new Bacon();
            WriteMessage($"putting {slices} slices of bacon in the pan");
            WriteMessage("cooking the bacon");
            await Task.Delay(bacon.Cooktime);
            for (var slice = 0; slice < slices; slice++)
            {
                WriteMessage("Fliping a slice of bacon");
            }

            WriteMessage("cooking the bacon");
            await Task.Delay(bacon.Cooktime);
            WriteMessage("putting the bacon on the plate");
            WriteMessage("Bacon is ready");
            return new Bacon();
        }

        #endregion
    }
}
