using System;
using System.Collections.Generic;

namespace cleveBatteryBalance
{
    class BatteryBalancer
    {
        public List<BatteryCell> Cells { get; set; }

        private int _totalCells;
        private int _totalValue;
        private int _targetCellValue;
        public int TotalCells { 
            get
            {
                return _totalCells;
            }
        }

        public BatteryBalancer(string values, int cellsTotal)
        {
            Cells = new List<BatteryCell>();
            _totalValue = 0;
            var valArray = values.Split('\n');
            if (valArray.Length % cellsTotal != 0) throw new ArgumentOutOfRangeException("The total number of values must be divisible by the number of cells total");

            _totalCells = cellsTotal;
            var totalBatteriesInEachCell = valArray.Length / cellsTotal;

            var bufferBatteryCell = new BatteryCell();
            for (int i = 0; i < valArray.Length; i++)
            {
                bufferBatteryCell.Add(i+1,valArray[i]);

                if ((i+1)%totalBatteriesInEachCell == 0)
                {
                    _totalValue += bufferBatteryCell.TotalValue;
                    Cells.Add(bufferBatteryCell);
                    bufferBatteryCell = new BatteryCell();
                }
            }

            _targetCellValue = _totalValue / _totalCells;
        }

        private void swap(BatteryCell sourceCell, int sourceIndex, BatteryCell targetCell, int targetIndex)
        {
            var sourceBatteryValue = sourceCell.GetWhereIndex(sourceIndex);
            var targetBatteryValue = targetCell.GetWhereIndex(targetIndex);

            sourceCell.RemoveWhereIndex(sourceIndex);
            targetCell.RemoveWhereIndex(targetIndex);

            sourceCell.Add(targetBatteryValue);
            targetCell.Add(sourceBatteryValue);
        }

        private void swap(BatteryCell sourceCell, int sourceIndex, BatteryCell targetCell, int targetIndex, bool useActIndex)
        {
            var sourceBatteryValue = sourceCell[sourceIndex];
            var targetBatteryValue = targetCell[targetIndex];

            sourceCell.RemoveAt(sourceIndex);
            targetCell.RemoveAt(targetIndex);

            sourceCell.Add(targetBatteryValue);
            targetCell.Add(sourceBatteryValue);
        }

        public void Balance()
        {
            display();
            Console.WriteLine("start balancing ------");

            swapByDifference();
            swapByDifference();
            swapByDifference();
            swapByDifference();
            swapByDifference();
            swapByDifference();
            swapByDifference();
            swapByDifference();
            swapByDifference();
            swapByDifference();
            swapByDifference();
            swapByDifference();
            swapByDifference();

            display();

        }

        private void swapByDifference()
        {
            //start with a sort
            Cells.Sort(new CellComparison());

            //get the cell thats the furthest away both high and low
            var smallest = 0;
            var largest = 0;
            var smallestIndex = -1;
            var largestIndex = -1;
            var diffs = new List<int>();

            for (int i = 0; i < Cells.Count; i++)
            {
                var currentCell = Cells[i];
                var diff = _targetCellValue - currentCell.TotalValue;
                diffs.Add(diff);
                if (diff > 0 && diff > largest)
                {
                    largest = diff;
                    largestIndex = i;
                }

                if (diff < 0 && diff < smallest)
                {
                    smallest = diff;
                    smallestIndex = i;
                }
            }

            //sort the cells
            var small = Cells[smallestIndex];
            var large = Cells[largestIndex];
            small.Sort(new BatteryCellComparision());
            large.Sort(new BatteryCellComparision());

            //get the largest value in the smallest cell
            BalanceCells(small, large);

            Cells.Sort(new CellComparison());
        }

