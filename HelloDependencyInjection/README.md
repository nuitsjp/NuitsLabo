こう言うと「私はDIコンテナーなんて使っていない！アンチDIだ！」とおっしゃる人もいるかもしれません。

まぁ落ち着いてください。今回は、DIコンテナーは脇役で、Dependency Injection（以後DI）パターンのお話です。

DIパターンとは、つぎのようなものだと私は考えています。

「依存性を外部から注入することで、ふるまいを変更する設計パターン」

詳細はコードを見つつ解説します。コードはC#で記載していますが、だれでも読めるレベルです。たぶん言語が違えど似たようなコードは誰もが書いたことがあるはずです。

ということで、さっそく見ていきましょう！

# あなたも使っているDIパターン

今回のお題は次の通りです。

**「何らかのリソースから文字列を読み取り、コンソールに出力する」**

もうネタはバレたかもしれません。

## ひとつめのDI

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

Streamは何らかのリソースへの入出力を提供する「ストリーム」を表す抽象クラスです。Streamは必ずしもテキストリソースだけを扱う訳ではなく、画像などのバイナリソースも扱うため、バイト列をもちいます。

文字列を扱うためには、バイト列から文字列へデコードする必要があります。StreamReaderクラスは、Streamからバイト列を取得し、デコードして利用者に文字列を提供します。StreamReaderクラスはバイト列をどのリソースからどのように取得するのか、一切関与しません。そのためStreamのみに依存し、FileStreamには一切依存しません。

Programクラスはこれらを組み合わせて、ローカルファイルを読み取ってコンソールへ出力しています。

StreamReaderは、抽象的な依存性（Stream）をコンストラクタ インジェクションしており、紛れもなくDIパターンが採用されています。

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

ほとんど同じ構造ですが、Streamの実装クラスが異なります。ひとつめはFileStreamですが、ふたつめはHttpClientが返してくる何らかのStreamの実装クラスであってその実態は分かりません。

どちらもStreamReaderに対して、抽象的な依存性（Stream）を注入するDIパターンによって実現されています。

オブジェクト指向言語をつかっている方であれば、どこかでDIをつかっているはずです。別に構えるほど特別なものではありません。

ところで、この例ではDIコンテナーは登場しません。DIコンテナーはDIパターンを利用するための道具であって、DIパターンを構成する必須要素ではありません。

# あらためてDIパターンとは何か？

## DIパターンの目的

DIパターンとは、抽象的な依存性を、外部から注入可能にすることで、ふるまいを変えるパターンです。

このとき、ふるまいを変える「目的」はつぎのものを得るためです。

- 再利用性
- 拡張性
- 保守性（レイヤー間の疎結合など）
- テスト容易性

など、ほかにもあります。[英語のWiki](https://en.wikipedia.org/wiki/Dependency_injection)が良くまとまっているので見てみるのも良いでしょう。

これらの目的を実現するための代表的なひとつの「手段」がDependency Injection Patternです。

## DIパターンと類似した他の手段

もちろん手段はひとつではありません。

DIパターンの対抗となる代表的なパターンはService Locatorパターンです。これは[DIが誕生した当初から議論されている](https://kakutani.com/trans/fowler/injection.html)ことです。FactoryなどもService Locatorと大きな違いはありません。

これらを比較したときのメリット・デメリットは簡単には語り切れませんが、ここでは代表的なケースについて簡単に記載します。

## DIのデメリット

Service Locatorと比較したとき、「難しい」ことだと私は思っています。習熟するとその難しさから遠ざかってしまいがちですが、DI推進派（私もここに所属します）はそこから目をそらすべきではないでしょう。

Service Locatorパターンは依存先のオブジェクトを利用する箇所で、依存先のオブジェクトを構築（もしくは取得）します。

対してDIパターンでは、依存先オブジェクトを利用する個所と、依存先オブジェクトを構築する個所が分離しています。

Service Locatorパターンでは普通にオブジェクトをnewして利用する代わりに、Service Locatorから取得して利用するだけで、そこに大きなパラダイムの変化はありません。これはDIと比較して「簡単な」解決策です。

## DIのメリット

逆にService Locatorでは解決できないケースもあります。そして今回のケースはこれに該当します。

DIやService Locatorの目的は「抽象的な依存性を切り替えることにより、ふるまいを変えること」です。

しかしService Locatorの場合、ふるまいを変えられる範囲に、DIよりも制限があります。

「ファイルとWeb上のリソースを読み取るためのリーダークラス」はService Locatorパターンでも作れるかもしれません。ファイルのアドレスもURLも文字列ですしね。

しかし「開発対象のシステムのBLOBストレージに格納されたバイナリオブジェクトを読み取れる汎用的なStreamReaderクラス」を作ることはService Locatorパターン単独で解決することは難しいでしょう。DIよりトリッキーなコードか、Service Locator（依存性）をInjectionするか、いずれか必要になりそうです。

## Service Locatorの難しさ

DIは難しいと書きましたが、逆にService Locatorの方が難しくなるケースもあります。

とくにユニットテストでは顕著です。

Service LocatorでMockを解決しようとした場合、依存オブジェクトの利用箇所から分離された箇所で、Mockに差し替える必要があります。これは先に書いたDIの難しさとまったく同じものです。

またテストケースをマルチスレッドで実行したいといった場合、Service Locatorをマルチスレッド対応する必要があります。[Thread-Specific Storageパターン](https://www.hyuki.com/dp/dpinfo_ThreadSpecificStorage.html)を利用して解決できるでしょうけど、「難しい」話しです。

# まとめ

- DIパターンとは、依存性を外部から注入することで、ふるまいを変えるパターンです
- DIコンテナーはDIパターンをサポートするツールで、DIパターンそのものではありません
- DIパターンを利用する目的には以下を得ることです
    - 再利用性
    - 拡張性
    - 保守性（レイヤー間の疎結合など）
    - テスト容易性
    - などなど
- 多くはService Locatorパターンで代替が可能ですが、代替できないケースもあります
- 依存性の利用箇所だけ見ると、Service Locatorパターンの方が簡単です
- テストを考慮するとDIの方が簡単なことはよくあります

個人的にはService Locatorじゃないといけない場合を除き、DIパターンを利用することが多いです。「慣れれば」そんなに難しいものではないですし、過去のXML Hellみたいなことは現代のDIにはありませんしね。

以上です。

