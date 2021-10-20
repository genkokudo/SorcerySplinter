using Xunit;
using Moq;  // Install-Package Moq
using Prism.Regions;
using SorcerySplinter.Modules.Common.ViewModels;

// どんなことをテストするのか？

// ViewModelに関してテストを行う
// サービスはモック（作成したサービスもテストを作成する）

// 1個目
// GetMessage()が1回呼び出されたことを確認する
// メッセージがモックに設定した通りであること（サービスで得た文字列に何も変更が入っていないこと）

// 2個目
// コンストラクタによってプロパティが変更されていること

namespace SorcerySqlinter.Modules.Common.Tests.ViewModels
{
    /// <summary>
    /// HomeViewModelのテスト
    /// </summary>
    public class HomeViewModelFixture
    {
        //Mock<IAaaaService> _messageServiceMock;
        //Mock<IRegionManager> _regionManagerMock;
        //const string MessageServiceDefaultMessage = "HelloWork!!";

        //public HomeViewModelFixture()
        //{
        //    var messageService = new Mock<IAaaaService>();  // モックを作成（重要）
        //    messageService.Setup(x => x.GetMessage()).Returns(MessageServiceDefaultMessage);     // メソッドでどんな仮値を返すか設定する。
        //    _messageServiceMock = messageService;

        //    _regionManagerMock = new Mock<IRegionManager>();  // もう1つモックを作成
        //}

        //[Fact(DisplayName = "DIの代わりにモックを使用してテスト")]
        //public void MessagePropertyValueUpdated()
        //{
        //    var vm = new HomeViewModel(_regionManagerMock.Object, _messageServiceMock.Object);

        //    _messageServiceMock.Verify(x => x.GetMessage(), Times.Once);    // HomeViewModel内でGetMessageが呼び出されたかを確認

        //    Assert.Equal(MessageServiceDefaultMessage, vm.Message);     // メッセージが正しく設定されたかを確認
        //}

        //[Fact(DisplayName = "プロパティ変更の確認")]
        //public void MessageINotifyPropertyChangedCalled()
        //{
        //    var vm = new HomeViewModel(_regionManagerMock.Object, _messageServiceMock.Object);

        //    // 第3引数の処理によって、第1引数のオブジェクトの第2引数の名前のプロパティが変更されたかを確認する
        //    Assert.PropertyChanged(vm, nameof(vm.Message), () => vm.Message = "Changed");
        //}
    }
}