        private void BalanceCells(BatteryCell a, BatteryCell b)
        {
            var balanceAvailable = true;
            var movedList = new List<int>();

            while (balanceAvailable)
            {
                //get the totals of each cell
                var aTotal = a.TotalValue;
                var bTotal = b.TotalValue;

                //get the total summation of all values
                var total = aTotal + bTotal;
                //get the average value
                var avg = total / 2;

                //find the difference between totals and avg val for ea cell
                var aTotalAvgDiff = avg - aTotal;
                var bTotalAvgDiff = avg - bTotal;

                var greatestDiff = (aTotalAvgDiff > bTotalAvgDiff) ? aTotalAvgDiff : bTotalAvgDiff;

                //find the difference between all swaps
                var swapDiffs = new List<KeyValuePair<KeyValuePair<int, int>, int>>();
                for (int i = 0; i < a.Count; i++)
                {
                    for (int j = 0; j < b.Count; j++)
                    {
                        swapDiffs.Add(new KeyValuePair<KeyValuePair<int, int>, int>(new KeyValuePair<int, int>(i, j), Math.Abs(a[i].Value - b[j].Value)));
                    }
                }

                //perform the swap where the difference is the closet to the diff between the total and avg
                var closestPair = new KeyValuePair<KeyValuePair<int, int>, int>();
                var smallestPair = new KeyValuePair<KeyValuePair<int, int>, int>();

                var closestValue = int.MaxValue;
                var smallestValue = int.MaxValue;
                foreach (var diff in swapDiffs)
                {
                    if (diff.Value < smallestValue)
                    {
                        smallestValue = diff.Value;
                        smallestPair = diff;
                    }

                    var currentDiff = greatestDiff - diff.Value;
                    if (movedList.Contains(currentDiff))
                    {
                        continue;
                    }

                    if (currentDiff > 0 && currentDiff < closestValue)
                    {
                        closestValue = currentDiff;
                        closestPair = diff;
                    }
                }

                if (smallestValue > greatestDiff)
                {
                    //break!
                    //repeat until the difference of the values are greater than the difference between the avg
                    balanceAvailable = false;
                }
                else
                {
                    if (!(closestPair.Value == 0 && closestPair.Key.Key ==0 && closestPair.Key.Value ==0))
                    {
                        swap(a, closestPair.Key.Key, b, closestPair.Key.Value,true);
                        movedList.Add(closestValue);
                    }
                    else
                    {
                        balanceAvailable = false;
                    }
                }
            }

        }

        private void swapSmallestWithLargest()
        {
            //start with a sort
            Cells.Sort(new CellComparison());

            //get the cell thats the furthest away both high and low
            var smallest = 0;
            var largest = 0;
            var smallestIndex = -1;
            var largestIndex = -1;

            for (int i = 0; i < Cells.Count; i++)
            {
                var currentCell = Cells[i];
                var diff = _targetCellValue - currentCell.TotalValue;
                if (diff > 0 && diff > largest)
                {
                    largest = diff;
                    largestIndex = i;
                }

                if (diff < 0 && diff < smallest)
                {
                    smallest = diff;
                    smallestIndex = i;
                }
            }

            //Console.WriteLine(smallest + " " + smallestIndex);
            //Console.WriteLine(largest + " " + largestIndex);

            //get the largest value in the smallest cell
            var smallestInSmall = Cells[smallestIndex].GetSmallestValueIndex;
            var largestInLarge = Cells[largestIndex].GetLargestValueIndex;

            //Console.WriteLine("s in s " + smallestInSmall);
            //Console.WriteLine("l in l " + largestInLarge);

            swap(Cells[smallestIndex], smallestInSmall.Index, Cells[largestIndex], largestInLarge.Index);
           
            //sort all
            Cells[smallestIndex].Sort(new BatteryCellComparision());
            Cells[largestIndex].Sort(new BatteryCellComparision());
            Cells.Sort(new CellComparison());
        }

        private void display()
        {
            foreach (var batteries in Cells)
            {
                foreach (var battery in batteries)
                {
                    Console.WriteLine(battery.Index + " " + battery.Value);
                }

                Console.WriteLine("Group Total " + batteries.TotalValue);
            }

            Console.WriteLine("Target Value " + _targetCellValue);
        }
    }
}
