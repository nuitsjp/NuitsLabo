# 文字数別の集計

正常・誤字各24文を2回ずつ評価。表の分母48は24種類の文の反復観測です。

## 100文字帯（実測 96〜105文字）

| 設定 | 正常正解 | 誤字正解 | エラー | 平均 ms | P95 ms | USD/件 |
|---|---:|---:|---:|---:|---:|---:|
| correction-luna56-v3 | 48/48 | 47/48 | 0 | 1805.7 | 2353.4 | 0.00029853 |
| correction-luna56-v3-edits-low | 48/48 | 46/48 | 0 | 1277.3 | 2076.0 | 0.00022585 |
| correction-luna6-v3 | 48/48 | 48/48 | 0 | 1912.1 | 2767.7 | 0.00015074 |
| correction-luna6-v3-edits-low | 48/48 | 48/48 | 0 | 1390.7 | 1981.0 | 0.00012179 |
| decision-openai-baseline | 38/48 | 48/48 | 0 | 328.6 | 527.9 | 0.00002524 |
| decision-openai-english | 48/48 | 46/48 | 0 | 339.7 | 374.2 | 0.00002554 |
| decision-typesafe-baseline | 48/48 | 32/48 | 0 | 282.8 | 348.4 | 0.00002510 |
| decision-typesafe-lean-calibrated | 48/48 | 30/48 | 0 | 279.7 | 320.7 | 0.00002414 |

判定と補正を組み合わせた推定値（誤字率50%）。直接補正は実測の集計。

| 判定 | 補正 | 正常維持 | 誤字完全一致 | 平均 ms | USD/件 |
|---|---|---:|---:|---:|---:|
| 直接補正 | correction-luna6-v3 | 48/48 | 48/48 | 1912.1 | 0.00015074 |
| 直接補正 | correction-luna6-v3-edits-low | 48/48 | 48/48 | 1390.7 | 0.00012179 |
| 直接補正 | correction-luna56-v3 | 48/48 | 47/48 | 1805.7 | 0.00029853 |
| 直接補正 | correction-luna56-v3-edits-low | 48/48 | 46/48 | 1277.3 | 0.00022585 |
| decision-openai-baseline | correction-luna6-v3 | 48/48 | 48/48 | 1424.0 | 0.00011517 |
| decision-openai-baseline | correction-luna6-v3-edits-low | 48/48 | 48/48 | 1250.7 | 0.00010234 |
| decision-openai-baseline | correction-luna56-v3 | 48/48 | 47/48 | 1482.1 | 0.00021341 |
| decision-openai-baseline | correction-luna56-v3-edits-low | 48/48 | 46/48 | 1238.7 | 0.00017368 |
| decision-openai-english | correction-luna6-v3 | 48/48 | 46/48 | 1207.0 | 0.00009674 |
| decision-openai-english | correction-luna6-v3-edits-low | 48/48 | 46/48 | 1100.1 | 0.00008813 |
| decision-openai-english | correction-luna56-v3 | 48/48 | 45/48 | 1283.0 | 0.00017832 |
| decision-openai-english | correction-luna56-v3-edits-low | 48/48 | 44/48 | 1111.5 | 0.00014887 |
| decision-typesafe-baseline | correction-luna6-v3 | 48/48 | 32/48 | 859.6 | 0.00007413 |
| decision-typesafe-baseline | correction-luna6-v3-edits-low | 48/48 | 32/48 | 785.4 | 0.00006787 |
| decision-typesafe-baseline | correction-luna56-v3 | 48/48 | 31/48 | 928.4 | 0.00013148 |
| decision-typesafe-baseline | correction-luna56-v3-edits-low | 48/48 | 30/48 | 815.9 | 0.00010991 |
| decision-typesafe-lean-calibrated | correction-luna6-v3 | 48/48 | 30/48 | 824.0 | 0.00007021 |
| decision-typesafe-lean-calibrated | correction-luna6-v3-edits-low | 48/48 | 30/48 | 754.2 | 0.00006445 |
| decision-typesafe-lean-calibrated | correction-luna56-v3 | 48/48 | 29/48 | 887.3 | 0.00012417 |
| decision-typesafe-lean-calibrated | correction-luna56-v3-edits-low | 48/48 | 28/48 | 782.2 | 0.00010377 |

