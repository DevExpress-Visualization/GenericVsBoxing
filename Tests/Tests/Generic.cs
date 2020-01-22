using System;
using System.Collections.Generic;

namespace Tests {
    abstract class GenericTest<T> : BaseTest {
        protected abstract T Default { get; }

        protected abstract void SetValue(List<T> data, int index);
        protected abstract T ReadValue(List<T> data, int index);
        protected override object PerformTest(int count) {
            List<T> data = new List<T>(count);
            for (int i = 0; i < count; i++)
                SetValue(data, i);
            T temp = Default;
            for (int i = 0; i < count; i++)
                temp = ReadValue(data, i);
            return temp;
        }
    }

    class DoubleGenericTest : GenericTest<double> {
        protected override double Default { get { return 0; } }
        public override string DataType { get { return DoubleData; } }
        public override string TestType { get { return GenericTest; } }

        protected override void SetValue(List<double> data, int index) {
            data.Add(index);
        }
        protected override double ReadValue(List<double> data, int index) {
            return data[index];
        }
    }

    class DateTimeGenericTest : GenericTest<DateTime> {
        protected override DateTime Default { get { return DateTime.MinValue; } }
        public override string DataType { get { return DateTimeData; } }
        public override string TestType { get { return GenericTest; } }

        protected override void SetValue(List<DateTime> data, int index) {
            data.Add(DateTime.MinValue.AddMilliseconds(index));
        }
        protected override DateTime ReadValue(List<DateTime> data, int index) {
            return data[index];
        }
    }

    class StringGenericTest : GenericTest<string> {
        protected override string Default { get { return string.Empty; } }
        public override string DataType { get { return StringData; } }
        public override string TestType { get { return GenericTest; } }

        protected override void SetValue(List<string> data, int index) {
            data.Add(index.ToString());
        }
        protected override string ReadValue(List<string> data, int index) {
            return data[index];
        }
    }
}
