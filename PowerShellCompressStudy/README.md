PowerShellによるzip圧縮の評価をします。

- case1 : 対象直下にファイルが存在する
- case2 : サブフォルダ内にのみファイルが存在する
- case3 : case1 + case2

これをそれぞれ、つぎのzipにアーカイブします

- case2-compress.zip
- case2-compress.zip
- case2-compress.zip

内部はつぎのように格納します。

- case1 : ルートにitem1.txtとitem2.txtが存在する
- case2 : ルートの下にchild/childディレクトリがあり、その下にitem1.txtとitem2.txtが存在する
- case3 : case1 + case2

