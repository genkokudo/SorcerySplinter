using ChainingAssertion;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace SorcerySqlinter.Modules.Common.Tests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }

        //�e�X�g�f�[�^
        public static object[][] AddTestData => Enumerable.Range(1, 10).Select(i => new object[] { i, i, i + i }).ToArray();

        // �����������@������
        public static IEnumerable<object[]> TestDataProp
        {
            get
            {
                yield return new object[] { 0, 0, 0, "���b�Z�[�W1" };
                yield return new object[] { 1, 2, 3, "�m�[�R�����g" };
            }
        }

        // �e�X�g�f�[�^�������\�b�h
        public static IEnumerable<object[]> MakeAddTestData(int from, int count)
        {
            return Enumerable.Range(from, count).Select(i => new object[] { i, i, i + i });
        }

        // 

        [Trait("Category", "Arithmetic")]
        [Fact(DisplayName = "1+2=3�̂͂�")]
        public void Test1()
        {
            (1 + 2).Is(3);
            output.WriteLine("Test1�����s");
        }


        [Trait("Category", "cccc")]
        [Theory(DisplayName = "1+1=2�̂͂�")]
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

        [Theory(DisplayName = "�v���p�e�B�⃁�\�b�h���g�����e�X�g")]
        [MemberData(nameof(AddTestData))]
        [MemberData(nameof(MakeAddTestData), 2, 100)]
        [MemberData(nameof(MakeAddTestData), 12, 100)]
        public void AddTest2(int x, int y, int ans)
        {
            Add(x, y).Is(ans);
            // ���̑��ɁA[ClassData(typeof(TestDataClass))]�̂悤�ɂ���
            // �e�X�g�f�[�^�����N���X���g�����@������i���A�o���邱�Ƃ͔����Ń����b�g�͂悭������Ȃ��B�j
        }
    }
}
