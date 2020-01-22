using System;
using System.Data;
using System.Diagnostics;

namespace Tests {
    public static class Tester {
        static void PerformTests(int itemsCount, DataTable table) {
            Console.WriteLine(string.Format("Perform tests for {0} items...", itemsCount));
            PerformTest(new DoubleBoxingTest(), table, itemsCount);
            PerformTest(new DoubleGenericTest(), table, itemsCount);
            PerformTest(new DateTimeBoxingTest(), table, itemsCount);
            PerformTest(new DateTimeGenericTest(), table, itemsCount);
            PerformTest(new StringBoxingTest(), table, itemsCount);
            PerformTest(new StringGenericTest(), table, itemsCount);
            Console.WriteLine();
        }
        static void PerformTest(BaseTest test, DataTable table, int itemsCount) {
            Console.Write(string.Format("{0}\t\t{1}\t\t", test.TestType, test.DataType));
            long time = test.Test(itemsCount);
            Console.WriteLine(string.Format("{0}\tms", time));
            table.Rows.Add(test.TestType, test.DataType, itemsCount, (int)time);
            GC.Collect();
        }
        public static void Run(string fileName) {
            DataTable table = new DataTable("GenericVersusBoxingTests");
            table.Columns.Add("TestType", typeof(string));
            table.Columns.Add("DataType", typeof(string));
            table.Columns.Add("ItemsCount", typeof(int));
            table.Columns.Add("Milliseconds", typeof(int));
            for (int i = 1000; i < 100000000; i *= 10)
                PerformTests(i, table);
            PerformTests(20000000, table);
            PerformTests(30000000, table);
            Console.Write(string.Format("Save tests result to the {0} file...", fileName));
            table.WriteXml(fileName);
            Console.WriteLine("Ok");
        }
    }

    abstract class BaseTest {
        public const string DoubleData = "double";
        public const string DateTimeData = "DateTime";
        public const string StringData = "string";
        public const string GenericTest = "Generic";
        public const string BoxingTest = "Boxing";

        public abstract string DataType { get; }
        public abstract string TestType { get; }

        protected abstract object PerformTest(int count);
        public long Test(int count) {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PerformTest(count);
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds;
        }
    }
}
