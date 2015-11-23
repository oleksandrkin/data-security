﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES_lab8
{
    public class DES
    {
        private int[][] IP =
        {
            new[] {58, 50, 42, 34, 26, 18, 10, 2},
            new[] {60, 52, 44, 36, 28, 20, 12, 4},
            new[] {62, 54, 46, 38, 30, 22, 14, 6},
            new[] {64, 56, 48, 40, 32, 24, 16, 8},
            new[] {57, 49, 41, 33, 25, 17, 9, 1},
            new[] {59, 51, 43, 35, 27, 19, 11, 3},
            new[] {61, 53, 45, 37, 29, 21, 13, 5},
            new[] {63, 55, 47, 39, 31, 23, 15, 7}
        };

        private int[][] IP_1 =
        {
            new[] {40, 8, 48, 16, 56, 24, 64, 32},
            new[] {39, 7, 47, 15, 55, 23, 63, 31},
            new[] {38, 6, 46, 14, 54, 22, 62, 30},
            new[] {37, 5, 45, 13, 53, 21, 61, 29},
            new[] {36, 4, 44, 12, 52, 20, 60, 28},
            new[] {35, 3, 43, 11, 51, 19, 59, 27},
            new[] {34, 2, 42, 10, 50, 18, 58, 26},
            new[] {33, 1, 41, 9, 49, 17, 57, 25}
        };

        private int[][] E =
        {
            new[] {32, 1, 2, 3, 4, 5},
            new[] {4, 5, 6, 7, 8, 9},
            new[] {8, 9, 10, 11, 12, 13},
            new[] {12, 13, 14, 15, 16, 17},
            new[] {16, 17, 18, 19, 20, 21},
            new[] {20, 21, 22, 23, 24, 25},
            new[] {24, 25, 26, 27, 28, 29},
            new[] {28, 29, 30, 31, 32, 1}
        };

        private int[][] P =
        {
            new[] {16, 7, 20, 21, 29, 12, 28, 17},
            new[] {1, 15, 23, 26, 5, 18, 31, 10},
            new[] {2, 8, 24, 14, 32, 27, 3, 9},
            new[] {19, 13, 30, 6, 22, 11, 4, 25}
        };

        private List<int[][]> S = new List<int[][]>()
        {
            new []
            {
                new[] {14, 4,  13, 1, 2	, 15, 11, 8	, 3	, 10, 6, 12, 5,  9,  0, 7},
            	new[] { 0, 15, 7,  4, 14, 2,  13, 1	, 10, 6, 12, 11, 9,  5,  3, 8},
            	new[] { 4, 1,  14, 8, 13, 6,  2,  11, 15, 12, 9, 7,  3,  10, 5, 0},
            	new[] {15, 12, 8,  2, 4	, 9,  1,  7,  5,  11, 3, 14, 10, 0,  6, 13}
            },
            new []
            {
                new[] {15	,1	,8	,14	,6	,11	,3	,4	,9	,7	,2	,13	,12	,0	,5	,10},
                new[] {3	,13	,4	,7	,15	,2	,8	,14	,12	,0	,1	,10	,6	,9	,11	,5},
                new[] {0	,14	,7	,11	,10	,4	,13	,1	,5	,8	,12	,6	,9	,3	,2	,15},
                new[] {13	,8	,10	,1	,3	,15	,4	,2	,11	,6	,7	,12	,0	,5	,14	,9}
            }
        };


    }
}
