using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets
{
    class Model : MonoBehaviour
    {
        public void lossFunction(GameObject[,] board, string weight) {
            if (weight == "combo") {
                ;
            }

        }
        public void calculateBoardCombo(GameObject[,] board) {
            int i = 0;
            int y = 0;
            while (i < board.GetLength(0)) {
                while (y < board.GetLength(1)) {
                    y++;
                }
                i++;
            }
        }
    }
}
