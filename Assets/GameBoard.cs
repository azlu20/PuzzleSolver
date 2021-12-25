using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[,] grid = new GameObject[6, 5];
    public GameObject[] prefab;
    private GameObject[,] preset = new GameObject[6, 5];
    private float x_offset = -3;
    private float y_offset = -2;
    bool flag = false;
    int moves = 100;
    int cur_i = 5;
    int cur_j = 4;
    string past = "";
    int cur = 10;
    void Start()
    {

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[i, y] = (GameObject)Instantiate(prefab[Random.Range(0, 5)], new Vector3(i + x_offset, y + y_offset, 0), Quaternion.identity);
            }
        }
        /*  preset[0, 0] = (GameObject)Instantiate(prefab[0], new Vector3( x_offset, y_offset, 0), Quaternion.identity);
          preset[1, 0] = (GameObject)Instantiate(prefab[0], new Vector3(1 +x_offset, y_offset, 0), Quaternion.identity);
          preset[2, 0] = (GameObject)Instantiate(prefab[0], new Vector3(2 + x_offset, y_offset, 0), Quaternion.identity);
          preset[3, 0] = (GameObject)Instantiate(prefab[1], new Vector3(3 + x_offset, y_offset, 0), Quaternion.identity);
          preset[4, 0] = (GameObject)Instantiate(prefab[1], new Vector3(4 + x_offset, y_offset, 0), Quaternion.identity);
          preset[5, 0] = (GameObject)Instantiate(prefab[1], new Vector3(5 + x_offset, y_offset, 0), Quaternion.identity);
          preset[0, 1] = (GameObject)Instantiate(prefab[1], new Vector3(x_offset, 1 + y_offset, 0), Quaternion.identity);
          preset[1, 1] = (GameObject)Instantiate(prefab[1], new Vector3(1 + x_offset, 1 + y_offset, 0), Quaternion.identity);
          preset[2, 1] = (GameObject)Instantiate(prefab[1], new Vector3(2 + x_offset, 1 + y_offset, 0), Quaternion.identity);
          preset[3, 1] = (GameObject)Instantiate(prefab[0], new Vector3(3 +x_offset, 1 + y_offset, 0), Quaternion.identity);
          preset[4, 1] = (GameObject)Instantiate(prefab[0], new Vector3(4 + x_offset, 1 +y_offset, 0), Quaternion.identity);
          preset[5, 1] = (GameObject)Instantiate(prefab[0], new Vector3(5 + x_offset, 1 + y_offset, 0), Quaternion.identity);
        */
        StartCoroutine(lossFunctionBackTrack());
        //StartCoroutine(test());
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void Awake()
    {
       
    }
    IEnumerator test() {
        simulateMoveLeft(cur_i, cur_j);
        yield return new WaitForSeconds(1);
        simulateMoveUp(cur_i, cur_j);
        yield return new WaitForSeconds(1);
        moveLeft(cur_i, cur_j);
        cur_i += 1;
    }
