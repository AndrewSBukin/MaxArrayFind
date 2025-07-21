По мотивам статьи на Habr и https://github.com/insanity13/sandbox/tree/main/Sandbox/SubArrays тоже написал свою версию алгоритма.
Значения в массивах от 1 до 100. (Изменение верхней границы до 10000 на производительность влияет незначительно.)

﻿Data length 100, array size up to 50000

| Method                  | Mean       | Error     | StdDev    | Allocated |
|------------------------ |-----------:|----------:|----------:|----------:|
| DimsFromDergachyVersion |  31.600 ms | 0.3540 ms | 0.3311 ms |   16832 B |
| DenisNP_Version         | 101.273 ms | 1.8410 ms | 1.7220 ms |   19232 B |
| insanity13_Version      |  13.861 ms | 0.0423 ms | 0.0375 ms |      32 B |
| Andrewsbukin_Version    |   3.447 ms | 0.0666 ms | 0.0713 ms |      32 B |

Data length 1000, array size up to 500

| Method                  | Mean        | Error     | StdDev    | Gen0    | Allocated |
|------------------------ |------------:|----------:|----------:|--------:|----------:|
| DimsFromDergachyVersion |  3,473.7 us |  66.73 us |  65.54 us | 35.1563 |  167968 B |
| DenisNP_Version         | 10,408.3 us | 202.90 us | 189.79 us | 31.2500 |  191920 B |
| insanity13_Version      |  1,364.7 us |  26.80 us |  25.07 us |       - |      32 B |
| Andrewsbukin_Version    |    388.2 us |   5.32 us |   4.71 us |       - |      32 B |


Data length 5000, array size up to 1000

| Method                  | Mean       | Error     | StdDev    | Gen0     | Allocated |
|------------------------ |-----------:|----------:|----------:|---------:|----------:|
| DimsFromDergachyVersion |  34.298 ms | 0.6834 ms | 0.7312 ms | 133.3333 |  839776 B |
| DenisNP_Version         | 100.872 ms | 1.0739 ms | 0.9520 ms | 200.0000 |  959584 B |
| insanity13_Version      |  11.604 ms | 0.2281 ms | 0.2133 ms |        - |      32 B |
| Andrewsbukin_Version    |   3.763 ms | 0.0694 ms | 0.0650 ms |        - |      32 B |


Data length 10000, array size up to 5000

| Method                  | Mean        | Error     | StdDev    | Allocated |
|------------------------ |------------:|----------:|----------:|----------:|
| DimsFromDergachyVersion |   339.64 ms |  6.092 ms |  5.698 ms | 1680000 B |
| DenisNP_Version         | 1,050.58 ms | 17.402 ms | 16.277 ms | 1919920 B |
| insanity13_Version      |   138.05 ms |  1.742 ms |  1.544 ms |      32 B |
| Andrewsbukin_Version    |    36.18 ms |  0.709 ms |  0.663 ms |      32 B |


Вывод: чем проще код, тем выше производительность ))
