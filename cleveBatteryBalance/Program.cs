namespace cleveBatteryBalance
{
    class Program
    {
        static void Main(string[] args)
        {
            var allValues = @"2092
2130
2196
2168
2380
2355
2327
2299
2372
2379
2378
2343
2363
2382
2382
2354
2345
2373
2440
2407
2446
2483
2415
2425
2451
2403
2581
2549
2512
2544
2560
2564
2520
2501
2583
2574
2503
2583
2596
2523
2558
2607
2607
2607
2670
2677
2607
2605";
            var balancer = new BatteryBalancer(allValues, 6);
            balancer.Balance();



        }
    }
}
