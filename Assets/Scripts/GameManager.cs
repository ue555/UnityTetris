using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // グリッドのサイズ
    public static readonly int gridWidth = 10;
    public static readonly int gridHeight = 20;

    // ゲーム盤の状態を保持する2次元配列
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    // テトリミノのプレハブを格納する配列
    public GameObject[] tetrominoPrefabs;
    public Text gameOverText;

    void Start()
    {
        SpawnNextTetromino();
    }

    // 次のテトリミノを生成する
    public void SpawnNextTetromino()
    {
        // ランダムにプレハブを選択
        int randomIndex = Random.Range(0, tetrominoPrefabs.Length);
        // 生成
        GameObject nextTetromino = Instantiate(tetrominoPrefabs[randomIndex], new Vector3(5, 18, 0), Quaternion.identity);

        // ▼▼▼ ゲームオーバー判定を追加 ▼▼▼
        if (!CheckIsValidPosition(nextTetromino))
        {
            GameOver();
            Destroy(nextTetromino); // 生成したけど置けないブロックは消す
            return; // ゲームオーバーなのでここで処理を終了
        }

    }

    // SceneビューやGameビューにデバッグ用の図形を描画します
    private void OnDrawGizmos()
    {
        // グリッドの枠線を描画
        Gizmos.color = Color.yellow; // 線の色を黄色に設定

        // グリッドの中心座標を計算
        // 例: width=10 の場合、中心は (10-1)/2 = 4.5
        Vector3 center = new Vector3((float)gridWidth / 2 - 0.5f, (float)gridHeight / 2 - 0.5f, 0);

        // グリッドのサイズを指定
        Vector3 size = new Vector3(gridWidth, gridHeight, 0);

        // ワイヤーフレームの四角形を描画
        Gizmos.DrawWireCube(center, size);
    }
    
    bool CheckIsValidPosition(GameObject tetromino)
    {
        foreach (Transform child in tetromino.transform)
        {
            int roundedX = Mathf.RoundToInt(child.position.x);
            int roundedY = Mathf.RoundToInt(child.position.y);

            // グリッドの範囲内にいるかチェック
            if (roundedX < 0 || roundedX >= gridWidth || roundedY < 0 || roundedY >= gridHeight)
            {
                return false;
            }

            // 他のブロックと重なっていないかチェック
            if (grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        return true;
    }
    
    public void GameOver()
    {
        Debug.Log("GAME OVER");
        gameOverText.enabled = true; // "Game Over"テキストを表示
        enabled = false; // GameManagerスクリプトを停止（新しいブロックが生成されなくなる）
    }
}
