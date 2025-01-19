namespace Feverfew.DiLib.Samples.UsageSample
{
    public class TestParameter
    {
        private static int _counter = 1;

        public int Id => _id;

        private int _id;

        public TestParameter()
        {
            _id = _counter;
            _counter++;
        }
    }
}
