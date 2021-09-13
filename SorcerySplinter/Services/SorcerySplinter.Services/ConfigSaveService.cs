using Microsoft.Extensions.Options;

namespace SorcerySplinter.Services
{
    /// <summary>
    /// AesServiceのオプションです
    /// </summary>
    public class ConfigSaveOption
    {
        /// <summary>
        /// 保存先ファイルパス
        /// </summary>
        public string Path { get; set; } = @"./data/config.mrt";
    }

    /// <summary>
    /// 適当な設定データ保存サービス
    /// 標準の設定ファイルは問題があるため使用しない（保存場所選べない、モジュールから参照できないとか。）
    /// </summary>
    public interface IConfigSaveService
    {
        /// <summary>
        /// 保存する
        /// </summary>
        /// <returns></returns>
        public void Save();

        /// <summary>
        /// 読み込む
        /// </summary>
        /// <returns></returns>
        public void Load();
    }

    public class ConfigSaveService : IConfigSaveService
    {
        /// <summary>
        /// 保存先ファイルパス
        /// </summary>
        private readonly string _path;

        public ConfigSaveService(IOptions<ConfigSaveOption> options)
        {
            _path = options.Value.Path;
        }

        public void Load()
        {
            // 無かったらnullでも返す。
            throw new System.NotImplementedException();
        }

        public void Save()
        {

            throw new System.NotImplementedException();
        }
    }

    // TODO:クラスを作ってシリアライズする
    // 余裕があればOption、<T>とかで汎用的にする。
}
