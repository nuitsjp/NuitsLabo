using System.Runtime.InteropServices.ComTypes;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace StringInterpolationBenchmark
{
    public class Benchmark
    {
        [Params(512, 1024)]
        public int Capacity;
        private string _source;
        private string _character = "MyNamespace";
        //        [Benchmark]
        //        public void StringInterpolation1()
        //        {
        //            _source = $@"
        //using System;

        //namespace {_character}";
        //        }
        ////        [Benchmark]
        ////        public void StringConcat1()
        ////        {
        ////            var concatString = string.Empty;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            _source = concatString;
        ////        }
        //        [Benchmark]
        //        public void StringBuilder1()
        //        {
        //            var builder = new StringBuilder(512);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            _source = builder.ToString();
        //        }

        //        [Benchmark]
        //        public void StringInterpolation2()
        //        {
        //            _source = $@"
        //using System;

        //namespace {_character}
        //using System;

        //namespace {_character}";
        //        }
        ////        [Benchmark]
        ////        public void StringConcat2()
        ////        {
        ////            var concatString = string.Empty;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            _source = concatString;
        ////        }
        //        [Benchmark]
        //        public void StringBuilder2()
        //        {
        //            var builder = new StringBuilder(512);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            _source = builder.ToString();
        //        }


        //        [Benchmark]
        //        public void StringInterpolation4()
        //        {
        //            _source = $@"
        //using System;

        //namespace {_character}
        //using System;

        //namespace {_character}
        //using System;

        //namespace {_character}
        //using System;

        //namespace {_character}";
        //        }
        ////        [Benchmark]
        ////        public void StringConcat4()
        ////        {
        ////            var concatString = string.Empty;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            _source = concatString;
        ////        }

        //        [Benchmark]
        //        public void StringBuilder4()
        //        {
        //            var builder = new StringBuilder(512);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            _source = builder.ToString();
        //        }

        //        [Benchmark]
        //        public void StringInterpolation6()
        //        {
        //            _source = $@"
        //using System;

        //namespace {_character}
        //using System;

        //namespace {_character}
        //using System;

        //namespace {_character}
        //using System;

        //namespace {_character}
        //using System;

        //namespace {_character}
        //using System;

        //namespace {_character}";
        //        }
        ////        [Benchmark]
        ////        public void StringConcat6()
        ////        {
        ////            var concatString = string.Empty;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            _source = concatString;
        ////        }

        //        [Benchmark]
        //        public void StringBuilder6()
        //        {
        //            var builder = new StringBuilder(512);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            _source = builder.ToString();
        //        }

        //        [Benchmark]
        //        public void StringInterpolation8()
        //        {
        //            _source = $@"
        //using System;

        //namespace {_character}
        //using System;

        //namespace {_character}
        //using System;

        //namespace {_character}
        //using System;

        //namespace {_character}
        //using System;

        //namespace {_character}
        //using System;

        //namespace {_character}
        //using System;

        //namespace {_character}
        //using System;

        //namespace {_character}";
        //        }
        ////        [Benchmark]
        ////        public void StringConcat8()
        ////        {
        ////            var concatString = string.Empty;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            _source = concatString;
        ////        }

        //        [Benchmark]
        //        public void StringBuilder8()
        //        {
        //            var builder = new StringBuilder(512);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            builder.Append(@"
        //using System;

        //namespace");
        //            builder.Append(_character);
        //            _source = builder.ToString();
        //        }

        [Benchmark]
        public void StringInterpolation10()
        {
            _source = $@"
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}";
        }

        ////        [Benchmark]
        ////        public void StringConcat10()
        ////        {
        ////            var concatString = string.Empty;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            concatString += @"
        ////using System;

        ////namespace";
        ////            concatString += _character;
        ////            _source = concatString;
        ////        }

        [Benchmark]
        public void StringBuilder10()
        {
            var builder = new StringBuilder(Capacity);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            _source = builder.ToString();
        }

        [Benchmark]
        public void StringInterpolation20()
        {
            _source = $@"
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}";
        }

        [Benchmark]
        public void StringBuilder20()
        {
            var builder = new StringBuilder(Capacity);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            _source = builder.ToString();
        }


        [Benchmark]
        public void StringInterpolation40()
        {
            _source = $@"
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}

using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}
using System;

namespace {_character}";
        }

        [Benchmark]
        public void StringBuilder40()
        {
            var builder = new StringBuilder(Capacity);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            builder.Append(@"
using System;

namespace");
            builder.Append(_character);
            _source = builder.ToString();
        }
    }
}