## 200文字帯（実測 192〜208文字）

| 設定 | 正常正解 | 誤字正解 | エラー | 平均 ms | P95 ms | USD/件 |
|---|---:|---:|---:|---:|---:|---:|
| correction-luna56-v3 | 48/48 | 47/48 | 0 | 2516.1 | 3085.4 | 0.00042973 |
| correction-luna56-v3-edits-low | 48/48 | 48/48 | 0 | 1374.2 | 2231.7 | 0.00024782 |
| correction-luna6-v3 | 48/48 | 48/48 | 0 | 2364.4 | 3106.8 | 0.00021091 |
| correction-luna6-v3-edits-low | 48/48 | 48/48 | 0 | 1632.1 | 2496.6 | 0.00013633 |
| decision-openai-baseline | 36/48 | 48/48 | 0 | 314.0 | 337.6 | 0.00003337 |
| decision-openai-english | 48/48 | 44/48 | 0 | 312.1 | 344.6 | 0.00003367 |
| decision-typesafe-baseline | 48/48 | 29/48 | 0 | 287.0 | 354.6 | 0.00002936 |
| decision-typesafe-lean-calibrated | 48/48 | 30/48 | 0 | 280.7 | 333.7 | 0.00002839 |

判定と補正を組み合わせた推定値（誤字率50%）。直接補正は実測の集計。

| 判定 | 補正 | 正常維持 | 誤字完全一致 | 平均 ms | USD/件 |
|---|---|---:|---:|---:|---:|
| 直接補正 | correction-luna6-v3 | 48/48 | 48/48 | 2364.4 | 0.00021091 |
| 直接補正 | correction-luna6-v3-edits-low | 48/48 | 48/48 | 1632.1 | 0.00013633 |
| 直接補正 | correction-luna56-v3 | 48/48 | 47/48 | 2516.1 | 0.00042973 |
| 直接補正 | correction-luna56-v3-edits-low | 48/48 | 48/48 | 1374.2 | 0.00024782 |
| decision-openai-baseline | correction-luna6-v3 | 48/48 | 48/48 | 1770.6 | 0.00016296 |
| decision-openai-baseline | correction-luna6-v3-edits-low | 48/48 | 48/48 | 1398.8 | 0.00012036 |
| decision-openai-baseline | correction-luna56-v3 | 48/48 | 47/48 | 1944.2 | 0.00031027 |
| decision-openai-baseline | correction-luna56-v3-edits-low | 48/48 | 48/48 | 1282.5 | 0.00019890 |
| decision-openai-english | correction-luna6-v3 | 48/48 | 44/48 | 1385.3 | 0.00012768 |
| decision-openai-english | correction-luna6-v3-edits-low | 48/48 | 44/48 | 1128.5 | 0.00009827 |
| decision-openai-english | correction-luna56-v3 | 48/48 | 43/48 | 1522.7 | 0.00023813 |
| decision-openai-english | correction-luna56-v3-edits-low | 48/48 | 44/48 | 1101.5 | 0.00016050 |
| decision-typesafe-baseline | correction-luna6-v3 | 48/48 | 29/48 | 1001.4 | 0.00009184 |
| decision-typesafe-baseline | correction-luna6-v3-edits-low | 48/48 | 29/48 | 834.6 | 0.00007185 |
| decision-typesafe-baseline | correction-luna56-v3 | 48/48 | 28/48 | 1081.6 | 0.00016448 |
| decision-typesafe-baseline | correction-luna56-v3-edits-low | 48/48 | 29/48 | 823.5 | 0.00011268 |
| decision-typesafe-lean-calibrated | correction-luna6-v3 | 48/48 | 30/48 | 1012.2 | 0.00009278 |
| decision-typesafe-lean-calibrated | correction-luna6-v3-edits-low | 48/48 | 30/48 | 836.5 | 0.00007201 |
| decision-typesafe-lean-calibrated | correction-luna56-v3 | 48/48 | 29/48 | 1100.6 | 0.00016771 |
| decision-typesafe-lean-calibrated | correction-luna56-v3-edits-low | 48/48 | 30/48 | 833.5 | 0.00011465 |

