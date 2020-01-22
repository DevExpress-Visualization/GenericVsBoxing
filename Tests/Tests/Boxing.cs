using System;
using System.Collections.Generic;

namespace Tests {
    abstract class BoxingTest : BaseTest {
        object ReadValue(List<object> data, int index) {
            return data[index];
        }
        protected abstract void SetValue(List<object> data, int index);
        protected override object PerformTest(int count) {
            List<object> data = new List<object>(count);
            for (int i = 0; i < count; i++)
                SetValue(data, i);
            object temp = null;
            for (int i = 0; i < count; i++)
                temp = ReadValue(data, i);
            return temp;
        }
    }

    class DoubleBoxingTest : BoxingTest {
        public override string DataType { get { return DoubleData; } }
        public override string TestType { get { return BoxingTest; } }

        protected override void SetValue(List<object> data, int index) {
            data.Add(index);
        }
    }

    class DateTimeBoxingTest : BoxingTest {
        public override string DataType { get { return DateTimeData; } }
        public override string TestType { get { return BoxingTest; } }

        protected override void SetValue(List<object> data, int index) {
            data.Add(DateTime.MinValue.AddMilliseconds(index));
        }
    }

    class StringBoxingTest : BoxingTest {
        public override string DataType { get { return StringData; } }
        public override string TestType { get { return BoxingTest; } }

        protected override void SetValue(List<object> data, int index) {
            data.Add(index.ToString());
        }
    }
}
