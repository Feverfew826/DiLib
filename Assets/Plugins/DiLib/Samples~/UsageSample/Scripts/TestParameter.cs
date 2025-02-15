using System;

using UnityEngine;

namespace Feverfew.DiLib.Samples.UsageSample
{
    public sealed class TestParameter : IDisposable
    {
        private static int _counter = 1;

        public int Id => _id;

        private int _id;

        public TestParameter()
        {
            _id = _counter;
            _counter++;
        }

        public void Dispose()
        {
            Debug.Log("TestParameter is disposed.");
        }
    }
}