## 400文字帯（実測 397〜411文字）

| 設定 | 正常正解 | 誤字正解 | エラー | 平均 ms | P95 ms | USD/件 |
|---|---:|---:|---:|---:|---:|---:|
| correction-luna56-v3 | 46/48 | 45/48 | 0 | 4023.0 | 5333.4 | 0.00066394 |
| correction-luna56-v3-edits-low | 46/48 | 47/48 | 0 | 1490.3 | 2078.3 | 0.00027173 |
| correction-luna6-v3 | 48/48 | 48/48 | 0 | 3272.3 | 4053.8 | 0.00030483 |
| correction-luna6-v3-edits-low | 48/48 | 47/48 | 0 | 1612.1 | 2268.6 | 0.00014317 |
| decision-openai-baseline | 32/48 | 48/48 | 0 | 330.0 | 635.7 | 0.00004989 |
| decision-openai-english | 48/48 | 44/48 | 0 | 313.3 | 375.2 | 0.00005019 |
| decision-typesafe-baseline | 48/48 | 30/48 | 0 | 285.1 | 348.0 | 0.00003790 |
| decision-typesafe-lean-calibrated | 48/48 | 27/48 | 0 | 287.8 | 356.5 | 0.00003693 |

判定と補正を組み合わせた推定値（誤字率50%）。直接補正は実測の集計。

| 判定 | 補正 | 正常維持 | 誤字完全一致 | 平均 ms | USD/件 |
|---|---|---:|---:|---:|---:|
| 直接補正 | correction-luna6-v3 | 48/48 | 48/48 | 3272.3 | 0.00030483 |
| 直接補正 | correction-luna6-v3-edits-low | 48/48 | 47/48 | 1612.1 | 0.00014317 |
| 直接補正 | correction-luna56-v3 | 46/48 | 45/48 | 4023.0 | 0.00066394 |
| 直接補正 | correction-luna56-v3-edits-low | 46/48 | 47/48 | 1490.3 | 0.00027173 |
| decision-openai-baseline | correction-luna6-v3 | 48/48 | 48/48 | 2492.0 | 0.00024966 |
| decision-openai-baseline | correction-luna6-v3-edits-low | 48/48 | 47/48 | 1403.7 | 0.00014645 |
| decision-openai-baseline | correction-luna56-v3 | 48/48 | 45/48 | 3001.9 | 0.00049322 |
| decision-openai-baseline | correction-luna56-v3-edits-low | 48/48 | 47/48 | 1386.8 | 0.00023786 |
| decision-openai-english | correction-luna6-v3 | 48/48 | 44/48 | 1762.8 | 0.00018494 |
| decision-openai-english | correction-luna6-v3-edits-low | 48/48 | 43/48 | 1080.5 | 0.00011770 |
| decision-openai-english | correction-luna56-v3 | 48/48 | 41/48 | 2174.2 | 0.00035850 |
| decision-openai-english | correction-luna56-v3-edits-low | 48/48 | 43/48 | 1072.7 | 0.00018321 |
| decision-typesafe-baseline | correction-luna6-v3 | 48/48 | 30/48 | 1247.0 | 0.00012755 |
| decision-typesafe-baseline | correction-luna6-v3-edits-low | 48/48 | 29/48 | 817.8 | 0.00008213 |
| decision-typesafe-baseline | correction-luna56-v3 | 48/48 | 29/48 | 1547.3 | 0.00024161 |
| decision-typesafe-baseline | correction-luna56-v3-edits-low | 48/48 | 29/48 | 806.3 | 0.00012418 |
| decision-typesafe-lean-calibrated | correction-luna6-v3 | 48/48 | 27/48 | 1150.1 | 0.00011728 |
| decision-typesafe-lean-calibrated | correction-luna6-v3-edits-low | 48/48 | 26/48 | 775.4 | 0.00007662 |
| decision-typesafe-lean-calibrated | correction-luna56-v3 | 48/48 | 26/48 | 1432.7 | 0.00022003 |
| decision-typesafe-lean-calibrated | correction-luna56-v3-edits-low | 48/48 | 26/48 | 759.1 | 0.00011373 |

