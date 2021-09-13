using Moq;
using Prism.Regions;
using SorcerySplinter.Modules.ModuleName.ViewModels;
using SorcerySplinter.Services.Interfaces;
using Xunit;

// どんなことをテストするのか？

// ViewModelに関してテストを行う
// サービスはモック（作成したサービスもテストを作成する）

// 1個目
// GetMessage()が1回呼び出されたことを確認する
// メッセージがモックに設定した通りであること（サービスで得た文字列に何も変更が入っていないこと）

// 2個目
// コンストラクタによってプロパティが変更されていること

namespace SorcerySplinter.Modules.ModuleName.Tests.ViewModels
{
    /// <summary>
    /// ViewAのテスト
    /// 
    /// </summary>
    public class ViewAViewModelFixture
    {
        Mock<IMessageService> _messageServiceMock;
        Mock<IRegionManager> _regionManagerMock;
        const string MessageServiceDefaultMessage = "Some Value";

        public ViewAViewModelFixture()
        {
            var messageService = new Mock<IMessageService>();
            messageService.Setup(x => x.GetMessage()).Returns(MessageServiceDefaultMessage);
            _messageServiceMock = messageService;

            _regionManagerMock = new Mock<IRegionManager>();
        }

        [Fact]
        public void MessagePropertyValueUpdated()
        {
            var vm = new ViewAViewModel(_regionManagerMock.Object, _messageServiceMock.Object);

            // GetMessage()が1回呼び出されたことを確認する
            _messageServiceMock.Verify(x => x.GetMessage(), Times.Once);

            // メッセージがモックに設定した通りであること（サービスで得た文字列に何も変更が入っていないこと）
            Assert.Equal(MessageServiceDefaultMessage, vm.Message);
        }

        [Fact]
        public void MessageINotifyPropertyChangedCalled()
        {
            // コンストラクタによってプロパティが変更されていること
            var vm = new ViewAViewModel(_regionManagerMock.Object, _messageServiceMock.Object);
            Assert.PropertyChanged(vm, nameof(vm.Message), () => vm.Message = "Changed");
        }
    }
}
