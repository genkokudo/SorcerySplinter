using Bbbb.ViewModels;
using Moq;
using Prism.Regions;
using SorcerySplinter.Services.Interfaces;
using Xunit;

namespace SorcerySplinter.Modules.ModuleName.Tests.ViewModels
{
    /// <summary>
    /// ViewBのテスト
    /// 
    /// </summary>
    public class ViewCViewModelFixture
    {
        Mock<IAaaaService> _messageServiceMock;
        Mock<IRegionManager> _regionManagerMock;
        const string MessageServiceDefaultMessage = "HelloWork!!";

        public ViewCViewModelFixture()
        {
            var messageService = new Mock<IAaaaService>();  // モックを作成（重要）
            messageService.Setup(x => x.GetMessage()).Returns(MessageServiceDefaultMessage);     // メソッドでどんな仮値を返すか設定する。
            _messageServiceMock = messageService;

            _regionManagerMock = new Mock<IRegionManager>();  // もう1つモックを作成
        }

        [Fact(DisplayName = "DIの代わりにモックを使用してテスト")]
        public void MessagePropertyValueUpdated()
        {
            var vm = new ViewCViewModel(_regionManagerMock.Object, _messageServiceMock.Object);

            _messageServiceMock.Verify(x => x.GetMessage(), Times.Once);    // ViewCViewModel内でGetMessageが呼び出されたかを確認

            Assert.Equal(MessageServiceDefaultMessage, vm.Message);     // メッセージが正しく設定されたかを確認
        }

        [Fact(DisplayName = "プロパティ変更の確認")]
        public void MessageINotifyPropertyChangedCalled()
        {
            var vm = new ViewCViewModel(_regionManagerMock.Object, _messageServiceMock.Object);

            // 第3引数の処理によって、第1引数のオブジェクトの第2引数の名前のプロパティが変更されたかを確認する
            Assert.PropertyChanged(vm, nameof(vm.Message), () => vm.Message = "Changed");
        }
    }
}