## 800文字帯（実測 795〜819文字）

| 設定 | 正常正解 | 誤字正解 | エラー | 平均 ms | P95 ms | USD/件 |
|---|---:|---:|---:|---:|---:|---:|
| correction-luna56-v3 | 47/48 | 48/48 | 0 | 6826.3 | 8161.8 | 0.00115816 |
| correction-luna56-v3-edits-low | 46/48 | 46/48 | 0 | 1799.5 | 2537.9 | 0.00036224 |
| correction-luna6-v3 | 46/48 | 48/48 | 0 | 4949.4 | 6541.5 | 0.00051006 |
| correction-luna6-v3-edits-low | 48/48 | 48/48 | 0 | 1647.7 | 2338.5 | 0.00017844 |
| decision-openai-baseline | 28/48 | 48/48 | 0 | 309.6 | 340.4 | 0.00008211 |
| decision-openai-english | 46/48 | 44/48 | 0 | 316.6 | 366.5 | 0.00008241 |
| decision-typesafe-baseline | 47/48 | 30/48 | 0 | 283.4 | 338.6 | 0.00005495 |
| decision-typesafe-lean-calibrated | 48/48 | 28/48 | 0 | 285.6 | 351.9 | 0.00005398 |

判定と補正を組み合わせた推定値（誤字率50%）。直接補正は実測の集計。

| 判定 | 補正 | 正常維持 | 誤字完全一致 | 平均 ms | USD/件 |
|---|---|---:|---:|---:|---:|
| 直接補正 | correction-luna6-v3 | 46/48 | 48/48 | 4949.4 | 0.00051006 |
| 直接補正 | correction-luna6-v3-edits-low | 48/48 | 48/48 | 1647.7 | 0.00017844 |
| 直接補正 | correction-luna56-v3 | 47/48 | 48/48 | 6826.3 | 0.00115816 |
| 直接補正 | correction-luna56-v3-edits-low | 46/48 | 46/48 | 1799.5 | 0.00036224 |
| decision-openai-baseline | correction-luna6-v3 | 46/48 | 48/48 | 3777.7 | 0.00044322 |
| decision-openai-baseline | correction-luna6-v3-edits-low | 48/48 | 48/48 | 1493.4 | 0.00021190 |
| decision-openai-baseline | correction-luna56-v3 | 48/48 | 48/48 | 5073.0 | 0.00089854 |
| decision-openai-baseline | correction-luna56-v3-edits-low | 48/48 | 46/48 | 1551.0 | 0.00034153 |
| decision-openai-english | correction-luna6-v3 | 47/48 | 44/48 | 2584.4 | 0.00032311 |
| decision-openai-english | correction-luna6-v3-edits-low | 48/48 | 44/48 | 1147.8 | 0.00017104 |
| decision-openai-english | correction-luna56-v3 | 48/48 | 44/48 | 3528.5 | 0.00063273 |
| decision-openai-english | correction-luna56-v3-edits-low | 48/48 | 42/48 | 1168.2 | 0.00025746 |
| decision-typesafe-baseline | correction-luna6-v3 | 48/48 | 30/48 | 1815.4 | 0.00021704 |
| decision-typesafe-baseline | correction-luna6-v3-edits-low | 48/48 | 30/48 | 830.6 | 0.00011497 |
| decision-typesafe-baseline | correction-luna56-v3 | 48/48 | 30/48 | 2416.7 | 0.00042564 |
| decision-typesafe-baseline | correction-luna56-v3-edits-low | 48/48 | 28/48 | 850.0 | 0.00017478 |
| decision-typesafe-lean-calibrated | correction-luna6-v3 | 48/48 | 28/48 | 1673.6 | 0.00020064 |
| decision-typesafe-lean-calibrated | correction-luna6-v3-edits-low | 48/48 | 28/48 | 785.7 | 0.00010825 |
| decision-typesafe-lean-calibrated | correction-luna56-v3 | 48/48 | 28/48 | 2221.4 | 0.00038829 |
| decision-typesafe-lean-calibrated | correction-luna56-v3-edits-low | 48/48 | 26/48 | 805.8 | 0.00016227 |
