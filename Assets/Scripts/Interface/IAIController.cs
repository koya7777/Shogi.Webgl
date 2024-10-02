using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interface
{
    interface IAIController
    {
        /// <summary>
        /// AIを実行する
        /// </summary>
        public void Exec();
        /// <summary>
        /// サーバーにリクエストした結果を返す
        /// </summary>
        /// <returns></returns>
        public string GetBestMove();
        /// <summary>
        /// サーバーにリクエスト中かどうかを返す
        /// </summary>
        /// <returns></returns>
        public bool GetRequestFlag();
        /// <summary>
        /// サーバーエラーかどうかを返す
        /// </summary>
        /// <returns></returns>
        public bool GetErrorFlag();
        /// <summary>
        /// サーバーにリクエストした結果をリセットする
        /// </summary>
        public void ResetBestMove();
    }
}
