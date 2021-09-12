namespace SorcerySplinter.Services
{
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
        /// 固定、設定はできない仕様
        /// 他のプログラムで使い回すなら、IOptionで設定できるようにする。
        /// </summary>
        private readonly string _path;

        public ConfigSaveService()
        {
            _path = "./data/config.mrt";        // 固定
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
