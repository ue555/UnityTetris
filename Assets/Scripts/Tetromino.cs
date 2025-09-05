using UnityEngine;

public class Tetromino : MonoBehaviour
{
    private float lastFallTime; // 最後に落下した時間
    public float fallSpeed = 1f; // 落下速度

    void Start()
    {
        lastFallTime = Time.time;
    }

    void Update()
    {
        // --- プレイヤーの入力 ---
        // 左移動
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!IsValidMove())
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
        // 右移動
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!IsValidMove())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        // 回転
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);
            if (!IsValidMove())
            {
                transform.Rotate(0, 0, 90);
            }
        }

        // --- 自動落下 ---
        if (Time.time - lastFallTime >= (Input.GetKey(KeyCode.DownArrow) ? fallSpeed / 10 : fallSpeed))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!IsValidMove())
            {
                // 移動できない場合（地面か他のブロックに接触）
                transform.position += new Vector3(0, 1, 0); // 1つ上に戻す
                AddToGrid(); // グリッドにブロックを登録
                CheckForLines(); // ラインが消えるかチェック
                this.enabled = false; // このスクリプトを無効化
                FindObjectOfType<GameManager>().SpawnNextTetromino(); // 次のブロックを生成
            }
            lastFallTime = Time.time;
        }
    }

    // 移動先が有効かどうかをチェックする
    bool IsValidMove()
    {
        foreach (Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.position.x);
            int roundedY = Mathf.RoundToInt(child.position.y);

            // グリッドの範囲外に出ていないか
            if (roundedX < 0 || roundedX >= GameManager.gridWidth || roundedY < 0 || roundedY >= GameManager.gridHeight)
            {
                return false;
            }

            // 他のブロックと重なっていないか
            if (GameManager.grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        return true;
    }

    // ブロックをグリッドに登録する
    void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.position.x);
            int roundedY = Mathf.RoundToInt(child.position.y);
            // グリッドに登録する前に、座標が範囲内か最終チェックを行う
            if (roundedX >= 0 && roundedX < GameManager.gridWidth && roundedY >= 0 && roundedY < GameManager.gridHeight)
            {
                GameManager.grid[roundedX, roundedY] = child;
            }
        }
    }

    // ラインがそろったかチェックし、消去する
    void CheckForLines()
    {
        for (int i = GameManager.gridHeight - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    // 指定された行がすべて埋まっているか
    bool HasLine(int y)
    {
        for (int x = 0; x < GameManager.gridWidth; x++)
        {
            if (GameManager.grid[x, y] == null)
                return false;
        }
        return true;
    }

    // 指定された行を削除する
    void DeleteLine(int y)
    {
        for (int x = 0; x < GameManager.gridWidth; x++)
        {
            Destroy(GameManager.grid[x, y].gameObject);
            GameManager.grid[x, y] = null;
        }
    }

    // 指定された行より上の行をすべて1段下げる
    void RowDown(int y)
    {
        for (int i = y; i < GameManager.gridHeight; i++)
        {
            for (int x = 0; x < GameManager.gridWidth; x++)
            {
                if (GameManager.grid[x, i] != null)
                {
                    GameManager.grid[x, i - 1] = GameManager.grid[x, i];
                    GameManager.grid[x, i] = null;
                    GameManager.grid[x, i - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }
}