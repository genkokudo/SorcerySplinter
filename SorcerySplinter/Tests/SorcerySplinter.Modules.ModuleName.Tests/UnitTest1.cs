using System;
using System.Collections.Generic;
using System.Linq;
using ChainingAssertion;
using Xunit;
using Xunit.Abstractions;

namespace SorcerySplinter.Modules.ModuleName.Tests
{
    // PM> Install-Package ChainingAssertion.Core.Xunit

    public class UnitTest1
    {
        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }

        //テストデータ
        public static object[][] AddTestData => Enumerable.Range(1, 10).Select(i => new object[] { i, i, i + i }).ToArray();

        // こういう方法もある
        public static IEnumerable<object[]> TestDataProp {
            get {
                yield return new object[] { 0, 0, 0, "メッセージ1" };
                yield return new object[] { 1, 2, 3, "ノーコメント" };
            }
        }

        // テストデータ生成メソッド
        public static IEnumerable<object[]> MakeAddTestData(int from, int count)
        {
            return Enumerable.Range(from, count).Select(i => new object[] { i, i, i + i });
        }

        // 

        [Trait("Category", "Arithmetic")]
        [Fact(DisplayName = "1+2=3のはず")]
        public void Test1()
        {
            (1 + 2).Is(3);
            output.WriteLine("Test1を実行");
        }


        [Trait("Category", "cccc")]
        [Theory(DisplayName = "1+1=2のはず")]
        [InlineData(1, 1, 2)]
        [InlineData(2, 3, 5)]
        public void AddTest(int x, int y, int ans)
        {
            Add(x, y).Is(ans);
        }

        private int Add(int x, int y)
        {
            return x + y;
        }

        [Theory(DisplayName = "プロパティやメソッドを使ったテスト")]
        [MemberData(nameof(AddTestData))]
        [MemberData(nameof(MakeAddTestData), 2, 100)]
        [MemberData(nameof(MakeAddTestData), 12, 100)]
        public void AddTest2(int x, int y, int ans)
        {
            Add(x, y).Is(ans);
            // この他に、[ClassData(typeof(TestDataClass))]のようにして
            // テストデータ生成クラスを使う方法もある（が、出来ることは微妙でメリットはよく分からない。）
        }
    }
}