IEnumerator stringToMove(List<string> move) {
        for (int i = move.Count - 1; i >= 1; i--)
        {
            if (move[i] == "left")
            {
                if (moveLeft(cur_i, cur_j)) ;
                cur_i -= 1;
                yield return new WaitForSeconds(0.1f); 
            }
            if (move[i] == "right")
            {
                if (moveRight(cur_i, cur_j))
                    cur_i += 1;
                yield return new WaitForSeconds(0.1f);
            }
            if (move[i] == "up")
            {
                if (moveUp(cur_i, cur_j)) ;
                cur_j += 1;
                yield return new WaitForSeconds(0.1f);
            }
            if (move[i] == "down")
            {
                if (moveDown(cur_i, cur_j)) ;
                cur_j -= 1;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
   public List<string> lossHelper(int depth, string past, string last, GameObject [,] board, int i, int j)
    {


        if (depth == 10) {
            
            var val = calculateBoardCombo(board, i, j, "whole");
            Debug.Log(val);
            List<string> combined = new List<string>();
            combined.Add(val.ToString());
            combined.Add(last);
            return combined;
        }

        int min = cur;
        List<string> move = null;
        if (past != "left")
        {
            var newboard = simulateMoveLeft(i, j);
            if (newboard != null)
            {
                var next = lossHelper(depth + 1, "", "left", newboard, i-1, j);
                if (int.Parse(next[0]) <= min) {
                    min = int.Parse(next[0]);
                    move = next;
                }
            }
        }
        if (past != "right")
        {
            var newboard = simulateMoveRight(i, j);
            if (newboard != null)
            {
                var next = lossHelper(depth + 1, "", "right",  newboard, i + 1, j);
                if (int.Parse(next[0]) <= min)
                {
                    min = int.Parse(next[0]);
                    move = next;
                }
            }
        }
        if (past != "up")
        {
            var newboard = simulateMoveUp(i, j);
            if (newboard != null)
            {
                var next = lossHelper(depth + 1, "", "up", newboard, i, j+1);
                if (int.Parse(next[0]) <= min)
                {
                    min = int.Parse(next[0]);
                    move = next;
                }
            }
        }
        if (past != "down")
        {
            var newboard = simulateMoveDown(i, j);
            if (newboard != null)
            {
                var next = lossHelper(depth + 1, "ped ", "down", newboard, i, j-1);
                if (int.Parse(next[0]) <= min)
                {
                    min = int.Parse(next[0]);
                    move = next;
                }
            }
        }
        if (move == null) {
            flag = true;
            move = new List<string>();
            move.Add((min+1).ToString());
            return move;
        }
        move.Add(last);
        return move;

    }
IEnumerator lossFunctionBackTrack() {
        while (moves > 0)
        {
            GameObject[,] copy = new GameObject[6, 5];
            cur = totalCombo(grid) * -10;
            var move = lossHelper(0, "", "", grid, cur_i, cur_j);
            if (move.Count < 1) {
                continue;
            }
            for (int i = move.Count - 1; i >= 1; i--)
            {
                if (move[i] == "left")
                {
                    if(moveLeft(cur_i, cur_j));
                    cur_i -= 1;
                    yield return new WaitForSeconds(0.1f);
                }
                if (move[i] == "right")
                {
                    if(moveRight(cur_i, cur_j))
                    cur_i+=1;
                    yield return new WaitForSeconds(0.1f);
                }
                if (move[i] == "up")
                {
                    if(moveUp(cur_i, cur_j));
                    cur_j+=1;
                    yield return new WaitForSeconds(0.1f);
                }
                if (move[i] == "down")
                {
                    if(moveDown(cur_i, cur_j));
                    cur_j-=1;
                    yield return new WaitForSeconds(0.1f);
                }
            }
            moves--;
        }
    }
    public void lossFunction(string weight)
    {
        int left = 100000;
        int right = 100000;
        int up = 100000;
        int down = 100000;
        if (weight == "combo")
        {
            if (past != "left") {
                var newboard = simulateMoveLeft(cur_i, cur_j);
                if (newboard != null) {
                    left = calculateBoardCombo(newboard, cur_i, cur_j, "whole");
                }
            }
            if (past != "right")
            {
                var newboard = simulateMoveRight(cur_i, cur_j);
                if (newboard != null)
                {
                    right = calculateBoardCombo(newboard, cur_i, cur_j, "whole");
                }
            }
            if (past != "up")
            {
                var newboard = simulateMoveUp(cur_i, cur_j);
                if (newboard != null)
                {
                    up = calculateBoardCombo(newboard, cur_i, cur_j, "whole");
                }
            }
            if (past != "down")
            {
                var newboard = simulateMoveDown(cur_i, cur_j);
                if (newboard != null)
                {
                    down = calculateBoardCombo(newboard, cur_i, cur_j, "whole");
                }
            }
            if (left < right && left < up && left < down)
            {
                moveLeft(cur_i, cur_j);
                past = "right";
                cur_i -= 1;
            }
            else if (right < left && right < up && right < down)
            {
                moveRight(cur_i, cur_j);
                past = "left";
                cur_i += 1;
            }
            else if (up < left && up < right && up < down)
            {
                moveUp(cur_i, cur_j);
                past = "down";
                cur_j += 1;
            }
            else if (down < left && down < right && down < up)
            {
                moveDown(cur_i, cur_j);
                past = "up";
                cur_j -= 1;
            }
            else {
                if (left != 100000)
                {
                    moveLeft(cur_i, cur_j);
                    past = "right";
                    cur_i -= 1;
                }
                else if (right != 100000)
                {
                    moveRight(cur_i, cur_j);
                    past = "left";
                    cur_i += 1;
                }
                else if (up != 100000)
                {
                    moveUp(cur_i, cur_j);
                    past = "down";
                    cur_j += 1;
                }
                else if (down != 100000) {
                    moveDown(cur_i, cur_j);
                    past = "up";
                    cur_j -= 1;
                }
            }
        }

    }
 
    public int calculateBoardCombo(GameObject[,] board, int i, int j, string type)
    {
        if (type == "single")
        {
            int score = 0;
            if (isCombo(board, i, j))
            {
                score -= 100;
            }
            else
            {
                score += manhattanDistance(board, i, j);
            }
            return score;
        }
        if (type == "whole") {
            int score = totalCombo(board) * -10;
            return score;
        }
        return 0;
    }
    private int totalCombo(GameObject[,] board) {
        HashSet<int> seen = new HashSet<int>();
        Queue<int[]> q = new Queue<int[]>();

        int combos = 0;
        int i = 0;
        int j = 0;
        while (i < board.GetLength(0)) {
            j = 0;
            while (j < board.GetLength(1))
            {
                if (seen.Contains(i * 10 + j) == false)
                {
                    seen.Add(i * 10 + j);
                    q.Enqueue(new int[] { i, j });
                    int count = 0;
                    while (q.Count != 0)
                    {
                        var index = q.Dequeue();
                        var color = board[index[0], index[1]].tag;
                        if (index[0] + 1 < board.GetLength(0) && seen.Contains((index[0] + 1) * 10 + index[1]) == false)
                        {
                            var right = peek(board, index[0], index[1], color, "right");
                            if (right == true)
                            {
                                q.Enqueue(new int[] { index[0] + 1, index[1] });
                                seen.Add((index[0] + 1) * 10 + index[1]);
                                count += 1;
                            }
                        }
                        if (index[0] - 1 >= 0 && seen.Contains((index[0] - 1) * 10 + index[1]) == false)
                        {
                            if (peek(board, index[0], index[1], color, "left") == true)
                            {
                                q.Enqueue(new int[] { index[0] - 1, index[1] });
                                seen.Add((index[0] - 1) * 10 + index[1]);
                                count += 1;
                            }
                        }
                        if (index[1] + 1 < board.GetLength(1) && seen.Contains((index[0]) * 10 + index[1] + 1) == false)
                        {
                            if (peek(board, index[0], index[1], color, "up") == true)
                            {
                                q.Enqueue(new int[] { index[0], index[1] + 1 });
                                seen.Add((index[0]) * 10 + index[1] + 1);
                                count += 1;
                            }
                        }
                        if (index[1] - 1 >= 0 && seen.Contains((index[0]) * 10 + index[1] - 1) == false)
                        {
                            if (peek(board, index[0], index[1], color, "down") == true)
                            {
                                q.Enqueue(new int[] { index[0], index[1] - 1 });
                                seen.Add((index[0]) * 10 + index[1] - 1);
                                count += 1;
                            }
                        }

                    }
                    if (count >= 2)
                    {
                        combos++;
                    }
                }
                j++;
            }
            i++;
        }
        return combos;
    }
    private bool peek(GameObject[,] board, int i, int j, string color, string direction)
    {
        if (direction == "right")
        {
                if (board[i + 1, j].tag == color)
                {
                    return true;
                }
                else {
                    return false;
                }
        }
        if (direction == "left")
        {
            if (board[i - 1, j].tag == color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if (direction == "up")
        {
            if (board[i, j+1].tag == color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if (direction == "down")
        {
            if (board[i, j-1].tag == color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    private int manhattanDistance(GameObject[,] board, int i, int j) {
        string color = board[i, j].tag;
        int distance = 0;
        for (int x = 0; x < board.GetLength(0); x++) {
            for (int y = 0; y < board.GetLength(1); y++) {
                if (color == board[x, y].tag) {
                    distance += Mathf.Abs(i - x) + Mathf.Abs(j - y);
                }
            }
        }
        return distance;
    }
    private bool isCombo(GameObject[,] board, int i, int j) {
        string color = board[i, j].tag;
        if (this.checkRight(board, i + 1, j, color) || this.checkLeft(board, i - 1, j, color)
            || this.checkDown(board, i, j - 1, color) || this.checkUp(board, i, j + 1, color)) {
            return true;
        }
        return false;

    }
    private bool checkRight(GameObject[,] board, int x, int y, string color) {
        int count = 0;
        var goal = x + 2;
        while (x < goal && x < board.GetLength(0))
        {
            if (board[x, y].tag == color)
            {
                count += 1;
            }
            x += 1;
        }
        if (count == 2)
        {
            return true;
        }
        return false;
    }
    private bool checkLeft(GameObject[,] board, int x, int y, string color)
    {
        int count = 0;
        var goal = x - 2;
        while (x < goal && x >= 0)
        {
            if (board[x, y].tag == color)
            {
                count += 1;
            }
            x -= 1;
        }
        if (count == 2)
        {
            return true;
        }
        return false;
    }
    private bool checkDown(GameObject[,] board, int x, int y, string color)
    {
        int count = 0;
        var goal = y - 2;
        while (y < goal && y >= 0)
        {
            if (board[x, y].tag == color)
            {
                count += 1;
            }
            y -= 1;
        }
        if (count == 2)
        {
            return true;
        }
        return false;
    }
    private bool checkUp(GameObject[,] board, int x, int y, string color)
    {
        int count = 0;
        var goal = y + 2;
        while (y < goal && y < board.GetLength(1))
        {
            if (board[x, y].tag == color)
            {
                count += 1;
            }
            y += 1;
        }
        if (count == 2)
        {
            return true;
        }
        return false;
    }
    bool moveLeft(int i, int j) {
        if (i > 0) {
            GameObject temp = grid[i - 1, j];
            Vector2 temp_position = temp.transform.position;
            grid[i - 1, j].transform.position = grid[i, j].transform.position;
            grid[i, j].transform.position = temp_position;
            grid[i - 1, j] = grid[i, j];
            grid[i, j] = temp;
            return true;
        }
        return false;
    }
    bool moveRight(int i, int j)
    {
        if (i < grid.GetLength(0)-1)
        {
            GameObject temp = grid[i + 1, j];
            Vector2 temp_position = temp.transform.position;
            grid[i + 1, j].transform.position = grid[i, j].transform.position;
            grid[i, j].transform.position = temp_position;
            grid[i + 1, j] = grid[i, j];
            grid[i, j] = temp;
            return true;
        }
        return false;
    }
    bool moveUp(int i, int j) {
        if (j < grid.GetLength(1) - 1)
        {
            GameObject temp = grid[i, j+1];
            Vector2 temp_position = temp.transform.position;
            grid[i, j+1].transform.position = grid[i, j].transform.position;
            grid[i, j].transform.position = temp_position;
            grid[i, j+1] = grid[i, j];
            grid[i, j] = temp;
            return true;
        }
        return false;
    }
    bool moveDown(int i, int j)
    {
        if (j > 0)
        {
            GameObject temp = grid[i, j - 1];
            Vector2 temp_position = temp.transform.position;
            grid[i, j - 1].transform.position = grid[i, j].transform.position;
            grid[i, j].transform.position = temp_position;
            grid[i, j - 1] = grid[i, j];
            grid[i, j] = temp;
            return true;
        }
        return false;
    }
    GameObject[,] simulateMoveLeft(int i, int j) {
        if (i > 0)
        {
            GameObject[,] copy = new GameObject[6, 5];
            System.Array.Copy(grid, copy, grid.Length);
            GameObject temp = copy[i-1, j];
            copy[i - 1, j] = copy[i, j];
            copy[i, j] = temp;
            return copy;
        }
        else {
            return null;
        }

    }
    GameObject[,] simulateMoveRight(int i, int j)
    {
        if (i < grid.GetLength(0)-1)
        {
            GameObject[,] copy = new GameObject[6, 5];
            System.Array.Copy(grid, copy, grid.Length);
            GameObject temp = copy[i + 1, j];
            copy[i + 1, j] = copy[i, j];
            copy[i, j] = temp;
            return copy;
        }
        else
        {
            return null;
        }

    }
    GameObject[,] simulateMoveDown(int i, int j)
    {
        if (j > 0)
        {
            GameObject[,] copy = new GameObject[6, 5];
            System.Array.Copy(grid, copy, grid.Length);
            GameObject temp = copy[i, j-1];
            copy[i, j-1] = copy[i, j];
            copy[i, j] = temp;
            return copy;
        }
        else
        {
            return null;
        }

    }
    GameObject[,] simulateMoveUp(int i, int j)
    {
        if (j < grid.GetLength(1)-1)
        {
            GameObject[,] copy = new GameObject[6, 5];
            System.Array.Copy(grid, copy, grid.Length);
            GameObject temp = copy[i, j + 1];
            copy[i, j + 1] = copy[i, j];
            copy[i, j] = temp;
            return copy;
        }
        else
        {
            return null;
        }

    }


}
