using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // グリッドのサイズ
    public static readonly int gridWidth = 10;
    public static readonly int gridHeight = 20;

    // ゲーム盤の状態を保持する2次元配列
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    // テトリミノのプレハブを格納する配列
    public GameObject[] tetrominoPrefabs;

    void Start()
    {
        SpawnNextTetromino();
    }

    // 次のテトリミノを生成する
    public void SpawnNextTetromino()
    {
        // ランダムにプレハブを選択
        int randomIndex = Random.Range(0, tetrominoPrefabs.Length);
        // 生成位置（画面上部中央）で生成
        Instantiate(tetrominoPrefabs[randomIndex], new Vector3(5, 18, 0), Quaternion.identity);
    }
}
