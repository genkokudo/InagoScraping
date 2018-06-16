using Prism.Commands;
using Scraping.Model;
using Scraping.Validation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.ComponentModel;
using Prism.Common;
using System;
using System.Reactive.Linq;

namespace Scraping
{
    class ViewModel : INotifyPropertyChanged
    {
#pragma warning disable 0067
        /// <summary>
        /// INotifyPropertyChangedを継承してPropertyChangedを実装しないとメモリリークする
        /// 警告が出るので無視設定する
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        /// <summary>
        /// POCO
        /// </summary>
        private BaseModel model;

        // Modelに関連したプロパティ
        public ReactiveProperty<string> Url { get; }  // 画面からの入力は文字列なのでstring

        // 読み取り専用プロパティ
        // ReadOnlyReactivePropertyを使うべきだが、エラーがうざいのでReactivePropertyを使ってしまう
        public ReactiveProperty<string> Display { get; }
        //public ReadOnlyReactiveProperty<string> Display { get; }

        /// <summary>
        /// ボタンにバインドするコマンド定義
        /// </summary>
        public ReactiveCommand SendSum { get; }

        public ViewModel()
        {
            // Modelクラスを初期化
            model = new BaseModel();

            // 対応する変数を連動設定する
            Url = model.ToReactivePropertyAsSynchronized(
                m => m.Url,   // BaseModel.Urlに対応付け
                x => x.ToString(),
                s => s,   // doubleに変換
                ReactivePropertyMode.DistinctUntilChanged       // 同じ値が設定されても変更イベントを実行しない
                    | ReactivePropertyMode.RaiseLatestValueOnSubscribe, // ReactivePropertyをSubscribeしたタイミングで現在の値をOnNextに発行する
                true);

            // 読み取り専用設定
            Display = model.ObserveProperty(m => m.Display).ToReactiveProperty();

            // コマンド
            // ReactiveCommandはIObservable<bool>を使って、処理の実行の許可/不許可を制御することができる

            // 実行の許可/不許可を制御するIObservable<bool>
            // このValueがtrueかfalseかで制御される
            ReactiveProperty<bool> gate = new ReactiveProperty<bool>(true);

            // gateを使って実行制御する
            SendSum = new ReactiveCommand(gate);
            SendSum.Subscribe(
                d =>
                {
                    // 実行可能時に実行する処理
                    model.DisplayData();
                }
            );
        }
    }
}
