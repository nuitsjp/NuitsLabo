こう言うと「私はDIコンテナーなんて使っていない！アンチDIだ！」とおっしゃる人もいるかもしれません。

まぁ落ち着いてください。今回は、DIコンテナーは脇役で、Dependency Injection（以後DI）パターンのお話です。

なおサンプルコードはC#で記載していますが、非常に簡単なコードなのでプログラミング経験があればだれでも読めると思います。また適宜図も併用して説明しますので、C#未経験の方もぜひご覧ください。

たぶん言語が違えど似たようなコードは誰もが書いたことがあるはずですし、とくにJava以降のオブジェクト指向言語であれば似たようなコードになるのではないかと思います。

ではさっそく見ていきましょう！

# あなたも使っているDIパターン

## ひとつめのDI

今回のお題は次の通りです。

**「何らかのリソースから文字列を読み取り、コンソールに出力する」**

私が何をネタに解説しようとしているか、もう理解されてしまった方もいるかもしれませんね。

さて、まずはローカルストレージ上のテキストを読み込んでコンソールに出力してみましょう。以下のコードをご覧ください。

```csharp
class Program
{
    static void Main(string[] args)
    {
        // ローカルの「README.txt」ファイルを読み取り専用で開く
        using var stream = new FileStream("README.txt", FileMode.Open);
        WriteConsole(stream);
    }

    static void WriteConsole(Stream stream)
    {
        // ストリームから文字列を読みだすため、StreamReaderを生成する
        using var reader = new StreamReader(stream);
        Console.WriteLine(reader.ReadToEnd());
    }
}
```

ここでは代表的なクラスとして、Program、FileStream、Stream、StreamReaderの4つのクラスが登場します。それらの関係は、つぎのようになっています。

![FileStream.jpg](https://qiita-image-store.s3.ap-northeast-1.amazonaws.com/0/99262/3f14b017-0467-3a8f-bb76-44adb438e56e.jpeg)

FileStreamはファイルへの入出力を提供するStreamの実装クラスです。

Streamは何らかのリソースへの入出力を提供する「ストリーム」を表す抽象クラスです。Streamは必ずしもテキストリソースだけを扱う訳ではなく、画像などのバイナリも扱うため、バイト列を扱います。

文字列を扱うためには、バイト列から文字列へデコードする必要があります。StreamReaderクラスは、Streamからバイト列を取得し、デコードして利用者に文字列を提供します。StreamReaderクラスはバイト列をどのリソースからどのように取得するのか、一切関与しません。そのためStreamのみに依存し、FileStreamには一切依存しません。

Programクラスはこれらを組み合わせて、ローカルファイルを読み取ってコンソールへ出力しています。


## ふたつめのDI

さて、ある時ローカルファイルではなく、Web上のリソースをコンソール出力したくなりました。

そこで、あなたはつぎのようにコードを書き換えました。

```csharp
class Program
{
    static async Task Main(string[] args)
    {
        //using var stream = new FileStream("README.txt", FileMode.Open);

        // HttpClientを利用してURL「https://www.google.com/」上のリソースを開く
        using var httpClient = new HttpClient();
        await using var stream = await httpClient.GetStreamAsync("https://www.google.com/");
        WriteConsole(stream);
    }

    static void WriteConsole(Stream stream)
    {
        // ストリームから文字列を読みだすため、StreamReaderを生成する
        using var reader = new StreamReader(stream);
        Console.WriteLine(reader.ReadToEnd());
    }
}
```

FileStreamの生成をコメントアウトし、HttpClientのGetStreamAsyncメソッドを利用して、指定アドレスからバイト列を読み取るためのStreamを非同期に取得するよう変更しました。

Streamを生成して以降のコードは一切変更していません。

## あなたも使っているDI

さて、私の伝えたいことがなんとなくわかったのではないでしょうか？

つぎの図は、ひとつめのモデルと、ふたつめのモデルを並べたものです。左がひとつめです。

![DependencyInjection.jpg](https://qiita-image-store.s3.ap-northeast-1.amazonaws.com/0/99262/e6684de4-29cb-b73c-16bb-94b8452859a5.jpeg)

ほとんど同じ構造ですが、Strreamの実装クラスが異なります。ひとつめはFileStreamですが、ふたつめはHttpClientが返してくる何らかのStreamの実装クラスであってその実態は分かりません。

お題にもどりましょう。

**「何らかのリソースから文字列を読み取り、コンソールに出力する」**

「何らかのリソースから文字列を読み取る」ためには、リソースからバイト列を取得し、バイト列をデコードする必要があります。

このとき、読みだし元のリソースがファイルであっても、Web上のページであっても、バイト列をデコードする処理はまったく変わりません。

そのため、何らかのリソースから「バイト列」を読み取る抽象的なStreamと、Streamからバイト列を読み取って文字列へデコードするStreamReaderにクラスを分割し、読み取りたいリソースごとにStreamの実装クラスを作ってあげれば良い。となりますよね？

そう、依存性（抽象的なStream）をStreamReaderへの注入（Injection）します。そうDIパターンの登場です。

ここで必ず覚えておいていただきたいことがひとつあります。DIコンテナーはDIパターンを利用するための道具であって、DIパターンを構成する必須要素ではないということです。

オブジェクト指向言語をつかっている方であれば、どこかでDIをつかっているはずです。別に構えるほど特別なものではありません。


# あらためてDIパターンとは何か？

DIパターンとは、抽象的な依存性を、外部から注入可能にすることで、ふるまいを変えるパターンです。

このとき、ふるまいを変える「目的」はつぎのようなものが考えられます。

- 戦略の変更（Storategy Pattern）。今回はこれ
- テスト容易性の実現（ユニットテストにおける依存先のMock（やFakeやStubなんでもいいです）への差し替え）
- 変更容易性の確保（レイヤー間の疎結合）

などなど。

これらの目的を実現するための代表的なひとつの「手段」がDependency Injection Patternです。

もちろん手段はひとつではありません。

DIパターンの対抗となる代表的なパターンはService Locatorパターンです。これは[DIが誕生した当初から議論されている](https://kakutani.com/trans/fowler/injection.html)ことです。

詳細は過去に[わたしのブログでも記載した](https://www.nuits.jp/entry/servicelocator-vs-dependencyinjection)ことがありますが、ここでも簡単に触れておきたいと思います。

## 戦略の変更とDI

今回のお題の実装を思い出してみましょう。

バイト列をデコードするためにStreamReaderクラスがあり、StreamReaderクラスへ抽象的なStreamを注入していますよね。この抽象を切り替えることで、あらゆるリソースから文字列を読み取れるようになります。

先にも記載したように、StreamReaderクラスはDIパターンで
